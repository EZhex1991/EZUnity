Shader "EZShader/GrayScale"{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Glossiness("Smoothness", Range(0, 1)) = 0.5
		_Metallic("Metallic", Range(0, 1)) = 0
	}
	SubShader{
		Tags{"RenderType" = "Opaque"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		half _Glossiness;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed grey = dot(texColor.rgb, fixed3(0.299, 0.587, 0.114));
			o.Albedo = fixed3(grey, grey, grey);
			o.Alpha = texColor.a;
			o.Smoothness = _Glossiness;
			o.Metallic = _Metallic;
		}
		ENDCG
	}
	Fallback "Diffuse"
}