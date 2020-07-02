// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "yq/FlowLight"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_speed("speed", Float) = 0
		_Float1("Float 1", Range( 0 , 10)) = 0
		_Float2("Float 2", Range( 0 , 10)) = 0
		_Float0("Float 0", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Float0;
		uniform sampler2D _MainTex;
		uniform float _speed;
		uniform float _Float1;
		uniform float _Float2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime45 = _Time.y * _speed;
			float2 break40 = i.uv_texcoord;
			float4 appendResult43 = (float4(( break40.x * 1.0 ) , ( break40.y * 1.0 ) , 0.0 , 0.0));
			float2 panner44 = ( mulTime45 * float2( 3,0 ) + appendResult43.xy);
			float3 desaturateInitialColor53 = ( float4( ( float3(0.5,0.5,0.5) * _Float0 ) , 0.0 ) * tex2D( _MainTex, panner44 ) ).rgb;
			float desaturateDot53 = dot( desaturateInitialColor53, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar53 = lerp( desaturateInitialColor53, desaturateDot53.xxx, _Float1 );
			float3 temp_cast_3 = (_Float2).xxx;
			o.Albedo = pow( desaturateVar53 , temp_cast_3 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
7;8;1661;1003;2455.017;1348.117;2.203761;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;39;-1708.266,-613.5525;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;40;-1531.273,-625.9097;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1224.933,-629.9153;Float;False;2;2;0;FLOAT;2;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1210.933,-500.3566;Float;False;2;2;0;FLOAT;3;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1161.029,-367.27;Float;False;Property;_speed;speed;1;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;-1073.222,-603.1821;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;45;-987.748,-397.8227;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;57;-578.78,-975.4379;Float;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;0.5,0.5,0.5;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;44;-777.8025,-583.5349;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;3,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-698.78,-782.4379;Float;False;Property;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;2.18;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-397.78,-912.4379;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;1;-535.9432,-568.6092;Float;True;Property;_MainTex;_MainTex;0;0;Create;True;0;0;False;0;None;ad0c98e22650c8a4b826845976808492;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-198.78,-869.4379;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-704.78,-682.4379;Float;False;Property;_Float1;Float 1;2;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;3.219971,-505.4379;Float;False;Property;_Float2;Float 2;3;0;Create;True;0;0;False;0;0;3.94;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;53;-127.78,-725.4379;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;55;67.21997,-680.4379;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;26;244.3143,-535.3002;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;yq/FlowLight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;39;0
WireConnection;41;0;40;0
WireConnection;42;0;40;1
WireConnection;43;0;41;0
WireConnection;43;1;42;0
WireConnection;45;0;51;0
WireConnection;44;0;43;0
WireConnection;44;1;45;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;1;1;44;0
WireConnection;60;0;59;0
WireConnection;60;1;1;0
WireConnection;53;0;60;0
WireConnection;53;1;54;0
WireConnection;55;0;53;0
WireConnection;55;1;56;0
WireConnection;26;0;55;0
ASEEND*/
//CHKSM=4CAA306030312062520E1F7711458DB17583E6E2