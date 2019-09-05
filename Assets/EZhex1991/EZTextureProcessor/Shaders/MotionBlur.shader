// Author:			ezhex1991@outlook.com
// CreateTime:		2019-08-27 16:16:35
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/MotionBlur" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}

		[Header(Blur)]
		_BlurWeightTex ("Blur Weight Texture", 2D) = "white" {}
		_BlurRadius ("Blur Radius", Float) = 5
		_BlurDirection ("Blur Direction", Vector) = (1, 0, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
			sampler1D _BlurWeightTex;
			int _BlurRadius;
			float2 _BlurDirection;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = v.uv0;
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				half4 color = half4(0, 0, 0, 0);
				float totalWeight = 0;
				for (int offset = -_BlurRadius; offset <= _BlurRadius; offset++){
					float2 uv = i.uv_MainTex + float2(offset, offset) * _BlurDirection.xy * _MainTex_TexelSize.xy;
					float weight = tex1D(_BlurWeightTex, (float)offset * 0.5f / _BlurRadius + 0.5f);
					color += tex2D(_MainTex, uv) * weight;
					totalWeight += weight;
				}
				return color / totalWeight;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
