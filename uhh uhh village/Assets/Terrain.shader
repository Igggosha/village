Shader "Custom/Terrain"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
       // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxLayerCount  = 8; 
        const static float epsilon = 0.0001; 
        
        int layerCount;
        float3 baseColors[maxLayerCount];
        float baseStartHeights[maxLayerCount];
        float baseBlends[maxLayerCount]; 
        float baseColorStrengths[maxLayerCount];
        float baseTextureScales[maxLayerCount];
        
        float minHeight;
        float maxHeight;


        UNITY_DECLARE_TEX2DARRAY(baseTextures); 

        
        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };


   


        float inverseLerp(float a, float b, float value)
        {
            return saturate((value - a) / (b - a));
        }  

        float3 triplaner(float3 worldPos, float scale, float3 blendAxis, int textureIndex)
        {
            float3 scaledWorldPos = worldPos/ scale;
 
            float3 xProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.y, scaledWorldPos.z, textureIndex)) * blendAxis.x;  
            float3 yProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.z, textureIndex)) * blendAxis.y;  
            float3 zProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.y, textureIndex)) * blendAxis.z;
            return xProjection + yProjection + zProjection;
        } 
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            float3 blendAxis = abs(IN.worldNormal);
            blendAxis /= blendAxis.x + blendAxis.y + blendAxis.z;
            
            for(int i = 0; i < layerCount; i++)
            {
               float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercent - baseStartHeights[i]);

                float3  baseColor = baseColors[i]  * baseColorStrengths[i];
                float3 textureColor = triplaner(IN.worldPos, baseTextureScales[i], blendAxis, i) * (1-baseColorStrengths[i]);; 
                
                o.Albedo = o.Albedo * (1 - drawStrength) +  (baseColor + textureColor) *  drawStrength;
            } 
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
