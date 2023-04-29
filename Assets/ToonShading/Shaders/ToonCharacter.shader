Shader "Toon/Character" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _RimLightOffset("Rim Light Offset",  Range(0, 1)) = 0.5
        _MainTex("Base (RGB)", 2D) = "white" {}
        _RampSmoothness("Toon Ramp Smoothness",  Range(0, 1)) = 0.02
        _Offset("Toon Ramp Offset",  Range(0, 1)) = 0 
        _OffsetPoint("Toon Ramp Point Offset",  Range(0, 1)) = 0.7  
        _Mask("R = Emis, G = Spec", 2D) = "black" {}
        _SpecTint("Spec Tint", Color) = (1,1,1,1)
        _Ambient("Ambient Light Strength",  Range(0, 1)) = 0 
    }
    
    SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 200
        Cull Off
        
        CGPROGRAM
        
        #pragma surface surf ToonRamp fullforwardshadows addshadow
        
        float _Offset,_OffsetPoint,_RampSmoothness,_Ambient;
        
        // custom lighting function based
        // on angle between light direction and normal
        inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
        {
            #ifndef USING_DIRECTIONAL_LIGHT
                lightDir = normalize(lightDir);
            #endif
            float d = dot(s.Normal, lightDir) ;
            float lightIntensity = smoothstep(_Offset,_Offset + _RampSmoothness, d);
          
                     
            #if POINT || SPOT
                lightIntensity = smoothstep(_OffsetPoint,_OffsetPoint + _RampSmoothness, d);
            #endif
              lightIntensity += _Ambient;
           
            
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * lightIntensity * atten;
            c.a = s.Alpha;
            return c;
        }

        
        
        sampler2D _MainTex;
        float4 _Color;
        sampler2D _Mask;
        float4 _SpecTint;
        float _RimLightOffset;
       
        
        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float3 viewDir;
        };
        
        
        void surf(Input IN, inout SurfaceOutput o) {
            // main texture
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            
            // effects mask
            half4 e = tex2D(_Mask, IN.uv_MainTex);
            
            // spec
            float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
            float spec = dot(IN.viewDir, o.Normal);// specular based on view and light direction
            float cutOff = step(saturate(spec), 0.8); // cutoff for where base color is
            float3 specularMain = c.rgb * (1 - cutOff) * e.g * _SpecTint * 4;// inverted base cutoff times specular color
            
            // highlight 
            float highlight = saturate(dot(normalize(lightDir + (IN.viewDir * 0.5)), o.Normal)); // highlight based on light direction
            float3 highlightMain = (step(0.9,highlight) * c.rgb *_SpecTint * 2) * e.g; //glowing highlight
            
            // rim
            half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));// standard rim calculation  
            
            // emissive glow based on red channel
            o.Emission = e.r * (c.rgb * 2);
            
            // final color
            o.Albedo = c.rgb + specularMain + highlightMain;
            
            // main rim
            // rim on the lit side
            float DotLight = dot(lightDir, o.Normal);
            // blend with normal rim
            float rimIntensity = rim * pow(DotLight, 0.1);
            // cutoff
            rimIntensity = smoothstep(_RimLightOffset,fwidth(rimIntensity) + _RimLightOffset, rimIntensity);
            
            // add strong rim 
            o.Albedo += (rimIntensity * 2 * c.rgb);

          
            
            
        }
        ENDCG
        
    }
    
    Fallback "Diffuse"
}