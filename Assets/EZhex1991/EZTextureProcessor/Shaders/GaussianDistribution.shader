// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-04 20:48:21
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/GaussianDistribution" {
	Properties {
		[Header(Gaussian)]
		[KeywordEnum(Wave, Lut1D, Lut2D)] _GaussianTextureType ("Texture Type", Float) = 0
		_GaussianRangeX ("Range X", Vector) = (-3, 3, 0, 0)
		_GaussianSigmaX ("Sigma X", Float) = 1
		_GaussianRangeY ("Range Y", Vector) = (-3, 3, 0, 0)
		_GaussianSigmaY ("Sigma Y", Float) = 1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _GAUSSIANTEXTURETYPE_WAVE _GAUSSIANTEXTURETYPE_LUT1D _GAUSSIANTEXTURETYPE_LUT2D

			#include "UnityCG.cginc"

			float2 _GaussianRangeX;
			float _GaussianSigmaX;
			float2 _GaussianRangeY;
			float _GaussianSigmaY;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			float Gaussian1D (float x, float sigma) {
				float sigma2 = sigma * sigma;
				float left = sqrt(1 / (2 * sigma2 * 3.1415926));
				float right = exp(-x * x / (2 * sigma2));
				return left * right;
			}
			float Gaussian2D (float x, float y, float sigma) {
				float sigma2 = sigma * sigma;
				float left = 1 / ( 2 * sigma2 * 3.1415926);
				float right = exp(-(x * x + y * y) / (2 * sigma2));
				return left * right;
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				float x = lerp(_GaussianRangeX.x, _GaussianRangeX.y, i.uv_MainTex.x);
				#if _GAUSSIANTEXTURETYPE_WAVE
					return step(Gaussian1D(x, _GaussianSigmaX), i.uv_MainTex.y);
				#elif _GAUSSIANTEXTURETYPE_LUT1D
					return Gaussian1D(x, _GaussianSigmaX);
				#elif _GAUSSIANTEXTURETYPE_LUT2D
					float y = lerp(_GaussianRangeY.x, _GaussianRangeY.y, i.uv_MainTex.y);
					return Gaussian1D(x, _GaussianSigmaX) * Gaussian1D(y, _GaussianSigmaY);
				#else
					return step(Gaussian1D(x, _GaussianSigmaX), i.uv_MainTex.y);
				#endif
			}
			ENDCG
		}
	}
}
