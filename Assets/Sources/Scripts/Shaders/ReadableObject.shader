Shader "Custom/ReadableObject"
{
    Properties
    {
        _MainTex ("Base Texture (RGB)", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        
        [Header(Glow Settings)]
        _GlowColor ("Glow Color", Color) = (0, 0.5, 1, 1) // Цвет внутренней подсветки
        _GlowPower ("Glow Power (Fresnel)", Range(0.5, 8.0)) = 3.0 // Резкость перехода у краев
        _GlowIntensity ("Glow Intensity", Range(0.0, 5.0)) = 1.5 // Яркость свечения
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        // Используем custom-освещение LightingHalfLambert для лучшей читаемости в тенях
        #pragma surface surf HalfLambert 

        sampler2D _MainTex;
        fixed4 _BaseColor;
        fixed4 _GlowColor;
        half _GlowPower;
        half _GlowIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir; // Направление взгляда камеры, Unity посчитает автоматически
        };

        // Кастомная модель освещения Half-Lambert
        // Она не дает объекту стать полностью черным в неосвещенных зонах
        half4 LightingHalfLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5; // Смещение диапазона из [-1, 1] в [0, 1]
            
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
            c.a = s.Alpha;
            return c;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Базовый цвет из текстуры
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _BaseColor;
            o.Albedo = texColor.rgb;

            // Расчет эффекта Френеля (читаемость силуэта)
            // dot(o.Normal, IN.viewDir) равен 1 в центре и 0 на краях геометрии
            half fresnel = 1.0 - saturate(dot(normalize(o.Normal), normalize(IN.viewDir)));
            
            // Возводим в степень для регулировки толщины свечения на краях
            fresnel = pow(fresnel, _GlowPower);

            // Добавляем свечение в поле Emission (оно игнорирует тени сцены)
            o.Emission = _GlowColor.rgb * fresnel * _GlowIntensity;
            o.Alpha = texColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}