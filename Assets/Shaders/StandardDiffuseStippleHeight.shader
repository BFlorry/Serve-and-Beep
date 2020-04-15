
Shader "Custom/StandardDiffuseStippleHeight" {
    Properties{
        _Striations("Striations", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Transparency("Transparency", Range(0,1)) = 1.0
        _LocalOffset("Local Offset", Vector) = (0,0,0,1)

        _NearDistance("NearDistance", Float) = 20
        _FarDistance("FarDistance", Float) = 30
    }
        SubShader{
        Tags{
            "RenderType" = "Opaque"
            "DisableBatching" = "True" // with current setup need to disable this cuz it messes with world positions
        }

        CGPROGRAM
        #pragma surface surf Standard vertex:vert fullforwardshadows nolightmap
        #pragma target 3.0
        //#pragma debug

        sampler2D _Striations;
        float4 _Striations_ST;

        half _Glossiness;
        half _Metallic;
        half _Transparency;
        float4 _LocalOffset;

        float _FarDistance;
        float _NearDistance;

        struct Input {
            float height;
            float3 norm;
            float4 screenPos;   // built in param used for transparency stippling
            float3 worldPos;
        };

        void vert(inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // this indicates direction from center of planet to this vertex (up vector)
            float3 pos = v.vertex + _LocalOffset;
            o.norm = v.normal;
            o.height = length(pos);

        }

        static const float4x4 thresholdMatrix = {
            1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0,  11.0 / 17.0,
            13.0 / 17.0, 5.0 / 17.0,  15.0 / 17.0, 7.0 / 17.0,
            4.0 / 17.0,  12.0 / 17.0, 2.0 / 17.0,  10.0 / 17.0,
            16.0 / 17.0, 8.0 / 17.0,  14.0 / 17.0, 6.0 / 17.0
        };
        static const float4x4 rowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };

        void surf(Input IN, inout SurfaceOutputStandard o) {

            float heightPerturb = 0.2 * IN.norm.y / _Striations_ST.x;
            
            o.Albedo = tex2D(_Striations, float2(IN.height + heightPerturb, IN.norm.x) * _Striations_ST.xy + _Striations_ST.zw);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;

            // Screen-door transparency: Discard pixel if below threshold.
            float2 pos = IN.screenPos.xy / IN.screenPos.w;
            pos *= _ScreenParams.xy; // pixel position

            //dist
            float2 v1 = IN.worldPos.xz;
            float2 v2 = _WorldSpaceCameraPos.xz;
            float dist = distance(v1, v2);

            if (dist < _NearDistance)
                _Transparency = 0;
            else
            {
                if (dist > _FarDistance)
                    _Transparency = 1;
                else
                    _Transparency = _Transparency + (_FarDistance - dist) / (_FarDistance - _NearDistance);
            }

            clip(_Transparency - thresholdMatrix[fmod(pos.x, 4)] * rowAccess[fmod(pos.y, 4)]);

            /*
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

            // Don't draw pixel if threshold is greater than the alpha
            // (the clip function discards the current pixel if the value is less than 0)
            clip(_Transparency - threshold);*/
        }
        ENDCG

    }
    Fallback "Diffuse"
}