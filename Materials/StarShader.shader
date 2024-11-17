Shader "Custom/StarShader"
{
    Properties
    {
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            uniform float _Size;
            uniform float4 _EmissionColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Apply scaling to size
                o.pos.xy *= _Size;

                o.color = _EmissionColor;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
