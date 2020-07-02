// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "yq/FlowLight"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_Speed("Speed", Float) = 1
		_vector("vector", Vector) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float2 _vector;
		uniform float _Speed;
		uniform float _Float0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime32 = _Time.y * _Speed;
			float2 panner31 = ( mulTime32 * float2( -1,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord30 = i.uv_texcoord * _vector + panner31;
			o.Emission = ( tex2D( _MainTex, uv_TexCoord30 ) * _Float0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
-1638;-1046;1524;922;901.9872;670.6312;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;34;-561.8783,-660.5735;Float;False;889.1667;213.5349;Comment;4;30;31;32;33;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-511.8793,-579.8417;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-368.4406,-572.8862;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;31;-183.4182,-616.1308;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;37;-133.308,-754.9887;Float;False;Property;_vector;vector;2;0;Create;True;0;0;False;0;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;28.09551,-610.5739;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-243.1313,-334.4767;Float;True;Property;_MainTex;_MainTex;0;0;Create;True;0;0;False;0;None;64e7766099ad46747a07014e44d0aea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-185.5144,-93.90179;Float;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;61.9982,-329.1767;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;26;312.5001,-336.8998;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;yq/FlowLight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;33;0
WireConnection;31;1;32;0
WireConnection;30;0;37;0
WireConnection;30;1;31;0
WireConnection;1;1;30;0
WireConnection;28;0;1;0
WireConnection;28;1;38;0
WireConnection;26;2;28;0
ASEEND*/
//CHKSM=AABEF620B69B550055BB3903F0E3634A0A6EFB07