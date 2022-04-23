// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clase08/Ocean Shader"
{
	Properties
	{
		_Steepness("Steepness", Range( 0 , 1)) = 0
		_Speed("Speed", Float) = 1
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 32
		_Amplitude("Amplitude", Float) = 1
		_Lenght("Lenght", Float) = 1
		[HideInInspector]_Direction3("Direction3", Vector) = (0,0,0,0)
		_NumberofWaves("Number of Waves", Float) = 0
		_WaveColor("Wave Color", Color) = (0,0,0,0)
		_CrestColor("Crest Color", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[NoScaleOffset]_HDRI("HDRI", CUBE) = "white" {}
		_SpecularIntensity("Specular Intensity", Range( 0 , 1)) = 0
		[NoScaleOffset]_MicroNormal("Micro Normal", 2D) = "bump" {}
		_MicroNormalTiling("Micro Normal Tiling", Int) = 1
		_MacroNormalTiling("Macro Normal Tiling", Int) = 1
		[HideInInspector]_Direction1("Direction1", Vector) = (1,0,0,0)
		[NoScaleOffset]_MacroNormal("Macro Normal", 2D) = "bump" {}
		[HideInInspector]_Direction2("Direction2", Vector) = (1,0,0,0)
		_MicroNormalScale("Micro Normal Scale", Range( 0 , 1)) = 0
		_MacroNormalScale("Macro Normal Scale", Range( 0 , 1)) = 0
		_VertexNormalIntensity("Vertex Normal Intensity", Range( 0 , 1)) = 0
		_CrestOpacity("Crest Opacity", Range( 0 , 1)) = 0
		_EmissiveIntensity("Emissive Intensity", Range( 0 , 1)) = 0
		_ProximityOpacity("Proximity Opacity", Range( 0 , 1)) = 0
		_WaveOpacity("Wave Opacity", Range( 0 , 1)) = 0
		_ShallowWaterOpacity("Shallow Water Opacity", Range( 0 , 1)) = 0
		[HideInInspector]_Direction4("Direction4", Vector) = (1,0,0,0)
		_MicroNormalSpeed("Micro Normal Speed", Vector) = (0,0,0,0)
		_MacroNormalSpeed("Macro Normal Speed", Vector) = (0,0,0,0)
		_ShallowWaterIntensity("Shallow Water Intensity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular alpha:fade keepalpha vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float3 _Direction1;
		uniform float _Lenght;
		uniform float _Speed;
		uniform float _Steepness;
		uniform float _Amplitude;
		uniform float _NumberofWaves;
		uniform float3 _Direction2;
		uniform float3 _Direction3;
		uniform float3 _Direction4;
		uniform float _ShallowWaterIntensity;
		uniform float _VertexNormalIntensity;
		uniform float _MicroNormalScale;
		uniform sampler2D _MicroNormal;
		uniform float2 _MicroNormalSpeed;
		uniform int _MicroNormalTiling;
		uniform float _MacroNormalScale;
		uniform sampler2D _MacroNormal;
		uniform float2 _MacroNormalSpeed;
		uniform int _MacroNormalTiling;
		uniform float4 _WaveColor;
		uniform float4 _CrestColor;
		uniform samplerCUBE _HDRI;
		uniform float _EmissiveIntensity;
		uniform float _SpecularIntensity;
		uniform float _Smoothness;
		uniform float _ShallowWaterOpacity;
		uniform float _ProximityOpacity;
		uniform float _WaveOpacity;
		uniform float _CrestOpacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 normalizeResult9_g20 = normalize( (_Direction1).xz );
			float2 break30_g20 = normalizeResult9_g20;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float dotResult10_g20 = dot( normalizeResult9_g20 , (ase_worldPos).xz );
			float W37_g20 = sqrt( ( ( 6.28318548202515 / _Lenght ) * 9.81 ) );
			float temp_output_24_0_g20 = ( ( dotResult10_g20 * W37_g20 ) + ( _Time.y * _Speed ) );
			float temp_output_26_0_g20 = cos( temp_output_24_0_g20 );
			float temp_output_35_0_g20 = _Amplitude;
			float temp_output_46_0_g20 = ( ( _Steepness / ( temp_output_35_0_g20 * W37_g20 * _NumberofWaves ) ) * temp_output_35_0_g20 );
			float3 appendResult28_g20 = (float3(( break30_g20.x * temp_output_26_0_g20 * temp_output_46_0_g20 ) , ( sin( temp_output_24_0_g20 ) * temp_output_35_0_g20 ) , ( break30_g20.y * temp_output_26_0_g20 * temp_output_46_0_g20 )));
			float2 normalizeResult9_g18 = normalize( (_Direction2).xz );
			float2 break30_g18 = normalizeResult9_g18;
			float dotResult10_g18 = dot( normalizeResult9_g18 , (ase_worldPos).xz );
			float W37_g18 = sqrt( ( ( 6.28318548202515 / _Lenght ) * 9.81 ) );
			float temp_output_24_0_g18 = ( ( dotResult10_g18 * W37_g18 ) + ( _Time.y * _Speed ) );
			float temp_output_26_0_g18 = cos( temp_output_24_0_g18 );
			float temp_output_35_0_g18 = _Amplitude;
			float temp_output_46_0_g18 = ( ( _Steepness / ( temp_output_35_0_g18 * W37_g18 * _NumberofWaves ) ) * temp_output_35_0_g18 );
			float3 appendResult28_g18 = (float3(( break30_g18.x * temp_output_26_0_g18 * temp_output_46_0_g18 ) , ( sin( temp_output_24_0_g18 ) * temp_output_35_0_g18 ) , ( break30_g18.y * temp_output_26_0_g18 * temp_output_46_0_g18 )));
			float2 normalizeResult9_g15 = normalize( (_Direction3).xz );
			float2 break30_g15 = normalizeResult9_g15;
			float dotResult10_g15 = dot( normalizeResult9_g15 , (ase_worldPos).xz );
			float temp_output_26_0 = ( _Lenght / 2.0 );
			float W37_g15 = sqrt( ( ( 6.28318548202515 / temp_output_26_0 ) * 9.81 ) );
			float temp_output_27_0 = ( _Speed * 2.0 );
			float temp_output_24_0_g15 = ( ( dotResult10_g15 * W37_g15 ) + ( _Time.y * temp_output_27_0 ) );
			float temp_output_26_0_g15 = cos( temp_output_24_0_g15 );
			float temp_output_35_0_g15 = _Amplitude;
			float temp_output_46_0_g15 = ( ( _Steepness / ( temp_output_35_0_g15 * W37_g15 * _NumberofWaves ) ) * temp_output_35_0_g15 );
			float3 appendResult28_g15 = (float3(( break30_g15.x * temp_output_26_0_g15 * temp_output_46_0_g15 ) , ( sin( temp_output_24_0_g15 ) * temp_output_35_0_g15 ) , ( break30_g15.y * temp_output_26_0_g15 * temp_output_46_0_g15 )));
			float2 normalizeResult9_g19 = normalize( (_Direction4).xz );
			float2 break30_g19 = normalizeResult9_g19;
			float dotResult10_g19 = dot( normalizeResult9_g19 , (ase_worldPos).xz );
			float W37_g19 = sqrt( ( ( 6.28318548202515 / temp_output_26_0 ) * 9.81 ) );
			float temp_output_24_0_g19 = ( ( dotResult10_g19 * W37_g19 ) + ( _Time.y * temp_output_27_0 ) );
			float temp_output_26_0_g19 = cos( temp_output_24_0_g19 );
			float temp_output_35_0_g19 = _Amplitude;
			float temp_output_46_0_g19 = ( ( _Steepness / ( temp_output_35_0_g19 * W37_g19 * _NumberofWaves ) ) * temp_output_35_0_g19 );
			float3 appendResult28_g19 = (float3(( break30_g19.x * temp_output_26_0_g19 * temp_output_46_0_g19 ) , ( sin( temp_output_24_0_g19 ) * temp_output_35_0_g19 ) , ( break30_g19.y * temp_output_26_0_g19 * temp_output_46_0_g19 )));
			float3 temp_output_25_0 = ( appendResult28_g20 + appendResult28_g18 + appendResult28_g15 + appendResult28_g19 );
			float3 lerpResult98 = lerp( temp_output_25_0 , ( temp_output_25_0 * _ShallowWaterIntensity ) , v.color.r);
			v.vertex.xyz += lerpResult98;
			float4 color62 = IsGammaSpace() ? float4(0,0,1,0) : float4(0,0,1,0);
			float4 lerpResult60 = lerp( color62 , float4( abs( temp_output_25_0 ) , 0.0 ) , _VertexNormalIntensity);
			v.normal = lerpResult60.rgb;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 temp_cast_0 = _MicroNormalTiling;
			float2 uv_TexCoord49 = i.uv_texcoord * temp_cast_0;
			float2 panner48 = ( 1.0 * _Time.y * _MicroNormalSpeed + uv_TexCoord49);
			float2 temp_cast_1 = _MacroNormalTiling;
			float2 uv_TexCoord51 = i.uv_texcoord * temp_cast_1;
			float2 panner50 = ( 1.0 * _Time.y * _MacroNormalSpeed + uv_TexCoord51);
			float3 NormalFromTextures58 = BlendNormals( UnpackScaleNormal( tex2D( _MicroNormal, panner48 ), _MicroNormalScale ) , UnpackScaleNormal( tex2D( _MacroNormal, panner50 ), _MacroNormalScale ) );
			float3 temp_output_59_0 = NormalFromTextures58;
			o.Normal = temp_output_59_0;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult29 = lerp( _WaveColor , _CrestColor , saturate( ase_vertex3Pos.y ));
			o.Albedo = lerpResult29.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 texCUBENode38 = texCUBE( _HDRI, reflect( -ase_worldViewDir , ase_worldNormal ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV63 = dot( mul(ase_tangentToWorldFast,NormalFromTextures58), ase_worldViewDir );
			float fresnelNode63 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV63, 5.0 ) );
			o.Emission = ( texCUBENode38 * saturate( fresnelNode63 ) * _EmissiveIntensity ).rgb;
			o.Specular = ( texCUBENode38 * _SpecularIntensity ).rgb;
			o.Smoothness = _Smoothness;
			float lerpResult80 = lerp( _WaveOpacity , _CrestOpacity , saturate( ase_vertex3Pos.y ));
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_viewPos = UnityObjectToViewPos( ase_vertex4Pos );
			float ase_screenDepth = -ase_viewPos.z;
			float cameraDepthFade85 = (( ase_screenDepth -_ProjectionParams.y - 0.0 ) / 1.0);
			float lerpResult86 = lerp( _ProximityOpacity , lerpResult80 , saturate( cameraDepthFade85 ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth89 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos )));
			float distanceDepth89 = abs( ( screenDepth89 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float lerpResult91 = lerp( _ShallowWaterOpacity , lerpResult86 , saturate( distanceDepth89 ));
			float Opacity83 = lerpResult91;
			o.Alpha = Opacity83;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
0;414;910;274;3022.025;49.73501;7.190309;True;False
Node;AmplifyShaderEditor.CommentaryNode;77;-1559.792,1166.77;Float;False;1863.509;656.5631;Normal from Textures;14;53;52;57;49;51;56;54;48;50;55;46;45;47;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;53;-1509.792,1556.511;Float;False;Property;_MacroNormalTiling;Macro Normal Tiling;18;0;Create;True;0;0;False;0;1;3;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;52;-1489.596,1234.632;Float;False;Property;_MicroNormalTiling;Micro Normal Tiling;17;0;Create;True;0;0;False;0;1;7;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;56;-1234.936,1338.499;Float;False;Property;_MicroNormalSpeed;Micro Normal Speed;31;0;Create;True;0;0;False;0;0,0;-0.14,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1241.444,1536.099;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;57;-1242.715,1662.333;Float;False;Property;_MacroNormalSpeed;Macro Normal Speed;32;0;Create;True;0;0;False;0;0,0;-0.14,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1215.748,1216.77;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-975.0963,1704.359;Float;False;Property;_MacroNormalScale;Macro Normal Scale;23;0;Create;True;0;0;False;0;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;24;-2223.104,-114.9923;Float;False;1196.128;1103.045;Gerstner Waves;15;14;18;19;20;9;21;22;23;8;15;16;17;7;26;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1004.537,1436.927;Float;False;Property;_MicroNormalScale;Micro Normal Scale;22;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;50;-896.5667,1565.476;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;93;744.6646,951.4387;Float;False;1946.656;531.1373;Opacity;14;78;82;81;85;79;80;88;89;87;90;86;92;91;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;48;-901.9659,1301.414;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;45;-678.4319,1315.47;Float;True;Property;_MicroNormal;Micro Normal;16;1;[NoScaleOffset];Create;True;0;0;False;0;None;a61bc28d4c595ac46a3384ff504e8e4d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-2111.382,187.8163;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;1.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;78;794.6646,1189.248;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-2113.789,258.5198;Float;False;Property;_Lenght;Lenght;8;0;Create;True;0;0;False;0;1;16.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;68;-916.8997,-256.4352;Float;False;942.9996;372.9999;HDRI;5;41;42;40;39;38;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;46;-673.2976,1512.886;Float;True;Property;_MacroNormal;Macro Normal;20;1;[NoScaleOffset];Create;True;0;0;False;0;None;74d198ff82eddb14ab34b3898c41d855;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CameraDepthFade;85;1290.259,1326.576;Float;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;23;-2128.83,813.53;Float;False;Property;_Direction4;Direction4;30;1;[HideInInspector];Create;True;0;0;False;0;1,0,0;0.9734805,0,0.2287703;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BlendNormalsNode;47;-287.6909,1419.14;Float;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;9;-2127.78,366.1645;Float;False;Property;_Direction1;Direction1;19;1;[HideInInspector];Create;True;0;0;False;0;1,0,0;0.9735968,0,0.2282753;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1739.581,559.0505;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;1052.396,1076.757;Float;False;Property;_WaveOpacity;Wave Opacity;28;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-1731.803,752.2523;Float;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;21;-2132.232,514.2909;Float;False;Property;_Direction2;Direction2;21;1;[HideInInspector];Create;True;0;0;False;0;1,0,0;0.9957835,0,-0.09173477;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;22;-2124.897,662.2285;Float;False;Property;_Direction3;Direction3;9;1;[HideInInspector];Create;True;0;0;False;0;0,0,0;0.7935833,0,-0.6084616;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;41;-866.8997,-206.4352;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;81;1058.972,1160.053;Float;False;Property;_CrestOpacity;Crest Opacity;25;0;Create;True;0;0;False;0;0;0.87;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2123.104,106.0077;Float;False;Property;_NumberofWaves;Number of Waves;10;0;Create;True;0;0;False;0;0;3.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2126.104,11.00769;Float;False;Property;_Amplitude;Amplitude;7;0;Create;True;0;0;False;0;1;0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2173.104,-64.99228;Float;False;Property;_Steepness;Steepness;0;0;Create;True;0;0;False;0;0;0.71;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;79;1083.287,1243.698;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1385.976,698.4561;Float;False;Gertner Wave;-1;;19;dfc9b7a09b52ca0458a54c329f79b3f1;0;6;33;FLOAT;0;False;35;FLOAT;1;False;42;FLOAT;1;False;20;FLOAT;1;False;14;FLOAT;1;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;12.716,1402.405;Float;False;NormalFromTextures;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;19;-1395.637,502.819;Float;False;Gertner Wave;-1;;15;dfc9b7a09b52ca0458a54c329f79b3f1;0;6;33;FLOAT;0;False;35;FLOAT;1;False;42;FLOAT;1;False;20;FLOAT;1;False;14;FLOAT;1;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;14;-1388.156,74.707;Float;False;Gertner Wave;-1;;20;dfc9b7a09b52ca0458a54c329f79b3f1;0;6;33;FLOAT;0;False;35;FLOAT;1;False;42;FLOAT;1;False;20;FLOAT;1;False;14;FLOAT;1;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;18;-1393.222,309.5972;Float;False;Gertner Wave;-1;;18;dfc9b7a09b52ca0458a54c329f79b3f1;0;6;33;FLOAT;0;False;35;FLOAT;1;False;42;FLOAT;1;False;20;FLOAT;1;False;14;FLOAT;1;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;40;-779.8997,-62.43533;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DepthFade;89;1830.984,1281.965;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;80;1416.009,1129.255;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;87;1559.482,1315.983;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;42;-682.8998,-166.4352;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;88;1429.528,1009.808;Float;False;Property;_ProximityOpacity;Proximity Opacity;27;0;Create;True;0;0;False;0;0;0.24;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;33;-1815.752,-688.5364;Float;False;779.6183;553.9546;Main Color;5;28;32;30;31;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-455.8465,271.0299;Float;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;86;1813.466,1082.083;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;28;-1780.752,-297.582;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;70;802.7724,24.33608;Float;False;727.0309;397.2697;Emission;4;63;67;64;65;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;92;1957.999,1001.439;Float;False;Property;_ShallowWaterOpacity;Shallow Water Opacity;29;0;Create;True;0;0;False;0;0;0.22;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;69;-34.41441,450.9976;Float;False;625.5699;569.2578;Local Vertex Normal;4;61;62;36;60;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;320.789,-275.2004;Float;False;58;NormalFromTextures;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;71;99.87901,-125.6621;Float;False;507.1796;326.9899;Specular/Smoothness;3;44;43;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;90;2100.959,1256.291;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;39;-528.9,-142.4353;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;32;-1614.919,-465.0048;Float;False;Property;_CrestColor;Crest Color;12;0;Create;True;0;0;False;0;0,0,0,0;0.4583482,0.9433962,0.7920914,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-302.9001,-170.4352;Float;True;Property;_HDRI;HDRI;14;1;[NoScaleOffset];Create;True;0;0;False;0;None;15cc2990ab3009945bf57cb9dcfce189;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;100;1097.082,556.7849;Float;False;Property;_ShallowWaterIntensity;Shallow Water Intensity;33;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;36;70.67204,671.412;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;102;697.2411,434.2355;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;44;149.879,-10.58933;Float;False;Property;_SpecularIntensity;Specular Intensity;15;0;Create;True;0;0;False;0;0;0.13;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;15.58559,905.2554;Float;False;Property;_VertexNormalIntensity;Vertex Normal Intensity;24;0;Create;True;0;0;False;0;0;0.24;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-1626.599,-638.5364;Float;False;Property;_WaveColor;Wave Color;11;0;Create;True;0;0;False;0;0,0,0,0;0.29106,0.3045241,0.594,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;30;-1479.764,-288.1356;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;63;852.7724,74.33607;Float;True;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;99.92435,500.9976;Float;False;Constant;_Color0;Color 0;23;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;101;955.9767,469.1443;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;91;2257.855,1090.479;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;60;407.1555,564.6579;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;64;1144.248,78.71898;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;2448.32,1049.994;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;1425.751,477.9637;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;67;1046.842,306.6057;Float;False;Property;_EmissiveIntensity;Emissive Intensity;26;0;Create;True;0;0;False;0;0;0.08;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;97;1446.886,589.2365;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;29;-1220.134,-432.4341;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;438.0586,-75.66214;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;103;1470.977,437.8662;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;72;575.6012,-430.8549;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;73;1358.656,647.2418;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;98;1672.657,433.328;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;1756.089,113.9447;Float;False;83;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;76;928.5137,-92.60361;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;1360.803,151.5539;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;169.2872,86.32776;Float;False;Property;_Smoothness;Smoothness;13;0;Create;True;0;0;False;0;0;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2164.752,-74.48822;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Clase08/Ocean Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;32;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;2;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;53;0
WireConnection;49;0;52;0
WireConnection;50;0;51;0
WireConnection;50;2;57;0
WireConnection;48;0;49;0
WireConnection;48;2;56;0
WireConnection;45;1;48;0
WireConnection;45;5;54;0
WireConnection;46;1;50;0
WireConnection;46;5;55;0
WireConnection;47;0;45;0
WireConnection;47;1;46;0
WireConnection;27;0;7;0
WireConnection;26;0;8;0
WireConnection;79;0;78;2
WireConnection;20;33;15;0
WireConnection;20;35;16;0
WireConnection;20;42;17;0
WireConnection;20;20;27;0
WireConnection;20;14;26;0
WireConnection;20;4;23;0
WireConnection;58;0;47;0
WireConnection;19;33;15;0
WireConnection;19;35;16;0
WireConnection;19;42;17;0
WireConnection;19;20;27;0
WireConnection;19;14;26;0
WireConnection;19;4;22;0
WireConnection;14;33;15;0
WireConnection;14;35;16;0
WireConnection;14;42;17;0
WireConnection;14;20;7;0
WireConnection;14;14;8;0
WireConnection;14;4;9;0
WireConnection;18;33;15;0
WireConnection;18;35;16;0
WireConnection;18;42;17;0
WireConnection;18;20;7;0
WireConnection;18;14;8;0
WireConnection;18;4;21;0
WireConnection;80;0;82;0
WireConnection;80;1;81;0
WireConnection;80;2;79;0
WireConnection;87;0;85;0
WireConnection;42;0;41;0
WireConnection;25;0;14;0
WireConnection;25;1;18;0
WireConnection;25;2;19;0
WireConnection;25;3;20;0
WireConnection;86;0;88;0
WireConnection;86;1;80;0
WireConnection;86;2;87;0
WireConnection;90;0;89;0
WireConnection;39;0;42;0
WireConnection;39;1;40;0
WireConnection;38;1;39;0
WireConnection;36;0;25;0
WireConnection;102;0;25;0
WireConnection;30;0;28;2
WireConnection;63;0;59;0
WireConnection;101;0;25;0
WireConnection;91;0;92;0
WireConnection;91;1;86;0
WireConnection;91;2;90;0
WireConnection;60;0;62;0
WireConnection;60;1;36;0
WireConnection;60;2;61;0
WireConnection;64;0;63;0
WireConnection;83;0;91;0
WireConnection;99;0;101;0
WireConnection;99;1;100;0
WireConnection;29;0;31;0
WireConnection;29;1;32;0
WireConnection;29;2;30;0
WireConnection;43;0;38;0
WireConnection;43;1;44;0
WireConnection;103;0;102;0
WireConnection;72;0;29;0
WireConnection;73;0;60;0
WireConnection;98;0;103;0
WireConnection;98;1;99;0
WireConnection;98;2;97;1
WireConnection;76;0;43;0
WireConnection;65;0;38;0
WireConnection;65;1;64;0
WireConnection;65;2;67;0
WireConnection;0;0;72;0
WireConnection;0;1;59;0
WireConnection;0;2;65;0
WireConnection;0;3;76;0
WireConnection;0;4;37;0
WireConnection;0;9;84;0
WireConnection;0;11;98;0
WireConnection;0;12;73;0
ASEEND*/
//CHKSM=AFA48FED9493BA22FA644996B0DB29EA17D7152F