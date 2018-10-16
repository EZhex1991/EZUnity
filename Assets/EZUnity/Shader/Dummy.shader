// Author:			熊哲
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Dummy" {
	Properties {
		_Float ("Float", Float) = 0.5
		_Range ("Range", Range (0, 1)) = 0.5
		[IntRange] _IntRange ("Int", Range (0, 10)) = 1
		[PowerSlider(3.0)] _PowerSlider ("PowerSlider", Range (0.01, 1)) = 0.08
		[HideInInspector] _HiddenRange ("HiddenRange", Range (0, 1)) = 0.5
		[Toggle] _ToggleFloat ("ToggleFloat", Float) = 1
		[KeywordEnum(None, Add, Multiply)] _Enum ("Enum", Float) = 0
		// Float, Range => float(32), half(16), fixed(11)
		// Int => int

		[Space(30)]
		[Header(Header1)]
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Vector ("Vector", Vector) = (1, 1, 1, 1)
		// Color, Vector => float4, half4, fixed4
		// use fixed4 for Color and Normalized Vector for better performance

		[Space(50)]
		[Header(Header2)]
		_2D ("2D", 2D) = "white" {}
		[NoScaleOffset] _2DWithoutOffset ("2DWithoutOffset", 2D) = "white" {}
		[Normal] _2DNormal ("2DNormal", 2D) = "white" {}
		[HDR] _2DHDR ("2DHDR", 2D) = "white" {}
		_Cube ("Cube", Cube) = "" {}
		_3D ("3D", 3D) = "" {}
		// 2D => sampler2D
		// 3D => sampler3D
		// Cube => samplerCUBE
	}
	SubShader {
		// Tags { "RenderType"="Opaque" }
		// Tags { "Queue" = "Background" } // 1000
		// Tags { "Queue" = "Geometry" } // 2000, default, for Opaque
		// Tags { "Queue" = "AlphaTest" } // 2450, alpha cutoff
		// Tags { "Queue" = "Transparent" } // 3000, alpha blending
		// Tags { "Queue" = "Overlay" } // 4000, UI etc
		//* Tags { "Queue" = "Geometry-1" } // Geometry - 1 = 1999, no space
		Pass {
			Blend Zero One
			ZWrite Off
		}
	}
}
