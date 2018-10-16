// Author:			熊哲
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dynamic/PlaneFlame" {
	Properties {
		[Header(Flame)]
		_FlameTex ("Flame Texture(R)", 2D) = "white" {}
		_FlameNoise1 ("Noise1 UV(RG) Alpha(B)", 2D) = "black" {}
		_FlameNoise2 ("Noise2 UV(RG) Alpha(B)", 2D) = "black" {}
		_FlameColor1 ("Flame Color1", Color) = (1, 0, 0, 1)
		_FlameColor2 ("Flame Color2", Color) = (0.95, 0.5, 0, 1)
		_Flame1 ("Flame1 Speed(XY) Alpha(Z) Noise(W)", Vector) = (-0.1, -0.3, 0.9, 1)
		_Flame2 ("Flame2 Speed(XY) Alpha(Z) Noise(W)", Vector) = (0.12, -0.6, 0.8, 1)
		
		[Header(Alpha Wave Waves(X) Speed(Y) Height(Z) Power(W))]
		_AlphaWave1 ("Alpha Wave1", Vector) = (1.975, 0.793, 0.15, 0.7)
		_AlphaWave2 ("Alpha Wave2", Vector) = (0.375, 0.193, 0.2, 0.4)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass {
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _FlameTex;
			sampler2D _FlameNoise1;
			sampler2D _FlameNoise2;
			fixed4 _FlameColor1;
			fixed4 _FlameColor2;
			fixed4 _Flame1;
			fixed4 _Flame2;

			fixed4 _AlphaWave1;
			fixed4 _AlphaWave2;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			fixed uvAlphaByWave (float2 uv, fixed4 flameWave) {
				fixed waveX = uv.x * flameWave.x + flameWave.y * _Time.y;
				fixed waveY = (sin(waveX * 6.28) + 1) * flameWave.z * 0.5;
				fixed alpha = pow(1 - saturate(uv.y / (1 -  waveY)), flameWave.w);
				return alpha;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed2 uv1 = frac(i.uv + _Flame1.xy * _Time.y);
				fixed2 uv2 = frac(i.uv + _Flame2.xy * _Time.y);
				fixed4 noise1 = tex2D(_FlameNoise1, uv1) * _Flame1.w;
				fixed4 noise2 = tex2D(_FlameNoise2, uv2) * _Flame2.w;
				fixed4 flameColor = lerp(_FlameColor1, _FlameColor2, tex2D(_FlameTex, i.uv + noise1.rg + noise2.rg).r);
				fixed alpha1 = uvAlphaByWave(i.uv, _AlphaWave1) * noise1.b * _Flame1.z;
				fixed alpha2 = uvAlphaByWave(i.uv, _AlphaWave2) * noise2.b * _Flame2.z;
				flameColor.a = alpha1 * alpha2;
				return flameColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
