#version 330 core

uniform sampler2D uTexture;

in vec4 fColor;
in vec2 fTextureCoords;
in vec2 fTextureOffset;
in vec3 fNormal;
flat in vec4 fSelectionColor;
flat in int fDrawOptions;
flat in int fRenderMode;
out vec4 color;

#define RENDER_LINES 1
#define RENDER_VERTEX_COLORS 2
#define RENDER_SMOOTH 4
#define RENDER_TEXTURE 8
#define RENDER_FLIP_UVS 32

void main()
{
	vec2 texCoords = fTextureCoords;
	if((fRenderMode & RENDER_FLIP_UVS) != 0)
	{
		texCoords = vec2(fTextureCoords.x, 1.0 - fTextureCoords.y);
	}
	
	vec2 finalTexCoord = texCoords + fTextureOffset;
	vec4 textureColor = texture2D(uTexture, finalTexCoord);
	color = vec4(fSelectionColor.xyz, 1.0);
	
	if(fDrawOptions != 0)
	{
		color.a = ((0.2126 * color.x) + (0.7152 * color.y) + (0.0722 * color.z)) * 2;
	}
	
	if((fRenderMode & RENDER_TEXTURE) != 0)
	{
		color = textureColor;
	}
	
	if((fRenderMode & RENDER_VERTEX_COLORS) != 0)
	{
		color *= fColor;
	}
}