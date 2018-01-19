Shader "EZShader/GrayScale"{
	Properties{
		_Color("RGB Weight", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
		_RGBWeight("RGB Weight", Color) = (0.299, 0.587, 0.114, 1)
	}
	SubShader{
		Tags{"RenderType" = "Opaque"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed _Cutoff;
		fixed4 _RGBWeight;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			clip(texColor.a - _Cutoff);
			fixed grey = dot(texColor.rgb, _RGBWeight.rgb);
			o.Albedo = fixed3(grey, grey, grey);
			o.Alpha = texColor.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}