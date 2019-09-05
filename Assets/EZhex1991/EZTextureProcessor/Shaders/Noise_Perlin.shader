// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-03 16:06:17
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Noise_Perlin" {
	Properties {
		[Header(Noise)]
		_NoiseDensity ("Noise Density", Vector) = (10, 10, 0, 0)
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

			float2 RandomVector (float2 uv) {
				uv = uv % 289;
				float x = (34 * uv.x + 1) * uv.x % 289 + uv.y;
				x = (34 * x + 1) * x % 289;
				x = frac(x / 41) * 2 - 1;
				return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
			}
			float NoiseValue (float2 uv, float2 cellDensity) {
				uv *= cellDensity;
				float2 grid = floor(uv);
				float2 uvInGrid = frac(uv);
				float v00 = dot(RandomVector(grid), uvInGrid);
				float v01 = dot(RandomVector(grid + float2(0, 1)), uvInGrid - float2(0, 1));
				float v10 = dot(RandomVector(grid + float2(1, 0)), uvInGrid - float2(1, 0));
				float v11 = dot(RandomVector(grid + float2(1, 1)), uvInGrid - float2(1, 1));
				uvInGrid = uvInGrid * uvInGrid * uvInGrid * (uvInGrid * (uvInGrid * 6 - 15) + 10);
				return 0.5 + lerp(
					lerp(v00, v01, uvInGrid.y),
					lerp(v10, v11, uvInGrid.y),
					uvInGrid.x
				);
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = NoiseValue(i.uv_MainTex, _NoiseDensity);
				return color;
			}
			ENDCG
		}
	}
}
