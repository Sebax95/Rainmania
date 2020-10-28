// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Bullet/Aura"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 3.4
		_Intensity("Intensity", Float) = 0
		_Speed("Speed", Float) = 0
		_Min("Min", Range( 0 , 1)) = 0
		_Max("Max", Range( 0 , 1)) = 0
		_Dir("Dir", Vector) = (0,0,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		_Div("Div", Range( 0 , 1)) = 0
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
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _Offset;
		uniform float3 _Dir;
		uniform float _Min;
		uniform float _Max;
		uniform float _Speed;
		uniform float _Intensity;
		uniform float _Div;
		uniform float _TessValue;


		float2 voronoihash23( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi23( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash23( n + g );
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
			float3 ase_vertex3Pos = v.vertex.xyz;
			float dotResult3 = dot( ( ase_vertex3Pos + _Offset ) , _Dir );
			float mulTime27 = _Time.y * _Speed;
			float time23 = mulTime27;
			float2 coords23 = v.texcoord.xy * 5.15;
			float2 id23 = 0;
			float2 uv23 = 0;
			float voroi23 = voronoi23( coords23, time23, id23, uv23, 0 );
			float smoothstepResult32 = smoothstep( _Min , _Max , voroi23);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( 1.0 - saturate( dotResult3 ) ) * smoothstepResult32 ) * ase_vertexNormal * _Intensity );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color5 = IsGammaSpace() ? float4(0,1,0,0) : float4(0,1,0,0);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult3 = dot( ( ase_vertex3Pos + _Offset ) , _Dir );
			o.Emission = ( color5 * saturate( dotResult3 ) ).rgb;
			float normalizeResult15 = normalize( dotResult3 );
			o.Alpha = ( normalizeResult15 * _Div );
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
0;586;1347;413;1989.691;203.97;1.780855;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1243.542,-131.6827;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;11;-1307.49,33.47659;Inherit;False;Property;_Offset;Offset;10;0;Create;True;0;0;False;0;False;0,0,0;0.35,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;28;-1355.502,833.4039;Inherit;False;Property;_Speed;Speed;6;0;Create;True;0;0;False;0;False;0;5.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;1;-1192.787,176.4727;Inherit;False;Property;_Dir;Dir;9;0;Create;True;0;0;False;0;False;0,0,0;2.49,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1043.896,-70.27853;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;3;-842.5422,-15.30868;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1151.631,510.2747;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1171.352,843.5405;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;18;-760.1108,308.2495;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;23;-890.5421,566.2023;Inherit;True;0;0;1;3;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;5.15;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;34;-960.1343,950.4703;Inherit;False;Property;_Max;Max;8;0;Create;True;0;0;False;0;False;0;0.907;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-968.3764,846.9064;Inherit;False;Property;_Min;Min;7;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-623.0689,327.2021;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;32;-616.4985,579.4552;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.09;False;2;FLOAT;0.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-600.6608,199.1047;Inherit;False;Property;_Div;Div;11;0;Create;True;0;0;False;0;False;0;0.516;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;24;-257.5569,743.067;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-413.7001,-194.049;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;False;0,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;29;-425.5116,-2.372914;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-58.19043,930.1237;Inherit;False;Property;_Intensity;Intensity;5;0;Create;True;0;0;False;0;False;0;0.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;15;-628.968,75.33334;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-271.5385,488.1231;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-113.6125,-38.9659;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-313.0151,110.6881;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.66;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-55.96764,478.8638;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;145.5214,27.0254;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;MyShaders/Bullet/Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;3.4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;2;0
WireConnection;9;1;11;0
WireConnection;3;0;9;0
WireConnection;3;1;1;0
WireConnection;27;0;28;0
WireConnection;18;0;3;0
WireConnection;23;0;21;0
WireConnection;23;1;27;0
WireConnection;19;0;18;0
WireConnection;32;0;23;0
WireConnection;32;1;33;0
WireConnection;32;2;34;0
WireConnection;29;0;3;0
WireConnection;15;0;3;0
WireConnection;22;0;19;0
WireConnection;22;1;32;0
WireConnection;4;0;5;0
WireConnection;4;1;29;0
WireConnection;16;0;15;0
WireConnection;16;1;13;0
WireConnection;26;0;22;0
WireConnection;26;1;24;0
WireConnection;26;2;25;0
WireConnection;0;2;4;0
WireConnection;0;9;16;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=876D871625FD01D3ACAD900EA0543F9091D972F2