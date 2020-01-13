Shader "EZUnity/Unlit/Curtain"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Spreading ("Spreading", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags {
			"RenderType"="Opaque"
			"PreviewType"="Plane"
		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Spreading;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// up
				float2 uv1 = float2(i.uv.x, i.uv.y - _Spreading);
				// down
				float2 uv2 = float2(i.uv.x, 1 - i.uv.y - _Spreading);
				// right
				float2 uv3 = float2(i.uv.y, i.uv.x - _Spreading);
				// left
				float2 uv4 = float2(i.uv.y, 1 - i.uv.x - _Spreading);

				fixed4 col = tex2D(_MainTex, uv1) + tex2D(_MainTex, uv2)
							+ tex2D(_MainTex, uv3) + tex2D(_MainTex, uv4);

				return col;
			}
			ENDCG
		}
	}
}
