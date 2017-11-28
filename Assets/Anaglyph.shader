Shader "Hidden/Anaglyph"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LCam ("Left Cam Texture", 2D) = "white" {}
		_RCam ("Right Cam Texture", 2D) = "white" {}
		_CamType ("Camera Type", Float) = 0.0 

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _LCam;
			sampler2D _RCam;
			Float _CamType;
			RWTexture2D<float4> Result;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if (_CamType == 1.0){ //Left
					col.g = 0;
					col.b = 0;
				}else if(_CamType == 2.0){ //Right
					col.r = 0;
				}else if(_CamType == 3.0){
					fixed4 col_L = tex2D(_LCam, i.uv);
					fixed4 col_R = tex2D(_RCam, i.uv);
					col = col_R + col_L;
				}
				return col;
			}
			ENDCG
		}
	}
}
