// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "bricks piso"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_intensitynormal("intensity normal", Float) = 0
		_MinOld("Min Old", Float) = 0
		_MaxOld("Max Old", Float) = 0
		_MinNew("Min New", Float) = 0
		_MusgoCOlor("Musgo COlor", Color) = (0.4383147,0.8867924,0.2133321,0)
		_BrickColor("Brick Color", Color) = (0.9528302,0.2961215,0.05842829,0)
		_MaxNew("Max New", Float) = 0
		[NoScaleOffset][SingleLineTexture]_sergunkuyucumedievalblocksheight("sergun-kuyucu-medieval-blocks-height", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_smoothsanddunes2048x2048("smooth+sand+dunes-2048x2048", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _sergunkuyucumedievalblocksheight;
		SamplerState sampler_sergunkuyucumedievalblocksheight;
		uniform float _MinOld;
		uniform float _MaxOld;
		uniform float _MinNew;
		uniform float _MaxNew;
		uniform float _intensitynormal;
		uniform sampler2D _smoothsanddunes2048x2048;
		uniform float4 _MusgoCOlor;
		uniform float4 _BrickColor;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_sergunkuyucumedievalblocksheight1 = v.texcoord;
			float4 tex2DNode1 = tex2Dlod( _sergunkuyucumedievalblocksheight, float4( uv_sergunkuyucumedievalblocksheight1, 0, 0.0) );
			float temp_output_5_0 = (_MinNew + (tex2DNode1.r - _MinOld) * (_MaxNew - _MinNew) / (_MaxOld - _MinOld));
			v.vertex.xyz += ( ase_vertexNormal * temp_output_5_0 * _intensitynormal );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_smoothsanddunes2048x20482 = i.uv_texcoord;
			float2 uv_sergunkuyucumedievalblocksheight1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _sergunkuyucumedievalblocksheight, uv_sergunkuyucumedievalblocksheight1 );
			float temp_output_5_0 = (_MinNew + (tex2DNode1.r - _MinOld) * (_MaxNew - _MinNew) / (_MaxOld - _MinOld));
			float4 lerpResult3 = lerp( ( tex2D( _smoothsanddunes2048x2048, uv_smoothsanddunes2048x20482 ) * _MusgoCOlor ) , ( tex2DNode1 * _BrickColor ) , saturate( temp_output_5_0 ));
			o.Albedo = lerpResult3.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
701;81;934;639;292.383;579.1858;1.3;False;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-860.5433,251.3179;Inherit;False;Property;_MinOld;Min Old;6;0;Create;True;0;0;False;0;False;0;0.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-981.2298,-50.5338;Inherit;True;Property;_sergunkuyucumedievalblocksheight;sergun-kuyucu-medieval-blocks-height;12;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;67081ee7dfced9540856df4c376e0d51;67081ee7dfced9540856df4c376e0d51;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-870.4234,337.0066;Inherit;False;Property;_MaxOld;Max Old;7;0;Create;True;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-873.4344,506.6277;Inherit;False;Property;_MaxNew;Max New;11;0;Create;True;0;0;False;0;False;0;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-874.4695,423.5738;Inherit;False;Property;_MinNew;Min New;8;0;Create;True;0;0;False;0;False;0;1.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-881.8638,-458.8282;Inherit;True;Property;_smoothsanddunes2048x2048;smooth+sand+dunes-2048x2048;13;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;39e724ce616e4a044806e6c258c56cfd;39e724ce616e4a044806e6c258c56cfd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-609.7594,132.9402;Inherit;False;Property;_BrickColor;Brick Color;10;0;Create;True;0;0;False;0;False;0.9528302,0.2961215,0.05842829,0;0.4551887,0.5,0.4699112,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;5;-621.2934,305.7103;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-611.9012,-278.0452;Inherit;False;Property;_MusgoCOlor;Musgo COlor;9;0;Create;True;0;0;False;0;False;0.4383147,0.8867924,0.2133321,0;0.4383147,0.8867924,0.2133321,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-258.0232,-466.7629;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;-358.8971,211.515;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-520.0435,-24.04919;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-179.4733,515.5185;Inherit;False;Property;_intensitynormal;intensity normal;5;0;Create;True;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;7;-171.6733,272.0776;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;26.31403,390.4831;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;3;-48.2074,-125.6149;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;254.9529,-113.4896;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;bricks piso;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;1
WireConnection;5;1;10;0
WireConnection;5;2;11;0
WireConnection;5;3;12;0
WireConnection;5;4;13;0
WireConnection;17;0;2;0
WireConnection;17;1;18;0
WireConnection;16;0;5;0
WireConnection;19;0;1;0
WireConnection;19;1;20;0
WireConnection;8;0;7;0
WireConnection;8;1;5;0
WireConnection;8;2;9;0
WireConnection;3;0;17;0
WireConnection;3;1;19;0
WireConnection;3;2;16;0
WireConnection;0;0;3;0
WireConnection;0;11;8;0
ASEEND*/
//CHKSM=C888F9D683194DEA2E379D6B7530FF9672E5B5D7