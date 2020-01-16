// Author:			ezhex1991@outlook.com
// CreateTime:		2019-07-29 20:09:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/EZShadowCollector" {
	Properties {
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float _EZShadowCollector_ShadowBias;

			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos.z -= _EZShadowCollector_ShadowBias;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				float depth = i.pos.z / i.pos.w;

				#if defined (SHADER_TARGET_GLSL)
					depth = depth * 0.5 + 0.5;
				#elif defined (UNITY_REVERSED_Z)
					depth = 1 - depth;
				#endif

				return depth;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
