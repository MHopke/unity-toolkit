// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.13 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.13;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:35743,y:33007,varname:node_1,prsc:2|emission-532-RGB,alpha-563-OUT,clip-533-OUT;n:type:ShaderForge.SFN_Tex2d,id:341,x:34314,y:33617,varname:node_7279,prsc:2,ntxv:0,isnm:False|UVIN-515-OUT,TEX-622-TEX;n:type:ShaderForge.SFN_Rotator,id:342,x:33674,y:33816,varname:node_342,prsc:2|UVIN-347-UVOUT,ANG-443-OUT;n:type:ShaderForge.SFN_TexCoord,id:347,x:33469,y:33672,varname:node_347,prsc:2,uv:0;n:type:ShaderForge.SFN_Pi,id:441,x:33043,y:33883,varname:node_441,prsc:2;n:type:ShaderForge.SFN_Divide,id:443,x:33244,y:33914,varname:node_443,prsc:2|A-441-OUT,B-445-OUT;n:type:ShaderForge.SFN_Vector1,id:445,x:33042,y:34036,varname:node_445,prsc:2,v1:-2;n:type:ShaderForge.SFN_Multiply,id:485,x:34186,y:33174,varname:node_485,prsc:2|A-653-UVOUT,B-489-OUT;n:type:ShaderForge.SFN_Append,id:489,x:34003,y:33293,varname:node_489,prsc:2|A-689-OUT,B-501-OUT;n:type:ShaderForge.SFN_Tex2d,id:495,x:34463,y:33174,varname:node_1737,prsc:2,ntxv:0,isnm:False|UVIN-485-OUT,TEX-622-TEX;n:type:ShaderForge.SFN_Vector1,id:501,x:33786,y:33368,varname:node_501,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:515,x:33970,y:33783,varname:node_515,prsc:2|A-342-UVOUT,B-519-OUT;n:type:ShaderForge.SFN_Vector1,id:517,x:33502,y:34007,varname:node_517,prsc:2,v1:1;n:type:ShaderForge.SFN_Append,id:519,x:33719,y:34007,varname:node_519,prsc:2|A-517-OUT,B-529-OUT;n:type:ShaderForge.SFN_OneMinus,id:527,x:34633,y:33354,varname:node_527,prsc:2|IN-341-RGB;n:type:ShaderForge.SFN_Slider,id:528,x:32865,y:33648,ptovrint:False,ptlb:Slope,ptin:_Slope,varname:node_5461,prsc:2,min:-1,cur:-0.6666959,max:1;n:type:ShaderForge.SFN_Negate,id:529,x:33490,y:34113,varname:node_529,prsc:2|IN-689-OUT;n:type:ShaderForge.SFN_If,id:530,x:34983,y:33267,varname:node_530,prsc:2|A-689-OUT,B-531-OUT,GT-527-OUT,EQ-527-OUT,LT-495-RGB;n:type:ShaderForge.SFN_Vector1,id:531,x:34633,y:33548,varname:node_531,prsc:2,v1:0;n:type:ShaderForge.SFN_Color,id:532,x:35445,y:32781,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1711,prsc:2,glob:False,c1:0,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ComponentMask,id:533,x:35453,y:33269,varname:node_533,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-547-OUT;n:type:ShaderForge.SFN_OneMinus,id:547,x:35229,y:33269,varname:node_547,prsc:2|IN-530-OUT;n:type:ShaderForge.SFN_Tex2d,id:549,x:34934,y:32715,ptovrint:False,ptlb:Gradient,ptin:_Gradient,varname:node_7294,prsc:2,ntxv:0,isnm:False|UVIN-679-OUT;n:type:ShaderForge.SFN_Vector3,id:558,x:34934,y:32909,varname:node_558,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Lerp,id:559,x:35228,y:32998,varname:node_559,prsc:2|A-549-RGB,B-558-OUT,T-617-OUT;n:type:ShaderForge.SFN_ComponentMask,id:563,x:35453,y:33093,varname:node_563,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-559-OUT;n:type:ShaderForge.SFN_Slider,id:617,x:34777,y:33038,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_481,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:622,x:34031,y:33533,ptovrint:False,ptlb:Corners,ptin:_Corners,varname:node_1771,glob:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Rotator,id:653,x:33887,y:33085,varname:node_653,prsc:2|UVIN-655-UVOUT,ANG-441-OUT;n:type:ShaderForge.SFN_TexCoord,id:655,x:33700,y:32920,varname:node_655,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:679,x:34671,y:32715,varname:node_679,prsc:2|A-653-UVOUT,B-685-OUT;n:type:ShaderForge.SFN_Vector1,id:681,x:34096,y:32790,varname:node_681,prsc:2,v1:1;n:type:ShaderForge.SFN_Append,id:685,x:34463,y:32956,varname:node_685,prsc:2|A-681-OUT,B-686-OUT;n:type:ShaderForge.SFN_Slider,id:686,x:33939,y:32893,ptovrint:False,ptlb:GradientScale,ptin:_GradientScale,varname:node_4483,prsc:2,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ConstantClamp,id:689,x:33244,y:33518,varname:node_689,prsc:2,min:-1,max:1|IN-528-OUT;proporder:528-686-532-617-549-622;pass:END;sub:END;*/

