// Author:			ezhex1991@outlook.com
// CreateTime:		2019-10-18 11:38:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/EZShadowProjector" {
	Properties {
		_ShadowTex ("Shadow Texture", 2D) = "black" {}
		_ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0.5)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "PreviewType" = "Plane" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _ShadowTex;
			half4 _ShadowColor;

			float4x4 unity_Projector;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float4 uv_ShadowTex : TEXCOORD1;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_ShadowTex = mul(unity_Projector, v.vertex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				fixed shadow = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uv_ShadowTex));
				half4 color = _ShadowColor * step(0.5 - i.uv_ShadowTex.z * 0.5, shadow);

				return color;
			}
			ENDCG
		}
	}
}
