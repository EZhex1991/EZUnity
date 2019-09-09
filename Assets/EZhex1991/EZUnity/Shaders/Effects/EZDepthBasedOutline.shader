// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-31 16:41:15
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZUnity/Effects/EZDepthBasedOutline" {
	Properties {
		[HideInInspector]
		_MainTex ("Main Tex", 2D) = "white" {}
		
		_SampleDistance ("Sample Distance", Float) = 0.5
		
		_DepthSensitivity ("Depth Sensitivity", Float) = 10
		_NormalSensitivity ("Normal Sensitivity", Float) = 10
		
		_CoverColor ("Cover Color", Color) = (0, 0, 0, 1)
		_CoverStrength ("Cover Strength", Range(0, 1)) = 0
		
		_OutlineColor ("Outline Color", Color) = (0, 0, 1, 1)
		_OutlineStrength ("Outline Strength", Range(0, 1)) = 1
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

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv[5] : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _CameraDepthNormalsTexture;
			
			fixed _SampleDistance;

			fixed _DepthSensitivity;
			fixed _NormalSensitivity;

			fixed4 _CoverColor;
			fixed _CoverStrength;

			fixed4 _OutlineColor;
			fixed _OutlineStrength;

			int edgeCheck (float4 sample1, float4 sample2) {
				fixed2 normal1 = sample1.xy;
				fixed depth1 = DecodeFloatRG(sample1.zw);
				fixed2 normal2 = sample2.xy;
				fixed depth2 = DecodeFloatRG(sample2.zw);

				fixed2 normalDiff = abs(normal1 - normal2);
				int normalCheck = (normalDiff.x + normalDiff.y) * _NormalSensitivity < 1.0;
				int depthCheck = abs(depth1 - depth2) * _DepthSensitivity < depth1;
				return normalCheck * depthCheck ? 0 : 1;
			}
			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv[0] = v.uv;
				float2 uv = v.uv;
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						uv.xy = 1 - uv.xy;
				#endif
				o.uv[1] = uv + _MainTex_TexelSize.xy * float2(1, 1) * _SampleDistance;
				o.uv[2] = uv + _MainTex_TexelSize.xy * float2(-1, 1) * _SampleDistance;
				o.uv[3] = uv + _MainTex_TexelSize.xy * float2(-1, -1) * _SampleDistance;
				o.uv[4] = uv + _MainTex_TexelSize.xy * float2(1, -1) * _SampleDistance;
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv[0]);

				float4 sample1 = tex2D(_CameraDepthNormalsTexture, i.uv[1]);
				float4 sample2 = tex2D(_CameraDepthNormalsTexture, i.uv[2]);
				float4 sample3 = tex2D(_CameraDepthNormalsTexture, i.uv[3]);
				float4 sample4 = tex2D(_CameraDepthNormalsTexture, i.uv[4]);
				int edge = edgeCheck(sample1, sample3) + edgeCheck(sample2, sample4);
				edge = saturate(edge);

				color = lerp(color, _CoverColor, _CoverStrength);
				color = lerp(color, _OutlineColor, edge * _OutlineStrength);

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
