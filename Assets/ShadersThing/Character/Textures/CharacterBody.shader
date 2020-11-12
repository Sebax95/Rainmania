// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Character/Body"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		[NoScaleOffset][SingleLineTexture]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal][SingleLineTexture]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset][SingleLineTexture]_MetallicAndSmoth("MetallicAndSmoth", 2D) = "white" {}
		_ColorXray1("ColorXray", Color) = (1,1,1,0)
		_XRayIntensity1("XRayIntensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0"}
		ZWrite On
		ZTest GEqual
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog alpha:fade  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		#include "UnityShaderVariables.cginc"
		
		
		
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};
		uniform sampler2D _Albedo;
		uniform float4 _ColorXray1;
		uniform float _XRayIntensity1;
		float _ASEOutlineWidth;
		
		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
		}


		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = 0.0;
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen11 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither11 = Dither4x4Bayer( fmod(clipScreen11.x, 4), fmod(clipScreen11.y, 4) );
			float2 uv_Albedo1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo1 );
			o.Emission = ( dither11 + ( tex2DNode1 * _ColorXray1 ) ).rgb;
			o.Alpha = ( _XRayIntensity1 * (_ColorXray1).a );
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZWrite On
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Albedo;
		uniform sampler2D _MetallicAndSmoth;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal2 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal2 ) );
			float2 uv_Albedo1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo1 );
			o.Albedo = tex2DNode1.rgb;
			float2 uv_MetallicAndSmoth3 = i.uv_texcoord;
			float4 tex2DNode3 = tex2D( _MetallicAndSmoth, uv_MetallicAndSmoth3 );
			o.Metallic = tex2DNode3.r;
			o.Smoothness = tex2DNode3.a;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
0;600;1379;399;2002.721;-447.1603;1.783187;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-716.026,-151.1076;Inherit;True;Property;_Albedo;Albedo;0;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;11a3d4f181dbe6e4da8d3c3be137e76e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;16;-413.5642,6.804539;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-1163.641,803.7801;Inherit;False;Property;_ColorXray1;ColorXray;3;0;Create;True;0;0;False;0;False;1,1,1,0;0.4862745,0.9254903,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;15;-905.4044,348.9545;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-929.8409,1008.428;Inherit;False;Property;_XRayIntensity1;XRayIntensity;4;0;Create;True;0;0;False;0;False;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-906.8015,740.3452;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;11;-909.4734,510.8042;Inherit;False;0;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;9;-924.8896,917.0297;Inherit;False;FLOAT;3;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-683.8353,500.1695;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-700.3593,823.3889;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-488.3723,145.4306;Inherit;True;Property;_Normal;Normal;1;3;[NoScaleOffset];[Normal];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;a4f4bfdab20aaca449da6feef53513c3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-474.0168,328.1021;Inherit;True;Property;_MetallicAndSmoth;MetallicAndSmoth;2;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;6f9130217d9f087488159aa69945fd82;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OutlineNode;4;-353.052,580.7565;Inherit;False;0;True;Transparent;1;4;Off;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;MyShaders/Character/Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;1;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;1;0
WireConnection;15;0;16;0
WireConnection;12;0;15;0
WireConnection;12;1;8;0
WireConnection;9;0;8;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;14;0;10;0
WireConnection;14;1;9;0
WireConnection;4;0;13;0
WireConnection;4;2;14;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;3;3;0
WireConnection;0;4;3;4
WireConnection;0;11;4;0
ASEEND*/
//CHKSM=7E50618670057547EF50A6ADE1481BD0889F17EB