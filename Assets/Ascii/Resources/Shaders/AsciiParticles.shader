Shader "Ascii/Particles/Additive"
{
  Properties
  {
    _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
    _MainTex("Particle Texture", 2D) = "white" {}
    _FactorAlpha("Factor alpha", Range(0.0, 1.0)) = 0.15
    _FactorColor("Factor color", Range(0.0, 1.0)) = 0.15
    _ThresholdColor("Threshold color", Color) = (0.0, 0.0, 0.0)
  }

  CGINCLUDE

  #include "UnityCG.cginc"

  sampler2D _MainTex;
  float4 _MainTex_ST;
  half4 _TintColor;
  half3 _ThresholdColor;
  half _FactorAlpha;
  half _FactorColor;

  struct appdata_t
  {
    float4 position : POSITION;
    float4 texcoord : TEXCOORD0;
    half4 color : COLOR;
  };

  struct v2f
  {
    float4 position : SV_POSITION;
    float2 texcoord : TEXCOORD0;
    half4 color : COLOR;
  };

  v2f vert(appdata_t v)
  {
    v2f o;
    o.position = mul(UNITY_MATRIX_MVP, v.position);
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.color = v.color;
    return o;
  }

  half4 frag(v2f i) : SV_Target
  {
    half4 tex = tex2D(_MainTex, i.texcoord);

    half3 pixel = i.color * _TintColor * tex.rgb;

    half luminosity = clamp((pixel.r + pixel.g + pixel.b) / 3.0, 0.0, 1.0);

    return half4(luminosity > _FactorColor ? pixel.rgb : _ThresholdColor, luminosity > _FactorAlpha ? 1.0 : 0.0);
  }

  ENDCG

  SubShader
  {
    Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

    Blend SrcAlpha OneMinusSrcAlpha
    //Blend One One
    
    BlendOp Add
    //BlendOp Sub
    //BlendOp RevSub
    //BlendOp Min
    //BlendOp Max

    Cull Off Lighting Off ZWrite Off Fog { Mode Off }

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_particles
      #pragma multi_compile_fog
      ENDCG
    }
  }
}