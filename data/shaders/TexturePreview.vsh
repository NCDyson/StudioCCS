#version 330 core

layout(location=0) in vec3 vPosition;
layout(location=1) in vec2 vTexCoord;

uniform mat4 mMatrix;
uniform vec3 mSize;

out vec2 fTexCoords;

void main()
{
	float vX = vPosition.x * mSize.x;
	float vY = vPosition.y * mSize.y;
	vec3 vPos1 = vec3(vX * mSize.z, vY * mSize.z, 0.0);
	gl_Position = mMatrix * vec4(vPos1, 1.0);
	fTexCoords = vTexCoord;
}