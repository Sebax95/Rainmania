// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clase05/ToonWater"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		[NoScaleOffset]_Pattern("Pattern", 2D) = "white" {}
		_SpeedX("SpeedX", Float) = 0
		_SpeedY("SpeedY", Float) = 0
		[NoScaleOffset]_Flowmap("Flowmap", 2D) = "white" {}
		_FlowmapIntensity("Flowmap Intensity", Range( 0 , 1)) = 0.2470588
		_Tiling("Tiling", Float) = 0
		_MainOpacity("Main Opacity", Range( 0 , 1)) = 0
		_Offset("Offset", Vector) = (0,0,1,0)
		_DepthDistance("Depth Distance", Float) = 0
		_FallOff("FallOff", Float) = 0
		_Color0("Color 0", Color) = (0.8584906,0,0,0)
		[NoScaleOffset]_Foam("Foam", 2D) = "white" {}
		_FoamOpacity("Foam Opacity", Range( 0 , 1)) = 0
		_Float0("Float 0", Float) = 1.36
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
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float3 _Offset;
		uniform float _Float0;
		uniform float4 _Color0;
		uniform sampler2D _Foam;
		uniform float _SpeedX;
		uniform float _SpeedY;
		uniform float _Tiling;
		uniform sampler2D _Flowmap;
		uniform float _FlowmapIntensity;
		uniform sampler2D _Pattern;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _FallOff;
		uniform float _FoamOpacity;
		uniform float _MainOpacity;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			v.vertex.xyz += ( _Offset * _Float0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult9 = (float2(_SpeedX , _SpeedY));
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord16 = i.uv_texcoord * temp_cast_0;
			float4 lerpResult17 = lerp( float4( uv_TexCoord16, 0.0 , 0.0 ) , tex2D( _Flowmap, uv_TexCoord16 ) , _FlowmapIntensity);
			float2 panner3 = ( _Time.y * appendResult9 + lerpResult17.rg);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth30 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth30 = abs( ( screenDepth30 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float temp_output_35_0 = saturate( pow( distanceDepth30 , _FallOff ) );
			float4 lerpResult36 = lerp( tex2D( _Foam, panner3 ) , tex2D( _Pattern, panner3 ) , temp_output_35_0);
			o.Emission = ( _Color0 * lerpResult36 ).rgb;
			float lerpResult37 = lerp( _FoamOpacity , _MainOpacity , temp_output_35_0);
			o.Alpha = lerpResult37;
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
Version=16700
-3;756;1369;253;-594.1754;53.6754;1.361405;True;False
Node;AmplifyShaderEditor.CommentaryNode;25;-1800.593,-497.2836;Float;False;1129.108;504.6927;Flowmap;5;23;16;18;10;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1750.593,-409.299;Float;False;Property;_Tiling;Tiling;10;0;Create;True;0;0;False;0;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;24;-576.4849,-184.378;Float;False;654.7547;376.9999;Panner;5;7;8;9;3;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1543.223,-447.2836;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-1298.633,-310.1776;Float;True;Property;_Flowmap;Flowmap;8;1;[NoScaleOffset];Create;True;0;0;False;0;None;d17e734c4d4a9ae4c8a99e0a1d4b9c49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-526.2838,-88.73058;Float;False;Property;_SpeedX;SpeedX;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-526.4849,-17.6802;Float;False;Property;_SpeedY;SpeedY;7;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1272.184,-124.4909;Float;False;Property;_FlowmapIntensity;Flowmap Intensity;9;0;Create;True;0;0;False;0;0.2470588;0.117;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-210.5877,347.7536;Float;False;Property;_DepthDistance;Depth Distance;13;0;Create;True;0;0;False;0;0;0.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-358.4221,-78.33338;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;17;-936.4848,-343.5906;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;30;28.41227,325.7536;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;144.3675,425.7446;Float;False;Property;_FallOff;FallOff;14;0;Create;True;0;0;False;0;0;1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-364.7316,60.7358;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;40;240.8188,-448.1031;Float;False;651.5063;494.7853;Textures;3;34;1;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;3;-192.7302,-134.3779;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;32;341.2872,323.3892;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;294.3861,-183.3179;Float;True;Property;_Pattern;Pattern;5;1;[NoScaleOffset];Create;True;0;0;False;0;None;64dc8695dea5f0143a597ed01e6d9b2b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;290.8188,-398.1031;Float;True;Property;_Foam;Foam;16;1;[NoScaleOffset];Create;True;0;0;False;0;None;4fa7d42e48b8cc1448e6d6c076728c1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;35;542.2618,324.4274;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;41;813.3939,73.82205;Float;False;546.1644;294.8972;Opacity;3;29;38;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;876.3577,655.2776;Float;False;Property;_Float0;Float 0;18;0;Create;True;0;0;False;0;1.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;863.5368,123.8221;Float;False;Property;_FoamOpacity;Foam Opacity;17;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;863.3939,196.3922;Float;False;Property;_MainOpacity;Main Opacity;11;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;708.3251,-292.2165;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;54;1033.51,-398.4442;Float;False;Property;_Color0;Color 0;15;0;Create;True;0;0;False;0;0.8584906,0,0,0;0.09420612,0.2735849,0.1693425,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;42;874.7231,492.1282;Float;False;Property;_Offset;Offset;12;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;37;1175.558,212.7193;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1309.236,-377.1173;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;1342.274,477.2726;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1581.055,-262.1571;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Clase05/ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;23;0
WireConnection;10;1;16;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;17;0;16;0
WireConnection;17;1;10;0
WireConnection;17;2;18;0
WireConnection;30;0;31;0
WireConnection;3;0;17;0
WireConnection;3;2;9;0
WireConnection;3;1;5;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;1;1;3;0
WireConnection;34;1;3;0
WireConnection;35;0;32;0
WireConnection;36;0;34;0
WireConnection;36;1;1;0
WireConnection;36;2;35;0
WireConnection;37;0;38;0
WireConnection;37;1;29;0
WireConnection;37;2;35;0
WireConnection;55;0;54;0
WireConnection;55;1;36;0
WireConnection;57;0;42;0
WireConnection;57;1;43;0
WireConnection;0;2;55;0
WireConnection;0;9;37;0
WireConnection;0;11;57;0
ASEEND*/
//CHKSM=FF6709AD3E34E94FBB987DB3EA205187CB1B11DA