Shader "Unlit/DrawCircle"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Radius("Radius", Range(0, 1)) = 0.15
		_Radius2("Radius2", Range(0, 1)) = 0.25
		_Radius3("Radius3", Range(0, 1)) = 0.35
		_Thickness("Thickness", Range(0, 1)) = 0.02
		_Angle("Angle", Range(0, 1)) = 0
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "IngnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 200

			Pass
			{
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

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
			float _Radius, _Thickness, _Angle;
			fixed4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				float alpha = 1.0f;
				fixed4 col = tex2D(_MainTex, i.uv)* _Color;
				float dis = (i.uv.x - 0.5)*(i.uv.x - 0.5) + (i.uv.y - 0.5)*(i.uv.y - 0.5);
				if (dis<_Radius || dis>_Radius + _Thickness)
				{
					col.a = 0.0;
					clip(-1);
				}
				else {
					float angletemp = atan2(i.uv.y - 0.5, i.uv.x - 0.5);
					if (angletemp < (_Angle - 0.5)*3.1415926 * 2)
					{
						col.a = 0.0;
						clip(-1);
					}
					else {
						col.a = (_Thickness*0.5 - abs(dis - _Radius - _Thickness * 0.5)) / 0.008;
					}
				}
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

			Pass
			{
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

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
				float _Radius2, _Thickness, _Angle;
				fixed4 _Color;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					float alpha = 1.0f;
					fixed4 col = tex2D(_MainTex, i.uv)* _Color;
					float dis = (i.uv.x - 0.5)*(i.uv.x - 0.5) + (i.uv.y - 0.5)*(i.uv.y - 0.5);
					if (dis<_Radius2 || dis>_Radius2 + _Thickness)
					{
						col.a = 0.0;
						clip(-1);
					}
					else {
						float angletemp = atan2(i.uv.y - 0.5, i.uv.x - 0.5);
						if (angletemp < (_Angle - 0.5)*3.1415926 * 2)
						{
							col.a = 0.0;
							clip(-1);
						}
						else {
							col.a = (_Thickness*0.5 - abs(dis - _Radius2 - _Thickness * 0.5)) / 0.008;
						}
					}
					// apply fog
					//UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}

			Pass
			{
					ZWrite Off
					Blend SrcAlpha OneMinusSrcAlpha

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
					float _Radius3, _Thickness, _Angle;
					fixed4 _Color;
					v2f vert(appdata v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.uv, _MainTex);
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						// sample the texture
						float alpha = 1.0f;
						fixed4 col = tex2D(_MainTex, i.uv)* _Color;
						float dis = (i.uv.x - 0.5)*(i.uv.x - 0.5) + (i.uv.y - 0.5)*(i.uv.y - 0.5);
						if (dis<_Radius3 || dis>_Radius3 + _Thickness)
						{
							col.a = 0.0;
							clip(-1);
						}
						else {
							float angletemp = atan2(i.uv.y - 0.5, i.uv.x - 0.5);
							if (angletemp < (_Angle - 0.5)*3.1415926 * 2)
							{
								col.a = 0.0;
								clip(-1);
							}
							else {
								col.a = (_Thickness*0.5 - abs(dis - _Radius3 - _Thickness * 0.5)) / 0.008;
							}
						}
						// apply fog
						//UNITY_APPLY_FOG(i.fogCoord, col);
						return col;
					}
					ENDCG
				}
		}

}
