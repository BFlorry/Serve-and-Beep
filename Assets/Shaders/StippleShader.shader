Shader "Custom/StippleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NearDistance("NearDistance", Float) = 5
        _FarDistance("FarDistance", Float) = 15
        _Transparency("Transparency", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;   // built in param used for transparency stippling
            float3 worldPos;
            half3 worldNormal;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        half _Transparency;
        float _FarDistance;
        float _NearDistance;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;


            // Stippling / screen-door transparency
            // Threshold values for each 4x4 block of pixels
            const float4x4 thresholdMatrix =
            {
                1, 9, 3, 11,
                13, 5, 15, 7,
                4, 12, 2, 10,
                16, 8, 14, 6
            };

            // Multiply screen pos by (width, height) of screen to get pixel coord
            float2 pixelPos = IN.screenPos.xy / IN.screenPos.w * _ScreenParams.xy;

            // Get threshold of current pixel and divide by 17 to get in range (0, 1)
            float threshold = thresholdMatrix[pixelPos.x % 4][pixelPos.y % 4] / 17;

            // Distance between camera and object
            float2 v1 = IN.worldPos.xz;
            float2 v2 = _WorldSpaceCameraPos.xz;
            float dist = distance(v1, v2);

            half trans = _Transparency;

            // Fade object if between Near and Far
            if (dist < _NearDistance)
                _Transparency = 0.3;
            else
            {
                if (dist > _FarDistance)
                    trans = 1;
                else
                    //Somehow this works. Magic. :3
                    trans = clamp ((trans + (dist - _NearDistance) / (_FarDistance - _NearDistance)), 0.3, threshold);
            }

            //back side must be transparent always
            float n = dot(IN.viewDir, IN.worldNormal);

            if (n < 0)
                trans = 0.3;

            // Don't draw pixel if threshold is greater than the alpha
            // (the clip function discards the current pixel if the value is less than 0)
            clip(trans - threshold);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
