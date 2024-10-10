Shader "Custom/PixelationWithNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(1, 100)) = 10
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
            float4 _MainTex_ST;
            float _PixelSize;
            float _NoiseIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Simple noise function using sine waves
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Lấy kích thước của màn hình hiện tại
                float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y);
                
                // Tính toán pixel size trong không gian UV
                float2 pixelSizeUV = _PixelSize / screenSize;

                // Làm tròn giá trị UV đến kích thước pixel gần nhất
                float2 uv = i.uv;
                uv = floor(uv / pixelSizeUV) * pixelSizeUV;

                // Lấy màu từ texture với UV đã pixel hóa
                fixed4 col = tex2D(_MainTex, uv);

                // Tạo noise dựa trên UV và thêm vào màu sắc
                float noise = rand(uv) * _NoiseIntensity;
                col.rgb += noise;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
