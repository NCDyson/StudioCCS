#version 330 core

uniform sampler2D fTexture;

in vec2 fTexCoords;
out vec4 color;

void main()
{
	color = texture2D(fTexture, fTexCoords);
}