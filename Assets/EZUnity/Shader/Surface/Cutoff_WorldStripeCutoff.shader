// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Cutoff/WorldStripeCutoff" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_AxisWeight ("Weight(XYZ) Offset(W)", Vector) = (1, 1, 1, 0)
		_FillRate ("Fill Rate", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};
		
		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _AxisWeight;
		fixed _FillRate;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			clip(_FillRate - frac(dot(IN.worldPos, _AxisWeight.xyz) + _AxisWeight.w));
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
