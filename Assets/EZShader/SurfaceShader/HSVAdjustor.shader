Shader "EZShader/HSVAdjustor"{
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
		_Hue("Hue", Range(0, 1)) = 0
		_Saturation("Saturation", Range(0, 1)) = 1
		_Value("Value", float) = 1
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
		fixed _Hue;
		fixed _Saturation;
		fixed _Value;

		fixed3 RGB2HSV(fixed3 rgb) {
			fixed3 hsv;
			fixed _min = min(min(rgb.r, rgb.g), rgb.b);
			fixed _max = max(max(rgb.r, rgb.g), rgb.b);
			fixed delta = _max - _min;
			hsv.z = _max;
			if (delta == 0) {
				hsv.x = 0;
				hsv.y = 0;
			}
			else {
				hsv.y = delta / _max;
				fixed del_R = (((_max - rgb.r) / 6) + (delta / 2)) / delta;
				fixed del_G = (((_max - rgb.g) / 6) + (delta / 2)) / delta;
				fixed del_B = (((_max - rgb.b) / 6) + (delta / 2)) / delta;
				if (rgb.r == _max) hsv.x = del_B - del_G;
				else if (rgb.g == _max) hsv.x = (1 / 3) + del_R - del_B;
				else if (rgb.b == _max) hsv.x = (2 / 3) + del_G - del_R;
				if (hsv.x < 0) hsv.x += 1;
				else if (hsv.x > 1) hsv.x -= 1;
			}
			return hsv;
		}
		fixed3 HSV2RGB(fixed3 hsv) {
			fixed3 rgb;
			if (hsv.y == 0) {
				rgb.x = hsv.z;
				rgb.y = hsv.z;
				rgb.z = hsv.z;
			}
			else {
				if (hsv.x == 1) hsv.x = 0;
				hsv.x = hsv.x * 6;
				int v0 = (int)hsv.x;
				fixed v1 = hsv.z * (1 - hsv.y);
				fixed v2 = hsv.z * (1 - hsv.y * (hsv.x - v0));
				fixed v3 = hsv.z * (1 - hsv.y * (1 - (hsv.x - v0)));
				if (v0 == 0) {
					rgb.x = hsv.z; rgb.y = v3; rgb.z = v1;
				}
				else if (v0 == 1) {
					rgb.x = v2; rgb.y = hsv.z; rgb.z = v1;
				}
				else if (v0 == 2) {
					rgb.x = v1; rgb.y = hsv.z; rgb.z = v3;
				}
				else if (v0 == 3) {
					rgb.x = v1; rgb.y = v2; rgb.z = hsv.z;
				}
				else if (v0 == 4) {
					rgb.x = v3; rgb.y = v1; rgb.z = hsv.z;
				}
				else {
					rgb.x = hsv.z; rgb.y = v1; rgb.z = v2;
				}
			}
			return rgb;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			clip(texColor.a - _Cutoff);
			fixed3 hsv = RGB2HSV(texColor);
			hsv.x += _Hue;
			hsv.y *= _Saturation;
			hsv.z *= _Value;
			if (hsv.x >= 1) hsv.x -= 1;
			o.Albedo = HSV2RGB(hsv);
			o.Alpha = texColor.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}