Shader "Unlit/OutlineShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0.5,0.5,0.5,1)
        _Outline("Outline width", Range(.0001, 0.03)) = .005
    }
        SubShader
        {
            Tags {"Queue" = "Overlay" }
            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _Outline;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG

            Pass
            {
                Name "OUTLINE"
                Tags { "LightMode" = "Always" }
                Cull Front
                ZWrite On
                ZTest LEqual
                ColorMask RGB
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : POSITION;
                    float4 color : COLOR;
                };

                float _Outline;
                float4 _OutlineColor;

                v2f vert(appdata v)
                {
                    // Copy the vertex data
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // Compute outline
                    float3 norm = mul((float3x3) unity_ObjectToWorld, v.normal);
                    o.pos.xy += norm.xy * o.pos.w * _Outline;

                    o.color = _OutlineColor;

                    return o;
                }

                float4 frag(v2f i) : COLOR
                {
                    return i.color;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}