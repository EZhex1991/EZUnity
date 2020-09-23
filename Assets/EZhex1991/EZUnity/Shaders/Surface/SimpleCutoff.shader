// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Surface/SimpleCutoff" {
	Properties {
		[Header(Main)]
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Cutoff)]
		[KeywordEnum(Local, World)] _CoordMode ("Coordinate Mode", Float) = 0
		[EZVectorSingleLine] _ClipPlane ("Clip Plane", Vector) = (0, 1, 0, 0)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma shader_feature _COORDMODE_LOCAL _COORDMODE_WORLD
				
		sampler2D _MainTex;
		fixed4 _Color;

		fixed4 _ClipPlane;

		struct Input {
			float2 uv_MainTex;
#if _COORDMODE_LOCAL
			float3 localPos;
#elif _COORDMODE_WORLD
			float3 worldPos;
#endif
		};
		
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
#if _COORDMODE_LOCAL
			o.localPos = v.vertex.xyz;
#endif
		}
		void surf (Input IN, inout SurfaceOutput o) {
#if _COORDMODE_LOCAL
			clip(dot(IN.localPos, _ClipPlane.xyz) - _ClipPlane.w);
#elif _COORDMODE_WORLD
			clip(dot(IN.worldPos, _ClipPlane.xyz) - _ClipPlane.w);
#endif
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
