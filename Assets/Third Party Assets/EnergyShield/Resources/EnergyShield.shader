
/*
EnergyShield.shader
Version 8
EnergyShield
*/



Shader "Custom/EnergyShield"
{
	Properties
	{

		_MainColor("MainColor", Color) = (1,1,1,1) //a color to tint the shield
        _CutOff("CutOff", Range(1, 0)) = 0 //cut off number
		_MainTex ("Texture", 2D) = "white" {} //a texture for distorting the objects inside
		_Fresnel("Fresnel Intensity", Range(1,100)) = 3.0 //intensity of color tint
		_FresnelWidth("Fresnel Width", Range(0,2)) = 3.0 //width of color tint
		_Distort("Distort", Range(0, 500)) = 1.0 //how much distortion
		_IntersectionThreshold("Highlight of intersection threshold", range(0,0.25)) = 0.01 //Max difference for intersections
		_ScrollSpeedU("Scroll U Speed",float) = 2 //used to animate the distortion
		_ScrollSpeedV("Scroll V Speed",float) = 0 //used to animate the distortion

		//[ToggleOff]_CullOff("Cull Front Side Intersection",float) = 1 //render the front of the shield or not

		//0 = render both sides
		//1 = render back side
		//2 = render front side (default)
		_CullOff("Cull Front Side Intersection",float) = 2 

		_NormalIncrease("_NormalIncrease",Range(0,0.25)) = 0.05 //allows to increase the radius of the shield


		[Space]
		/*
		the rest of the properties are collision point data
		*/
		[Space]

		_CollisionPoint00("_CollisionPoint00", Vector) = (0., 0., 0., 1.0)
		_Radius00("_Radius00", Float) = 0.0
		_FadePower00("_FadePower00", Float) = 2.0
		_inThickness00("_inThickness00", Float) = 1
		_outThickness00("_outThickness00", Float) = 1
		_Color00("_Color00", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint01("_CollisionPoint01", Vector) = (0., 0., 0., 1.0)
		_Radius01("_Radius01", Float) = 0.0
		_FadePower01("_FadePower01", Float) = 2.0
		_inThickness01("_inThickness01", Float) = 1
		_outThickness01("_outThickness01", Float) = 1
		_Color01("_Color01", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint02("_CollisionPoint02", Vector) = (0., 0., 0., 1.0)
		_Radius02("_Radius02", Float) = 0.0
		_FadePower02("_FadePower02", Float) = 2.0
		_inThickness02("_inThickness02", Float) = 1
		_outThickness02("_outThickness02", Float) = 1
		_Color02("_Color02", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint03("_CollisionPoint03", Vector) = (0., 0., 0., 1.0)
		_Radius03("_Radius03", Float) = 0.0
		_FadePower03("_FadePower03", Float) = 2.0
		_inThickness03("_inThickness03", Float) = 1
		_outThickness03("_outThickness03", Float) = 1
		_Color03("_Color03", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint04("_CollisionPoint04", Vector) = (0., 0., 0., 1.0)
		_Radius04("_Radius04", Float) = 0.0
		_FadePower04("_FadePower04", Float) = 2.0
		_inThickness04("_inThickness04", Float) = 1
		_outThickness04("_outThickness04", Float) = 1
		_Color04("_Color04", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint05("_CollisionPoint05", Vector) = (0., 0., 0., 1.0)
		_Radius05("_Radius05", Float) = 0.0
		_FadePower05("_FadePower05", Float) = 2.0
		_inThickness05("_inThickness05", Float) = 1
		_outThickness05("_outThickness05", Float) = 1
		_Color05("_Color05", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint06("_CollisionPoint06", Vector) = (0., 0., 0., 1.0)
		_Radius06("_Radius06", Float) = 0.0
		_FadePower06("_FadePower06", Float) = 2.0
		_inThickness06("_inThickness06", Float) = 1
		_outThickness06("_outThickness06", Float) = 1
		_Color06("_Color06", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint07("_CollisionPoint07", Vector) = (0., 0., 0., 1.0)
		_Radius07("_Radius07", Float) = 0.0
		_FadePower07("_FadePower07", Float) = 2.0
		_inThickness07("_inThickness07", Float) = 1
		_outThickness07("_outThickness07", Float) = 1
		_Color07("_Color07", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint08("_CollisionPoint08", Vector) = (0., 0., 0., 1.0)
		_Radius08("_Radius08", Float) = 0.0
		_FadePower08("_FadePower08", Float) = 2.0
		_inThickness08("_inThickness08", Float) = 1
		_outThickness08("_outThickness08", Float) = 1
		_Color08("_Color08", Color) = (0.0, 0.0, 0.0, 1.0)

		_CollisionPoint09("_CollisionPoint09", Vector) = (0., 0., 0., 1.0)
		_Radius09("_Radius09", Float) = 0.0
		_FadePower09("_FadePower09", Float) = 2.0
		_inThickness09("_inThickness09", Float) = 1
		_outThickness09("_outThickness09", Float) = 1
		_Color09("_Color09", Color) = (0.0, 0.0, 0.0, 1.0)

	}
	SubShader
	{ 
		//Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		//Start InterectionEffect
		Pass
		{


			Blend One One
			Cull [_CullOff] Lighting Off ZWrite [_CullOff]
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


            struct appdata
            {
                fixed4 vertex : POSITION;
                fixed4 screenPos: TEXCOORD1;
                fixed3 uv : TEXCOORD0;
            };

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed4 screenPos: TEXCOORD1;
                fixed2 uv : TEXCOORD0;
			};

			sampler2D _CameraDepthTexture;
			fixed _IntersectionThreshold,_Fresnel;
			fixed4 _MainColor;

            uniform sampler2D _MainTex;
            uniform fixed4 _MainTex_ST;
            uniform float _CutOff;

			v2f vert (appdata v)//(appdata_base v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.screenPos.z = -UnityObjectToViewPos(v.vertex.xyz).z;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				UNITY_TRANSFER_DEPTH(o.screenPos.z);// eye space depth of the vertex 
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//back intersection
				fixed zBuffer = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
				fixed intersect = (abs(zBuffer - i.screenPos.z)) / _IntersectionThreshold;
				intersect *= 0.25;

                fixed4 m = tex2D(_MainTex, i.uv);
                float co = (m.a > _CutOff)? 1:0 ;



				_MainColor.rgb *= 1 - saturate(intersect) ;
				//_MainColor.rgb += 1 - saturate(intersect) ;
				return _MainColor * _Fresnel * .01 * co;
				
			}

			
			ENDCG

			
		}
		//End InterectionEffect

		//Start DistortionEffect
		GrabPass{ "_GrabTexture" }
		Pass
		{

			//Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

			//Lighting Off ZWrite Off
			Blend One One
			Cull [_CullOff] Lighting Off ZWrite [_CullOff]

            Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				fixed4 vertex : POSITION;
				fixed3 normal: NORMAL;
				fixed3 uv : TEXCOORD0;
			};

			struct v2f
			{
				fixed2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
				//fixed4 worldPos : TEXCOORD1;
				fixed3 rimColor :TEXCOORD2;
				fixed4 screenPos: TEXCOORD3;
				float4 position_in_world_space : TEXCOORD5;
			};
            
			sampler2D _MainTex, _CameraDepthTexture, _GrabTexture;
			fixed4 _MainTex_ST,_MainColor,_GrabTexture_ST, _GrabTexture_TexelSize;
			fixed _Fresnel, _FresnelWidth, _Distort, _IntersectionThreshold, _ScrollSpeedU, _ScrollSpeedV;
			fixed _NormalIncrease;

            uniform float _CutOff;
            
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				//increase normals
				o.vertex = UnityObjectToClipPos(v.vertex + v.normal * _NormalIncrease);
				o.position_in_world_space = mul(unity_ObjectToWorld, v.vertex);

				//scroll uv
				o.uv.x += _Time * _ScrollSpeedU;
				o.uv.y += _Time * _ScrollSpeedV;

				//fresnel 
				fixed3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				fixed dotProduct = 1 - saturate(dot(v.normal, viewDir));
				o.rimColor = smoothstep(1 - _FresnelWidth, 1.0, dotProduct) * 0.25f;
				o.screenPos = ComputeScreenPos(o.vertex);

				//o.screenPos.z = pow(o.screenPos.z,0.95);

				COMPUTE_EYEDEPTH(o.screenPos.z);//eye space depth of the vertex 

				return o;
			}


			
			fixed4 frag (v2f i) : SV_Target
			{
				//intersection
				fixed zBuffer = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
				fixed intersect = (abs(zBuffer - i.screenPos.z)) / _IntersectionThreshold;

				fixed4 main = tex2D(_MainTex, i.uv);
                

                //float co = (main.a > _CutOff)? 1:0 ;
				float co = ( main.a - _CutOff) * 10;


				/*
				if (distance(main, 0) <= 0) //discard black texture
                {
                    discard;
                }	
				*/

                main *= 2;
                main -= 1;
				


				//distortion
				fixed2 offset = main * _Distort * _GrabTexture_TexelSize.xy;
				//offset.x -= _Distort/1500; //equalize distortion

				i.screenPos.xy = offset + i.screenPos.xy;
				fixed3 distortColor = tex2Dproj(_GrabTexture, i.screenPos);
				distortColor *= _MainColor * _MainColor.a + 1;

				//intersect hightlight
				fixed3 col = main * _MainColor * pow(_Fresnel,i.rimColor) ;
    			
				//lerp distort color & fresnel color
				col = lerp(distortColor, col, i.rimColor.r);
				col += saturate(1 - intersect) * _MainColor * _Fresnel * .02;
    
                
				return fixed4(col,co);

                //return fixed4(1,1,1,1);
			}
			ENDCG
		}
		//End DistortionEffect

		//Start CollisionEffect
		Pass
		{

			//Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

			Lighting Off ZWrite Off

			//Blend One One
			Cull[_CullOff] Lighting Off ZWrite[_CullOff]

			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 
			#include "UnityCG.cginc" 

            //*
            uniform sampler2D _MainTex;
            uniform fixed4 _MainTex_ST;
            uniform float _CutOff;
            fixed _ScrollSpeedU, _ScrollSpeedV;

			uniform float _NormalIncrease;

			uniform float4 _CollisionPoint00;
			uniform float _Radius00;
			uniform float _FadePower00;
			uniform float _inThickness00;
			uniform float _outThickness00;
			uniform float4 _Color00;

			uniform float4 _CollisionPoint01;
			uniform float _Radius01;
			uniform float _FadePower01;
			uniform float _inThickness01;
			uniform float _outThickness01;
			uniform float4 _Color01;

			uniform float4 _CollisionPoint02;
			uniform float _Radius02;
			uniform float _FadePower02;
			uniform float _inThickness02;
			uniform float _outThickness02;
			uniform float4 _Color02;

			uniform float4 _CollisionPoint03;
			uniform float _Radius03;
			uniform float _FadePower03;
			uniform float _inThickness03;
			uniform float _outThickness03;
			uniform float4 _Color03;

			uniform float4 _CollisionPoint04;
			uniform float _Radius04;
			uniform float _FadePower04;
			uniform float _inThickness04;
			uniform float _outThickness04;
			uniform float4 _Color04;

			uniform float4 _CollisionPoint05;
			uniform float _Radius05;
			uniform float _FadePower05;
			uniform float _inThickness05;
			uniform float _outThickness05;
			uniform float4 _Color05;

			uniform float4 _CollisionPoint06;
			uniform float _Radius06;
			uniform float _FadePower06;
			uniform float _inThickness06;
			uniform float _outThickness06;
			uniform float4 _Color06;

			uniform float4 _CollisionPoint07;
			uniform float _Radius07;
			uniform float _FadePower07;
			uniform float _inThickness07;
			uniform float _outThickness07;
			uniform float4 _Color07;

			uniform float4 _CollisionPoint08;
			uniform float _Radius08;
			uniform float _FadePower08;
			uniform float _inThickness08;
			uniform float _outThickness08;
			uniform float4 _Color08;

			uniform float4 _CollisionPoint09;
			uniform float _Radius09;
			uniform float _FadePower09;
			uniform float _inThickness09;
			uniform float _outThickness09;
			uniform float4 _Color09;


			struct vertexInput
			{

				float4 vertex : POSITION;
				float3 normal : NORMAL;
                fixed2 uv : TEXCOORD0;

			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
                fixed2 uv : TEXCOORD1;
			};


			vertexOutput vert(vertexInput input)
			{

				//vertexOutput output; 
				//output.pos =  UnityObjectToClipPos(input.vertex);
				//output.position_in_world_space = 
				//mul(unity_ObjectToWorld, input.vertex);
				//return output;

				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex + input.normal * _NormalIncrease);
				output.position_in_world_space = mul(unity_ObjectToWorld, input.vertex);


                output.uv = TRANSFORM_TEX(input.uv, _MainTex);

                output.uv.x += _Time * _ScrollSpeedU;
                output.uv.y += _Time * _ScrollSpeedV;

				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{

                //*
                fixed4 m = tex2D(_MainTex, input.uv);


                
				//float co = (m.a > _CutOff)? 1:0 ;
				float co = ( m.a - _CutOff) * 10;

				float4 FinalColor = float4(0.0,0.0,0.0,0.0);

				float dist = 0;
				float diff = 0;
				float ia = 0;
				float oa = 0;
				float a = 0;

				float r = 0;
				float g = 0;
				float b = 0;

				dist = distance(input.position_in_world_space,_CollisionPoint00);
				diff = dist - _Radius00;

				ia = ((pow(-diff,_FadePower00)) / -_inThickness00) + 1;
				oa = ((pow(diff,_FadePower00)) / -_outThickness00) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color00.a *= a;
				_Color00.a = clamp(_Color00.a,0,1 * co);
				FinalColor = _Color00;

				dist = distance(input.position_in_world_space,_CollisionPoint01);
				diff = dist - _Radius01;

				ia = ((pow(-diff,_FadePower01)) / -_inThickness01) + 1;
				oa = ((pow(diff,_FadePower01)) / -_outThickness01) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color01.a *= a;
				_Color01.a = clamp(_Color01.a,0,1 * co);

				if (_Color01.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color01,_Color01.a);
				}


				dist = distance(input.position_in_world_space,_CollisionPoint02);
				diff = dist - _Radius02;

				ia = ((pow(-diff,_FadePower02)) / -_inThickness02) + 1;
				oa = ((pow(diff,_FadePower02)) / -_outThickness02) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color02.a *= a;
				_Color02.a = clamp(_Color02.a,0,1* co);

				if (_Color02.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color02,_Color02.a);
				}


				dist = distance(input.position_in_world_space,_CollisionPoint03);
				diff = dist - _Radius03;

				ia = ((pow(-diff,_FadePower03)) / -_inThickness03) + 1;
				oa = ((pow(diff,_FadePower03)) / -_outThickness03) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color03.a *= a;
				_Color03.a = clamp(_Color03.a,0,1* co);

				if (_Color03.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color03,_Color03.a);
				}


				dist = distance(input.position_in_world_space,_CollisionPoint04);
				diff = dist - _Radius04;

				ia = ((pow(-diff,_FadePower04)) / -_inThickness04) + 1;
				oa = ((pow(diff,_FadePower04)) / -_outThickness04) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color04.a *= a;
				_Color04.a = clamp(_Color04.a,0,1* co);

				if (_Color04.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color04,_Color04.a);
				}

				dist = distance(input.position_in_world_space,_CollisionPoint05);
				diff = dist - _Radius05;

				ia = ((pow(-diff,_FadePower05)) / -_inThickness05) + 1;
				oa = ((pow(diff,_FadePower05)) / -_outThickness05) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color05.a *= a;
				_Color05.a = clamp(_Color05.a,0,1* co);

				if (_Color05.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color05,_Color05.a);
				}

				dist = distance(input.position_in_world_space,_CollisionPoint06);
				diff = dist - _Radius06;

				ia = ((pow(-diff,_FadePower06)) / -_inThickness06) + 1;
				oa = ((pow(diff,_FadePower06)) / -_outThickness06) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color06.a *= a;
				_Color06.a = clamp(_Color06.a,0,1* co);

				if (_Color06.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color06,_Color06.a);
				}

				dist = distance(input.position_in_world_space,_CollisionPoint07);
				diff = dist - _Radius07;

				ia = ((pow(-diff,_FadePower07)) / -_inThickness07) + 1;
				oa = ((pow(diff,_FadePower07)) / -_outThickness07) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color07.a *= a;
				_Color07.a = clamp(_Color07.a,0,1* co);

				if (_Color07.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color07,_Color07.a);
				}

				dist = distance(input.position_in_world_space,_CollisionPoint08);
				diff = dist - _Radius08;

				ia = ((pow(-diff,_FadePower08)) / -_inThickness08) + 1;
				oa = ((pow(diff,_FadePower08)) / -_outThickness08) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color08.a *= a;
				_Color08.a = clamp(_Color08.a,0,1* co);

				if (_Color08.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color08,_Color08.a);
				}

				dist = distance(input.position_in_world_space,_CollisionPoint09);
				diff = dist - _Radius09;

				ia = ((pow(-diff,_FadePower09)) / -_inThickness09) + 1;
				oa = ((pow(diff,_FadePower09)) / -_outThickness09) + 1;

				if (diff > 0)
				{
					a = oa;
				}
				else
				{
					a = ia;
				}

				_Color09.a *= a;
				_Color09.a = clamp(_Color09.a,0,1* co);

				if (_Color09.a > 0)
				{
					FinalColor = lerp(FinalColor,_Color09,_Color09.a);
				}



				return FinalColor;


		}

			ENDCG
		}

		
		//End CollisionEffect
	}
}