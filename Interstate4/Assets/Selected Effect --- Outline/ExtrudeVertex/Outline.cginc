#ifndef OUTLINE_INCLUDED
#define OUTLINE_INCLUDED

#include "UnityCG.cginc"

float3 _OutlineColor;
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

	// view space vertex position
	float3 vp = UnityObjectToViewPos(v.vertex);
	
	// view space distance from camera
//	float3 vc = mul(UNITY_MATRIX_V, _WorldSpaceCameraPos);
//	float dist = distance(vp, vc)*0.01;

	o.pos = UnityViewToClipPos(vp + dir * -vp.z * ow * 0.001);
	return o;
}
float4 frag (v2f i) : SV_TARGET
{
	return float4(_OutlineColor, 1);
}

#endif