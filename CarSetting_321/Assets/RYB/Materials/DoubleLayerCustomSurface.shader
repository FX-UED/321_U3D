// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DoubleLayerCustomSurface"
{
	Properties
	{
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Color("Color", Color) = (1,0.9310344,0,0)
		_CoatBump("Coat Bump", Range( 0 , 1)) = 0
		_CoatAmount("Coat Amount", Range( 0 , 1)) = 0
		_CoatSmoothness("Coat Smoothness", Range( 0 , 1)) = 0
		_CoatNormal("Coat Normal", 2D) = "bump" {}
		_FlakesNormal("Flakes Normal", 2D) = "bump" {}
		_CoatTiling("Coat Tiling", Float) = 0
		_FlakesTiling("Flakes Tiling", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelMin("Fresnel Min", Float) = 0
		_FresnelMax("Fresnel Max", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _Color;
		uniform sampler2D _FlakesNormal;
		uniform float _FlakesTiling;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _FresnelScale;
		uniform sampler2D _CoatNormal;
		uniform float _CoatTiling;
		uniform sampler2D Texture0;
		uniform float _CoatBump;
		uniform float _CoatSmoothness;
		uniform float _CoatAmount;
		uniform float _FresnelMin;
		uniform float _FresnelMax;


		inline float3 TriplanarSamplingSNF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= projNormal.x + projNormal.y + projNormal.z;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( topTexMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( topTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( topTexMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			xNorm.xyz = half3( UnpackNormal( xNorm ).xy * float2( nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz = half3( UnpackNormal( yNorm ).xy * float2( nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz = half3( UnpackNormal( zNorm ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			SurfaceOutputStandard s1 = (SurfaceOutputStandard ) 0;
			s1.Albedo = _Color.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 localTangent = mul( unity_WorldToObject, float4( ase_worldTangent, 0 ) );
			float3 localBitangent = mul( unity_WorldToObject, float4( ase_worldBitangent, 0 ) );
			float3 localNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3x3 objectToTangent = float3x3(localTangent, localBitangent, localNormal);
			float3 localPos = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) );
			float3 triplanar320 = TriplanarSamplingSNF( _FlakesNormal, localPos, localNormal, 1.0, _FlakesTiling, 0 );
			float3 tanTriplanarNormal320 = mul( objectToTangent, triplanar320 );
			s1.Normal = WorldNormalVector( i, tanTriplanarNormal320);
			s1.Emission = float3( 0,0,0 );
			s1.Metallic = _Metallic;
			s1.Smoothness = _Smoothness;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNDotV279 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode279 = ( 0.05 + _FresnelScale * pow( 1.0 - fresnelNDotV279, 5.0 ) );
			s1.Occlusion = saturate( ( 1.0 - fresnelNode279 ) );

			data.light = gi.light;

			UnityGI gi1 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g1 = UnityGlossyEnvironmentSetup( s1.Smoothness, data.worldViewDir, s1.Normal, float3(0,0,0));
			gi1 = UnityGlobalIllumination( data, s1.Occlusion, s1.Normal, g1 );
			#endif

			float3 surfResult1 = LightingStandard ( s1, viewDir, gi1 ).rgb;
			surfResult1 += s1.Emission;

			SurfaceOutputStandardSpecular s166 = (SurfaceOutputStandardSpecular ) 0;
			s166.Albedo = float3( 0,0,0 );
			float3 triplanar325 = TriplanarSamplingSNF( _CoatNormal, localPos, localNormal, 1.0, _CoatTiling, 0 );
			float3 tanTriplanarNormal325 = mul( objectToTangent, triplanar325 );
			float3 triplanar337 = TriplanarSamplingSNF( Texture0, localPos, localNormal, 1.0, _CoatTiling, 0 );
			float3 tanTriplanarNormal337 = mul( objectToTangent, triplanar337 );
			float3 lerpResult339 = lerp( tanTriplanarNormal325 , tanTriplanarNormal337 , _CoatBump);
			s166.Normal = WorldNormalVector( i, lerpResult339);
			s166.Emission = float3( 0,0,0 );
			float3 temp_cast_1 = (1.0).xxx;
			s166.Specular = temp_cast_1;
			s166.Smoothness = _CoatSmoothness;
			s166.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi166 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g166 = UnityGlossyEnvironmentSetup( s166.Smoothness, data.worldViewDir, s166.Normal, float3(0,0,0));
			gi166 = UnityGlobalIllumination( data, s166.Occlusion, s166.Normal, g166 );
			#endif

			float3 surfResult166 = LightingStandardSpecular ( s166, viewDir, gi166 ).rgb;
			surfResult166 += s166.Emission;

			float clampResult342 = clamp( ( fresnelNode279 * _CoatAmount ) , _FresnelMin , _FresnelMax );
			float3 lerpResult208 = lerp( surfResult1 , surfResult166 , clampResult342);
			c.rgb = lerpResult208;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14301
230;380;1621;784;1175.244;12.76324;3.317798;True;True
Node;AmplifyShaderEditor.CommentaryNode;280;-204.687,1211.783;Float;False;963.3314;486.4381;Simple fresnel blend;9;343;298;296;279;47;237;344;345;342;Blend Factor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;343;-182.8443,1306.929;Float;False;Property;_FresnelScale;Fresnel Scale;11;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;281;811.5423,1734.861;Float;False;1386.212;798.8774;This mirror layer that to mimc a coating layer;10;172;211;180;337;340;166;339;325;328;326;Coating Layer;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;293;962.3416,455.6819;Float;False;1229.873;717.1765;Comment;7;324;329;233;219;320;216;1;Base Layer With Flakes;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;326;974.6612,1935.627;Float;True;Property;_CoatNormal;Coat Normal;6;0;Create;True;None;c07805f3d07dc744c80d0a671b76ac9a;True;bump;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;340;980.9832,2236.523;Float;True;Global;Texture0;Texture 0;10;0;Create;True;None;None;True;bump;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;328;986.8218,2140.071;Float;False;Property;_CoatTiling;Coat Tiling;8;0;Create;True;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;279;37.12727,1265.027;Float;True;World;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.05;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;337;1258.414,2188.758;Float;True;Spherical;Object;True;Top Texture 2;_TopTexture2;bump;1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1.0;False;1;SAMPLER2D;;False;6;FLOAT;0.0;False;2;SAMPLER2D;;False;7;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;324;972.783,586.4591;Float;True;Property;_FlakesNormal;Flakes Normal;7;0;Create;True;None;82f612afe20927641a0d814d3d0f7b7b;True;bump;LockedToTexture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;329;1174.13,879.7802;Float;False;Property;_FlakesTiling;Flakes Tiling;9;0;Create;True;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;52.21623,1500.947;Float;False;Property;_CoatAmount;Coat Amount;4;0;Create;True;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;296;377.7553,1312.378;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;1320.475,2396.534;Float;False;Property;_CoatBump;Coat Bump;3;0;Create;True;0;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;325;1259.125,1986.938;Float;True;Spherical;Object;True;Top Texture 1;_TopTexture1;bump;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1.0;False;1;SAMPLER2D;;False;6;FLOAT;0.0;False;2;SAMPLER2D;;False;7;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;211;1709.035,2127.2;Float;False;Constant;_Spec;Spec;18;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;298;597.1613,1309.084;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;1561.506,1073.432;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0.549;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;320;1492.739,772.047;Float;True;Spherical;Object;True;Top Texture 0;_TopTexture0;bump;0;None;Mid Texture 0;_MidTexture0;white;1;None;Bot Texture 0;_BotTexture0;white;3;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1.0;False;1;SAMPLER2D;;False;6;FLOAT;0.0;False;2;SAMPLER2D;;False;7;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;172;1710.997,2211.273;Float;False;Property;_CoatSmoothness;Coat Smoothness;5;0;Create;True;0;0.95;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;237;386.3292,1413.917;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;216;1591.186,557.3604;Float;False;Property;_Color;Color;2;0;Create;True;1,0.9310344,0,0;0.232,0.058,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;339;1693.931,1983.309;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;344;368.2117,1529.2;Float;False;Property;_FresnelMin;Fresnel Min;12;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;345;372.1937,1612.391;Float;False;Property;_FresnelMax;Fresnel Max;13;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;219;1549.775,969.1467;Float;False;Property;_Metallic;Metallic;0;0;Create;True;0;0.372;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;342;605.4821,1542.715;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;166;1939.917,1819.001;Float;False;Specular;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0.0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;1.0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomStandardSurface;1;1935.582,919.9841;Float;False;Metallic;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;1.0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;208;2426.687,1263.925;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2753.206,1085.791;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;DoubleLayerCustomSurface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0.0,0,0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;279;2;343;0
WireConnection;337;0;340;0
WireConnection;337;3;328;0
WireConnection;296;0;279;0
WireConnection;325;0;326;0
WireConnection;325;3;328;0
WireConnection;298;0;296;0
WireConnection;320;0;324;0
WireConnection;320;3;329;0
WireConnection;237;0;279;0
WireConnection;237;1;47;0
WireConnection;339;0;325;0
WireConnection;339;1;337;0
WireConnection;339;2;180;0
WireConnection;342;0;237;0
WireConnection;342;1;344;0
WireConnection;342;2;345;0
WireConnection;166;1;339;0
WireConnection;166;3;211;0
WireConnection;166;4;172;0
WireConnection;1;0;216;0
WireConnection;1;1;320;0
WireConnection;1;3;219;0
WireConnection;1;4;233;0
WireConnection;1;5;298;0
WireConnection;208;0;1;0
WireConnection;208;1;166;0
WireConnection;208;2;342;0
WireConnection;0;13;208;0
ASEEND*/
//CHKSM=FC61C325614E951C6097833F11876557FE5BBB2B