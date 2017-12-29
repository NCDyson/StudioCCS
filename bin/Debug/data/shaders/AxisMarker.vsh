#version 330 core
layout(location=0) in vec3 Position;
layout(location=1) in vec4 Color;

uniform mat4 Matrix;
uniform float Scale;

out vec4 FragmentBaseColor;

void main(){
	vec3 scaledPos = Position * Scale;
	gl_Position = Matrix * vec4(scaledPos, 1.0);
	FragmentBaseColor = Color;
}