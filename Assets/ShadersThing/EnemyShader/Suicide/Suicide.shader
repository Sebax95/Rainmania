// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Enemy/Suicide"
{
	Properties
	{
		[NoScaleOffset][SingleLineTexture]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_AO("AO", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset][SingleLineTexture]_Heighmap("Heighmap", 2D) = "white" {}
		[HDR]_Emision("Emision", Color) = (0,0,0,0)
		_SpeedEmission("Speed Emission", Float) = 0
		_Freq("Freq", Float) = 0
		_Divide("Divide", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _AO;
		uniform float4 _Emision;
		uniform float _SpeedEmission;
		uniform float _Freq;
		uniform float _Divide;
		uniform sampler2D _Heighmap;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal5 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal5 ) );
			float2 uv_Albedo1 = i.uv_texcoord;
			float2 uv_AO4 = i.uv_texcoord;
			float4 tex2DNode4 = tex2D( _AO, uv_AO4 );
			o.Albedo = ( tex2D( _Albedo, uv_Albedo1 ) * tex2DNode4 ).rgb;
			float mulTime16 = _Time.y * _SpeedEmission;
			float clampResult27 = clamp( ( ( ( sin( ( mulTime16 + _Freq ) ) + 1.0 ) * 0.5 ) / _Divide ) , 0.0 , 1.0 );
			o.Emission = ( ( 1.0 - tex2DNode4 ) * ( _Emision * clampResult27 ) ).rgb;
			float2 uv_Heighmap8 = i.uv_texcoord;
			o.Smoothness = tex2D( _Heighmap, uv_Heighmap8 ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;540;1341;459;1414.711;29.83498;1.971527;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-1642.79,405.2296;Inherit;False;Property;_SpeedEmission;Speed Emission;6;0;Create;True;0;0;False;0;False;0;2.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-1380.747,396.4474;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1370.208,494.8083;Inherit;False;Property;_Freq;Freq;7;0;Create;True;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1194.563,378.8828;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;17;-1052.291,331.4587;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-905.177,269.3475;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-734.3742,399.9602;Inherit;False;Property;_Divide;Divide;8;0;Create;True;0;0;False;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-739.9613,259.1266;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-587.4283,246.147;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-688.1719,-205.5233;Inherit;True;Property;_AO;AO;2;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;353f34527f9782649aed852e9724e57a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-611.4252,57.33738;Inherit;False;Property;_Emision;Emision;5;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;4.986103,57.0379,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;27;-440.4089,252.8277;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-309.4625,101.521;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-428.4801,-394.9189;Inherit;True;Property;_Albedo;Albedo;1;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;38c36c908f0961247ab7dcf026224fdf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;14;-330.0111,-19.88502;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-163.5493,182.0294;Inherit;True;Property;_Normal;Normal;3;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;d90d6a508b08e2e4488e2eded550f7a5;d90d6a508b08e2e4488e2eded550f7a5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-130.9908,11.26242;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;7.395224,-164.6941;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;8;-133.9617,451.5597;Inherit;True;Property;_Heighmap;Heighmap;4;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;4c2e29d9910f8f44d9a1f9861d9e30fa;4c2e29d9910f8f44d9a1f9861d9e30fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;208.9671,-17.86043;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;MyShaders/Enemy/Suicide;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;18;0
WireConnection;19;0;16;0
WireConnection;19;1;20;0
WireConnection;17;0;19;0
WireConnection;21;0;17;0
WireConnection;22;0;21;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;27;0;24;0
WireConnection;15;0;10;0
WireConnection;15;1;27;0
WireConnection;14;0;4;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;9;0;1;0
WireConnection;9;1;4;0
WireConnection;0;0;9;0
WireConnection;0;1;5;0
WireConnection;0;2;13;0
WireConnection;0;4;8;0
ASEEND*/
//CHKSM=5F31C238D8DCB098668EF92D39D48019A0A934A1