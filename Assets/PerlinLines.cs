using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinLines : MonoBehaviour {
	public int density = 100;
	public float maxHeihgt = 30;
	public float rectRange = 2;
	public Material mat;
	public int _fractalLevel;
	public float speed = 3;
	public float lineWidth = 0.05f;

	public Transform[] tf;

	LineRenderer[,] lineRenderers;
	Vector3 center;
	private float step = 0;
	// Use this for initialization
	void Start () {		
		step = rectRange * 2 / density;

		var time = Time.time * speed;

		center = Vector3.zero;
		for (int i = 0; i < tf.Length; i++) {
			center += tf [i].position;
		} 
		center = center  / tf.Length;

		lineRenderers = new LineRenderer[ density, density ];

		for (int i = 0; i < density; i++) {
			for (int j = 0; j < density; j++) {
				
				GameObject line = new GameObject ("perlinLine");
				line.transform.parent = this.transform;
				lineRenderers[i, j] = line.AddComponent<LineRenderer> ();
				lineRenderers[i, j].SetWidth (lineWidth, lineWidth);
				lineRenderers[i, j].material = mat;
				lineRenderers[i, j].positionCount = 2;
				lineRenderers[i, j].SetPosition (0, new Vector3 ( -rectRange + step*i ,this.transform.position.y, -rectRange + step*j));
				lineRenderers[i, j].SetPosition (1, new Vector3 ( -rectRange + step*i ,this.transform.position.y, -rectRange + step*j));
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {
		var time = Time.time * speed;
		center = Vector3.zero;
		for (int i = 0; i < tf.Length; i++) {
			center += tf [i].position;	
		} 
		center = center  / tf.Length;

		for (int i = 0; i < density; i++) {
			for (int j = 0; j < density; j++) {
				float pn = Perlin.Fbm (step*i, step*j, time,_fractalLevel) + 0.5f;

				lineRenderers[i, j].SetPosition (1, 
					Vector3.ClampMagnitude(
						center -  new Vector3( -rectRange + step*i ,this.transform.position.y, -rectRange + step*j), 
						pn * maxHeihgt ) 
					+ new Vector3( -rectRange + step*i ,this.transform.position.y, -rectRange + step*j) 
				);
			}	
		}

		
	}
}
