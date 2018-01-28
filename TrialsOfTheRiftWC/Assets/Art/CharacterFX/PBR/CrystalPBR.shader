Shader "Character/CrystalPBR" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_FrozenTint ("Frozen Tint", Color) = (1,1,1,1)
		_MainTex("Diffuse (RGB)", 2D) = "white" {}
        _MetallicTex("Metallic (RGB)", 2D) = "white" {}
		[NoScaleOffset] _RefractTex("Refraction Texture", Cube) = "" {}
		_FrostTex ("Frost (RGB)", 2D) = "white" {}
		_BumpMap ("Bump (RGB) Illumin (A)", 2D) = "bump" {}
		_BumpScale("Bump Scale", Range(0,1)) = 1
		_BaseTex ("Base (RGB)", 2D) = "white" {}
		_Scale("Scale", Float) = 1
		_Tighten("Tighten",Range(0.1,0.45)) = 0.3
		_DiffuseAmount("Diffuse Amount", Range(0,1)) = 0.2
		_FrostAmount("Frost Amount", Range(0,2)) = 1
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_FrostLevel("Frost Level", Range(0,1)) = 0.3
		_IceVisibility("Ice Visibility", Range(0,1)) = 0.8
	    _FrozenNormalScale("Normal Scale (Frozen)", Range(0,1)) = 0.5
	}
	
	SubShader
	{  
		Tags { "RenderType"="Opaque" }
		LOD 200
 
 
		Cull front
		Zwrite on

		// First pass - Draw the refractions with an inverse reflection.
		CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0
			samplerCUBE _RefractTex;
			sampler2D _MainTex;
			fixed4 _FrozenTint;

			struct Input
			{
				float2 uv_MainTex;
				half3  worldPos;
				float3 worldRefl;
				fixed3 worldNormal;	INTERNAL_DATA
			};
			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				float3 invworldRef1 = IN.worldRefl * -1.0;
				o.Albedo = _FrozenTint * (tex2D(_MainTex, IN.uv_MainTex).rgb * 0.5);
				o.Emission = texCUBE(_RefractTex, invworldRef1).rgb * _FrozenTint;
			}

		ENDCG
 
		// Second pass - do a zwrite with the front faces, so when we draw them later 
		// we don't get any overwrite
		Pass{
			Cull Back
			ZWrite On
			ColorMask 0
		}

        // Third pass - Draw the front facing polygons. This portion is similar to the
	    // statue shader - it mixes standard rendering with a triplanar render based on the diffuse amount.
		Cull Back
		Zwrite on
		CGPROGRAM
		#pragma surface surf Standard alpha fullforwardshadows
        #pragma target 3.0
		
		sampler2D _MainTex; 
		sampler2D _FrostTex;
		sampler2D _BumpMap;
		sampler2D _BaseTex;
        sampler2D _MetallicTex;
		fixed4 _Color;
		fixed _FrostAmount;
		fixed _DiffuseAmount;
		half  _Scale;
		half _Tighten;
        half _Glossiness;
        half _Metallic;
		half _FrostLevel;
		half _IceVisibility;
		half _BumpScale;
		half _FrozenNormalScale;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			half3 worldPos;
			fixed3 worldNormal;	INTERNAL_DATA		
		};
 
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// tighten the blending zone
			float4 nrm = tex2D(_BumpMap, IN.uv_BumpMap);
			o.Normal = UnpackScaleNormal(nrm,_BumpScale);
			float3 nrmFrozen = UnpackScaleNormal(nrm, _FrozenNormalScale);
			float3 realNormal = WorldNormalVector(IN, o.Normal);

			float3 blend_weights = abs(realNormal) - _Tighten;
			blend_weights = max(blend_weights, 0);
			blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z).xxx;

			float4 d = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float4 Frost = tex2D(_FrostTex, IN.worldPos.xz / _Scale);
			float4 cy = lerp(tex2D(_BaseTex, IN.worldPos.xz / _Scale), Frost, _FrostAmount);
			float4 cz = tex2D(_BaseTex, IN.worldPos.xy / _Scale);
			float4 cx = tex2D(_BaseTex, IN.worldPos.zy / _Scale);
			float4 result = cx.xyzw * blend_weights.xxxx +
				cy.xyzw * blend_weights.yyyy +
				cz.xyzw * blend_weights.zzzz;

			float Alpha = 0;

			if (realNormal.y > _FrostLevel)
			{
				float amt = (realNormal.y - _FrostLevel) * (1.0 / _FrostLevel);
				Alpha = lerp(0, 1, amt);
				Alpha = lerp(Alpha, result.a, _DiffuseAmount);
			}

			if (Alpha < _IceVisibility)
			{
				Alpha = _IceVisibility;
			}

            
		  o.Albedo = lerp(result.rgb * _Color.rgb, d, _DiffuseAmount);
		  o.Alpha = lerp(Alpha, 1.0f, _DiffuseAmount);
          fixed4 c = tex2D (_MetallicTex, IN.uv_MainTex);
          o.Metallic = _Metallic * c.r;
		  o.Normal = lerp(nrmFrozen, o.Normal , _DiffuseAmount);
          o.Smoothness = _Glossiness;
		}
		ENDCG
	}
 
	Fallback "Diffuse"
}