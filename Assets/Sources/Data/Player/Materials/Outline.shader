Shader "Custom/Outline" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (.002, 0.1)) = .005
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        // ПЕРВЫЙ ПРОХОД: Рисуем обводку
        Pass {
            Tags { "LightMode" = "Always" }
            Cull Front // Рисуем только задние стенки, "раздутые" наружу

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v) {
                v2f o;
                // Сдвигаем вершины вдоль нормалей
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return _OutlineColor;
            }
            ENDCG
        }

        // ВТОРОЙ ПРОХОД: Рисуем саму модель поверх обводки
        // Здесь используется стандартный код отрисовки текстуры
        Pass {
            Cull Back
            SetTexture [_MainTex] { combine texture }
        }
    }
}