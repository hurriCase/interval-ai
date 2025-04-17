Shader "UI/CircularSegmentedProgress"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _InnerRadius ("Inner Radius", Range(0, 1)) = 0.6
        _OuterRadius ("Outer Radius", Range(0, 1)) = 0.9
        _Segments ("Segments", Vector) = (0.25, 0.25, 0.25, 0.25) // X: Known, Y: Learning, Z: Repeating, W: Learned
        _GapSize ("Gap Size", Range(0, 0.2)) = 0.02

        _KnownColor ("Known Color", Color) = (0.8, 0.8, 0.8, 1)
        _LearningColor ("Learning Color", Color) = (1, 0.8, 0.2, 1)
        _RepeatingColor ("Repeating Color", Color) = (0.2, 0.4, 1, 1)
        _LearnedColor ("Learned Color", Color) = (0.8, 0.2, 0.8, 1)

        _EmptyColor ("Empty Color", Color) = (0.9, 0.9, 0.9, 0.5)
        _Smoothness ("Edge Smoothness", Range(0, 0.1)) = 0.005
        _SegmentRoundness ("Segment Roundness", Range(0, 0.2)) = 0.05

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Default"

        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                half4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;
            int _UIVertexColorAlwaysGammaSpace;

            float _InnerRadius;
            float _OuterRadius;
            float4 _Segments;
            float _GapSize;
            fixed4 _KnownColor;
            fixed4 _LearningColor;
            fixed4 _RepeatingColor;
            fixed4 _LearnedColor;
            fixed4 _EmptyColor;
            float _Smoothness;
            float _SegmentRoundness;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
                OUT.mask = half4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                if (_UIVertexColorAlwaysGammaSpace)
                {
                    if(!IsGammaSpace())
                    {
                        v.color.rgb = GammaToLinearSpace(v.color.rgb);
                    }
                }

                OUT.color = v.color * _Color;
                return OUT;
            }

            // Calculate distance to a circle
            float distToCircle(float2 p, float2 center, float radius) {
                return length(p - center) - radius;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Get UV coordinates
                float2 uv = IN.texcoord * 2.0 - 1.0;

                // Calculate distance from center and angle
                float dist = length(uv);
                float angle = atan2(uv.y, uv.x) / (3.14159265 * 2.0);
                // Adjust angle to start from top (0) and go clockwise
                angle = frac(0.75 - angle);

                // Check if we're in the ring
                float ringMask = smoothstep(_InnerRadius - _Smoothness, _InnerRadius + _Smoothness, dist) *
                                 smoothstep(_OuterRadius + _Smoothness, _OuterRadius - _Smoothness, dist);

                // Calculate the thickness of the ring
                float thickness = _OuterRadius - _InnerRadius;
                float avgRadius = (_InnerRadius + _OuterRadius) * 0.5;

                // Calculate the total value for normalization
                float total = _Segments.x + _Segments.y + _Segments.z + _Segments.w;
                if (total <= 0.001)
                    return _EmptyColor;

                // Normalize segments
                float4 normalizedSegments = _Segments / total;

                // Calculate segment boundaries
                float knownStart = 0;
                float knownEnd = normalizedSegments.x;
                float learningStart = knownEnd + _GapSize;
                float learningEnd = learningStart + normalizedSegments.y;
                float repeatingStart = learningEnd + _GapSize;
                float repeatingEnd = repeatingStart + normalizedSegments.z;
                float learnedStart = repeatingEnd + _GapSize;
                float learnedEnd = learnedStart + normalizedSegments.w;

                // Calculate segment angles in radians
                float PI2 = 3.14159265 * 2.0;
                float knownStartRad = knownStart * PI2;
                float knownEndRad = knownEnd * PI2;
                float learningStartRad = learningStart * PI2;
                float learningEndRad = learningEnd * PI2;
                float repeatingStartRad = repeatingStart * PI2;
                float repeatingEndRad = repeatingEnd * PI2;
                float learnedStartRad = learnedStart * PI2;
                float learnedEndRad = learnedEnd * PI2;

                // Calculate segment endpoints for rounded caps
                float2 knownStartPos = float2(sin(knownStartRad), -cos(knownStartRad)) * avgRadius;
                float2 knownEndPos = float2(sin(knownEndRad), -cos(knownEndRad)) * avgRadius;
                float2 learningStartPos = float2(sin(learningStartRad), -cos(learningStartRad)) * avgRadius;
                float2 learningEndPos = float2(sin(learningEndRad), -cos(learningEndRad)) * avgRadius;
                float2 repeatingStartPos = float2(sin(repeatingStartRad), -cos(repeatingStartRad)) * avgRadius;
                float2 repeatingEndPos = float2(sin(repeatingEndRad), -cos(repeatingEndRad)) * avgRadius;
                float2 learnedStartPos = float2(sin(learnedStartRad), -cos(learnedStartRad)) * avgRadius;
                float2 learnedEndPos = float2(sin(learnedEndRad), -cos(learnedEndRad)) * avgRadius;

                // Cap size proportional to thickness
                float capRadius = thickness * _SegmentRoundness * 2.0;

                // Distance to segment caps
                float distKnownStart = distToCircle(uv, knownStartPos, capRadius);
                float distKnownEnd = distToCircle(uv, knownEndPos, capRadius);
                float distLearningStart = distToCircle(uv, learningStartPos, capRadius);
                float distLearningEnd = distToCircle(uv, learningEndPos, capRadius);
                float distRepeatingStart = distToCircle(uv, repeatingStartPos, capRadius);
                float distRepeatingEnd = distToCircle(uv, repeatingEndPos, capRadius);
                float distLearnedStart = distToCircle(uv, learnedStartPos, capRadius);
                float distLearnedEnd = distToCircle(uv, learnedEndPos, capRadius);

                // Initialize with empty color
                fixed4 finalColor = _EmptyColor;

                // Calculate segment masks including the main segments and caps
                bool isInKnownSegment = angle >= knownStart && angle <= knownEnd;
                bool isInLearningSegment = angle >= learningStart && angle <= learningEnd;
                bool isInRepeatingSegment = angle >= repeatingStart && angle <= repeatingEnd;
                bool isInLearnedSegment = angle >= learnedStart && angle <= learnedEnd;

                // Check if close to caps
                float capSmoothness = _Smoothness * 3.0;
                float knownStartMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distKnownStart);
                float knownEndMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distKnownEnd);
                float learningStartMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distLearningStart);
                float learningEndMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distLearningEnd);
                float repeatingStartMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distRepeatingStart);
                float repeatingEndMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distRepeatingEnd);
                float learnedStartMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distLearnedStart);
                float learnedEndMask = 1.0 - smoothstep(-capSmoothness, capSmoothness, distLearnedEnd);

                // Apply segments with rounded caps
                if (isInKnownSegment || knownStartMask > 0.0 || knownEndMask > 0.0) {
                    finalColor = _KnownColor;
                }
                else if (isInLearningSegment || learningStartMask > 0.0 || learningEndMask > 0.0) {
                    finalColor = _LearningColor;
                }
                else if (isInRepeatingSegment || repeatingStartMask > 0.0 || repeatingEndMask > 0.0) {
                    finalColor = _RepeatingColor;
                }
                else if (isInLearnedSegment || learnedStartMask > 0.0 || learnedEndMask > 0.0) {
                    finalColor = _LearnedColor;
                }

                // Apply ring mask
                finalColor = lerp(_EmptyColor, finalColor, ringMask);

                // Apply UI clipping
                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                finalColor.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (finalColor.a - 0.001);
                #endif

                return finalColor;
            }
        ENDCG
        }
    }
}