Shader "Shader Forge/NewShader" {
    Properties {
        _Slope ("Slope", Range(-1, 1)) = -0.6666959
        _GradientScale ("GradientScale", Range(0, 1)) = 1
        _Color ("Color", Color) = (0,1,1,1)
        _Transparency ("Transparency", Range(0, 1)) = 0
        _Gradient ("Gradient", 2D) = "white" {}
        _Corners ("Corners", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float _Slope;
            uniform float4 _Color;
            uniform sampler2D _Gradient; uniform float4 _Gradient_ST;
            uniform float _Transparency;
            uniform sampler2D _Corners; uniform float4 _Corners_ST;
            uniform float _GradientScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float node_689 = clamp(_Slope,-1,1);
                float node_530_if_leA = step(node_689,0.0);
                float node_530_if_leB = step(0.0,node_689);
                float node_441 = 3.141592654;
                float node_653_ang = node_441;
                float node_653_spd = 1.0;
                float node_653_cos = cos(node_653_spd*node_653_ang);
                float node_653_sin = sin(node_653_spd*node_653_ang);
                float2 node_653_piv = float2(0.5,0.5);
                float2 node_653 = (mul(i.uv0-node_653_piv,float2x2( node_653_cos, -node_653_sin, node_653_sin, node_653_cos))+node_653_piv);
                float2 node_485 = (node_653*float2(node_689,1.0));
                float4 node_1737 = tex2D(_Corners,TRANSFORM_TEX(node_485, _Corners));
                float node_342_ang = (node_441/(-2.0));
                float node_342_spd = 1.0;
                float node_342_cos = cos(node_342_spd*node_342_ang);
                float node_342_sin = sin(node_342_spd*node_342_ang);
                float2 node_342_piv = float2(0.5,0.5);
                float2 node_342 = (mul(i.uv0-node_342_piv,float2x2( node_342_cos, -node_342_sin, node_342_sin, node_342_cos))+node_342_piv);
                float2 node_515 = (node_342*float2(1.0,(-1*node_689)));
                float4 node_7279 = tex2D(_Corners,TRANSFORM_TEX(node_515, _Corners));
                float3 node_527 = (1.0 - node_7279.rgb);
                clip((1.0 - lerp((node_530_if_leA*node_1737.rgb)+(node_530_if_leB*node_527),node_527,node_530_if_leA*node_530_if_leB)).r - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = _Color.rgb;
                float3 finalColor = emissive;
                float2 node_679 = (node_653*float2(1.0,_GradientScale));
                float4 _Gradient_var = tex2D(_Gradient,TRANSFORM_TEX(node_679, _Gradient));
                fixed4 finalRGBA = fixed4(finalColor,lerp(_Gradient_var.rgb,float3(0,0,0),_Transparency).r);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float _Slope;
            uniform sampler2D _Corners; uniform float4 _Corners_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float node_689 = clamp(_Slope,-1,1);
                float node_530_if_leA = step(node_689,0.0);
                float node_530_if_leB = step(0.0,node_689);
                float node_441 = 3.141592654;
                float node_653_ang = node_441;
                float node_653_spd = 1.0;
                float node_653_cos = cos(node_653_spd*node_653_ang);
                float node_653_sin = sin(node_653_spd*node_653_ang);
                float2 node_653_piv = float2(0.5,0.5);
                float2 node_653 = (mul(i.uv0-node_653_piv,float2x2( node_653_cos, -node_653_sin, node_653_sin, node_653_cos))+node_653_piv);
                float2 node_485 = (node_653*float2(node_689,1.0));
                float4 node_1737 = tex2D(_Corners,TRANSFORM_TEX(node_485, _Corners));
                float node_342_ang = (node_441/(-2.0));
                float node_342_spd = 1.0;
                float node_342_cos = cos(node_342_spd*node_342_ang);
                float node_342_sin = sin(node_342_spd*node_342_ang);
                float2 node_342_piv = float2(0.5,0.5);
                float2 node_342 = (mul(i.uv0-node_342_piv,float2x2( node_342_cos, -node_342_sin, node_342_sin, node_342_cos))+node_342_piv);
                float2 node_515 = (node_342*float2(1.0,(-1*node_689)));
                float4 node_7279 = tex2D(_Corners,TRANSFORM_TEX(node_515, _Corners));
                float3 node_527 = (1.0 - node_7279.rgb);
                clip((1.0 - lerp((node_530_if_leA*node_1737.rgb)+(node_530_if_leB*node_527),node_527,node_530_if_leA*node_530_if_leB)).r - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
