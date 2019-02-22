// Author:			ezhex1991@outlook.com
// CreateTime:		2018-07-31 16:25:24
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/SurfaceCutoff/LocalStripeCutoff" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_AxisWeight ("Weight(XYZ) Offset(W)", Vector) = (1, 1, 1, 0)
		_FillRate ("Fill Rate", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		struct Input {
			float2 uv_MainTex;
			float3 localPos;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _AxisWeight;
		fixed _FillRate;

		void vert (inout appdata_base v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
		}
		void surf (Input IN, inout SurfaceOutput o) {
			clip(_FillRate - frac(dot(IN.localPos, _AxisWeight.xyz) + _AxisWeight.w));
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
