Shader "Unlit/LBM_Output"
{
	Properties
	{
		_MainTex ("tex2D", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float _TexSize; 
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float4 computeColor(float normal_value)
			{
				float3 color;
				if (normal_value<0.0) normal_value = 0.0;
				if (normal_value>1.0) normal_value = 1.0;
				float v1 = 1.0 / 7.0;
				float v2 = 2.0 / 7.0;
				float v3 = 3.0 / 7.0;
				float v4 = 4.0 / 7.0;
				float v5 = 5.0 / 7.0;
				float v6 = 6.0 / 7.0;
				//compute color
				if (normal_value<v1)
				{
					float c = normal_value / v1;
					color.x = 70.*(1. - c);
					color.y = 70.*(1. - c);
					color.z = 219.*(1. - c) + 91.*c;
				}
				else if (normal_value<v2)
				{
					float c = (normal_value - v1) / (v2 - v1);
					color.x = 0.;
					color.y = 255.*c;
					color.z = 91.*(1. - c) + 255.*c;
				}
				else if (normal_value<v3)
				{
					float c = (normal_value - v2) / (v3 - v2);
					color.x = 0.*c;
					color.y = 255.*(1. - c) + 128.*c;
					color.z = 255.*(1. - c) + 0.*c;
				}
				else if (normal_value<v4)
				{
					float c = (normal_value - v3) / (v4 - v3);
					color.x = 255.*c;
					color.y = 128.*(1. - c) + 255.*c;
					color.z = 0.;
				}
				else if (normal_value<v5)
				{
					float c = (normal_value - v4) / (v5 - v4);
					color.x = 255.*(1. - c) + 255.*c;
					color.y = 255.*(1. - c) + 96.*c;
					color.z = 0.;
				}
				else if (normal_value<v6)
				{
					float c = (normal_value - v5) / (v6 - v5);
					color.x = 255.*(1. - c) + 107.*c;
					color.y = 96.*(1. - c);
					color.z = 0.;
				}
				else
				{
					float c = (normal_value - v6) / (1. - v6);
					color.x = 107.*(1. - c) + 223.*c;
					color.y = 77.*c;
					color.z = 77.*c;
				}
				return float4(color.r / 255.0, color.g / 255.0, color.b / 255.0, 1.0);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//only one pixel out of 4 stores the moments
				int ix = int(floor(i.uv.x * _TexSize));
				int iy = int(floor(i.uv.y * _TexSize));
				float3 m = tex2D(_MainTex, (float2(2 * ix + 1, 2 * iy + 1) + 0.5)).xyz;
				float solid = m.x;
				float vx = m.y;
				float vy = m.z;
				float U = sqrt(vx*vx + vy*vy);
				float4 fragColor = computeColor(U / 0.2);
				if (solid > 0.5)
					fragColor = float4(0.0, 0.0, 0.0, 0.0);

				return fragColor;
			}
			ENDCG
		}
	}
}
