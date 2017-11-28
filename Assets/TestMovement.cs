using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class TestMovement : MonoBehaviour {
	public enum m_type { VERTICAL, SPIN_A, SPIN_B }
	public m_type MovementType;
	public string objectName = "";
	Vector3 initPos;


	void Start () {
		initPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var data = OscMaster.GetData ("/" + objectName);
		if (data!=null) {
			Debug.Log (data.Length );
			foreach (var item in data) {
				Debug.Log (item);
			}
			OscMaster.ClearData ("/" + objectName);
		}

		switch (MovementType) {
		case m_type.SPIN_A:			
			this.transform.RotateAround(Vector3.zero, Vector3.up, 20* Time.deltaTime);
			break;
		case m_type.SPIN_B:			
			this.transform.RotateAround(Vector3.zero, Vector3.down, 20* Time.deltaTime);
			break;
		case m_type.VERTICAL:
			this.transform.RotateAround(new Vector3(0,initPos.y/2,0), Vector3.left, 20* Time.deltaTime);
			break;

		default:
			break;
		}

	}
}
