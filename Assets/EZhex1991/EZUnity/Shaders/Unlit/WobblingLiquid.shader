// Author:			ezhex1991@outlook.com
// CreateTime:		2019-12-26 10:23:51
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/WobblingLiquid" {
	Properties {
		[Header(Filling)]
		_SurfaceHeight ("Surface Height", Float) = 1
		_FoamThickness ("Foam Thickness", Float) = 0.1

		[Header(Color)]
		_SurfaceColor ("Surface Color", Color) = (1, 0.8, 0.8, 1)
		_FoamColor ("Foam Color", Color) = (1, 0.4, 0.4, 1)
		_LiquidColor ("Liquid Color", Color) = (1, 0, 0, 1)

		[Header(Wobble)]
		_WaveHeight ("Wave Height", Float) = 0.2
		_Pivot ("Pivot", Vector) = (0, 0, 0, 1)
		_Centroid ("Centroid", Vector) = (0, 0, 0, 1)


		[Header(Rim)]
		[PowerSlider(8)] _RimPower ("Rim Power", Range(0.01, 128)) = 2
		[EZMinMaxSlider(0, 1)] _RimSoftness ("Rim Softness", Vector) = (0, 1, 0, 1)
		_RimColor ("Rim Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			fixed _SurfaceHeight;
			fixed _FoamThickness;

			fixed _WaveHeight;
			fixed4 _Pivot;
			fixed4 _Centroid;
			
			half4 _SurfaceColor;
			half4 _FoamColor;
			half4 _LiquidColor;

			fixed _RimPower;
			fixed4 _RimSoftness;
			half4 _RimColor;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float4 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				float3 pivot : TEXCOORD4;
			};
			
			inline float InverseLerp (in float a, in float b, in float value) {
				return saturate((value - a) / max(b - a, 1e-8));
			}

			inline float InverseLerp (in float2 range, in float value) {
				return saturate((value - range.x) / max(range.y - range.x, 1e-8));
			}

			float GetWaveHeight (float3 worldPos, float3 pivot) {
				float3 dir = worldPos - pivot;
				float distance = length(dir.xz);
				float amplitude = dot(dir.xz, _Centroid.xz);
				return _SurfaceHeight + distance * amplitude * _WaveHeight;
			}

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.pivot = mul(unity_ObjectToWorld, _Pivot);
				return o;
			}
			half4 frag (v2f i, fixed Facing : VFACE) : SV_Target {
				float surface = GetWaveHeight(i.worldPos, i.pivot);
				clip(surface + _FoamThickness - i.worldPos.y);

				half4 color = 1;
				color = lerp(_LiquidColor, _FoamColor, step(surface, i.worldPos.y));

				float dotNV = dot(i.worldNormal, i.worldViewDir);
				fixed rim = pow(1.0 - max(0, dotNV), _RimPower);
				rim = InverseLerp(_RimSoftness.x, _RimSoftness.y, rim);
				half4 rimColor = rim * _RimColor;
				color.rgb += rimColor.rgb;

				return Facing > 0 ? color : _SurfaceColor;
			}
			ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
