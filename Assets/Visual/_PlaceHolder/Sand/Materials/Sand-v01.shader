Shader "Custom/MySand"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Normal]
		_BumpMap("Bumpmap", 2D) = "bump" {}
		[NoScaleOffset]
		_Metallic("Metallic", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5

		[NoScaleOffset]
		_HeightMap("HeightMap", 2D) = "white" {}
		_SecondTex("Second texture", 2D) = "white" {}
		_NoiseColor("Noise Color", Color) = (1,1,1,1)
		_NoiseTex("Noise texture", 2D) = "grey" {}

		_MaxDistance("Distortion distance", Range(0, 30)) = 1
			_SpeedX("Dist speed X", Range(0, 5)) = 1
			_SpeedY("Dist speed Y", Range(0, 5)) = 1
			_MoveX("Move speed X", Range(-25, 25)) = 1
			_MoveY("Move speed Y", Range(-25, 25)) = 1

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
		sampler2D _BumpMap;
		sampler2D _Metallic;

		sampler2D _HeightMap;
		sampler2D _SecondTex;
		float4 _NoiseColor;
		sampler2D _NoiseTex;

		float _SpeedX;
		float _SpeedY;
		float _MoveX;
		float _MoveY;
		float _MaxDistance;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_SecondTex;
        };

        half _Glossiness;
		fixed4 _Color;

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

			half2 uv2 = IN.uv_SecondTex;
			half noiseVal = tex2D(_NoiseTex, IN.uv_MainTex).r;
			//the lower, the quicker  --> so the darker, the quicker
			half heightVal = clamp((1 - tex2D(_HeightMap, IN.uv_MainTex).r), 0.1, 1);
			
			half worldMoveX = _Time.y * _MoveX * 0.01f;
			half roundMoveX = noiseVal * sin(_Time.y * _SpeedX) * _MaxDistance;
			half worldMoveY = _Time.y * _MoveY * 0.01f;
			half roundMoveY = noiseVal * sin(_Time.y * _SpeedY) * _MaxDistance;

			uv2.x = uv2.x + (worldMoveX + roundMoveX * heightVal);
			uv2.y = uv2.y + (worldMoveY + roundMoveY * heightVal);
			//uv2.x = uv2.x + worldMoveX * heightVal;
			//uv2.y = uv2.y + worldMoveY * heightVal;
			fixed4 cNoise = tex2D(_SecondTex, uv2);
			c.rgb = c.rgb * (1 - cNoise.a) + _NoiseColor * cNoise.a;

			o.Albedo = c.rgb;

			fixed4 bump = tex2D(_BumpMap, IN.uv_BumpMap);
			bump.rgb = bump.rbg * (1 - cNoise.a) + float3(0,0,1) * cNoise.a;
			o.Normal = UnpackNormal(bump);

			fixed4 spec = tex2D(_Metallic, IN.uv_MainTex);
			spec.rgb = spec.rgb * (1 - cNoise.a) + cNoise.rgb * cNoise.a;
			spec.a = spec.a - cNoise.a;
			o.Metallic = spec.rgb;
			o.Smoothness = spec.a * _Glossiness;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
