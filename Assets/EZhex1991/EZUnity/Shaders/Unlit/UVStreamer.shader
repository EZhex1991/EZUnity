// Author:			ezhex1991@outlook.com
// CreateTime:		2019-07-02 13:15:56
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/UVStreamer" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(UV Movements)]
		[EZVectorSingleLine] _DirU ("Dir U", Vector) = (1, 0, 0, 0)
		[EZVectorSingleLine] _DirV ("Dir V", Vector) = (0, 1, 0, 0)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			fixed4 _DirU;
			fixed4 _DirV;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float2 uvOffset = float2(dot(viewDir, _DirU), dot(viewDir, _DirV));
				o.uv_MainTex += uvOffset;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;				
				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
