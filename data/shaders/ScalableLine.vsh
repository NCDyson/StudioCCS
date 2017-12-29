#version 330 core
in vec3 Position;

uniform mat4 Matrix;
uniform vec4 Color;
uniform vec3 Scale;

out vec4 FragmentBaseColor;

void main()
{
	vec3 Scaled = Position * Scale;
	gl_Position = Matrix * vec4(Scaled, 1.0);
	FragmentBaseColor = Color;
}