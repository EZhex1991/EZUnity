// Author:			ezhex1991@outlook.com
// CreateTime:		2019-03-06 20:21:31
// Organization:	#ORGANIZATION#
// Description:		

#ifndef EZUnity_CG_INCLUDE
#define EZUnity_CG_INCLUDE

float4 SmoothCurve(float4 x) {
	return x * x * (3 - 2 * x);
}
float4 TriangleWave(float4 x) {
	return abs(frac(x + 0.5) * 2 - 1);
}
float4 SmoothTriangleWave(float4 x) {
	return SmoothCurve(TriangleWave(x));
}
float InverseLerp(float min, float max, float value) {
	return saturate((value - min) / (max - min));
}
float Remap(float2 fromRange, float2 toRange, float value) {
	return lerp(toRange.x, toRange.y, InverseLerp(fromRange.x, fromRange.y, value));
}
float Offset01(float offset, float value) {
	return value * (1 - offset) + offset;
}

half3 RGB2HSV(half3 rgbColor) {
	fixed3 hsvColor;
	fixed _min = min(min(rgbColor.r, rgbColor.g), rgbColor.b);
	fixed _max = max(max(rgbColor.r, rgbColor.g), rgbColor.b);
	fixed delta = _max - _min;
	hsvColor.z = _max;
	if (delta == 0) {
		hsvColor.x = 0;
		hsvColor.y = 0;
	}
	else {
		hsvColor.y = delta / _max;
		fixed del_R = (((_max - rgbColor.r) / 6) + (delta / 2)) / delta;
		fixed del_G = (((_max - rgbColor.g) / 6) + (delta / 2)) / delta;
		fixed del_B = (((_max - rgbColor.b) / 6) + (delta / 2)) / delta;
		if (rgbColor.r == _max) hsvColor.x = del_B - del_G;
		else if (rgbColor.g == _max) hsvColor.x = (1 / 3) + del_R - del_B;
		else if (rgbColor.b == _max) hsvColor.x = (2 / 3) + del_G - del_R;
		if (hsvColor.x < 0) hsvColor.x += 1;
		else if (hsvColor.x > 1) hsvColor.x -= 1;
	}
	return hsvColor;
}
half3 HSV2RGB(half3 hsvColor) {
	fixed3 rgbColor;
	if (hsvColor.y == 0) {
		rgbColor.x = hsvColor.z;
		rgbColor.y = hsvColor.z;
		rgbColor.z = hsvColor.z;
	}
	else {
		if (hsvColor.x == 1) hsvColor.x = 0;
		hsvColor.x = hsvColor.x * 6;
		int v0 = (int)hsvColor.x;
		fixed v1 = hsvColor.z * (1 - hsvColor.y);
		fixed v2 = hsvColor.z * (1 - hsvColor.y * (hsvColor.x - v0));
		fixed v3 = hsvColor.z * (1 - hsvColor.y * (1 - (hsvColor.x - v0)));
		if (v0 == 0) {
			rgbColor.x = hsvColor.z; rgbColor.y = v3; rgbColor.z = v1;
		}
		else if (v0 == 1) {
			rgbColor.x = v2; rgbColor.y = hsvColor.z; rgbColor.z = v1;
		}
		else if (v0 == 2) {
			rgbColor.x = v1; rgbColor.y = hsvColor.z; rgbColor.z = v3;
		}
		else if (v0 == 3) {
			rgbColor.x = v1; rgbColor.y = v2; rgbColor.z = hsvColor.z;
		}
		else if (v0 == 4) {
			rgbColor.x = v3; rgbColor.y = v1; rgbColor.z = hsvColor.z;
		}
		else {
			rgbColor.x = hsvColor.z; rgbColor.y = v1; rgbColor.z = v2;
		}
	}
	return rgbColor;
}

half4 EZBlend_Alpha (half4 srcColor, half4 dstColor) {
	return srcColor * srcColor.a + dstColor * (1 - srcColor.a);
}
half4 EZBlend_Premultiply (half4 srcColor, half4 dstColor) {
	return srcColor + dstColor * (1 - srcColor.a);
}
half4 EZBlend_Additive (half4 srcColor, half4 dstColor) {
	return srcColor + dstColor;
}
half4 EZBlend_Multiply (half4 srcColor, half4 dstColor) {
	return srcColor * dstColor;
}

#endif
