#version 330 core

uniform sampler2D fTexture;

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

void main()
{
	
	//vec2 finalTexCoord = fTextureCoords + fTextureOffset;
	
	//color = fColor;
	////Flat Shaded with Texture/
	//if(fSelectionColor.w == 0.5)
	//{
	//	color = texture2D(fTexture, finalTexCoord) * fColor;
	//}
	
	//if(fDrawOptions == 9)
	//{
	//	//color.a = color.x;
	//	//percieved lum
	//	//color.a = (0.299 * color.x) + (0.587 * color.y) + (0.114 * color.z);
	//	//standard lum
	//	color.a = (0.2126 * color.x) + (0.7152 * color.y) + (0.0722 * color.z);
	//}
	
	//if(fSelectionColor.w == 1.0)
	//{
	//	color = vec4(fSelectionColor.xyz, 1.0);
	//}
	
	
	vec2 finalTexCoord = fTextureCoords + fTextureOffset;
	vec4 textureColor = texture2D(fTexture, finalTexCoord);
	color = vec4(fSelectionColor.xyz, 1.0);
	
	
	if((fRenderMode & RENDER_TEXTURE) != 0)
	{
		color = textureColor;
		if(fDrawOptions != 0)
		{
			color.a = ((0.2126 * color.x) + (0.7152 * color.y) + (0.0722 * color.z)) * 2;
		}
		
		//if(color.a < 0.5)
		//{
	//		discard;
	//	}
	}
	
	if((fRenderMode & RENDER_VERTEX_COLORS) != 0)
	{
		color *= fColor;
	}
	
	//if(fDrawOptions == 9)
	//{
	//	//color.a = color.x;
	//	//percieved lum
	//	//color.a = (0.299 * color.x) + (0.587 * color.y) + (0.114 * color.z);
	//	//standard lum
	//	color.a = (0.2126 * color.x) + (0.7152 * color.y) + (0.0722 * color.z);
	//}
	
}