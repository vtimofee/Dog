Shader "Selected Effect --- Outline/Aura Outline" {
	Properties{
		[Header(Standard)]
		_MainTex    ("Albedo", 2D) = "white" {}
		_BumpMap    ("Bump", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic   ("Metallic", Range(0, 1)) = 0
		[Header(Outline)]
		_OutlineWidth ("Outline Width", Range(0.002, 2)) = 0.3
		_OutlineZ     ("Outline Z", Range(-0.06, 0)) = -0.05
		_ColorInside  ("Aura Inside", Color) = (0, 0, 1, 1)
		_ColorOutside ("Aura Outside", Color) = (0, 1, 1, 1)
		_Brightness   ("Brightness", Range(0.5, 3)) = 2
		_Edge         ("Rim Edge", Range(0.0, 1)) = 0.1
		_RimPower     ("Rim Power", Range(0.01, 10.0)) = 1
		[NoScaleOffset]
		_NoiseTex     ("Noise", 2D) = "white" {}
		_Scale        ("Noise Scale", Range(0, 0.05)) = 0.01
		_SpeedX       ("Speed X", Range(-20, 20)) = 0
		_SpeedY       ("Speed Y", Range(-20, 20)) = 3
		_Opacity      ("Opacity", Range(0.01, 10.0)) = 10
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Standard
		sampler2D _MainTex, _BumpMap;
		float _Glossiness, _Metallic;
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
		
		Pass {
			Tags { "LightMode" = "Always" }
			Cull Back ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_FOG_COORDS(0)
				float3 view : TEXCOORD1;
				float3 norm : TEXCOORD2;
			};

			float _OutlineWidth, _OutlineZ, _RimPower;
			sampler2D _NoiseTex;
			float _Scale, _Opacity, _Edge;
			float4 _ColorInside, _ColorOutside;
			float _Brightness, _SpeedX, _SpeedY;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 nrm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(nrm.xy);
				o.pos.xy += offset * _OutlineWidth * o.pos.z;
				o.pos.z += _OutlineZ;
				o.norm = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
				o.view = normalize(WorldSpaceViewDir(v.vertex));
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = float2(i.pos.x * _Scale - (_Time.x * _SpeedX), i.pos.y * _Scale - (_Time.x * _SpeedY));
				float4 nis = tex2D(_NoiseTex, uv);
				float4 rim = pow(saturate(dot(i.view, i.norm)), _RimPower);
				rim -= nis;
				float4 rim1 = saturate(rim.a * _Opacity);
				float4 rim2 = (saturate((_Edge + rim.a) * _Opacity) - rim1) * _Brightness;
				float4 c = (_ColorInside * rim1) + (_ColorOutside * rim2);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}