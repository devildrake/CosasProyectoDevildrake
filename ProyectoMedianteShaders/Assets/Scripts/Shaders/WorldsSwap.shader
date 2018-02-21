Shader "Custom/MyShader"
{
	Properties
	{
		_MyTex1 ("Dusk", 2D) = "white" {}
		_MyTex2("Dawn", 2D) = "white" {}
		_MyAlpha("Alpha", 2D) = "white" {}
		
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
			
			sampler2D _MyTex1;
			sampler2D _MyTex2;
			sampler2D _MyAlpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				fixed4 alpha = tex2D(_MyAlpha, i.uv);
				if (alpha.r > 0.1) {
					col = tex2D(_MyTex1, i.uv);
				}
				else {
					col = tex2D(_MyTex2, i.uv);
				}
				//fixed4 col = fixed4(0.0,0.5,0.5,1.0);
				// just invert the colors
				//col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
