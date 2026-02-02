Shader "UI/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Blur Size", Range(0, 10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Size;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 col = fixed4(0,0,0,0);

                // Simple 9-tap blur
                float offset = _Size / 1000; // ajusta según tamaño de la imagen
                col += tex2D(_MainTex, uv + float2(-offset, -offset));
                col += tex2D(_MainTex, uv + float2(-offset, 0));
                col += tex2D(_MainTex, uv + float2(-offset, offset));
                col += tex2D(_MainTex, uv + float2(0, -offset));
                col += tex2D(_MainTex, uv);
                col += tex2D(_MainTex, uv + float2(0, offset));
                col += tex2D(_MainTex, uv + float2(offset, -offset));
                col += tex2D(_MainTex, uv + float2(offset, 0));
                col += tex2D(_MainTex, uv + float2(offset, offset));

                col /= 9;

                return col;
            }
            ENDCG
        }
    }
}
