// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-24 13:48:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/MultiTexture3x" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[KeywordEnum(UV0, UV1)] _MainUV ("Main UV", Int) = 0
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Additional 1)]
		_Add1Tex ("Texture", 2D) = "black" {}
		[KeywordEnum(UV0, UV1)] _Add1UV ("UV", Int) = 0
		_Add1Color ("Color", Color) = (1, 1, 1, 1)
		[KeywordEnum(AlphaBlend, Add)] _Add1BlendMode ("Blend Mode", Int) = 0
		
		[Header(Additional 2)]
		_Add2Tex ("Texture", 2D) = "black" {}
		[KeywordEnum(UV0, UV1)] _Add2UV ("UV", Int) = 0
		_Add2Color ("Color", Color) = (1, 1, 1, 1)
		[KeywordEnum(AlphaBlend, Add)] _Add2BlendMode ("Blend Mode", Int) = 0
		
		// EZShaderGUI Properties
		[HideInInspector] _RenderingMode ("_RenderingMode", Float) = 0
		[HideInInspector] _SrcBlendMode ("_SrcBlendMode", Float) = 1
		[HideInInspector] _DstBlendMode ("_DstBlendMode", Float) = 0
		[HideInInspector] _AlphaCutoff ("_AlphaCutoff", Range(0, 1)) = 0.5
		[HideInInspector] _ZWriteMode ("_ZWriteMode", Float) = 1
		[HideInInspector] _CullMode ("_CullMode", Float) = 2
		[HideInInspector] _OffsetFactor ("_OffsetFactor", Float) = 0
		[HideInInspector] _OffsetUnit ("_OffsetUnit", Float) = 0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			Blend [_SrcBlendMode] [_DstBlendMode]
			ZWrite [_ZWriteMode]
			Cull [_CullMode]
			Offset [_OffsetFactor], [_OffsetUnit]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _MainUV;
			fixed4 _Color;

			sampler2D _Add1Tex;
			float4 _Add1Tex_ST;
			int _Add1UV;
			fixed4 _Add1Color;
			int _Add1BlendMode;

			sampler2D _Add2Tex;
			float4 _Add2Tex_ST;
			int _Add2UV;
			fixed4 _Add2Color;
			int _Add2BlendMode;
			
			// EZShaderGUI Properties
			int _RenderingMode;
			fixed _AlphaCutoff;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float2 add1UV : TEXCOORD1;
				float2 add2UV : TEXCOORD2;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX((_MainUV == 0 ? v.uv0 : v.uv1), _MainTex);
				o.add1UV = TRANSFORM_TEX((_Add1UV == 0 ? v.uv0 : v.uv1), _Add1Tex);
				o.add2UV = TRANSFORM_TEX((_Add2UV == 0 ? v.uv0 : v.uv1), _Add2Tex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.mainUV) * _Color;

				fixed4 add1Color = tex2D(_Add1Tex, i.add1UV) * _Add1Color;
				if (_Add1BlendMode == 0) {
					color.rgb = add1Color.rgb * add1Color.a + color.rgb * (1 - add1Color.a);
				} else if (_Add1BlendMode == 1) {
					color += add1Color;
				}
				
				fixed4 add2Color = tex2D(_Add2Tex, i.add2UV) * _Add2Color;
				if (_Add2BlendMode == 0) {
					color.rgb = add2Color.rgb * add2Color.a + color.rgb * (1 - add2Color.a);
				} else if (_Add2BlendMode == 1) {
					color += add2Color;
				}
				
				if (_RenderingMode == 1) {
					clip(color.a - _AlphaCutoff);
				}
				return color;
			}
			ENDCG
		}
		//UsePass "VertexLit/SHADOWCASTER"
	}
	CustomEditor "EZShaderGUI"
}
