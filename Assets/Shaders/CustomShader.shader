Shader "Custom/CustomShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormMap ("BumpMap", 2D) = "bump" {}
        _AngleCutX ("AngleCutX", Range(-1.0, 1.0)) = 1
        _AngleCutY ("AngleCutY", Range(-1.0, 1.0)) = 1
        _AngleCutZ ("AngleCutZ", Range(-1.0, 1.0)) = 1
        _CutSize ("CutSize", Range(0, 1.0)) = 0.5
        _CutCount ("CutCount", float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormMap;
        float _AngleCutX;
        float _AngleCutY;
        float _AngleCutZ;
        float _CutSize;
        float _CutCount;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormMap;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // All cutting changes in code line below
            clip (frac((IN.worldPos.x * _AngleCutX + IN.worldPos.y * _AngleCutY + IN.worldPos.z * _AngleCutZ) * _CutCount) - _CutSize);
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            o.Normal = UnpackNormal(tex2D(_NormMap, IN.uv_NormMap));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
