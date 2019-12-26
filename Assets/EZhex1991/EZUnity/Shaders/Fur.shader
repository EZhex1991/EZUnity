// Author:			ezhex1991@outlook.com
// CreateTime:		2019-03-06 11:42:19
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Fur" {
	Properties {
		[Header(Base)]
		[EZTextureSingleLine(_Color)] _MainTex ("Main Texture", 2D) = "white" {}
		[HideInInspector][HDR] _Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Fur)]
		[EZTextureMini(_FurLength)] _FurTex ("Fur Texture", 2D) = "white" {}
		[HideInInspector] _FurLength ("Fur Length", Float) = 0.2
		[EZVectorSingleLine] _FurOffset ("Fur Offset", Vector) = (0.1, -0.5, 1, 1)
		[EZVectorSingleLine] _FurUVOffset ("Fur UV Offset(XY) Scale(ZW)", Vector) = (5, 5, 0.01, 0.01)
		[PowerSlider(2)] _AlphaPower ("Alpha Power", Range(0.01, 4)) = 0.5

		[EZKeywordEnumHeader(Vertex, Pixel)] _LightingMode ("Lighting Mode", Float) = 0
		_LambertOffset ("Lambert Offset", Range(0, 1)) = 0.5

		[EZSingleLine(_AOPower)]
		_AOColor ("AO Color, Power", Color) = (0.2, 0.2, 0.2, 1)
		[HideInInspector][PowerSlider(2)] _AOPower ("AO Power", Range(0.01, 2)) = 0.5
		_AOOffset ("AO Offset", Range(0, 1)) = 0.2
		
		[EZSingleLine(_Spec1Power)]
		[HDR] _Spec1Color ("Spec1 Color, Power", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector][PowerSlider(4)] _Spec1Power ("Spec1 Power", Range(0.01, 32)) = 1

		[EZSingleLine(_Spec2Power)]
		[HDR] _Spec2Color ("Spec2 Color, Power", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector][PowerSlider(4)] _Spec2Power ("Spec2 Power", Range(0.01, 32)) = 32

		[EZSingleLine(_RimPower)]
		[HDR] _RimColor ("Rim Color", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector][PowerSlider(4)] _RimPower ("Rim Power", Range(0.01, 32)) = 1
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_Main : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_Main = TRANSFORM_TEX(v.uv0, _MainTex);
				return o;
			}

			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv_Main) * _Color;
				return color;
			}

			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.1

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.2

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.3

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.4

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.5

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.6

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.7

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.8

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 0.9

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _LIGHTINGMODE_VERTEX _LIGHTINGMODE_PIXEL
			#define FUR_LAYER 1.0

			#include "UnityCG.cginc"
			#include "CGIncludes/Fur.cginc"
			
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
