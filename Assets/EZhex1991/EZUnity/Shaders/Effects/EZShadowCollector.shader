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
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				return 1;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
