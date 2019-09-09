// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-18 16:44:03
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZUnity/Effects/EZReflection" {
	Properties {
		[HideInInspector]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[HideInInspector]
		_ReflectionTex ("Reflection Texture", 2D) = "black" {}
		[HideInInspector]
		_ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
		
		[HideInInspector]
		_RefractionTex ("Refraction Texture", 2D) = "black" {}
		[HideInInspector]
		_RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ _REFLECTION_ON
			#pragma shader_feature _ _REFRACTION_ON

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float4 screenPos : TEXCOORD01;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			sampler2D _ReflectionTex;
			fixed _ReflectionStrength;
			sampler2D _RefractionTex;
			fixed _RefractionStrength;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				o.screenPos = ComputeNonStereoScreenPos(o.pos);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;

				float4 projCoord = UNITY_PROJ_COORD(i.screenPos);

				#if _REFLECTION_ON
					half4 reflection = tex2Dproj(_ReflectionTex, projCoord);
					color = (color + reflection * _ReflectionStrength) / (1 + _ReflectionStrength);
				#endif

				#if _REFRACTION_ON
					half4 refraction = tex2Dproj(_RefractionTex, projCoord);
					color *= (color + refraction * _RefractionStrength) / (1 + _RefractionStrength);
				#endif

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
