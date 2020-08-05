// Author:			ezhex1991@outlook.com
// CreateTime:		2019-03-08 11:14:59
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/EdgeFadeOut" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Edge)]
		[PowerSlider(8)] _AlphaPower ("Alpha Power", Range(0.01, 128)) = 32
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass {
			Blend SrcAlpha One
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			fixed _AlphaPower;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldView : TEXCOORD2;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldView = normalize(WorldSpaceViewDir(v.vertex));
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				half dotNV = dot(i.worldNormal, i.worldView);
				half alpha = pow(saturate(dotNV), _AlphaPower);

				color.a *= alpha;
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Transparent"
}
