// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flying Launcher"
{
	Properties
	{
		[NoScaleOffset][SingleLineTexture]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_AO("AO", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_NormalMap("NormalMap", 2D) = "bump" {}
		[HDR]_AOColorm("AO Colorm", Color) = (0,0,0,0)
		[NoScaleOffset][SingleLineTexture]_Specular("Specular", 2D) = "white" {}
		_Freq("Freq", Float) = 0
		_SpeedEmision("Speed Emision", Float) = 0
		[HDR]_ColorDamage("Color Damage", Color) = (0,0,0,0)
		_ColorLerp("ColorLerp", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _NormalMap;
		uniform sampler2D _Albedo;
		uniform sampler2D _AO;
		uniform float4 _AOColorm;
		uniform float _SpeedEmision;
		uniform float _Freq;
		uniform float4 _ColorDamage;
		uniform float _ColorLerp;
		uniform sampler2D _Specular;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_NormalMap10 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap10 ) );
			float2 uv_Albedo2 = i.uv_texcoord;
			float4 tex2DNode2 = tex2D( _Albedo, uv_Albedo2 );
			float2 uv_AO3 = i.uv_texcoord;
			float4 tex2DNode3 = tex2D( _AO, uv_AO3 );
			o.Albedo = ( tex2DNode2 * tex2DNode3 ).rgb;
			float mulTime2_g2 = _Time.y * _SpeedEmision;
			float temp_output_13_0_g2 = _Freq;
			float clampResult11_g2 = clamp( ( ( ( sin( ( mulTime2_g2 + temp_output_13_0_g2 ) ) + 1.0 ) * 0.5 ) / ( temp_output_13_0_g2 * 0.5 ) ) , 0.0 , 1.0 );
			float4 temp_output_9_0_g3 = _ColorDamage;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV4_g3 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4_g3 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV4_g3, 5.0 ) );
			float4 lerpResult10_g3 = lerp( float4( 0,0,0,0 ) , ( saturate( ( tex2DNode2 * temp_output_9_0_g3 ) ) + ( temp_output_9_0_g3 * fresnelNode4_g3 ) ) , _ColorLerp);
			o.Emission = ( ( ( 1.0 - tex2DNode3 ) * ( _AOColorm * clampResult11_g2 ) ) + lerpResult10_g3 ).rgb;
			float2 uv_Specular12 = i.uv_texcoord;
			o.Specular = tex2D( _Specular, uv_Specular12 ).rgb;
			o.Alpha = tex2DNode2.a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
			sampler3D _DitherMaskLOD;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;525;1387;474;1931.638;206.8963;1.33713;True;False
Node;AmplifyShaderEditor.RangedFloatNode;34;-1762.049,462.1602;Inherit;False;Property;_SpeedEmision;Speed Emision;6;0;Create;True;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1739.189,571.282;Inherit;False;Property;_Freq;Freq;5;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-1524.965,497.7361;Inherit;False;Sin Time Aura;-1;;2;6e752aecaf773d74a8ab3c614b704a38;0;2;12;FLOAT;0;False;13;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1591.366,-120.9699;Inherit;True;Property;_AO;AO;1;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;ec75845061c2fc043b2cf6fa625233f7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-1555.888,300.8314;Inherit;False;Property;_AOColorm;AO Colorm;3;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,2,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-1015.115,364.177;Inherit;False;Property;_ColorDamage;Color Damage;7;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;2,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1304.764,335.4981;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;7;-1274.373,18.55216;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1102.445,584.0044;Inherit;False;Property;_ColorLerp;ColorLerp;8;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1589.583,-321.3057;Inherit;True;Property;_Albedo;Albedo;0;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;f71a1d6596d11d14dad9070d8515484a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-857.262,745.8389;Inherit;True;Property;_Specular;Specular;4;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;d15991fc3ba826143b93c887cf3a1cee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1102.368,137.0645;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;38;-711.3185,348.9787;Inherit;False;Damage;-1;;3;40101450285c38e44b46157d2b8074e5;0;3;8;COLOR;0,0,0,0;False;9;COLOR;0,0,0,0;False;11;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-819.3885,-203.1631;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;10;-486.9147,-14.85354;Inherit;True;Property;_NormalMap;NormalMap;2;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;9d2846f50306310438a1b7df749535e7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-396.1115,206.1436;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;14;-158.74,399.5239;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;58.49999,-3.9;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Flying Launcher;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;12;34;0
WireConnection;44;13;26;0
WireConnection;23;0;5;0
WireConnection;23;1;44;0
WireConnection;7;0;3;0
WireConnection;8;0;7;0
WireConnection;8;1;23;0
WireConnection;38;8;2;0
WireConnection;38;9;40;0
WireConnection;38;11;41;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;48;0;8;0
WireConnection;48;1;38;0
WireConnection;14;0;12;0
WireConnection;0;0;4;0
WireConnection;0;1;10;0
WireConnection;0;2;48;0
WireConnection;0;3;14;0
WireConnection;0;9;2;4
ASEEND*/
//CHKSM=43E1DA9862396B783557CAE2855D51C97F2B1327