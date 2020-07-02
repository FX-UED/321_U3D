// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "yq/radarPlane"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HDR]_Color("Color", Color) = (1,0.4196078,0,1)
		_force("force", Range( 0 , 1)) = 0
		_Vector("Vector", Vector) = (0,0,0,0)
		_UV("UV", Vector) = (0,0,0,0)
		_AO("AO", 2D) = "white" {}
		_speed("speed", Float) = 0
		_Brightness("Brightness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		Stencil
		{
			Ref 1
			CompFront Always
			PassFront Replace
		}
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform sampler2D _MainTexture;
		uniform float2 _UV;
		uniform float _speed;
		uniform float2 _Vector;
		uniform float _Brightness;
		uniform float _force;
		uniform sampler2D _AO;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime78 = _Time.y * _speed;
			float2 panner80 = ( mulTime78 * _Vector + float2( 0,0 ));
			float2 uv_TexCoord81 = i.uv_texcoord * _UV + panner80;
			o.Emission = ( _Color * tex2D( _MainTexture, uv_TexCoord81 ) * _Brightness ).rgb;
			o.Alpha = _force;
			clip( tex2D( _AO, uv_TexCoord81 ).r - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
735;73;588;967;275.1133;927.1949;1.335328;False;False
Node;AmplifyShaderEditor.RangedFloatNode;88;-900.692,7.062858;Float;False;Property;_speed;speed;12;0;Create;True;0;0;False;0;0;0.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;78;-759.2944,10.95327;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;79;-802.8462,-164.9626;Float;False;Property;_Vector;Vector;7;0;Create;True;0;0;False;0;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;85;-461.7357,-336.9622;Float;False;Property;_UV;UV;9;0;Create;True;0;0;False;0;0,0;1,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;80;-567.2685,-120.9787;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-269.9366,-171.7067;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;83;-21.63262,-391.9856;Float;False;Property;_Color;Color;3;1;[HDR];Create;True;0;0;False;0;1,0.4196078,0,1;0,61.48964,78.79325,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;99;-1.371133,-529.2673;Float;False;Property;_Brightness;Brightness;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-37.60295,-193.5265;Float;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;None;45c43a63eb1110f409d4bfdf6cbbd850;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-569.67,437.7879;Float;False;Property;_XRayScale;XRayScale;6;0;Create;True;0;0;False;0;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-556.5536,332.5812;Float;False;Property;_XRayBias;XRayBias;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-518.2053,512.8088;Float;False;Property;_XRayPower;XRayPower;2;0;Create;True;0;0;False;0;0;1.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;87;77.4285,119.2851;Float;True;Property;_AO;AO;11;0;Create;True;0;0;False;0;None;45c43a63eb1110f409d4bfdf6cbbd850;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;86;147.9481,12.65198;Float;False;Property;_force;force;5;0;Create;True;0;0;False;0;0;0.78;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;39.4459,580.5815;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-8.272298,473.3741;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-150.5542,865.5815;Float;False;Property;_XRayIntensity;XRayIntensity;10;0;Create;True;0;0;False;0;0;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;98;238.7041,486.4183;Float;False;0;True;Transparent;2;7;Back;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;282.0543,-249.8113;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;93;-331.9277,382.8565;Float;True;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;92;-438.3348,646.0192;Float;False;Property;_XRayColor;XRayColor;4;0;Create;True;0;0;False;0;0,0,0,0;1,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;95;-110.5541,734.5815;Float;False;FLOAT;3;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;596.1226,-305.096;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;yq/radarPlane;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;1;False;-1;1;False;-1;0;False;0;1,0.4344827,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;78;0;88;0
WireConnection;80;2;79;0
WireConnection;80;1;78;0
WireConnection;81;0;85;0
WireConnection;81;1;80;0
WireConnection;82;1;81;0
WireConnection;87;1;81;0
WireConnection;96;0;93;0
WireConnection;96;1;95;0
WireConnection;96;2;94;0
WireConnection;97;0;93;0
WireConnection;97;1;92;0
WireConnection;98;0;97;0
WireConnection;98;2;96;0
WireConnection;84;0;83;0
WireConnection;84;1;82;0
WireConnection;84;2;99;0
WireConnection;93;1;91;0
WireConnection;93;2;90;0
WireConnection;93;3;89;0
WireConnection;95;0;92;0
WireConnection;0;2;84;0
WireConnection;0;9;86;0
WireConnection;0;10;87;0
ASEEND*/
//CHKSM=32579EE33EAA679BBEBE33A04BC475D83410D954