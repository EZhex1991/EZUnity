// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-31 16:44:46
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dynamic/Skybox" {
	Properties {
		[Header(Sky)]
		[NoScaleOffset] _SkyTex ("Sky Texture", Cube) = "" {}
		_SkyColor ("Sky Color", Color) = (1, 1, 1, 1)
		_SkyExposure ("Sky Exposure", Range(0, 8)) = 1
		_SkySpeed ("Sky Speed", Float) = 0

		[Header(Cloud 1)]
		[NoScaleOffset] _Cloud1Tex ("Cloud1 Texture", Cube) = "" {}
		_Cloud1Color ("Cloud1 Color", Color) = (1, 1, 1, 0.5)
		[KeywordEnum(Disabled, AlphaBlend, Add)] _Cloud1BlendMode ("Cloud1 Blend Mode", Int) = 0
		_Cloud1Exposure ("Cloud1 Exposure", Range(0, 8)) = 1
		_Cloud1Speed ("Cloud1 Speed", Float) = 1

		[Header(Cloud 2)]
		[NoScaleOffset] _Cloud2Tex ("Cloud2 Texture", Cube) = "" {}
		_Cloud2Color ("Cloud2 Color", Color) = (1, 1, 1, 0.5)
		[KeywordEnum(Disabled, AlphaBlend, Add)] _Cloud2BlendMode ("Cloud2 Blend Mode", Int) = 0
		_Cloud2Exposure ("Cloud2 Exposure", Range(0, 8)) = 1
		_Cloud2Speed ("Cloud2 Speed", Float) = 2
	}
	SubShader {
		Tags { "RenderType" = "Background" "Queue" = "Background" "PreviewType" = "Skybox" }
		Cull Off
		ZWrite Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float3 skyCoord : TEXCOORD0;
				float3 cloud1Coord : TEXCOORD1;
				float3 cloud2Coord : TEXCOORD2;
			};

			samplerCUBE _SkyTex;
			half4 _SkyTex_HDR;
			fixed4 _SkyColor;
			fixed _SkyExposure;
			fixed _SkySpeed;
			
			samplerCUBE _Cloud1Tex;
			half4 _Cloud1Tex_HDR;
			fixed4 _Cloud1Color;
			int _Cloud1BlendMode;
			fixed _Cloud1Exposure;
			fixed _Cloud1Speed;
			
			samplerCUBE _Cloud2Tex;
			half4 _Cloud2Tex_HDR;
			fixed4 _Cloud2Color;
			int _Cloud2BlendMode;
			fixed _Cloud2Exposure;
			fixed _Cloud2Speed;

			float3 RotateAroundY (float3 vertex, float speed) {
				float r = radians(fmod(_Time.y * speed, 360));
				float sina, cosa;
				sincos(r, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float3(mul(m, vertex.xz), vertex.y).xzy;
			}
			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.skyCoord = RotateAroundY(v.vertex.xyz, _SkySpeed);
				o.cloud1Coord = RotateAroundY(v.vertex.xyz, _Cloud1Speed);
				o.cloud2Coord = RotateAroundY(v.vertex.xyz, _Cloud2Speed);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				half4 color = texCUBE(_SkyTex, i.skyCoord);
				color.rgb = DecodeHDR(color, _SkyTex_HDR) * _SkyColor.rgb * _SkyExposure;

				if (_Cloud1BlendMode != 0) {
					half4 cloud1Color = texCUBE(_Cloud1Tex, i.cloud1Coord);
					cloud1Color.rgb = DecodeHDR(cloud1Color, _Cloud1Tex_HDR);
					cloud1Color = cloud1Color * _Cloud1Color * _Cloud1Exposure;
					if (_Cloud1BlendMode == 1) {
						color.rgb = lerp(color.rgb, cloud1Color.rgb, cloud1Color.a);
					} else if (_Cloud1BlendMode == 2) {
						color.rgb += cloud1Color.rgb * cloud1Color.a;
					}
				}

				if (_Cloud2BlendMode != 0) {
					half4 cloud2Color = texCUBE(_Cloud2Tex, i.cloud2Coord);
					cloud2Color.rgb = DecodeHDR(cloud2Color, _Cloud2Tex_HDR);
					cloud2Color = cloud2Color * _Cloud2Color * _Cloud2Exposure;
					if (_Cloud2BlendMode == 1) {
						color.rgb = lerp(color.rgb, cloud2Color.rgb, cloud2Color.a);
					} else if (_Cloud2BlendMode == 2) {
						color.rgb += cloud2Color.rgb * cloud2Color.a;
					}
				}

				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
