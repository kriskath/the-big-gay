Shader "Custom/GrayscaleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half luminance = dot(col.rgb, half3(0.3, 0.59, 0.11)) * 0.3;
                half3 grayscaleColor = half3(luminance, luminance, luminance);

                // Blend factor (adjust this value to control the blending)
                half blendFactor = 0.9; // blend between original and grayscale

                col.rgb = lerp(col.rgb, grayscaleColor, blendFactor);
                return col; // Return the blended color
            }
            ENDCG
        }
    }
}
