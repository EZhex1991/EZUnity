// Author:			ezhex1991@outlook.com
// CreateTime:		2019-03-06 13:42:09
// Organization:	#ORGANIZATION#
// Description:		

#ifndef Fur_CG_INCLUDE
#define Fur_CG_INCLUDE

#include "EZUnity.cginc"
#include "Lighting.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
half4 _Color;

fixed4 _FurOffset;
sampler2D _FurTex;
float4 _FurTex_ST;
float _FurLength;
fixed4 _FurUVOffset;
fixed _AlphaPower;

fixed _LambertOffset;
half4 _AOColor;
fixed _AOOffset;
fixed _AOPower;
half4 _Spec1Color;
fixed _Spec1Power;
half4 _Spec2Color;
fixed _Spec2Power;
half4 _RimColor;
fixed _RimPower;

struct appdata {
	float4 vertex : POSITION;
	float2 uv0 : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
};

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv_MainTex : TEXCOORD0;
	float2 uv_FurTex : TEXCOORD1;
#if _LIGHTINGMODE_VERTEX
	half3 diffColor : TEXCOORD2;
	half3 spec1Color : TEXCOORD3;
	half3 spec2Color : TEXCOORD4;
	half3 rimColor : TEXCOORD5;
#elif _LIGHTINGMODE_PIXEL
	float3 worldNormal : TEXCOORD2;
	float3 worldTangent : TEXCOORD3;
	float3 worldBitangent : TEXCOORD4;
	float3 worldView : TEXCOORD5;
	float3 worldHalf : TEXCOORD6;
	float ao : TEXCOORD7;
#endif
};

v2f vert (appdata v) {
	v2f o;

	float3 shiftedNormal = normalize(v.normal + _FurOffset);
	v.vertex.xyz += lerp(v.normal, shiftedNormal, FUR_LAYER) * FUR_LAYER * _FurLength;
	o.pos = UnityObjectToClipPos(v.vertex);

	v.uv0 += _FurUVOffset.xy * _FurUVOffset.zw * FUR_LAYER;
	o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
	o.uv_FurTex = TRANSFORM_TEX(v.uv0, _FurTex);

#if _LIGHTINGMODE_VERTEX
	float3 worldNormal = UnityObjectToWorldNormal(v.normal);
	float3 worldTangent = UnityObjectToWorldDir(v.tangent);
	float3 worldBitangent = cross(worldNormal, worldTangent);
	float3 worldView = normalize(WorldSpaceViewDir(v.vertex));
	float3 worldHalf = normalize(_WorldSpaceLightPos0 + worldView);
	
	float dotNL = dot(worldNormal, _WorldSpaceLightPos0);
	float diff = Offset01(_LambertOffset, smoothstep(0, 1, dotNL));
	float ao = pow(Offset01(_AOOffset, FUR_LAYER), _AOPower);
	o.diffColor = lerp(_AOColor.rgb, 1, ao * diff);

	float dotTH = max(0, 1 - abs(dot(worldBitangent, worldHalf)));
	float spec = dotTH * saturate(dotNL + 1);
	float spec1 = pow(spec, _Spec1Power) * FUR_LAYER;
	float spec2 = pow(spec, _Spec2Power) * FUR_LAYER;
	o.spec1Color = spec1 * _Spec1Color.rgb * _Spec1Color.a;
	o.spec2Color = spec2 * _Spec2Color.rgb * _Spec2Color.a;
	
	float fresnel = 1 - max(0, dot(worldNormal, worldView));
	float rim = pow(fresnel, _RimPower) * FUR_LAYER;
	o.rimColor = rim * _RimColor.rgb * _RimColor.a;
#elif _LIGHTINGMODE_PIXEL
	o.worldNormal = UnityObjectToWorldNormal(v.normal);
	o.worldTangent = UnityObjectToWorldDir(v.tangent);
	o.worldBitangent = cross(o.worldNormal, o.worldTangent);
	o.worldView = normalize(WorldSpaceViewDir(v.vertex));
	o.worldHalf = normalize(_WorldSpaceLightPos0 + o.worldView);
	o.ao = pow(Offset01(_AOOffset, FUR_LAYER), _AOPower);
#endif

	return o;
}

half4 frag (v2f i) : SV_Target {
	half4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;

	half alpha = tex2D(_FurTex, i.uv_FurTex).r;
	alpha = pow(saturate(alpha - FUR_LAYER), _AlphaPower);
	
#if _LIGHTINGMODE_VERTEX
	color.rgb *= i.diffColor;
	color.rgb += (i.spec1Color + i.spec2Color) * alpha;
	color.rgb += i.rimColor;
#elif _LIGHTINGMODE_PIXEL
	float dotNL = dot(i.worldNormal, _WorldSpaceLightPos0);
	float diff = Offset01(_LambertOffset, smoothstep(0, 1, dotNL));
	half3 diffColor = lerp(_AOColor.rgb, 1, i.ao * diff);
	
	float dotTH = max(0, 1 - abs(dot(i.worldBitangent, i.worldHalf)));
	float spec = dotTH * saturate(dotNL + 1);
	float spec1 = pow(spec, _Spec1Power) * FUR_LAYER;
	float spec2 = pow(spec, _Spec2Power) * FUR_LAYER;
	half3 spec1Color = spec1 * _Spec1Color.rgb * _Spec1Color.a;
	half3 spec2Color = spec2 * _Spec2Color.rgb * _Spec2Color.a;

	float fresnel = 1 - max(0, dot(i.worldNormal, i.worldView));
	float rim = pow(fresnel, _RimPower) * FUR_LAYER;
	half3 rimColor = rim * _RimColor.rgb * _RimColor.a;
	
	color.rgb *= diffColor;
	color.rgb += (spec1Color + spec2Color) * alpha;
	color.rgb += rimColor;
#endif

	color.a *= alpha;

	return color;
}

#endif
