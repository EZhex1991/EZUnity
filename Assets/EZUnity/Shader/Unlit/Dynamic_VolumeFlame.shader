// Author:			熊哲
// CreateTime:		2018-08-01 17:11:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dynamic/VolumeFlame" {
	Properties {
		[Header(Flame)]
		_FlameTex ("Flame Texture(R)", 2D) = "white" {}
		_FlameNoise1 ("Noise1 UV(RG) Alpha(B)", 2D) = "black" {}
		_FlameNoise2 ("Noise2 UV(RG) Alpha(B)", 2D) = "black" {}
		_FlameColor1 ("Flame Color1", Color) = (1, 0, 0, 1)
		_FlameColor2 ("Flame Color2", Color) = (0.95, 0.5, 0, 1)
		_Flame1 ("Flame1 Speed(XY) Alpha(Z) Noise(W)", Vector) = (-0.1, -0.3, 1, 1)
		_Flame2 ("Flame2 Speed(XY) Alpha(Z) Noise(W)", Vector) = (0.12, -0.6, 1, 1)

		[Header(Flame Shape)]
		_VertexShape ("Shape", Vector) = (1, 1, 1, 1)
		_VertexAlphaPower ("Alpha Power", Float) = 4
		_VertexWave ("Wave", Vector) = (0.193, 0.375, 0.793, 1.975)
		_VertexWaveSpeed ("Speed", Float) = 0.2
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

			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 worldViewDir : ViewDir;
			};
			
			sampler2D _FlameTex;
			sampler2D _FlameNoise1;
			sampler2D _FlameNoise2;
			fixed4 _FlameColor1;
			fixed4 _FlameColor2;
			fixed4 _Flame1;
			fixed4 _Flame2;

			fixed4 _VertexWave;
			fixed _VertexAlphaPower;
			fixed _VertexWaveSpeed;
			fixed4 _VertexShape;

			v2f vert (appdata v) {
				fixed shape1 = sin(v.uv.y * 3.14) * (1 - pow(v.uv.y, _VertexShape.x)) * _VertexShape.y;
				fixed shape2 = pow(v.uv.y, _VertexShape.z) * _VertexShape.w;
				fixed4 shape3 = frac(_VertexWave * _Time.y * _VertexWaveSpeed) * 2.0 - 1.0;
				shape3 = SmoothTriangleWave(shape3);
				v.vertex.xyz += v.normal.xyz * (shape1 + shape2) * shape3;
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				return o;
			}
			fixed uvAlphaByWave (float2 uv, fixed4 flameWave) {
				fixed waveX = uv.x * flameWave.x + flameWave.y * _Time.y;
				fixed waveY = (sin(waveX * 6.28) + 1) * flameWave.z * 0.5;
				fixed alpha = pow(1 - saturate(uv.y / (1 -  waveY)), flameWave.w);
				return alpha;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed alpha = pow(abs(dot(i.worldViewDir, i.worldNormal)), _VertexAlphaPower);
				fixed2 uv1 = frac(i.uv + _Flame1.xy * _Time.y);
				fixed2 uv2 = frac(i.uv + _Flame2.xy * _Time.y);
				fixed4 noise1 = tex2D(_FlameNoise1, uv1) * _Flame1.w;
				fixed4 noise2 = tex2D(_FlameNoise2, uv2) * _Flame2.w;
				fixed4 flameColor = lerp(_FlameColor1, _FlameColor2, tex2D(_FlameTex, i.uv + noise1.rg + noise2.rg).r);
				flameColor.a = alpha * noise1.b * _Flame1.z * noise2.b * _Flame2.z;
				return flameColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
