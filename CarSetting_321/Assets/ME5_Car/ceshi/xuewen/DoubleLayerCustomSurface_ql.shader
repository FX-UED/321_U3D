// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DoubleLayerCustomSurface"
{
	Properties
	{
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Float11("Float 11", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Float6("Float 6", Range( 0 , 1)) = 0
		_Color("Color", Color) = (1,0.9310344,0,0)
		_Color0("Color 0", Color) = (1,0.9310344,0,0)
		_CoatBump("Coat Bump", Range( 0 , 1)) = 0
		_CoatBump2("Coat Bump2", Range( 0 , 1)) = 0
		_Float3("Float 3", Range( 0 , 1)) = 0
		_CoatAmount("Coat Amount", Range( 0 , 1)) = 0
		_CoatSmoothness2("Coat Smoothness2", Range( 0 , 1)) = 0
		_CoatSmoothness("Coat Smoothness", Range( 0 , 1)) = 0
		_CoatNormal("Coat Normal", 2D) = "bump" {}
		_Texture1("Texture 1", 2D) = "bump" {}
		_FlakesNormal("Flakes Normal", 2D) = "bump" {}
		_Texture2("Texture 2", 2D) = "bump" {}
		_CoatTiling("Coat Tiling", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float5("Float 5", Float) = 0
		_FlakesTiling("Flakes Tiling", Float) = 0
		_Float0("Float 0", Float) = 1
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelMin("Fresnel Min", Float) = 0
		_Float8("Float 8", Float) = 0
		_FresnelMax("Fresnel Max", Float) = 1
		_Float10("Float 10", Float) = 1
		_Metallic_r03("Metallic_r03", 2D) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
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
		uniform sampler2D _Metallic_r03;
		uniform float4 _Metallic_r03_ST;
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
		uniform float4 _Color0;
		uniform sampler2D _Texture2;
		uniform float _Float5;
		uniform float _Float11;
		uniform float _Float6;
		uniform float _Float0;
		uniform sampler2D _Texture1;
		uniform float _Float1;
		uniform sampler2D _Texture0;
		uniform float _CoatBump2;
		uniform float _CoatSmoothness2;
		uniform float _Float3;
		uniform float _Float8;
		uniform float _Float10;


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
			float2 uv2_Metallic_r03 = i.uv2_texcoord2 * _Metallic_r03_ST.xy + _Metallic_r03_ST.zw;
			float4 tex2DNode392 = tex2D( _Metallic_r03, uv2_Metallic_r03 );
			s1.Metallic = ( tex2DNode392 * _Metallic ).r;
			s1.Smoothness = ( tex2DNode392 * _Smoothness ).r;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV279 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode279 = ( 0.05 + _FresnelScale * pow( 1.0 - fresnelNdotV279, 5.0 ) );
			s1.Occlusion = saturate( ( 1.0 - fresnelNode279 ) );

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
			float3 lerpResult339 = lerp( tanTriplanarNormal325 , tanTriplanarNormal337 , _CoatBump);
			s166.Normal = WorldNormalVector( i , lerpResult339 );
			s166.Emission = float3( 0,0,0 );
			float3 temp_cast_3 = (1.0).xxx;
			s166.Specular = temp_cast_3;
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

			#ifdef UNITY_PASS_FORWARDADD//166
			surfResult166 -= s166.Emission;
			#endif//166
			float clampResult342 = clamp( ( fresnelNode279 * _CoatAmount ) , _FresnelMin , _FresnelMax );
			float3 lerpResult208 = lerp( surfResult1 , surfResult166 , clampResult342);
			SurfaceOutputStandard s388 = (SurfaceOutputStandard ) 0;
			s388.Albedo = _Color0.rgb;
			float3 triplanar378 = TriplanarSamplingSNF( _Texture2, ase_vertex3Pos, ase_vertexNormal, 1.0, _Float5, 1.0, 0 );
			float3 tanTriplanarNormal378 = mul( objectToTangent, triplanar378 );
			s388.Normal = WorldNormalVector( i , tanTriplanarNormal378 );
			s388.Emission = float3( 0,0,0 );
			s388.Metallic = _Float11;
			s388.Smoothness = _Float6;
			float fresnelNdotV367 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode367 = ( 0.05 + _Float0 * pow( 1.0 - fresnelNdotV367, 5.0 ) );
			s388.Occlusion = saturate( ( 1.0 - fresnelNode367 ) );

			data.light = gi.light;

			UnityGI gi388 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g388 = UnityGlossyEnvironmentSetup( s388.Smoothness, data.worldViewDir, s388.Normal, float3(0,0,0));
			gi388 = UnityGlobalIllumination( data, s388.Occlusion, s388.Normal, g388 );
			#endif

			float3 surfResult388 = LightingStandard ( s388, viewDir, gi388 ).rgb;
			surfResult388 += s388.Emission;

			#ifdef UNITY_PASS_FORWARDADD//388
			surfResult388 -= s388.Emission;
			#endif//388
			SurfaceOutputStandardSpecular s386 = (SurfaceOutputStandardSpecular ) 0;
			s386.Albedo = float3( 0,0,0 );
			float3 triplanar374 = TriplanarSamplingSNF( _Texture1, ase_vertex3Pos, ase_vertexNormal, 1.0, _Float1, 1.0, 0 );
			float3 tanTriplanarNormal374 = mul( objectToTangent, triplanar374 );
			float3 triplanar368 = TriplanarSamplingSNF( _Texture0, ase_vertex3Pos, ase_vertexNormal, 1.0, _Float1, 1.0, 0 );
			float3 tanTriplanarNormal368 = mul( objectToTangent, triplanar368 );
			float3 lerpResult380 = lerp( tanTriplanarNormal374 , tanTriplanarNormal368 , _CoatBump2);
			s386.Normal = WorldNormalVector( i , lerpResult380 );
			s386.Emission = float3( 0,0,0 );
			float3 temp_cast_5 = (1.0).xxx;
			s386.Specular = temp_cast_5;
			s386.Smoothness = _CoatSmoothness2;
			s386.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi386 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g386 = UnityGlossyEnvironmentSetup( s386.Smoothness, data.worldViewDir, s386.Normal, float3(0,0,0));
			gi386 = UnityGlobalIllumination( data, s386.Occlusion, s386.Normal, g386 );
			#endif

			float3 surfResult386 = LightingStandardSpecular ( s386, viewDir, gi386 ).rgb;
			surfResult386 += s386.Emission;

			#ifdef UNITY_PASS_FORWARDADD//386
			surfResult386 -= s386.Emission;
			#endif//386
			float clampResult387 = clamp( ( fresnelNode367 * _Float3 ) , _Float8 , _Float10 );
			float3 lerpResult389 = lerp( surfResult388 , surfResult386 , clampResult387);
			float3 lerpResult346 = lerp( lerpResult208 , lerpResult389 , 0.5);
			c.rgb = lerpResult346;
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
				float2 customPack1 : TEXCOORD1;
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
-824;109;1661;1004;469.3099;-180.5069;2.038785;True;False
Node;AmplifyShaderEditor.CommentaryNode;280;-204.687,1211.783;Inherit;False;963.3314;486.4381;Simple fresnel blend;9;343;298;296;279;47;237;344;345;342;Blend Factor;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;360;192.5092,3656.662;Inherit;False;963.3314;486.4381;Simple fresnel blend;9;387;384;382;379;375;371;370;367;361;Blend Factor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;343;-182.8443,1306.929;Float;False;Property;_FresnelScale;Fresnel Scale;23;0;Create;True;0;0;False;0;1;3.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;281;561.2941,1788.343;Inherit;False;1386.212;798.8774;This mirror layer that to mimc a coating layer;10;172;211;180;337;340;166;339;325;328;326;Coating Layer;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;362;958.4903,4233.222;Inherit;False;1386.212;798.8774;This mirror layer that to mimc a coating layer;10;386;383;381;380;374;369;368;366;365;364;Coating Layer;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;361;214.352,3751.808;Float;False;Property;_Float0;Float 0;22;0;Create;True;0;0;False;0;1;0.328;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;328;736.5735,2193.552;Float;False;Property;_CoatTiling;Coat Tiling;16;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;326;724.413,1989.109;Float;True;Property;_CoatNormal;Coat Normal;12;0;Create;True;0;0;False;0;None;0fb69c1cf4ff91d4abfc2da1c923c651;False;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;340;728.735,2290.004;Float;True;Global;Texture0;Texture 0;21;0;Create;True;0;0;False;0;None;None;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FresnelNode;367;434.3235,3709.906;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.05;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;366;1133.77,4638.431;Float;False;Property;_Float1;Float 1;17;0;Create;True;0;0;False;0;0;0.634;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;365;1121.609,4433.988;Float;True;Property;_Texture1;Texture 1;13;0;Create;True;0;0;False;0;None;None;False;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;364;1125.932,4734.882;Float;True;Global;_Texture0;Texture 0;20;0;Create;True;0;0;False;0;None;None;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;363;1359.538,2900.561;Inherit;False;1229.873;717.1765;Comment;7;388;385;378;377;376;373;372;Base Layer With Flakes;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;293;962.3416,455.6819;Inherit;False;1229.873;717.1765;Comment;7;324;329;320;216;1;394;395;Base Layer With Flakes;1,1,1,1;0;0
Node;AmplifyShaderEditor.FresnelNode;279;37.12727,1265.027;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.05;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;373;1428.326,3266.159;Float;False;Property;_Float5;Float 5;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;370;447.5963,3945.826;Float;False;Property;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;324;972.783,586.4591;Float;True;Property;_FlakesNormal;Flakes Normal;14;0;Create;True;0;0;False;0;None;d92130db6df948b419dd1dec490816d0;False;bump;LockedToTexture2D;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TriplanarNode;374;1406.073,4485.299;Inherit;True;Spherical;Object;True;Top Texture 4;_TopTexture4;bump;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;368;1405.362,4687.118;Inherit;True;Spherical;Object;True;Top Texture 3;_TopTexture3;bump;1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;52.21623,1500.947;Float;False;Property;_CoatAmount;Coat Amount;9;0;Create;True;0;0;False;0;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;371;774.9515,3757.257;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;325;1008.877,2040.42;Inherit;True;Spherical;Object;True;Top Texture 1;_TopTexture1;bump;0;None;Mid Texture 3;_MidTexture3;white;-1;None;Bot Texture 3;_BotTexture3;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;392;911.6549,968.1399;Inherit;True;Property;_Metallic_r03;Metallic_r03;28;0;Create;True;0;0;False;0;-1;0e566900fa09f0642be7cd7002894e16;0e566900fa09f0642be7cd7002894e16;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;372;1369.979,3031.338;Float;True;Property;_Texture2;Texture 2;15;0;Create;True;0;0;False;0;None;0fb69c1cf4ff91d4abfc2da1c923c651;False;bump;LockedToTexture2D;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;369;1467.423,4894.894;Float;False;Property;_CoatBump2;Coat Bump2;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;1070.227,2450.015;Float;False;Property;_CoatBump;Coat Bump;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;296;377.7553,1312.378;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;337;1008.166,2242.239;Inherit;True;Spherical;Object;True;Top Texture 2;_TopTexture2;bump;1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;219;926.9973,1182.691;Float;False;Property;_Metallic;Metallic;0;0;Create;True;0;0;False;0;0;0.97;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;1243.484,1180.612;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0.18;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;329;1031.13,821.2802;Float;False;Property;_FlakesTiling;Flakes Tiling;19;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;395;1587.764,1083.47;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;211;1458.787,2180.681;Float;False;Constant;_Spec;Spec;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;383;1855.982,4625.56;Float;False;Constant;_Spec2;Spec2;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;376;1958.701,3518.311;Float;False;Property;_Float6;Float 6;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;381;1857.945,4709.632;Float;False;Property;_CoatSmoothness2;Coat Smoothness2;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;339;1443.683,2036.791;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;377;1988.381,3002.239;Float;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;1,0.9310344,0,0;0.4800195,0.5197284,0.6320754,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;378;1750.834,3185.726;Inherit;True;Spherical;Object;True;Top Texture 5;_TopTexture5;bump;0;None;Mid Texture 4;_MidTexture4;white;1;None;Bot Texture 4;_BotTexture4;white;3;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;345;372.1937,1612.391;Float;False;Property;_FresnelMax;Fresnel Max;26;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;379;783.5252,3858.796;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;237;386.3292,1413.917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;384;769.3897,4057.27;Float;False;Property;_Float10;Float 10;27;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;344;368.2117,1529.2;Float;False;Property;_FresnelMin;Fresnel Min;24;0;Create;True;0;0;False;0;0;0.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;382;765.4078,3974.079;Float;False;Property;_Float8;Float 8;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;320;1327.244,659.7767;Inherit;True;Spherical;Object;True;Top Texture 0;_TopTexture0;bump;0;None;Mid Texture 5;_MidTexture5;white;1;None;Bot Texture 5;_BotTexture5;white;3;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;216;1640.206,480.0608;Float;False;Property;_Color;Color;4;0;Create;True;0;0;False;0;1,0.9310344,0,0;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;385;1946.97,3414.026;Float;False;Property;_Float11;Float 11;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;394;1347.232,993.0063;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;380;1840.878,4481.669;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;375;994.3575,3753.962;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;298;597.1613,1309.084;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;1460.749,2264.754;Float;False;Property;_CoatSmoothness;Coat Smoothness;11;0;Create;True;0;0;False;0;0;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;1;1984.983,848.4844;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;387;1002.678,3987.594;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;166;1689.669,1872.483;Inherit;False;Specular;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomStandardSurface;388;2382.178,3293.363;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;342;605.4821,1542.715;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;386;2086.864,4317.362;Inherit;False;Specular;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;389;2980.182,3536.231;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;208;2426.687,1263.925;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;359;3233.722,2482.267;Float;False;Constant;_Float4;Float 4;19;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;393;616.9497,988.8932;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;346;3222.568,2230.231;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;390;1184.323,1346.673;Inherit;True;Property;_ao;ao;28;0;Fetch;True;0;0;False;0;-1;222668baa6e8ad54ab1d3ce1c0a71888;222668baa6e8ad54ab1d3ce1c0a71888;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;391;1596.193,1302.17;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4091.227,1548.898;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;DoubleLayerCustomSurface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;367;2;361;0
WireConnection;279;2;343;0
WireConnection;374;0;365;0
WireConnection;374;3;366;0
WireConnection;368;0;364;0
WireConnection;368;3;366;0
WireConnection;371;0;367;0
WireConnection;325;0;326;0
WireConnection;325;3;328;0
WireConnection;296;0;279;0
WireConnection;337;0;340;0
WireConnection;337;3;328;0
WireConnection;395;0;392;0
WireConnection;395;1;233;0
WireConnection;339;0;325;0
WireConnection;339;1;337;0
WireConnection;339;2;180;0
WireConnection;378;0;372;0
WireConnection;378;3;373;0
WireConnection;379;0;367;0
WireConnection;379;1;370;0
WireConnection;237;0;279;0
WireConnection;237;1;47;0
WireConnection;320;0;324;0
WireConnection;320;3;329;0
WireConnection;394;0;392;0
WireConnection;394;1;219;0
WireConnection;380;0;374;0
WireConnection;380;1;368;0
WireConnection;380;2;369;0
WireConnection;375;0;371;0
WireConnection;298;0;296;0
WireConnection;1;0;216;0
WireConnection;1;1;320;0
WireConnection;1;3;394;0
WireConnection;1;4;395;0
WireConnection;1;5;298;0
WireConnection;387;0;379;0
WireConnection;387;1;382;0
WireConnection;387;2;384;0
WireConnection;166;1;339;0
WireConnection;166;3;211;0
WireConnection;166;4;172;0
WireConnection;388;0;377;0
WireConnection;388;1;378;0
WireConnection;388;3;385;0
WireConnection;388;4;376;0
WireConnection;388;5;375;0
WireConnection;342;0;237;0
WireConnection;342;1;344;0
WireConnection;342;2;345;0
WireConnection;386;1;380;0
WireConnection;386;3;383;0
WireConnection;386;4;381;0
WireConnection;389;0;388;0
WireConnection;389;1;386;0
WireConnection;389;2;387;0
WireConnection;208;0;1;0
WireConnection;208;1;166;0
WireConnection;208;2;342;0
WireConnection;346;0;208;0
WireConnection;346;1;389;0
WireConnection;346;2;359;0
WireConnection;391;1;390;0
WireConnection;0;13;346;0
ASEEND*/
//CHKSM=103ED385E8ECE0BF95242E8CA564E3B82FA08416