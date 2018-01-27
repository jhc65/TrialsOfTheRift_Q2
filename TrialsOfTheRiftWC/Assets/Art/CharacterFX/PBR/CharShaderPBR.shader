  Shader "Character/CharShaderPBR" {
    Properties {
      _Color ("Main Color", Color) = (1,1,1,1)
      _MainTex ("Texture", 2D) = "white" {}
      _MaskTex ("MaskTex (RGBA)", 2D) = "black" {}
      _MetallicTex("Metallic (RGB)", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
      _EyeColor   ("Eye Color", Color) = (0.5,0.5,0.5,1)
      _SkinColor  ("Skin Color", Color) = (0.5,0.5,0.5,1)
      _HairColor  ("Hair Color", Color) = (0.5,0.5,0.5,1)
      _Glossiness ("Smoothness", Range(0,1)) = 0.5
      _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM

      #pragma surface surf Standard fullforwardshadows
      #pragma target 3.0
               
      struct Input {
        float2 uv_MainTex;
        float2 uv_BumpMap;
      };
      
      sampler2D _MainTex;
      sampler2D _BumpMap;
      sampler2D _MaskTex;
      sampler2D _MetallicTex;

      fixed4    _Color;           
      fixed4    _EyeColor;
      fixed4    _SkinColor;
      fixed4    _HairColor;
      half      _Glossiness;
      half      _Metallic;

              
      void surf (Input IN, inout SurfaceOutputStandard o) 
      {
      	float4 basecol = tex2D (_MainTex, IN.uv_MainTex) * _Color;
      	float4 maskcol = tex2D (_MaskTex, IN.uv_MainTex);
      	float3 newcol;
      
      	if (maskcol.r > 0)
      	{
      		float3 graycol = dot(basecol.rgb,float3(0.3,0.59,0.11));
      		newcol = graycol * 2 * _EyeColor.rgb;
      		basecol.rgb = lerp(basecol,newcol,maskcol.r); 
      	}
      	
      	if (maskcol.g > 0)
      	{
      	    newcol = basecol * 2 * _SkinColor.rgb;
      		basecol.rgb = lerp(basecol,newcol,maskcol.g);
      	}
      	if (maskcol.b > 0)
      	{
      		newcol = basecol * 2 * _HairColor.rgb;
      		basecol.rgb = lerp(basecol,newcol,maskcol.b);
      	}
      	      	
         o.Albedo = basecol;
         o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap)); 

         fixed4 c = tex2D (_MetallicTex, IN.uv_MainTex);

         o.Metallic   = _Metallic * c.r;
         o.Smoothness = _Glossiness;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }