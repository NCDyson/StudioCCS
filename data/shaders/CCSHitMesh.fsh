#version 330 core
in vec4 BaseColor;
out vec4 color;

void main()
{
	color = vec4(BaseColor.xyz, 0.5);
}