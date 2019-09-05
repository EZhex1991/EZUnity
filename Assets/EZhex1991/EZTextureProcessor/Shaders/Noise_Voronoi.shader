// Author:			ezhex1991@outlook.com
// CreateTime:		2019-09-03 15:22:51
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/Noise_Voronoi" {
	Properties {
		[Header(Voronoi)]
		[KeywordEnum(Gradient, Random)] _FillType ("Fill Type", Float) = 0
		_VoronoiDensity ("Voronoi Density", Vector) = (10, 10, 0, 0)
		_VoronoiAngleOffset ("Voronoi Angle Offset", Float) = 2
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _FILLTYPE_GRADIENT _FILLTYPE_RANDOM

			#include "UnityCG.cginc"

			float2 _VoronoiDensity;
			float _VoronoiAngleOffset;

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
			float2 RandomVector (float2 uv, float offset) {
				float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
				uv = frac(sin(mul(uv, m)) * 46839.32);
				return float2(sin(uv.y * offset) * 0.5 + 0.5, cos(uv.x * offset) * 0.5 + 0.5);
			}
			float VoronoiValue (float2 uv, float angleOffset, float cellDensity) {
				uv *= cellDensity;
				float2 grid = floor(uv);
				float2 uvInGrid = frac(uv);

				float minDistance = 8.0;
				float2 voronoiPoint = float2(0, 0);

				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						float2 lattice = float2(x, y);
						float2 offset = RandomVector(lattice + grid, angleOffset);
						float dist = distance(lattice + offset, uvInGrid);

						if (dist < minDistance) {
							minDistance = dist;
							voronoiPoint = grid + lattice;
						}
					}
				}

				#if _FILLTYPE_RANDOM
					return RandomValue(voronoiPoint);
				#elif _FILLTYPE_GRADIENT
					return minDistance;
				#endif
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = VoronoiValue(i.uv_MainTex, _VoronoiAngleOffset, _VoronoiDensity);
				return color;
			}
			ENDCG
		}
	}
}
