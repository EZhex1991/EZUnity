// Author:			ezhex1991@outlook.com
// CreateTime:		#CREATETIME#
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/_Dummy" {
	Properties {
		[EZKeywordEnumHeader(Value1, Value2)] _EZKeywordEnumHeader("EZKeywordEnumHeader", Float) = 0
		[KeywordEnum(Value1, Value2)] _DefaultKeywordEnum("Default Keyword Enum", Float) = 0


		[Header(EZMinMaxSlider)]
		_DefaultVector ("Default Vector", Vector) = (0, 1, 0, 1)
		[EZMinMaxSlider] _EZMinMaxSlider("EZMinMaxSlider", Vector) = (0, 1, 0, 1)
		[EZMinMaxSlider(0, 1)] _EZMinMaxSliderFixedLimit("EZMinMaxSlider (Fixed Limit)", Vector) = (0, 1, 0, 1)


		[Header(EZSingleLine)]
		[EZSingleLine(_EZSingleLineColor2)]
		_EZSingleLineColor1 ("Color1, Color2", Color) = (1, 1, 1, 1)
		[HideInInspector][HDR] _EZSingleLineColor2 ("Color2", Color) = (1, 1, 1, 1)

		[EZSingleLine(_EZSingleLineFloat2)]
		_EZSingleLineFloat1 ("Float1, Float2", Float) = 1
		[HideInInspector] _EZSingleLineFloat2 ("Float2", Float) = 1

		[EZSingleLine(_EZSingleLineColorRange2)]
		_EZSingleLineColorRange1 ("Color, Range", Color) = (1, 1, 1, 1)
		[HideInInspector] _EZSingleLineColorRange2 ("Range", Range(0, 1)) = 1

		[EZSingleLine(_EZSingleLineColorEnum2)]
		_EZSingleLineColorEnum1 ("Color, Enum", Color) = (1, 1, 1, 1)
		[HideInInspector][Enum(A, 0, B, 1, C, 2)] _EZSingleLineColorEnum2 ("Enum", Float) = 0

		[EZSingleLine(_EZSingleLineColorToggle2)]
		_EZSingleLineColorToggle1 ("Color, Toggle", Color) = (1, 1, 1, 1)
		[HideInInspector][Toggle] _EZSingleLineColorToggle2 ("Toggle", Float) = 1


		[Header(EZTextureSingleLine)]
		[EZTextureSingleLine(_EZTextureSingleLineColorFloat2, _EZTextureSingleLineColorFloat3)]
		_EZTextureSingleLineColorFloat1 ("Texture, Color, Float", 2D) = "white" {}
		[HideInInspector] _EZTextureSingleLineColorFloat2 ("Color", Color) = (1, 1, 1, 1)
		[HideInInspector] _EZTextureSingleLineColorFloat3 ("Float", Float) = 1

		[EZTextureSingleLine(_EZTextureSingleLineRangeToggle2, _EZTextureSingleLineRangeToggle3)]
		_EZTextureSingleLineRangeToggle1 ("Texture, Range, Toggle", 2D) = "white" {}
		[HideInInspector] _EZTextureSingleLineRangeToggle2 ("Range", Range(0, 1)) = 1
		[HideInInspector][Toggle] _EZTextureSingleLineRangeToggle3 ("Toggle", Float) = 1

		[EZTextureSingleLine(_EZTextureSingleLineRangeEnum2, _EZTextureSingleLineRangeEnum3)]
		_EZTextureSingleLineRangeEnum1 ("Texture, Enum, Range", 2D) = "white" {}
		[HideInInspector][EZMinMaxSlider] _EZTextureSingleLineRangeEnum2 ("Range", Vector) = (0, 1, 0, 1)
		[HideInInspector][Enum(UV0, 0, UV1, 1)] _EZTextureSingleLineRangeEnum3 ("Enum", Float) = 0

		
		[Header(EZTextureMini)]
		[EZTextureMini(_EZTextureMiniColorFloat2, _EZTextureMiniColorFloat3)]
		_EZTextureMiniColorFloat1 ("Texture, Color, Float", 2D) = "white" {}
		[HideInInspector] _EZTextureMiniColorFloat2 ("Color", Color) = (1, 1, 1, 1)
		[HideInInspector] _EZTextureMiniColorFloat3 ("Float", Float) = 1

		[EZTextureMini(_EZTextureMiniRangeToggle2, _EZTextureMiniRangeToggle3)]
		_EZTextureMiniRangeToggle1 ("Texture, Range, Toggle", 2D) = "white" {}
		[HideInInspector] _EZTextureMiniRangeToggle2 ("Range", Range(0, 1)) = 1
		[HideInInspector][Toggle] _EZTextureMiniRangeToggle3 ("Toggle", Float) = 1

		[EZTextureMini(_EZTextureMiniRangeEnum2, _EZTextureMiniRangeEnum3)]
		_EZTextureMiniRangeEnum1 ("Texture, Enum, Range", 2D) = "white" {}
		[HideInInspector][EZMinMaxSlider] _EZTextureMiniRangeEnum2 ("Range", Vector) = (0, 1, 0, 1)
		[HideInInspector][Enum(UV0, 0, UV1, 1)] _EZTextureMiniRangeEnum3 ("Enum", Float) = 0


		[Header(EZVectorSingleLine)]
		_DefaultVector1 ("Default Vector1", Vector) = (1, 1, 1, 1)
		_DefaultVector2 ("Default Vector2", Vector) = (1, 1, 1, 1)
		[EZVectorSingleLine] _EZSingleLineVector1 ("Single Line Vector1", Vector) = (1, 1, 1, 1)
		[EZVectorSingleLine] _EZSingleLineVector2 ("Single Line Vector2", Vector) = (1, 1, 1, 1)
	}
	CustomEditor "EZhex1991.EZUnity.EZShaderGUI"
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
