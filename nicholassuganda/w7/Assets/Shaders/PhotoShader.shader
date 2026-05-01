Shader "Unlit/PhotoShader"
{
    Properties
    {
        _LeftMainTex  ("Left Eye Texture",  2D) = "white" {}
        _RightMainTex ("Right Eye Texture", 2D) = "white" {}
        _Zoom       ("Zoom",       Range(0.1, 1.0)) = 1.0
        _Exposure   ("Exposure",   Range(0.0, 3.0)) = 1.0
        _Saturation ("Saturation", Range(0.0, 2.0)) = 1.0
        _Contrast   ("Contrast",   Range(0.5, 2.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex    : SV_POSITION;
                float4 objvertex : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _LeftMainTex;
            sampler2D _RightMainTex;
            float _Zoom;
            float _Exposure;
            float _Saturation;
            float _Contrast;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex    = UnityObjectToClipPos(v.vertex);
                o.objvertex = v.vertex;
                return o;
            }

            // Blend between greyscale and full colour
            float3 ApplySaturation(float3 col, float sat)
            {
                // Luminance coefficients for linear colour space
                float luma = dot(col, float3(0.2126, 0.7152, 0.0722));
                return lerp(float3(luma, luma, luma), col, sat);
            }

            // S-curve contrast around the midpoint
            float3 ApplyContrast(float3 col, float contrast)
            {
                return saturate((col - 0.5) * contrast + 0.5);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                // Spherical coordinates from object-space position
                float xz        = length(i.objvertex.xz);
                float latitude  = atan2(i.objvertex.y, xz);
                float longitude = atan2(i.objvertex.z, i.objvertex.x);

                // Equirectangular UV — _Zoom narrows the sampled range (zoom in)
                float2 uv;
                uv.x = 0.25 - longitude * _Zoom / (2.0 * UNITY_PI);
                uv.y = 0.5  + latitude  * _Zoom / UNITY_PI;

                fixed4 col = (unity_StereoEyeIndex == 0)
                    ? tex2D(_LeftMainTex,  uv)
                    : tex2D(_RightMainTex, uv);

                // Colour grading — applied in order: exposure → saturation → contrast
                col.rgb *= _Exposure;
                col.rgb  = ApplySaturation(col.rgb, _Saturation);
                col.rgb  = ApplyContrast(col.rgb, _Contrast);

                return col;
            }
            ENDCG
        }
    }
}
