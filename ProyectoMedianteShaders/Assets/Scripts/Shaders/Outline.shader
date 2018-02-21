Shader "Custom/Outline" {
    Properties {
		_Color("Main color", Color) = (.5,.5,.5,1)
		_MainTex("Texture", 2D) = "white"{}
		_OutlineColor("Outline color", Color) = (0,0,0,1)
		_OutlineWidth("Outline width", Range(1.0,5.0)) = 1.01
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f{
		float4 pos : POSITION;
		float3 normal : NORMAL;
	};

	float _OutlineWidth;
	float4 _OutlineColor;

	v2f vertexShader (appdata v){
		v.vertex.xyz *= _OutlineWidth;

		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		
		return o;
	}

	ENDCG

	SubShader {
		Tags{
			"Queue" = "Geometry"
		}
		Pass{ //RENDER Outline
			ZWrite Off

			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader

			half4 fragmentShader(v2f i) : COLOR{
				return _OutlineColor;
			}

			ENDCG
		}

		Pass{ //NORMAL RENDER
			ZWrite On

			Material{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]{
				Combine previous * primary DOUBLE
			}
		}
	}
}