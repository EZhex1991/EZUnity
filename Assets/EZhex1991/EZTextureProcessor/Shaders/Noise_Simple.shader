// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-03 15:43:08
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Noise_Simple" {
	Properties {
		[Header(Noise)]
		_NoiseDensity ("Noise Density", Vector) = (50, 50, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float2 _NoiseDensity;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			float RandomValue (float2 uv) {
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}
			float NoiseValue (float2 uv) {
				float2 grid = floor(uv);
				float2 uvInGrid = frac(uv);
				uvInGrid = uvInGrid * uvInGrid * (3.0 - 2.0 * uvInGrid);

				float2 c00 = grid + float2(0, 0);
				float2 c10 = grid + float2(1, 0);
				float2 c01 = grid + float2(0, 1);
				float2 c11 = grid + float2(1, 1);

				float v00 = RandomValue(c00);
				float v10 = RandomValue(c10);
				float v01 = RandomValue(c01);
				float v11 = RandomValue(c11);

				return lerp(
					lerp(v00, v10, uvInGrid.x),
					lerp(v01, v11, uvInGrid.x),
					uvInGrid.y
				);
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				float value = 0;
				value += NoiseValue(i.uv_MainTex * _NoiseDensity);
				value += NoiseValue(i.uv_MainTex * _NoiseDensity / 2) * 2;
				value += NoiseValue(i.uv_MainTex * _NoiseDensity / 4) * 4;
				return value / 7;
			}
			ENDCG
		}
	}
}
