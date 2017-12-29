#version 330 core

layout(location=0) in vec3 vPosition;
layout(location=1) in vec3 vPosition1;
layout(location=2) in vec3 vPosition2;
layout(location=3) in vec3 vPosition3;
layout(location=4) in vec2 vTexCoords;
layout(location=5) in vec4 vColor;
layout(location=6) in vec3 vNormal;
layout(location=7) in ivec4 vBoneIDs;
layout(location=8) in vec4 vBoneWeights;

uniform mat4 uMatrix;
uniform float uAlpha;
uniform vec2 uTextureOffset;
uniform int uDrawOptions;
uniform vec4 uSelectionColor;
uniform int uRenderMode;
uniform samplerBuffer uMatrixList;

out vec4 fColor;
out vec2 fTextureCoords;
out vec2 fTextureOffset;
out vec3 fNormal;
flat out vec4 fSelectionColor;
flat out int fDrawOptions;
flat out int fRenderMode;


mat4 GetMatrix(int matrixID)
{
	int rowIndex = matrixID * 4;
	return mat4(texelFetch(uMatrixList, rowIndex), texelFetch(uMatrixList, rowIndex + 1), texelFetch(uMatrixList, rowIndex + 2), texelFetch(uMatrixList, rowIndex + 3));
}


vec4 SkinVertex()
{
	vec4 v0 = vec4(vPosition, 1.0);
	vec4 v1 = vec4(vPosition1, 1.0);
	vec4 v2 = vec4(vPosition2, 1.0);
	vec4 v3 = vec4(vPosition3, 1.0);
	mat4 b0 = GetMatrix(vBoneIDs[0]);
	mat4 b1 = GetMatrix(vBoneIDs[1]);
	mat4 b2 = GetMatrix(vBoneIDs[2]);
	mat4 b3 = GetMatrix(vBoneIDs[3]);
	vec4 w = vBoneWeights;
	return ((b0 * v0) * w[0]) + ((b1 * v1) * w[1]) + ((b2 * v2) * w[2]) + ((b3 * v3) * w[3]);
	//return ((b0 * v0) * w[0]) + ((b1 * v0) * w[1]) + ((b2 * v0) * w[2]) + ((b3 * v0) * w[3]);
}

vec4 SkinVertex2()
{
	vec4 w = vBoneWeights;
	vec3 finalPos = vPosition;// * w[0] + (vPosition1 * w[1]) + (vPosition2 * w[2]) + (vPosition3 * w[3]);
	vec4 v = vec4(finalPos, 1.0);
	mat4 b0 = GetMatrix(vBoneIDs[0]);
	mat4 b1 = GetMatrix(vBoneIDs[1]);
	mat4 b2 = GetMatrix(vBoneIDs[2]);
	mat4 b3 = GetMatrix(vBoneIDs[3]);
	
	return (b0 * v) * w[0] + (b1 * v) * w[1] + (b2 * v) * w[2] + (b3 * v) * w[3];
}

void main()
{
	gl_Position = uMatrix * SkinVertex();
	
	fColor = vec4(vColor.xyz, vColor.w * uAlpha);
	fTextureCoords = vTexCoords;
	fTextureOffset = uTextureOffset;
	fNormal = vNormal;
	fDrawOptions = uDrawOptions;
	fSelectionColor = uSelectionColor;
	fRenderMode = uRenderMode;
}
