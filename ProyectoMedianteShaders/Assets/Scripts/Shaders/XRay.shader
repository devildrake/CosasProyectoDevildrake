Shader "Custom/XRay" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_XTexture("Texture for x-ray", 2D) = "white"{}
		_XColor("Color for tint the x-ray", Color) = (1,1,1,1)
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		ZTest Greater
		
		CGPROGRAM
		#pragma surface surf Standard

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _XTexture;
		fixed4 _XColor;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_XTexture, IN.uv_MainTex) * _XColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

	}
}
