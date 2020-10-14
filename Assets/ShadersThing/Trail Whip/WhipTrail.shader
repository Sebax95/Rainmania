// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Whip/WhipTrail"
{
	Properties
	{
		[NoScaleOffset][SingleLineTexture]_Gradient("Gradient", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_MaskGradient("MaskGradient", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (0,0,0,0)
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
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
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color;
		uniform sampler2D _Gradient;
		uniform sampler2D _MaskGradient;


		inline float2 UnityVoronoiRandomVector( float2 UV, float offset )
		{
			float2x2 m = float2x2( 15.27, 47.63, 99.41, 89.98 );
			UV = frac( sin(mul(UV, m) ) * 46839.32 );
			return float2( sin(UV.y* +offset ) * 0.5 + 0.5, cos( UV.x* offset ) * 0.5 + 0.5 );
		}
		
		//x - Out y - Cells
		float3 UnityVoronoi( float2 UV, float AngleOffset, float CellDensity, inout float2 mr )
		{
			float2 g = floor( UV * CellDensity );
			float2 f = frac( UV * CellDensity );
			float t = 8.0;
			float3 res = float3( 8.0, 0.0, 0.0 );
		
			for( int y = -1; y <= 1; y++ )
			{
				for( int x = -1; x <= 1; x++ )
				{
					float2 lattice = float2( x, y );
					float2 offset = UnityVoronoiRandomVector( lattice + g, AngleOffset );
					float d = distance( lattice + offset, f );
		
					if( d < res.x )
					{
						mr = f - lattice - offset;
						res = float3( d, offset.x, offset.y );
					}
				}
			}
			return res;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Gradient50 = i.uv_texcoord;
			float mulTime60 = _Time.y * i.uv2_tex4coord2.z;
			float2 uv57 = 0;
			float3 unityVoronoy57 = UnityVoronoi(i.uv_texcoord,mulTime60,i.uv2_tex4coord2.w,uv57);
			float4 temp_cast_0 = (i.uv2_tex4coord2.x).xxxx;
			float4 temp_output_63_0 = pow( ( tex2D( _Gradient, uv_Gradient50 ) * unityVoronoy57.x ) , temp_cast_0 );
			o.Emission = ( ( ( _Color * temp_output_63_0 ) * i.uv2_tex4coord2.y ) * i.vertexColor ).rgb;
			float2 uv_MaskGradient51 = i.uv_texcoord;
			o.Alpha = ( ( tex2D( _MaskGradient, uv_MaskGradient51 ) * temp_output_63_0 ) * i.vertexColor.a ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv2_tex4coord2;
				o.customPack2.xyzw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv2_tex4coord2 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
0;540;1341;459;4193.024;636.6981;3.873116;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;74;-2495.841,463.6692;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;60;-2105.991,461.3181;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;57;-1749.081,423.788;Inherit;True;0;0;1;0;3;False;1;True;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.WireNode;81;-1690.203,740.699;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-2194.531,55.5141;Inherit;True;Property;_Gradient;Gradient;0;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;a61a9ba640336814faa52bd50966fa24;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;83;-1149.673,474.6531;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1398.665,291.5365;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;53;-1121.079,-319.3289;Inherit;False;Property;_Color;Color;2;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;2.670157,2.670157,2.670157,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;63;-1062.514,296.8293;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;79;-2100.358,391.6087;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;82;-1098.591,165.2874;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-872.3807,-202.8648;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;51;-1065.653,595.1349;Inherit;True;Property;_MaskGradient;MaskGradient;1;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;cbe358d16e48f6349bbd3b525b5ea992;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;55;-762.592,30.22517;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-666.2517,-203.2759;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-553.7355,382.6047;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-491.157,-131.1562;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-287.6915,308.5751;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;112;-91.10844,-72.15398;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;MyShaders/Whip/WhipTrail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;0;74;3
WireConnection;57;1;60;0
WireConnection;57;2;74;4
WireConnection;81;0;74;1
WireConnection;83;0;81;0
WireConnection;58;0;50;0
WireConnection;58;1;57;0
WireConnection;63;0;58;0
WireConnection;63;1;83;0
WireConnection;79;0;74;2
WireConnection;82;0;79;0
WireConnection;54;0;53;0
WireConnection;54;1;63;0
WireConnection;73;0;54;0
WireConnection;73;1;82;0
WireConnection;65;0;51;0
WireConnection;65;1;63;0
WireConnection;56;0;73;0
WireConnection;56;1;55;0
WireConnection;66;0;65;0
WireConnection;66;1;55;4
WireConnection;112;2;56;0
WireConnection;112;9;66;0
ASEEND*/
//CHKSM=A856E41969968F8F87412DA758344FF45C8B64E6