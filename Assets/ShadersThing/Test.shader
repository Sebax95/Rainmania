// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 14.2
		_Border("Border", Float) = 0.9691849
		_Float2("Float 2", Range( 0 , 1)) = 0.45
		_Tiling("Tiling", Float) = 0
		_Waves("Waves", Float) = 0
		_Divide("Divide", Float) = 0
		_Speed("Speed", Float) = 0
		_ReflexColor("Reflex Color", Color) = (1,0,0.01978827,0)
		_Espuma("Espuma", Color) = (0.8699616,1,0,0)
		_EspumaInterna("Espuma Interna", Color) = (1,0,0,0)
		_WaterColor("Water Color", Color) = (0,0.6115854,1,0.5647059)
		_Offset("Offset", Vector) = (0,0,0,0)
		_WavesTess("Waves Tess", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
			float2 uv_texcoord;
		};

		uniform float _Float2;
		uniform float _Speed;
		uniform float _Waves;
		uniform float _Tiling;
		uniform float2 _Offset;
		uniform float _WavesTess;
		uniform float4 _WaterColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Border;
		uniform float4 _ReflexColor;
		uniform float4 _Espuma;
		uniform float4 _EspumaInterna;
		uniform float _Divide;
		uniform float _TessValue;


		float2 voronoihash65( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi65( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash65( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2 - F1;
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime28 = _Time.y * _Speed;
			float temp_output_30_0 = ( sin( ( _Float2 + mulTime28 ) ) + _Waves );
			float time65 = temp_output_30_0;
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord23 = v.texcoord.xy * temp_cast_0 + ( _Offset * _Time.y );
			float2 coords65 = uv_TexCoord23 * 1.0;
			float2 id65 = 0;
			float2 uv65 = 0;
			float fade65 = 0.5;
			float voroi65 = 0;
			float rest65 = 0;
			for( int it65 = 0; it65 <8; it65++ ){
			voroi65 += fade65 * voronoi65( coords65, time65, id65, uv65, 0 );
			rest65 += fade65;
			coords65 *= 2;
			fade65 *= 0.5;
			}//Voronoi65
			voroi65 /= rest65;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( 1.0 - voroi65 ) * _WavesTess * ase_vertexNormal );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _WaterColor.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth13 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth13 = abs( ( screenDepth13 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.24 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV79 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode79 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV79, 1.95 ) );
			float fresnelNdotV87 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode87 = ( 0.14 + 0.58 * pow( 1.0 - fresnelNdotV87, 1.24 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float dotResult120 = dot( ase_vertexNormal , i.viewDir );
			float mulTime28 = _Time.y * _Speed;
			float temp_output_30_0 = ( sin( ( _Float2 + mulTime28 ) ) + _Waves );
			float time65 = temp_output_30_0;
			float2 temp_cast_1 = (_Tiling).xx;
			float2 uv_TexCoord23 = i.uv_texcoord * temp_cast_1 + ( _Offset * _Time.y );
			float2 coords65 = uv_TexCoord23 * 1.0;
			float2 id65 = 0;
			float2 uv65 = 0;
			float fade65 = 0.5;
			float voroi65 = 0;
			float rest65 = 0;
			for( int it65 = 0; it65 <8; it65++ ){
			voroi65 += fade65 * voronoi65( coords65, time65, id65, uv65, 0 );
			rest65 += fade65;
			coords65 *= 2;
			fade65 *= 0.5;
			}//Voronoi65
			voroi65 /= rest65;
			o.Emission = ( ( ( _WaterColor * ( 1.0 - _WaterColor ) ) * step( distanceDepth13 , _Border ) ) + ( _ReflexColor * fresnelNode79 ) + ( saturate( ( ( _Espuma * fresnelNode87 ) + ( dotResult120 * _EspumaInterna ) ) ) * step( voroi65 , ( temp_output_30_0 / _Divide ) ) ) ).rgb;
			o.Alpha = _WaterColor.a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
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
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
0;578;1369;423;3219.474;312.6565;3.445071;True;False
Node;AmplifyShaderEditor.RangedFloatNode;34;-1909.275,1176.856;Inherit;False;Property;_Speed;Speed;10;0;Create;True;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-1708.782,1136.271;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1753.758,905.8401;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;False;0;False;0.45;0.476;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1479.902,1013.799;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;27;-1350.702,997.1876;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1184.133,1102.156;Inherit;False;Property;_Waves;Waves;8;0;Create;True;0;0;False;0;False;0;1.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1024.075,990.5066;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-2021.241,814.5072;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;122;-1951.177,461.8065;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;119;-1977.796,327.2142;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;45;-2044.122,651.1051;Inherit;False;Property;_Offset;Offset;15;0;Create;True;0;0;False;0;False;0,0;0.17,0.23;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;86;-1552.064,86.71133;Inherit;False;Property;_Espuma;Espuma;12;0;Create;True;0;0;False;0;False;0.8699616,1,0,0;0,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1773.69,618.2781;Inherit;False;Property;_Tiling;Tiling;7;0;Create;True;0;0;False;0;False;0;9.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;125;-1486.251,491.4729;Inherit;False;Property;_EspumaInterna;Espuma Interna;12;0;Create;True;0;0;False;0;False;1,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;120;-1760.294,393.8148;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;83;-934.1113,970.12;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1691.764,-165.3446;Inherit;False;Constant;_TreshBorder;Tresh Border;0;0;Create;True;0;0;False;0;False;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;87;-1557.012,258.6086;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.14;False;2;FLOAT;0.58;False;3;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1771.481,750.9058;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;44;-1661.532,-482.3788;Inherit;False;Property;_WaterColor;Water Color;14;0;Create;True;0;0;False;0;False;0,0.6115854,1,0.5647059;0,0.3755063,0.9607843,0.7137255;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;136;-1413.996,-286.1522;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1385.077,-33.4957;Inherit;False;Property;_Border;Border;5;0;Create;True;0;0;False;0;False;0.9691849;2.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1005.088,1169.713;Inherit;False;Property;_Divide;Divide;9;0;Create;True;0;0;False;0;False;0;20.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1268.125,79.00531;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;84;-1094.5,872.8064;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-1235.885,385.7629;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1558.098,682.9669;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;13;-1437.687,-164.6932;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-965.4923,123.7445;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-1184.78,-317.9225;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.7184393,1,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;32;-825.587,1010.456;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;65;-1261.259,723.6532;Inherit;True;0;0;1;2;8;False;49;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.StepOpNode;14;-1158.147,-60.86013;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;79;-795.0977,-125.6861;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1.95;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;116;-667.5356,211.2783;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-953.5389,-143.7534;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;50;-759.77,-350.5514;Inherit;False;Property;_ReflexColor;Reflex Color;11;0;Create;True;0;0;False;0;False;1,0,0.01978827,0;0.3921568,1,0.5166987,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;19;-638.4916,685.2389;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;43;-704.1442,1147.449;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;127;-612.124,121.9886;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;42;-665.2031,940.2347;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-377.4635,214.5269;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-506.2975,-61.08611;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-664.891,1056.605;Inherit;False;Property;_WavesTess;Waves Tess;16;0;Create;True;0;0;False;0;False;1;0.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-202.4201,149.2551;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-380.954,909.521;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;142;-385.6685,-375.7814;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;143;-281.8244,-432.7792;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;109.3624,22.24321;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Test;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;14.2;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;34;0
WireConnection;29;0;20;0
WireConnection;29;1;28;0
WireConnection;27;0;29;0
WireConnection;30;0;27;0
WireConnection;30;1;31;0
WireConnection;120;0;119;0
WireConnection;120;1;122;0
WireConnection;83;0;30;0
WireConnection;39;0;45;0
WireConnection;39;1;38;0
WireConnection;136;0;44;0
WireConnection;88;0;86;0
WireConnection;88;1;87;0
WireConnection;84;0;83;0
WireConnection;124;0;120;0
WireConnection;124;1;125;0
WireConnection;23;0;25;0
WireConnection;23;1;39;0
WireConnection;13;0;16;0
WireConnection;117;0;88;0
WireConnection;117;1;124;0
WireConnection;134;0;44;0
WireConnection;134;1;136;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;65;0;23;0
WireConnection;65;1;84;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;116;0;117;0
WireConnection;129;0;134;0
WireConnection;129;1;14;0
WireConnection;19;0;65;0
WireConnection;19;1;32;0
WireConnection;127;0;129;0
WireConnection;42;0;65;0
WireConnection;85;0;116;0
WireConnection;85;1;19;0
WireConnection;80;0;50;0
WireConnection;80;1;79;0
WireConnection;24;0;127;0
WireConnection;24;1;80;0
WireConnection;24;2;85;0
WireConnection;40;0;42;0
WireConnection;40;1;41;0
WireConnection;40;2;43;0
WireConnection;142;0;44;4
WireConnection;143;0;44;0
WireConnection;0;0;143;0
WireConnection;0;2;24;0
WireConnection;0;9;142;0
WireConnection;0;11;40;0
ASEEND*/
//CHKSM=FBD484A84C95C4C17A521B49FC9F28928232A86B