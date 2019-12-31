// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-24 10:11:02
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/DynamicFluid" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Trasparency)]
		[PowerSlider(8)]_AlphaPower ("Alpha Power", Range(0.1, 128)) = 4
		[EZVectorSingleLine] _Refrection ("Refrection", Vector) = (1.1, 1.2, 1.3, 1)

		[Header(Reflection)]
		[EZTextureMini] _ReflectionCube ("Reflection Cube", Cube) = "" {}
		[PowerSlider(8)]_ReflectionPower ("Reflection Power", Range(0.1, 128)) = 0.8
		_ReflectionStrength ("Reflection Strength", Range(0, 1)) = 1

		[Header(Roughness)]
		[EZTextureMini] _RoughTex ("Rough Texture", 2D) = "black" {}
		[EZVectorSingleLine] _Roughness ("Roughness Speed(XY) Refl(Z) Refr(W)", Vector) = (0.1, 0.1, 0.5, 0.5)
		
		[Header(Specular)]
		_SpecAtten ("Spec Atten", Range(0, 1)) = 1
		[HDR] _SpecColor ("Spec Color", Color) = (1, 1, 1, 1)
		[PowerSlider(8)]_SpecPower ("Spec Power", Range(0.1, 128)) = 16
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent+100" }
		//Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha

		GrabPass {
			"_GrabTexture"
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 mainUV : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
				float4 grabPosR : TEXCOORD3;
				float4 grabPosG : TEXCOORD4;
				float4 grabPosB : TEXCOORD5;
				float2 roughnessUV : TEXCOORD6;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			fixed _AlphaPower;
			fixed4 _Refrection;

			samplerCUBE _ReflectionCube;
			fixed _ReflectionPower;
			fixed _ReflectionStrength;
			
			sampler2D _RoughTex;
			float4 _RoughTex_ST;
			fixed4 _Roughness;
			
			fixed _SpecAtten;
			fixed4 _SpecColor;
			fixed _SpecPower;

			sampler2D _GrabTexture;

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.mainUV = TRANSFORM_TEX(v.uv, _MainTex);
				o.roughnessUV = TRANSFORM_TEX(v.uv, _RoughTex);
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.worldViewDir = normalize(WorldSpaceViewDir(v.vertex));
				fixed4 refrection = (1 - dot(o.worldNormal, o.worldViewDir)) * _Refrection;
				fixed4 grabPosR = UnityObjectToClipPos(v.vertex - v.normal * refrection.r * _Refrection.w);
				o.grabPosR = ComputeGrabScreenPos(grabPosR);
				fixed4 grabPosG = UnityObjectToClipPos(v.vertex - v.normal * refrection.g * _Refrection.w);
				o.grabPosG = ComputeGrabScreenPos(grabPosG);
				fixed4 grabPosB = UnityObjectToClipPos(v.vertex - v.normal * refrection.b * _Refrection.w);
				o.grabPosB = ComputeGrabScreenPos(grabPosB);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 rough = tex2D(_RoughTex, i.roughnessUV + frac(_Roughness.xy * _Time.y));

				// grab
				fixed4 grabColor = 1;
				grabColor.r = tex2Dproj(_GrabTexture, i.grabPosR + rough * _Roughness.w).r;
				grabColor.g = tex2Dproj(_GrabTexture, i.grabPosG + rough * _Roughness.w).g;
				grabColor.b = tex2Dproj(_GrabTexture, i.grabPosB + rough * _Roughness.w).b;

				// base color
				fixed4 color = tex2D(_MainTex, i.mainUV) * _Color;
				fixed alpha = pow(1 - abs(dot(i.worldNormal, i.worldViewDir)), _AlphaPower);
				color.a *= alpha;

				// reflection
				float3 worldRefl = reflect(-i.worldViewDir, i.worldNormal + rough * _Roughness.z);
				//float4 reflectionData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
				//fixed4 reflectionColor = fixed4(DecodeHDR(reflectionData, unity_SpecCube0_HDR), 1);
				fixed4 reflectionColor = texCUBE(_ReflectionCube, worldRefl);
				fixed dotNV = dot(i.worldNormal, i.worldViewDir);
				fixed reflectionStrength = pow(1 - saturate(dotNV), _ReflectionPower) * _ReflectionStrength;

				// specular
				float3 worldHalf = normalize(i.worldViewDir + _WorldSpaceLightPos0);
				fixed dotNH = dot(i.worldNormal, worldHalf);
				fixed specular = pow(abs(dotNH), _SpecPower);
				specular *= lerp(1, LIGHT_ATTENUATION(i), _SpecAtten);
				
				// blend
				color *= lerp(1, reflectionColor, reflectionStrength);
				color.rgb += _SpecColor.rgb * _SpecColor.a * specular;
				color = lerp(color, grabColor, 1 - color.a);
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
