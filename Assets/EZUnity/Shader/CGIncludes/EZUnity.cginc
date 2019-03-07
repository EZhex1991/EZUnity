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

#endif
