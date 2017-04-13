Shader "Unlit/LBM_Buffer_Shader"
{
	Properties
	{
		_MainTex ("tex2D", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
																	 
			#define f0(x,y) tex2D(_MainTex,    (float2(2*x,   2*y)    +0.5) / _TexSize.xy).r;
			#define f1(x,y) tex2D(_MainTex,    (float2(2*x,   2*y)    +0.5) / _TexSize.xy).g;
			#define f2(x,y) tex2D(_MainTex,    (float2(2*x,   2*y)    +0.5) / _TexSize.xy).b;
			#define f3(x,y) tex2D(_MainTex,    (float2(2*x+1, 2*y)    +0.5) / _TexSize.xy).r;
			#define f4(x,y) tex2D(_MainTex,    (float2(2*x+1, 2*y)    +0.5) / _TexSize.xy).g;
			#define f5(x,y) tex2D(_MainTex,    (float2(2*x+1, 2*y)    +0.5) / _TexSize.xy).b;
			#define f6(x,y) tex2D(_MainTex,    (float2(2*x,   2*y+1)  +0.5) / _TexSize.xy).r;
			#define f7(x,y) tex2D(_MainTex,    (float2(2*x,   2*y+1)  +0.5) / _TexSize.xy).g;
			#define f8(x,y) tex2D(_MainTex,    (float2(2*x,   2*y+1)  +0.5) / _TexSize.xy).b;
			#define solid(x,y) tex2D(_MainTex, (float2(2*x+1,2*y+1)   +0.5) / _TexSize.xy).r;

			//channel velocity
			#define VEL 0.1

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;

			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 wPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform int iFrame;
			uniform float4 iMouse;
			uniform float _W;
			uniform float2 _TexSize;


			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.texcoord;
				//o.uv.y = 1.0 - o.uv.y;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			//relaxation time
			   float w = _W;
			//i.uv.y = 1.0 
			float2 fragCoord = (i.uv.xy * _TexSize.xy);
			//fragCoord.x *= _TexSize.x/ _TexSize.y ;

			//fragColor=tex2D(_MainTex, i.ve=rtex);
			int LatSizeX = int(_TexSize.x / 2.0);
			int LatSizeY = int(_TexSize.y / 2.0);
			//int LatSizeX = 400;
			//int LatSizeY = 200;
			//4 texels per voxel
			//all 4 pixels do the same computations
			int ix = int(floor(i.vertex.x/2.0));
			int iy = int(floor(i.vertex.y/2.0));


			if (ix >= LatSizeX || iy >= LatSizeY)
			{
				return float4(0.5,0.5,0.5,1);
			}

			int itx = float(fragCoord.x) - (2 * ix);
			int ity = float(fragCoord.y) - (2 * iy); 

			float f0,f1,f2,f3,f4,f5,f6,f7,f8; //distribution functions
			float rho, vx, vy; //moments
			float solid = solid(ix,iy);
			f0 = f0(ix,iy);

			if ((iFrame == 0) || (f0 == 0.0)) //initialisation
			{
				rho = 1.0;
				vx = VEL*(1.0 + 0.1 * (i.vertex.y) / _TexSize.y);
				vy = 0.0;
				float sq_term = -1.5 * (vx*vx + vy*vy);

				f0 = 4. / 9. *rho*(1. + sq_term);
				f1 = 1. / 9. *rho*(1. + 3.*vx         + 4.5 * vx*vx + sq_term);
				f2 = 1. / 9. *rho*(1. - 3.*vx         + 4.5 * vx*vx + sq_term);
				f3 = 1. / 9. *rho*(1. + 3.*vy         + 4.5 * vy*vy + sq_term);
				f4 = 1. / 9. *rho*(1. - 3.*vy         + 4.5 * vy*vy + sq_term);
				f5 = 1. / 36.*rho*(1. + 3.*(vx + vy)  + 4.5 * (vx + vy)*(vx + vy) + sq_term);
				f6 = 1. / 36.*rho*(1. - 3.*(vx + vy)  + 4.5 * (vx + vy)*(vx + vy) + sq_term);
				f7 = 1. / 36.*rho*(1. + 3.*(-vx + vy) + 4.5 * (-vx + vy)*(-vx + vy) + sq_term);
				f8 = 1. / 36.*rho*(1. - 3.*(-vx + vy) + 4.5 * (-vx + vy)*(-vx + vy) + sq_term);

				//add a small disk near the entrance
				if (distance(float2(50.0,LatSizeY / 2),float2(ix,iy)) < 10.0)
					solid = 1.0;
				else
					solid = 0.0;
			}
			else //normal time-step
			{
				//=== STREAMING STEP (PERIODIC) =======================
				int xplus = ((ix == LatSizeX - 1) ? (0) : (ix + 1));
				int xminus = ((ix == 0) ? (LatSizeX - 1) : (ix - 1));
				int yplus = ((iy == LatSizeY - 1) ? (0) : (iy + 1));
				int yminus = ((iy == 0) ? (LatSizeY - 1) : (iy - 1));
				//f0 = f0( ix    ,iy    );
				f1 = f1(xminus,iy);
				f2 = f2(xplus ,iy);
				f3 = f3(ix    ,yminus);
				f4 = f4(ix    ,yplus);
				f5 = f5(xminus,yminus);
				f6 = f6(xplus ,yplus);
				f7 = f7(xplus ,yminus);
				f8 = f8(xminus,yplus);

				//=== COMPUTE MOMENTS =================================
				//density
				rho = f0 + f1 + f2 + f3 + f4 + f5 + f6 + f7 + f8;
				//velocity
				vx = 1. / rho*(f1 - f2 + f5 - f6 - f7 + f8);
				vy = 1. / rho*(f3 - f4 + f5 - f6 + f7 - f8);

				//velocity cap for stability
				float norm = sqrt(vx*vx + vy*vy);

				if (norm>0.2)
				{
					vx *= 0.2 / norm;
					vy *= 0.2 / norm;
				}
				if (ix == 0 || ix == LatSizeX - 1)//boundary condition
				{
					rho = 1.0;
					vx = VEL;
					vy = 0.0;
					w = 1.0;
				}
				if (distance(iMouse.xy, float2(fragCoord.x, fragCoord.y) / _TexSize) < 0.01)

				{
					rho = 1.0;
					vx *= 0.9;
					vy *= 0.9;
					w = 1.0;
				}

				if (solid > 0.5)
				{
					rho = 1.0;
					vx = 0.0;
					vy = 0.0;
					w = 1.0;
				}

				float sq_term = -1.5 * (vx*vx + vy*vy);
				float f0eq = 4. / 9. *rho*(1. + sq_term);
				float f1eq = 1. / 9. *rho*(1. + 3.*vx         + 4.5 * vx*vx + sq_term);
				float f2eq = 1. / 9. *rho*(1. - 3.*vx         + 4.5 * vx*vx + sq_term);
				float f3eq = 1. / 9. *rho*(1. + 3.*vy         + 4.5 * vy*vy + sq_term);
				float f4eq = 1. / 9. *rho*(1. - 3.*vy         + 4.5 * vy*vy + sq_term);
				float f5eq = 1. / 36.*rho*(1. + 3.*(vx + vy)  + 4.5 * (vx + vy)*(vx + vy) + sq_term);
				float f6eq = 1. / 36.*rho*(1. - 3.*(vx + vy)  + 4.5 * (vx + vy)*(vx + vy) + sq_term);
				float f7eq = 1. / 36.*rho*(1. + 3.*(-vx + vy) + 4.5 * (-vx + vy)*(-vx + vy) + sq_term);
				float f8eq = 1. / 36.*rho*(1. - 3.*(-vx + vy) + 4.5 * (-vx + vy)*(-vx + vy) + sq_term);
				//=== RELAX TOWARD EQUILIBRIUM ========================
				f0 = (1. - w) * f0 + w * f0eq;
				f1 = (1. - w) * f1 + w * f1eq;
				f2 = (1. - w) * f2 + w * f2eq;
				f3 = (1. - w) * f3 + w * f3eq;
				f4 = (1. - w) * f4 + w * f4eq;
				f5 = (1. - w) * f5 + w * f5eq;
				f6 = (1. - w) * f6 + w * f6eq;
				f7 = (1. - w) * f7 + w * f7eq;
				f8 = (1. - w) * f8 + w * f8eq;
			}
			//solid -= _Time.y/1000000;
			if (itx == 0 && ity == 0)//stores f0,f1,f2
				 return  float4(f0,f1,f2, 1.0);
			else if (itx == 1 && ity == 0)//stores f3,f4,f5
				 return float4(f3,f4,f5, 1.0);
			else if (itx == 0 && ity == 1)//stores f6,f7,f8
				 return  float4(f6,f7,f8, 1.0);
			else //stores rho,vx,vy
				 return  float4(solid,vx,vy, 1.0);
			}
			ENDCG
		}
	}
}
