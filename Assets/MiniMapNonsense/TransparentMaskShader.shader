Shader "Custom/BlackToTransparent" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader{
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 200

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                float threshold = 0.35; 
                if (texColor.r < threshold && texColor.g < threshold && texColor.b < threshold) {
                    texColor.a = 0;
                }

                return texColor;
            }
            ENDCG
        }
    }
}



