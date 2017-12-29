#version 330 core

layout(location=0) in vec3 vPosition;
uniform mat4 mMatrix;
uniform vec4 vColor;
out vec4 BaseColor;

void main()
{
	gl_Position = mMatrix * vec4(vPosition, 1.0);
	BaseColor = vColor;
}
