// Author:			ezhex1991@outlook.com
// CreateTime:		2019-04-29 10:50:09
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/Cloud" {
	Properties {
		[Header(Main)]
		_CloudTex ("Cloud Texture R(Brightness) G(Alpha)", 2D) = "white" {}
		[HDR] _AmbientColor ("Ambient Color", Color) = (0.3, 0.6, 0.95, 1)
		[HDR] _SunColor ("Sun Color", Color) = (0.9, 0.9, 1, 1)

		_CloudContrast ("Cloud Contrast", Range(-16, 16)) = 3
		[PowerSlider(8)] _CloudDensity ("Cloud Density", Range(0.01, 128)) = 1
		[PowerSlider(8)] _AlphaPower ("Alpha Power", Range(0.01, 128)) = 1
	}
	SubShader {
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _CloudTex;
			float4 _CloudTex_ST;
			half4 _AmbientColor;
			half4 _SunColor;

			fixed _CloudContrast;
			fixed _CloudDensity;
			fixed _AlphaPower;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_CloudTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_CloudTex = TRANSFORM_TEX(v.uv0, _CloudTex);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 cloudTex = tex2D(_CloudTex, i.uv_CloudTex);

				fixed brightness = pow(saturate((cloudTex.r - 0.5) * _CloudContrast + 0.5), _CloudDensity);
				half4 color = lerp(_AmbientColor, _SunColor, brightness);

				color.a = pow(cloudTex.g, _AlphaPower);

				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
