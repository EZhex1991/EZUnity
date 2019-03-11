// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-24 10:11:02
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/DynamicGas" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Glossy)]
		[PowerSlider(8)]_AlphaPower ("Alpha Power", Range(0.1, 128)) = 4
		
		[Header(Specular)]
		_SpecAtten ("Spec Atten", Range(0, 1)) = 1
		[HDR] _SpecColor ("Spec Color", Color) = (1, 1, 1, 1)
		[PowerSlider(8)]_SpecPower ("Spec Power", Range(0.1, 128)) = 16

		[Header(Scanner)]
		_ScannerTex ("Scanner Texture", 2D) = "black" {}
		[HDR] _ScannerColor ("Scanner Color", Color) = (1, 1, 1, 1)
		_ScannerSpeed ("Scanner Speed(XY)", Vector) = (0, 0.3, 1, 1)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float2 scannerUV : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			fixed _AlphaPower;
			
			fixed _SpecAtten;
			half4 _SpecColor;
			fixed _SpecPower;

			sampler2D _ScannerTex;
			float4 _ScannerTex_ST;
			half4 _ScannerColor;
			fixed4 _ScannerSpeed;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv0, _MainTex);
				o.scannerUV = TRANSFORM_TEX(v.uv0, _ScannerTex) + _ScannerSpeed.xy * _Time.y;
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				return o;
			}
			half4 frag (v2f i) : SV_Target {
				// base color
				half4 color = tex2D(_MainTex, i.mainUV) * _Color;

				// alpha
				fixed dotNV = dot(i.worldNormal, i.worldViewDir);
				fixed alpha = pow(1 - abs(dotNV), _AlphaPower);
				color.a *= alpha;

				// specular
				float3 worldHalf = normalize(i.worldViewDir + _WorldSpaceLightPos0);
				fixed dotNH = dot(i.worldNormal, worldHalf);
				fixed specular = pow(abs(dotNH), _SpecPower);
				specular *= lerp(1, LIGHT_ATTENUATION(i), _SpecAtten);
				color.rgb += _SpecColor.rgb * _SpecColor.a * specular;

				// scanner
				half4 scannerTex = tex2D(_ScannerTex, i.scannerUV) * _ScannerColor;
				color = lerp(color, scannerTex, scannerTex.a);

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
