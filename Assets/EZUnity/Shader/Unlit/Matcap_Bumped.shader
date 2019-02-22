// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-22 19:33:10
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Matcap/Bumped" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[HDR] _Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Bump)]
		_BumpTex ("Bump Texture", 2D) = "bump" {}
		_Bumpiness ("Bumpiness", Range(0, 8)) = 1
		
		[Header(Matcap)]
		[NoScaleOffset] _MatcapTex ("Matcap Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			sampler2D _BumpTex;
			fixed _Bumpiness;
			
			sampler2D _MatcapTex;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float3 worldNormal : TEXCOORD2;
				float3 tbnSpace0 : TEXCOORD3;
				float3 tbnSpace1 : TEXCOORD4;
				float3 tbnSpace2 : TEXCOORD5;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				float3 worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				float3 worldBitangent = cross(o.worldNormal, worldTangent) * tangentSign;
				o.tbnSpace0 = float3(worldTangent.x, worldBitangent.x, o.worldNormal.x);
				o.tbnSpace1 = float3(worldTangent.y, worldBitangent.y, o.worldNormal.y);
				o.tbnSpace2 = float3(worldTangent.z, worldBitangent.z, o.worldNormal.z);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;

				// Unpack Normal
				float3 bumpTex = UnpackNormal(tex2D(_BumpTex, i.mainUV));
				float3 normal;
				normal.x = dot(i.tbnSpace0, bumpTex);
				normal.y = dot(i.tbnSpace1, bumpTex);
				normal.z = dot(i.tbnSpace2, bumpTex);
				normal = lerp(i.worldNormal, normal, _Bumpiness);

				// Matcap
				float2 matcapUV = mul(UNITY_MATRIX_IT_MV, normal) * 0.5 + 0.5;
				half4 matcapColor = tex2D(_MatcapTex, matcapUV);
				color.rgb *= matcapColor.rgb;

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
