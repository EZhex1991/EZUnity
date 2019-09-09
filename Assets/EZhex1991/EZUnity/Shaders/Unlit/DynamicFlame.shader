// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/DynamicFlame" {
	Properties {
		[Header(Flame)]
		[EZTextureMini] _FlameTex ("FlameTex", 2D) = "white" {}
		[EZTextureMini] _FlameNoise1 ("Noise1 UV(RG) Alpha(B)", 2D) = "white" {}
		[EZTextureMini] _FlameNoise2 ("Noise2 UV(RG) Alpha(B)", 2D) = "white" {}
		[EZVectorSingleLine] _FlameFactor1 ("Flame1 Speed(XY) Strength(Z)", Vector) = (-0.2, -0.5, 1, 1)
		[EZVectorSingleLine] _FlameFactor2 ("Flame2 Speed(XY) Strength(Z)", Vector) = (0.12, -0.6, 1, 1)
		[HDR] _FlameColor1 ("Flame Color1", Color) = (2, 0, 0, 1)
		[HDR] _FlameColor2 ("Flame Color2", Color) = (2, 1, 0, 1)

		[Header(Shape)]
		[KeywordEnum(Plane, Volume)] _FlameMode ("Flame Mode", Float) = 0
		[EZVectorSingleLine] _AlphaFactor ("Alpha Power(XY) Strength(ZW)", Vector) = (1, 1, 1, 1)
		[EZVectorSingleLine] _ShapeFactor ("Shape Form(X) Inflate(Y) Speed(Z)", Vector) = (1, 1, 1, 1)
	}
	CustomEditor "EZhex1991.EZUnity.EZUnlitDynamicFlameShaderGUI"
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
			#pragma shader_feature _FLAMEMODE_PLANE _FLAMEMODE_VOLUME

			#include "UnityCG.cginc"
			
			sampler2D _FlameTex;
			float4 _FlameTex_ST;
			sampler2D _FlameNoise1;
			float4 _FlameNoise1_ST;
			sampler2D _FlameNoise2;
			float4 _FlameNoise2_ST;
			half4 _FlameColor1;
			half4 _FlameColor2;
			fixed4 _FlameFactor1;
			fixed4 _FlameFactor2;
			
			fixed4 _AlphaFactor;
			fixed4 _ShapeFactor;
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_FlameTex : TEXCOORD1;
				float2 uv_FlameNoise1 : TEXCOORD2;
				float2 uv_FlameNoise2 : TEXCOORD3;
#if _FLAMEMODE_VOLUME
				float3 worldNormal : NORMAL;
				float3 worldViewDir : TEXCOORD4;
#endif
			};

			float4 SmoothCurve(float4 x) {
				return x * x * (3 - 2 * x);
			}
			float4 TriangleWave(float4 x) {
				return abs(frac(x + 0.5) * 2 - 1);
			}
			float4 SmoothTriangleWave(float4 x) {
				return SmoothCurve(TriangleWave(x));
			}

			v2f vert (appdata v) {
				v2f o;
#if _FLAMEMODE_VOLUME
				fixed shape = SmoothTriangleWave(v.uv.y) * (1 - pow(v.uv.y, _ShapeFactor.x)) * _ShapeFactor.y;
				fixed wave = SmoothTriangleWave(v.uv.y + _Time.y * _ShapeFactor.z);
				v.vertex.xyz += v.normal.xyz * shape * wave;
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
#endif
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_FlameTex = TRANSFORM_TEX(v.uv, _FlameTex);
				o.uv_FlameNoise1 = TRANSFORM_TEX(v.uv, _FlameNoise1);
				o.uv_FlameNoise2 = TRANSFORM_TEX(v.uv, _FlameNoise2);
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				float2 noiseUV1 = frac(i.uv_FlameNoise1 + _FlameFactor1.xy * _Time.y);
				float2 noiseUV2 = frac(i.uv_FlameNoise2 + _FlameFactor2.xy * _Time.y);
				half4 noise1 = tex2D(_FlameNoise1, noiseUV1) * _FlameFactor1.z;
				half4 noise2 = tex2D(_FlameNoise2, noiseUV2) * _FlameFactor2.z;
				float2 flameUV = i.uv_FlameTex + noise1.rg + noise2.rg;
				half3 flame = tex2D(_FlameTex, flameUV);
				half4 flameColor = lerp(_FlameColor1, _FlameColor2, flame.r);
#if _FLAMEMODE_PLANE
				float alphaX = smoothstep(1, 0, abs(i.uv.x - 0.5) * 2);
				alphaX = pow(alphaX, _AlphaFactor.x) * _AlphaFactor.z;
				float alphaY = smoothstep(1, 0, i.uv.y);
				alphaY = pow(alphaY, _AlphaFactor.y) * _AlphaFactor.w;
				flameColor.a = noise1.b * noise2.b * alphaX * alphaY;
#elif _FLAMEMODE_VOLUME
				float alphaX = smoothstep(0, 1, abs(dot(i.worldViewDir, i.worldNormal)));
				alphaX = pow(alphaX, _AlphaFactor.x) * _AlphaFactor.z;
				float alphaY = smoothstep(1, 0, i.uv.y);
				alphaY = pow(alphaY, _AlphaFactor.y) * _AlphaFactor.w;
				flameColor.a = noise1.b * noise2.b * alphaX * alphaY;
#endif
				return flameColor;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
