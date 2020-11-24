// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Brush"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 16
		[HideInInspector][NoScaleOffset]_Normal1("Normal 1", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal2("Normal 2", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal3("Normal 3", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal4("Normal 4", 2D) = "white" {}
		[NoScaleOffset]_Texture1("Texture 1", 2D) = "white" {}
		_Tilling1("Tilling 1", Float) = 1
		_IntensityNormal1("Intensity Normal 1", Range( 1 , 2)) = 1
		[NoScaleOffset]_Texture2("Texture 2", 2D) = "white" {}
		_Tilling2("Tilling 2", Float) = 1
		[NoScaleOffset]_Texture3("Texture 3", 2D) = "white" {}
		_Tilling3("Tilling 3", Float) = 1
		[NoScaleOffset]_Texture4("Texture 4", 2D) = "white" {}
		_Tilling4("Tilling 4", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _IntensityNormal1;
		uniform sampler2D _Normal1;
		uniform float _Tilling1;
		uniform sampler2D _Normal2;
		uniform float _Tilling2;
		uniform sampler2D _Normal3;
		uniform float _Tilling3;
		uniform sampler2D _Normal4;
		uniform float _Tilling4;
		uniform sampler2D _Texture1;
		uniform sampler2D _Texture2;
		uniform sampler2D _Texture3;
		uniform sampler2D _Texture4;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Tilling1).xx;
			float2 uv_TexCoord17 = i.uv_texcoord * temp_cast_0;
			float2 temp_cast_1 = (_Tilling2).xx;
			float2 uv_TexCoord19 = i.uv_texcoord * temp_cast_1;
			float Red61 = i.vertexColor.r;
			float4 lerpResult59 = lerp( tex2D( _Normal1, uv_TexCoord17 ) , tex2D( _Normal2, uv_TexCoord19 ) , Red61);
			float2 temp_cast_2 = (_Tilling3).xx;
			float2 uv_TexCoord22 = i.uv_texcoord * temp_cast_2;
			float Green62 = i.vertexColor.g;
			float4 lerpResult47 = lerp( lerpResult59 , tex2D( _Normal3, uv_TexCoord22 ) , Green62);
			float2 temp_cast_3 = (_Tilling4).xx;
			float2 uv_TexCoord24 = i.uv_texcoord * temp_cast_3;
			float Blue60 = i.vertexColor.b;
			float4 lerpResult50 = lerp( lerpResult47 , tex2D( _Normal4, uv_TexCoord24 ) , Blue60);
			o.Normal = lerpResult50.rgb;
			float4 lerpResult7 = lerp( tex2D( _Texture1, uv_TexCoord17 ) , tex2D( _Texture2, uv_TexCoord19 ) , Red61);
			float4 lerpResult8 = lerp( lerpResult7 , tex2D( _Texture3, uv_TexCoord22 ) , Green62);
			float4 lerpResult9 = lerp( lerpResult8 , tex2D( _Texture4, uv_TexCoord24 ) , Blue60);
			o.Albedo = lerpResult9.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
0;0;1920;1019;2682.229;1760.303;1.036927;True;False
Node;AmplifyShaderEditor.VertexColorNode;2;-2574.948,-728.9564;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-2213.755,-1205.432;Float;False;Property;_Tilling2;Tilling 2;13;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2251.887,-1605.893;Float;False;Property;_Tilling1;Tilling 1;10;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-2308.936,-807.2318;Float;False;Red;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2017.891,-1223.466;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2053.129,-1624.376;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1742.422,-1333.658;Inherit;True;Property;_Texture2;Texture 2;12;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;6a0df7fe94a829a46bbaf524676149dd;6a0df7fe94a829a46bbaf524676149dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;63;-1613.683,-1853.341;Inherit;False;61;Red;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1756.16,-1751.827;Inherit;True;Property;_Texture1;Texture 1;9;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;c8f02b7ea69760f4faca480b68deba9a;c8f02b7ea69760f4faca480b68deba9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1731.898,-665.9513;Float;False;Property;_Tilling3;Tilling 3;16;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;-1751.824,-1540.129;Inherit;True;Property;_Normal1;Normal 1;5;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;False;-1;b4d413cfd29006f47a7e9c4dc61e2e59;b4d413cfd29006f47a7e9c4dc61e2e59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;57;-1752.859,-1138.163;Inherit;True;Property;_Normal2;Normal 2;6;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;False;-1;c79ac03a965535345946ab8d7b16dcd8;c79ac03a965535345946ab8d7b16dcd8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-2315.528,-686.9244;Float;False;Green;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;-1307.769,-1358.944;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-1522.077,-682.4483;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;7;-1312.105,-1570.641;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1363.227,-157.6403;Float;False;Property;_Tilling4;Tilling 4;19;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1235.592,-846.5572;Inherit;True;Property;_Texture3;Texture 3;15;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;52ff6b17d53841649b808b85e6f0b247;52ff6b17d53841649b808b85e6f0b247;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;66;-998.5569,-1091.07;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;58;-1223.475,-630.9189;Inherit;True;Property;_Normal3;Normal 3;7;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;False;-1;58ad6a3618bcd434790e97c3917e339c;58ad6a3618bcd434790e97c3917e339c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;65;-1023.509,-1411.881;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-1139.855,-957.7747;Inherit;False;62;Green;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1171.227,-173.6403;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;8;-818.1521,-845.6895;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;47;-787.6494,-621.7399;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-2315.529,-599.5781;Float;False;Blue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-865.4893,-337.1555;Inherit;True;Property;_Texture4;Texture 4;18;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;b670ab0748ba6b1469f68eb37c16c92e;b670ab0748ba6b1469f68eb37c16c92e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;69;-604.5918,-483.016;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-780.6473,-446.765;Inherit;False;60;Blue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-859.2533,-113.8659;Inherit;True;Property;_Normal4;Normal 4;8;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;False;-1;859e520becd378b429dcc1abfc918c23;859e520becd378b429dcc1abfc918c23;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;68;-554.5587,-644.887;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1166.827,-31.95463;Float;False;Property;_IntensityNormal4;Intensity Normal 4;20;0;Create;True;0;0;False;0;False;2;2;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2049.647,-1474.101;Float;False;Property;_IntensityNormal1;Intensity Normal 1;11;0;Create;True;0;0;True;0;False;1;1;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-497.1686,-352.5379;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;50;-489.3533,-127.4565;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-2098.565,-1065.666;Float;False;Property;_IntensityNormal2;Intensity Normal 2;14;0;Create;True;0;0;False;0;False;0;0;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-1553.359,-524.0497;Float;False;Property;_IntensityNormal3;Intensity Normal 3;17;0;Create;True;0;0;False;0;False;1;1;1;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-218.756,-351.337;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Brush;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;16;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;2;1
WireConnection;19;0;20;0
WireConnection;17;0;18;0
WireConnection;13;1;19;0
WireConnection;12;1;17;0
WireConnection;55;1;17;0
WireConnection;57;1;19;0
WireConnection;62;0;2;2
WireConnection;59;0;55;0
WireConnection;59;1;57;0
WireConnection;59;2;63;0
WireConnection;22;0;21;0
WireConnection;7;0;12;0
WireConnection;7;1;13;0
WireConnection;7;2;63;0
WireConnection;15;1;22;0
WireConnection;66;0;59;0
WireConnection;58;1;22;0
WireConnection;65;0;7;0
WireConnection;24;0;23;0
WireConnection;8;0;65;0
WireConnection;8;1;15;0
WireConnection;8;2;64;0
WireConnection;47;0;66;0
WireConnection;47;1;58;0
WireConnection;47;2;64;0
WireConnection;60;0;2;3
WireConnection;16;1;24;0
WireConnection;69;0;47;0
WireConnection;48;1;24;0
WireConnection;68;0;8;0
WireConnection;9;0;68;0
WireConnection;9;1;16;0
WireConnection;9;2;70;0
WireConnection;50;0;69;0
WireConnection;50;1;48;0
WireConnection;50;2;70;0
WireConnection;0;0;9;0
WireConnection;0;1;50;0
ASEEND*/
//CHKSM=F29B07973FD6F60108310E4F268C808099E12F63