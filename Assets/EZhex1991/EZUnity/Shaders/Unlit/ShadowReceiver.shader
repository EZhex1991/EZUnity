// Author:			ezhex1991@outlook.com
// CreateTime:		2019-02-28 16:01:26
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/ShadowReceiver" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			half4 _Color;
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				UNITY_SHADOW_COORDS(1)
			};
			
			v2f vert (appdata v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				UNITY_TRANSFER_SHADOW(o, v.uv0)

				return o;
			}

			half4 frag (v2f i) : SV_Target {
				half4 color = _Color;

				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
				color.a = 1 - atten;

				return color;
			}

			ENDCG
		}
		UsePass "VertexLit/SHADOWCASTER"
	}
}
