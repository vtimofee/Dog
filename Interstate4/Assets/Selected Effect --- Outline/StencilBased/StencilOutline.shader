Shader "Selected Effect --- Outline/Stencil Based/Outline" {
	Properties {
		[Header(Basic)]
		_MainTex    ("Main", 2D) = "white" {}
		_BumpMap    ("Bump", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic   ("Metallic", Range(0, 1)) = 0
		[Header(Mask)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest2ndPass ("ZTest 2nd Pass", Int) = 0
		[Header(Fill)]
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest3rdPass ("ZTest 3rd Pass", Int) = 0
		_OutlineColor   ("Outline Color", Color) = (1, 1, 1, 1)
		_OutlineWidth   ("Outline Width", Range(0, 10)) = 2
		_OutlineFactor  ("Outline Factor", Range(0, 1)) = 1
		[MaterialToggle] _OutlineBasedVertexColorR ("Outline Based Vertex Color R", Float) = 0.0
	}
	SubShader {
		Tags { "Queue" = "Transparent" }

		// 1st pass
		CGPROGRAM
		#pragma surface surf Standard
		sampler2D _MainTex, _BumpMap;
		float4 _OverlayColor;
		float _Overlay, _Glossiness, _Metallic;
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(c.rgb, _OverlayColor.rgb, _Overlay);
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
		
		// 2nd pass
		Pass {
			Cull Off ZTest [_ZTest2ndPass] ZWrite Off ColorMask 0
			Stencil {
				Ref 1
				Pass Replace
			}
		}
		
		// 3rd pass
		Pass {
			Cull Off ZTest [_ZTest3rdPass] ZWrite Off Blend SrcAlpha OneMinusSrcAlpha ColorMask RGB
			Stencil {
				Ref 1
				Comp NotEqual
			}
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			fixed4 _OutlineColor;
			float _OutlineWidth, _OutlineFactor, _OutlineBasedVertexColorR;

			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert (appdata_full v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				// extrude direction
				float3 dir1 = normalize(v.vertex.xyz);
				float3 dir2 = v.normal;
				float3 dir = lerp(dir1, dir2, _OutlineFactor);
				dir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, dir));

				// is outline based on R channel of vertex color ?
				float ow = _OutlineWidth * v.color.r * _OutlineBasedVertexColorR + (1.0 - _OutlineBasedVertexColorR);

				float3 vp = UnityObjectToViewPos(v.vertex);

				o.pos = UnityViewToClipPos(vp + dir * -vp.z * ow * 0.001);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
	FallBack "VertexLit"
}
