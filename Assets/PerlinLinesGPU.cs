using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinLinesGPU : MonoBehaviour {
	public Mesh _mesh;
	public float _density;
	public float _range;
	public float _maxHeight;
	public float _speed = 3;
	public Transform[] tf;

	private int _instanceCount;
	private Material _mat;
	private ComputeBuffer _positionBuffer;
	private ComputeBuffer _argsBuffer;
	private Vector3 _center;
	private float _step;
	private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

	void Start () {
		if (_mat == null) {
			_mat = new Material (Shader.Find ("Instanced/PerlinesGPU"));
			_mat.hideFlags = HideFlags.HideAndDontSave;
		}
		_instanceCount = (int) Mathf.Pow (_density, 2);
		_step = (_range * 2) / _density;
	}
	

	void Update () {
		UpdateBuffers();
		Graphics.DrawMeshInstancedIndirect(_mesh, 0, _mat, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), _argsBuffer);
	}

	void UpdateBuffers(){
		
		if (_positionBuffer != null) {
			_positionBuffer.Release ();
		}
		_positionBuffer = new ComputeBuffer (_instanceCount, 4 * 4);	
		Vector4[] positions = new Vector4[_instanceCount];

		var time = Time.time * _speed;
		for (int i = 0; i < _density; i++) {
			for (int j = 0; j < _density; j++) {
				float pn = Perlin.Fbm (_step*i, _step*j, time, 1) + 0.5f;
				positions [i] = new Vector4 (-_range + _step * i, this.transform.position.y, -_range + _step * j, pn);
			}
		}
		_positionBuffer.SetData (positions);

		_center = Vector3.zero;
		for (int i = 0; i < tf.Length; i++) {
			_center += tf [i].position;	
		} 
		_center = _center  / tf.Length;

		_mat.SetBuffer ("positionBuffer", _positionBuffer);
		_mat.SetVector ("centerPos", _center); 


		uint numIndices = (_mesh != null) ? (uint)_mesh.GetIndexCount(0) : 0;
		args[0] = numIndices;
		args[1] = (uint)_instanceCount;
		_argsBuffer.SetData(args);
	}

	void OnDisable(){
		if (_positionBuffer != null)
			_positionBuffer.Release();
		_positionBuffer = null;

		if (_argsBuffer != null)
			_argsBuffer.Release();
		_argsBuffer = null;
	}
}
