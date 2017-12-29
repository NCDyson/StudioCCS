#version 330 core

layout(location=0) in vec3 vPosition;
layout(location=1) in vec2 vTexCoords;
layout(location=2) in vec3 vNormal;
layout(location=3) in vec4 vColor;

uniform mat4 mMatrix;
uniform float mAlpha;
uniform vec2 mTextureOffset;
uniform int mDrawOptions;
uniform vec4 mSelectionColor;
uniform int mRenderMode;


out vec4 fColor;
out vec2 fTextureCoords;
out vec2 fTextureOffset;
out vec3 fNormal;
flat out vec4 fSelectionColor;
flat out int fDrawOptions;
flat out int fRenderMode;

void main()
{
	gl_Position = mMatrix * vec4(vPosition, 1.0);
	fColor = vec4(vColor.xyz, vColor.w * mAlpha);
	fTextureCoords = vTexCoords;
	fTextureOffset = mTextureOffset;
	fNormal = vNormal;
	fDrawOptions = mDrawOptions;
	fSelectionColor = mSelectionColor;
	fRenderMode = mRenderMode;
}