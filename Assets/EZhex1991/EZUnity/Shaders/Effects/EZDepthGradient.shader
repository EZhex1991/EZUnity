// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-31 16:41:15
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZUnity/Effects/EZDepthGradient" {
	Properties {
		[HideInInspector]
		_MainTex ("Main Tex", 2D) = "white" {}
		
		[HDR] _ColorNear ("Color Near", Color) = (1, 1, 1, 1)
		[HDR] _ColorFar ("Color Far", Color) = (0.9, 1, 1, 1)

		[PowerSlider(8)] _GradientPower ("Gradient Power", Range(0.1, 128)) = 1
		[EZMinMaxSlider(0, 1)] _GradientSoftness ("Gradient Softness", Vector) = (0, 1, 0, 1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }
		Cull Off
		ZWrite Off
		ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "../CGIncludes/EZUnity.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _CameraDepthTexture;

			half4 _ColorNear;
			half4 _ColorFar;

			fixed _GradientPower;
			fixed4 _GradientSoftness;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.xy = 1 - o.uv.xy;
				#endif
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = tex2D(_MainTex, i.uv);

				float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv)));

				depth = InverseLerp(_GradientSoftness.x, _GradientSoftness.y, pow(depth, _GradientPower));

				color *= lerp(_ColorNear, _ColorFar, depth);

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
