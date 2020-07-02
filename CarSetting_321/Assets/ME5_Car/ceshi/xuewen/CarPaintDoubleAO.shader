// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "YQ/CarPaintDoubleAO"
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
		_AO("AO", 2D) = "white" {}
		_AO2("AO2", 2D) = "white" {}
		_Spec("Spec", Float) = 1
		_MR("MR", 2D) = "white" {}
		_SP("SP", 2D) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float2 uv2_texcoord2;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _Color;
		uniform sampler2D _FlakesNormal;
		uniform float _FlakesTiling;
		uniform sampler2D _MR;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _FresnelScale;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;
		uniform sampler2D _CoatNormal;
		uniform float _CoatTiling;
		uniform sampler2D Texture0;
		uniform float _CoatBump;
		uniform sampler2D _SP;
		uniform float _Spec;
		uniform float _CoatSmoothness;
		uniform sampler2D _AO2;
		uniform float4 _AO2_ST;
		uniform float _CoatAmount;
		uniform float _FresnelMin;
		uniform float _FresnelMax;


		inline float3 TriplanarSamplingSNF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
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
			float4 ase_vertexTangent = mul( unity_WorldToObject, float4( ase_worldTangent, 0 ) );
			float3 ase_vertexBitangent = mul( unity_WorldToObject, float4( ase_worldBitangent, 0 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3x3 objectToTangent = float3x3(ase_vertexTangent.xyz, ase_vertexBitangent, ase_vertexNormal);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 triplanar320 = TriplanarSamplingSNF( _FlakesNormal, ase_vertex3Pos, ase_vertexNormal, 1.0, _FlakesTiling, 1.0, 0 );
			float3 tanTriplanarNormal320 = mul( objectToTangent, triplanar320 );
			s1.Normal = WorldNormalVector( i , tanTriplanarNormal320 );
			s1.Emission = float3( 0,0,0 );
			float4 tex2DNode353 = tex2D( _MR, i.uv2_texcoord2 );
			s1.Metallic = ( tex2DNode353 * _Metallic ).r;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV279 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode279 = ( 0.05 + _FresnelScale * pow( 1.0 - fresnelNdotV279, 5.0 ) );
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			float4 temp_output_347_0 = ( saturate( ( 1.0 - fresnelNode279 ) ) + tex2D( _AO, uv_AO ) );
			s1.Smoothness = ( ( tex2DNode353 * _Smoothness ) * temp_output_347_0 ).r;
			s1.Occlusion = temp_output_347_0.r;

			data.light = gi.light;

			UnityGI gi1 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g1 = UnityGlossyEnvironmentSetup( s1.Smoothness, data.worldViewDir, s1.Normal, float3(0,0,0));
			gi1 = UnityGlobalIllumination( data, s1.Occlusion, s1.Normal, g1 );
			#endif

			float3 surfResult1 = LightingStandard ( s1, viewDir, gi1 ).rgb;
			surfResult1 += s1.Emission;

			#ifdef UNITY_PASS_FORWARDADD//1
			surfResult1 -= s1.Emission;
			#endif//1
			SurfaceOutputStandardSpecular s166 = (SurfaceOutputStandardSpecular ) 0;
			s166.Albedo = float3( 0,0,0 );
			float3 triplanar325 = TriplanarSamplingSNF( _CoatNormal, ase_vertex3Pos, ase_vertexNormal, 1.0, _CoatTiling, 1.0, 0 );
			float3 tanTriplanarNormal325 = mul( objectToTangent, triplanar325 );
			float3 triplanar337 = TriplanarSamplingSNF( Texture0, ase_vertex3Pos, ase_vertexNormal, 1.0, _CoatTiling, 1.0, 0 );
			float3 tanTriplanarNormal337 = mul( objectToTangent, triplanar337 );
			float3 lerpResult339 = lerp( tanTriplanarNormal325 , tanTriplanarNormal337 , ( 1.0 - _CoatBump ));
			s166.Normal = WorldNormalVector( i , lerpResult339 );
			s166.Emission = float3( 0,0,0 );
			s166.Specular = ( tex2D( _SP, i.uv2_texcoord2 ) * _Spec ).rgb;
			s166.Smoothness = _CoatSmoothness;
			float2 uv_AO2 = i.uv_texcoord * _AO2_ST.xy + _AO2_ST.zw;
			s166.Occlusion = tex2D( _AO2, uv_AO2 ).r;

			data.light = gi.light;

			UnityGI gi166 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g166 = UnityGlossyEnvironmentSetup( s166.Smoothness, data.worldViewDir, s166.Normal, float3(0,0,0));
			gi166 = UnityGlobalIllumination( data, s166.Occlusion, s166.Normal, g166 );
			#endif

			float3 surfResult166 = LightingStandardSpecular ( s166, viewDir, gi166 ).rgb;
			surfResult166 += s166.Emission;

			#ifdef UNITY_PASS_FORWARDADD//166
			surfResult166 -= s166.Emission;
			#endif//166
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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv2_texcoord2;
				o.customPack1.xy = v.texcoord1;
				o.customPack1.zw = customInputData.uv_texcoord;
				o.customPack1.zw = v.texcoord;
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
				surfIN.uv2_texcoord2 = IN.customPack1.xy;
				surfIN.uv_texcoord = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
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
Version=17500
7;1;1661;1010;-647.3707;-760.1817;1.648676;True;True
Node;AmplifyShaderEditor.CommentaryNode;280;224.6482,1046.399;Inherit;False;963.3314;486.4381;Simple fresnel blend;9;343;298;296;279;47;237;344;345;342;Blend Factor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;343;246.491,1141.545;Float;False;Property;_FresnelScale;Fresnel Scale;11;0;Create;True;0;0;False;0;1;3.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;279;466.4625,1099.643;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.05;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;293;962.3416,455.6819;Inherit;False;1229.873;717.1765;Comment;10;324;329;233;219;320;216;352;353;355;356;Base Layer With Flakes;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;296;807.0915,1146.994;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;355;1081.609,1109.694;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;281;802.3861,1514.566;Inherit;False;1386.212;798.8774;This mirror layer that to mimc a coating layer;12;172;211;180;337;340;166;339;325;328;326;348;359;Coating Layer;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;326;855.1158,1666.529;Float;True;Property;_CoatNormal;Coat Normal;6;0;Create;True;0;0;False;0;None;0fb69c1cf4ff91d4abfc2da1c923c651;False;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;353;1374.026,818.7073;Inherit;True;Property;_MR;MR;17;0;Create;True;0;0;False;0;-1;None;0e566900fa09f0642be7cd7002894e16;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;233;1532.342,1042.651;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;False;0;0;0.18;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;328;835.801,1879.053;Float;False;Property;_CoatTiling;Coat Tiling;8;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;340;773.4786,2016.226;Float;True;Global;Texture0;Texture 0;10;0;Create;True;0;0;False;0;None;None;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;180;1176.527,2206.45;Float;False;Property;_CoatBump;Coat Bump;3;0;Create;True;0;0;False;0;0;0.644;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;346;1527.013,1307.5;Inherit;True;Property;_AO;AO;14;0;Create;True;0;0;False;0;-1;None;222668baa6e8ad54ab1d3ce1c0a71888;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;298;1026.498,1143.7;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;359;1531.56,2198.573;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;219;1626.92,1138.489;Float;False;Property;_Metallic;Metallic;0;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;356;1798.679,973.2368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;337;1201.968,1973.715;Inherit;True;Spherical;Object;True;Top Texture 2;_TopTexture2;bump;1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;481.5514,1335.563;Float;False;Property;_CoatAmount;Coat Amount;4;0;Create;True;0;0;False;0;0;0.285;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;1990.978,1864.817;Float;False;Property;_Spec;Spec;16;0;Create;True;0;0;False;0;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;357;1918.22,2319.694;Inherit;True;Property;_SP;SP;18;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;329;1174.13,879.7802;Float;False;Property;_FlakesTiling;Flakes Tiling;9;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;325;1212.784,1745.727;Inherit;True;Spherical;Object;True;Top Texture 1;_TopTexture1;bump;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;347;1903.537,1280.761;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;324;1089,660.2415;Float;True;Property;_FlakesNormal;Flakes Normal;7;0;Create;True;0;0;False;0;None;d92130db6df948b419dd1dec490816d0;False;bump;LockedToTexture2D;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;344;797.5479,1363.816;Float;False;Property;_FresnelMin;Fresnel Min;12;0;Create;True;0;0;False;0;0;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;345;801.5298,1447.007;Float;False;Property;_FresnelMax;Fresnel Max;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;358;2245.899,1821.667;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;237;815.6653,1248.533;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;352;1950.842,909.4724;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;339;1677.559,1822.179;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;348;1648.11,1556.672;Inherit;True;Property;_AO2;AO2;15;0;Create;True;0;0;False;0;-1;None;222668baa6e8ad54ab1d3ce1c0a71888;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;172;1701.84,1990.976;Float;False;Property;_CoatSmoothness;Coat Smoothness;5;0;Create;True;0;0;False;0;0;0.963;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;320;1796.739,700.047;Inherit;True;Spherical;Object;True;Top Texture 0;_TopTexture0;bump;0;None;Mid Texture 0;_MidTexture0;white;1;None;Bot Texture 0;_BotTexture0;white;3;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;351;1981.443,1098.417;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;216;1591.186,557.3604;Float;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;1,0.9310344,0,0;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;342;1034.819,1377.331;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;166;2071.828,1539.047;Inherit;False;Specular;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomStandardSurface;1;2241.355,952.949;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;208;2531.195,1256.441;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2797.49,996.9466;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;YQ/CarPaintDoubleAO;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;279;2;343;0
WireConnection;296;0;279;0
WireConnection;353;1;355;0
WireConnection;298;0;296;0
WireConnection;359;0;180;0
WireConnection;356;0;353;0
WireConnection;356;1;233;0
WireConnection;337;0;340;0
WireConnection;337;3;328;0
WireConnection;357;1;355;0
WireConnection;325;0;326;0
WireConnection;325;3;328;0
WireConnection;347;0;298;0
WireConnection;347;1;346;0
WireConnection;358;0;357;0
WireConnection;358;1;211;0
WireConnection;237;0;279;0
WireConnection;237;1;47;0
WireConnection;352;0;353;0
WireConnection;352;1;219;0
WireConnection;339;0;325;0
WireConnection;339;1;337;0
WireConnection;339;2;359;0
WireConnection;320;0;324;0
WireConnection;320;3;329;0
WireConnection;351;0;356;0
WireConnection;351;1;347;0
WireConnection;342;0;237;0
WireConnection;342;1;344;0
WireConnection;342;2;345;0
WireConnection;166;1;339;0
WireConnection;166;3;358;0
WireConnection;166;4;172;0
WireConnection;166;5;348;0
WireConnection;1;0;216;0
WireConnection;1;1;320;0
WireConnection;1;3;352;0
WireConnection;1;4;351;0
WireConnection;1;5;347;0
WireConnection;208;0;1;0
WireConnection;208;1;166;0
WireConnection;208;2;342;0
WireConnection;0;13;208;0
ASEEND*/
//CHKSM=E8ECEFBE52AA9E133D00B4DD76C742FBEFD65095