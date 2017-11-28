using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererPosSetup : MonoBehaviour {
	
	LineRenderer lineRenderer;
	public Transform objL;
	public Transform objR;
	public Transform objH;
	Vector3[] pos = new Vector3[4];

	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.positionCount = 4;
	}

	// Update is called once per frame
	void Update () {
		pos [0] = objH.position;
		pos [1] = objL.position;
		pos [2] = objR.position;
		pos [3] = objH.position;

		lineRenderer.SetPositions (pos);
	}
}
