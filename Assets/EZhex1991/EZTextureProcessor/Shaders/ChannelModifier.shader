// Author:			ezhex1991@outlook.com
// CreateTime:		2020-01-07 15:16:17
// Organization:	#ORGANIZATION#
// Description:		

Shader "Hidden/EZTextureProcessor/ChannelModifier" {
	Properties {
		[Header(Main)]
		_TexR ("Texture R", 2D) = "white" {}
		_ChannelR ("Channel R", Int) = 0
		_LutR ("Lut R", 2D) = "white" {}
		
		_TexG ("Texture G", 2D) = "white" {}
		_ChannelR ("Channel G", Int) = 1
		_LutG ("Lut G", 2D) = "white" {}
		
		_TexB ("Texture B", 2D) = "white" {}
		_ChannelR ("Channel B", Int) = 2
		_LutB ("Lut B", 2D) = "white" {}
		
		_TexA ("Texture A", 2D) = "white" {}
		_ChannelR ("Channel A", Int) = 3
		_LutA ("Lut A", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _TexR;
			sampler2D _TexG;
			sampler2D _TexB;
			sampler2D _TexA;
			int _ChannelR;
			int _ChannelG;
			int _ChannelB;
			int _ChannelA;
			sampler1D _LutR;
			sampler1D _LutG;
			sampler1D _LutB;
			sampler1D _LutA;

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
				half4 color;

				color.r = tex1D(_LutR, tex2D(_TexR, i.uv_MainTex)[_ChannelR]).r;
				color.g = tex1D(_LutG, tex2D(_TexG, i.uv_MainTex)[_ChannelG]).r;
				color.b = tex1D(_LutB, tex2D(_TexB, i.uv_MainTex)[_ChannelB]).r;
				color.a = tex1D(_LutA, tex2D(_TexA, i.uv_MainTex)[_ChannelA]).r;

				return color;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
