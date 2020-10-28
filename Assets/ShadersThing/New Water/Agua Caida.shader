// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Water/Caida"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 6.4
		_Border("Border", Float) = 0.9691849
		_ScalarSpeed("Scalar Speed", Range( 0 , 1)) = 0.45
		_Tiling("Tiling", Float) = 0
		_Waves("Waves", Float) = 0
		_Divide("Divide", Float) = 0
		_Speed("Speed", Float) = 0
		_Espuma("Espuma", Color) = (1,0,0,0)
		_BorderIntensity("Border Intensity", Float) = 0
		_BorderColor("BorderColor", Color) = (0,0,0,0)
		_WaterColor("Water Color", Color) = (0,0.6115854,1,0.5647059)
		_Offset("Offset", Vector) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
		};

		uniform float4 _WaterColor;
		uniform float4 _BorderColor;
		uniform float _BorderIntensity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Border;
		uniform float4 _Espuma;
		uniform float _ScalarSpeed;
		uniform float _Speed;
		uniform float _Waves;
		uniform float2 _Offset;
		uniform float _Tiling;
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
			return (F2 + F1) * 0.5;
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ColorWater173 = _WaterColor;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth225 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth225 = abs( ( screenDepth225 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.24 ) );
			float4 BorderAndTransparency154 = ( ( _BorderColor * _BorderIntensity ) * step( distanceDepth225 , _Border ) );
			o.Albedo = ( ColorWater173 + BorderAndTransparency154 ).rgb;
			float mulTime28 = _Time.y * _Speed;
			float temp_output_30_0 = ( sin( ( _ScalarSpeed + mulTime28 ) ) + _Waves );
			float time65 = temp_output_30_0;
			float3 ase_worldPos = i.worldPos;
			float2 panner227 = ( _Time.y * _Offset + ( ase_worldPos * _Tiling ).xy);
			float2 coords65 = panner227 * 1.0;
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
			o.Emission = ( _Espuma * step( voroi65 , ( temp_output_30_0 / _Divide ) ) ).rgb;
			float Opacity160 = _WaterColor.a;
			o.Alpha = Opacity160;
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
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
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
Version=18600
0;388;1347;611;2632.568;-256.7147;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;34;-2159.396,1227.086;Inherit;False;Property;_Speed;Speed;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;159;-3721.33,-821.3095;Inherit;False;1390.388;614.8828;Comment;12;160;173;154;129;44;188;14;15;189;190;16;225;Border  and Transparency;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2153.313,993.4292;Inherit;False;Property;_ScalarSpeed;Scalar Speed;6;0;Create;True;0;0;False;0;False;0.45;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-1958.903,1186.501;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-3681.286,-331.187;Inherit;False;Constant;_TreshBorder;Tresh Border;5;0;Create;True;0;0;False;0;False;0.24;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1704.58,536.4914;Inherit;False;Property;_Tiling;Tiling;7;0;Create;True;0;0;False;0;False;0;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;225;-3482.727,-419.6505;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1730.023,1064.029;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;190;-3398.64,-691.7419;Inherit;False;Property;_BorderColor;BorderColor;14;0;Create;True;0;0;False;0;False;0,0,0,0;0.145098,0.572549,0.2078432,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;226;-1737.176,336.5469;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;189;-3387.39,-508.9352;Inherit;False;Property;_BorderIntensity;Border Intensity;13;0;Create;True;0;0;False;0;False;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-3364.643,-322.4268;Inherit;False;Property;_Border;Border;5;0;Create;True;0;0;False;0;False;0.9691849;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-2049.458,744.8542;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;27;-1600.823,1047.418;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;14;-3161.131,-439.6948;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-1547.257,419.4711;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1434.254,1152.386;Inherit;False;Property;_Waves;Waves;8;0;Create;True;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;45;-2205.139,490.0922;Inherit;False;Property;_Offset;Offset;16;0;Create;True;0;0;False;0;False;0,0;0,0.61;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-3106.148,-538.4653;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1270.784,955.81;Inherit;False;Property;_Divide;Divide;9;0;Create;True;0;0;False;0;False;0;12.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-2857.799,-564.7538;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;-3692.968,-766.594;Inherit;False;Property;_WaterColor;Water Color;15;0;Create;True;0;0;False;0;False;0,0.6115854,1,0.5647059;0,1,0.3686275,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1311.494,817.4011;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;227;-1340.368,479.0147;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;-3384.697,-781.7667;Inherit;False;ColorWater;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;65;-1140.171,413.412;Inherit;True;0;0;1;3;8;False;49;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-2624.09,-561.08;Inherit;False;BorderAndTransparency;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;32;-966.2619,650.4512;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;158;-3731.364,-105.8232;Inherit;False;1057.477;481.8657;Comment;4;119;191;120;124;Foam Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;-3679.094,-561.3154;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;19;-783.4719,387.2538;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;125;-743.4924,120.5193;Inherit;False;Property;_Espuma;Espuma;12;0;Create;True;0;0;False;0;False;1,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;155;-433.8011,-65.70129;Inherit;False;154;BorderAndTransparency;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;152;-3696.076,-1432.623;Inherit;False;868.5426;481.8652;Comment;4;50;79;80;151;Fresnel Water;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;174;-392.4359,-287.0606;Inherit;False;173;ColorWater;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1955.846,601.4582;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;79;-3656.389,-1156.346;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.15;False;2;FLOAT;2.06;False;3;FLOAT;11.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-109.4857,-98.91241;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;-477.5808,-207.7964;Inherit;False;151;FresnelWater;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;50;-3610.748,-1382.623;Inherit;False;Property;_ReflexColor;Reflex Color;11;0;Create;True;0;0;False;0;False;1,0,0.01978827,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-446.9323,252.3552;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-35.10169,248.1224;Inherit;False;160;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-3072.525,-1206.321;Inherit;False;FresnelWater;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;120;-3422.373,78.50285;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-2897.964,70.45087;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-3347.942,-1218.436;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;119;-3653.53,-5.031639;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceViewDirHlpNode;191;-3682.192,152.8886;Inherit;False;1;0;FLOAT4;0,0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;224;-2012.988,263.3542;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;157.2874,-0.3692107;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;MyShaders/Water/Caida;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;6.4;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;34;0
WireConnection;225;0;16;0
WireConnection;29;0;20;0
WireConnection;29;1;28;0
WireConnection;27;0;29;0
WireConnection;14;0;225;0
WireConnection;14;1;15;0
WireConnection;147;0;226;0
WireConnection;147;1;25;0
WireConnection;188;0;190;0
WireConnection;188;1;189;0
WireConnection;129;0;188;0
WireConnection;129;1;14;0
WireConnection;30;0;27;0
WireConnection;30;1;31;0
WireConnection;227;0;147;0
WireConnection;227;2;45;0
WireConnection;227;1;38;0
WireConnection;173;0;44;0
WireConnection;65;0;227;0
WireConnection;65;1;30;0
WireConnection;154;0;129;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;160;0;44;4
WireConnection;19;0;65;0
WireConnection;19;1;32;0
WireConnection;24;0;174;0
WireConnection;24;1;155;0
WireConnection;85;0;125;0
WireConnection;85;1;19;0
WireConnection;151;0;80;0
WireConnection;120;0;119;0
WireConnection;120;1;191;0
WireConnection;124;0;120;0
WireConnection;80;0;50;0
WireConnection;80;1;79;0
WireConnection;0;0;24;0
WireConnection;0;2;85;0
WireConnection;0;9;162;0
ASEEND*/
//CHKSM=0C627A3162D16B747D8E9B966E562B5926F6595C