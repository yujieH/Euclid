using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaglyphEffect : MonoBehaviour {
	public enum CamType {Left, Right, Final};
	private Material mat;
	public Shader shader;
	public CamType cType;

	public static RenderTexture Ldest;
	public static RenderTexture Rdest;

	void Start(){
		if (shader!=null) {
			mat = new Material (shader);
			mat.hideFlags = HideFlags.HideAndDontSave;	
		}
		switch (cType) {
		case CamType.Left:
			mat.SetFloat ("_CamType", 1f);
			Ldest = new RenderTexture (Screen.width, Screen.height, 24);
			Ldest.Create ();
			this.GetComponent<Camera> ().targetTexture = Ldest;
			break;
		case CamType.Right:
			mat.SetFloat ("_CamType", 2f);
			Rdest = new RenderTexture (Screen.width, Screen.height, 24);
			Rdest.Create ();
			this.GetComponent<Camera> ().targetTexture = Rdest;
			break;
		case CamType.Final:
			mat.SetFloat ("_CamType", 3f);
			break;
		default:
			break;
		}
	}

	void Update(){
		if (cType == CamType.Left && ( Ldest.width != Screen.width || Ldest.height != Screen.height) ) {
			Ldest = new RenderTexture (Screen.width, Screen.height, 24);
			Ldest.Create ();
			this.GetComponent<Camera> ().targetTexture = Ldest;
		}

		if (cType == CamType.Right && ( Rdest.width != Screen.width || Rdest.height != Screen.height) ) {
			Rdest = new RenderTexture (Screen.width, Screen.height, 24);
			Rdest.Create ();
			this.GetComponent<Camera> ().targetTexture = Rdest;
		}
	}


	void OnRenderImage(RenderTexture src, RenderTexture dest) {	
		switch (cType) {
		case CamType.Left:
			Graphics.Blit (src, dest, mat);
			break;
		case CamType.Right:
			Graphics.Blit(src, dest, mat);
			break;
		case CamType.Final:		
			mat.SetTexture ("_LCam", Ldest);
			mat.SetTexture ("_RCam", Rdest);		
			Graphics.Blit(src, dest, mat);
			break;
		default:
			break;
		}
	}
}
