// Author:			ezhex1991@outlook.com
// CreateTime:		2019-08-23 15:27:28
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/GaussianBlur" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_BlurRadius ("Blur Radius", Float) = 1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
		
			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			int _BlurRadius;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv0;
				return o;
			}
			float GaussWeight (float x, float y, float sigma) {
				float sigma2 = sigma * sigma;
				float left = 1 / (2 * sigma2 * 3.1415926);
				float right = exp(-(x * x + y * y) / (2 * sigma2));
				return left * right;
			}
			half4 frag (v2f i) : SV_Target {
				float sigma = _BlurRadius / 3.0f;
				half4 color = float4(0, 0, 0, 0);
				for (int x = -_BlurRadius; x <= _BlurRadius; x++){
					for (int y = -_BlurRadius; y <= _BlurRadius; y++){
						float2 uv = i.uv + float2(x, y) * _MainTex_TexelSize;
						color += tex2D(_MainTex, uv) * GaussWeight(x, y, sigma);
					}
				}
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
