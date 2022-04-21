// Author:			ezhex1991@outlook.com
// CreateTime:		2019-05-15 11:13:11
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Disc" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[PowerSlider(2)]_Power ("Power", Range(0.1, 128)) = 2
		_Split ("Split", Range(0, 1)) = 0
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			fixed _Power;
			fixed _Split;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float3 worldTangentDir : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
				float3 worldHalf : TEXCOORD5;

				float3 r: TEXCOORD6;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				// v.vertex and cross() can lead to zero vectors error
				half valid = step(1e-5, length(v.vertex.xyz));
				half3 crs = cross(v.normal, normalize(lerp(v.tangent, v.vertex, valid)));
				o.worldTangentDir = UnityObjectToWorldDir(crs);

				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.worldHalf = _WorldSpaceLightPos0 + o.worldViewDir;
				o.r = crs;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

				float dotTH = saturate(1 - abs(abs(dot(normalize(i.worldTangentDir), normalize(i.worldHalf))) - _Split));
				float spec = pow(dotTH, _Power);

				return spec;;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
