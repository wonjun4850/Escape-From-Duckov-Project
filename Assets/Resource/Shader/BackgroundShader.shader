Shader "UI/MaskReader" {
    Properties { _MainTex ("Texture", 2D) = "white" {} _Color("Color", Color) = (0,0,0,1) }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            ZWrite Off
            Stencil {
                Ref 1
                Comp NotEqual
            }
            SetTexture [_MainTex] { combine primary }
        }
    }
}