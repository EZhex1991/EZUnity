// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/_Dummy" {
	Properties {
		[EZKeywordEnumHeader(Value1, Value2)] _EZKeywordEnumHeader("EZKeywordEnumHeader", Float) = 0
		[EZMinMaxSlider] _EZMinMaxSlider("EZMinMaxSlider", Vector) = (0, 1, 1, 1)
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
