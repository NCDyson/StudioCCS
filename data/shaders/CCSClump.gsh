#version 330

layout (points) in;
layout (triangle_strip) out;
layout (max_vertices = 22) out;

uniform mat4 uMatrix;
uniform samplerBuffer uMatrixList;

in BoneGeo
{
	vec4 bColor;
	int bStartpointID;
	int bEndpointID;
} bones[];

out vec4 Vertex_Color;


mat4 GetMatrix(int matrixID)
{
	int rowIndex = matrixID * 4;
	return mat4(texelFetch(uMatrixList, rowIndex), texelFetch(uMatrixList, rowIndex + 1), texelFetch(uMatrixList, rowIndex + 2), texelFetch(uMatrixList, rowIndex + 3));
}

void main()
{
	float vScale = 0.05;
	vec4 scale = vec4(vScale, vScale, vScale, 1.0f);
	vec4 v0 = vec4(  0,   0,   0, 1.0) * scale; 			//Bottom
	vec4 v1 = vec4(0.5,   1,   1, 1.0) * scale; 	//Left Front
	vec4 v2 = vec4(0.5,   1,  -1, 1.0) * scale; 	//Right Front
	vec4 v3 = vec4(  2,   0,   0, 1.0) * scale;			//Top
	if(bones[0].bStartpointID != bones[0].bEndpointID)
	{
		v3 = vec4(0, 0, 0, 1.0);
	}
	vec4 v4 = vec4(0.5,  -1,   1, 1.0) * scale;	//BackLeft
	vec4 v5 = vec4(0.5,  -1,  -1, 1.0) * scale;	//BackRight
	
	mat4 hMatrix = GetMatrix(bones[0].bStartpointID);
	mat4 tMatrix = GetMatrix(bones[0].bEndpointID);
	
	hMatrix = uMatrix * hMatrix;
	tMatrix = uMatrix * tMatrix;
	
	v0 = (hMatrix * v0);
	v1 = (hMatrix * v1);
	v2 = (hMatrix * v2);
	v3 = (tMatrix * v3);
	v4 = (hMatrix * v4);
	v5 = (hMatrix * v5);
	
	//TODO: Optimize
	//front face
	gl_Position = v0;
	Vertex_Color = bones[0].bColor * 0.5;
	EmitVertex();
	
	gl_Position = v1;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v2;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v3;
	Vertex_Color = bones[0].bColor + vec4(0.8, 0.8, 0.8, 1.0);
	EmitVertex();
	
	//Degenerate triangles
	gl_Position = v2;
	EmitVertex();
	gl_Position = v2;
	EmitVertex();
	
	//Right Face
	gl_Position = v0;
	Vertex_Color = bones[0].bColor * 0.5;
	EmitVertex();
	
	gl_Position = v2;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v5;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v3;
	Vertex_Color = bones[0].bColor + vec4(0.8, 0.8, 0.8, 1.0);
	EmitVertex();
	//Degenerate triangles
	gl_Position = v5;
	EmitVertex();
	gl_Position = v5;
	EmitVertex();
	
	//Back Face
	gl_Position = v0;
	Vertex_Color = bones[0].bColor * 0.5;
	EmitVertex();
	
	gl_Position = v5;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v4;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v3;
	Vertex_Color = bones[0].bColor + vec4(0.8, 0.8, 0.8, 1.0);
	EmitVertex();
	//Degenerate triangles
	gl_Position = v4;
	EmitVertex();
	gl_Position = v4;
	EmitVertex();

	//Left Face
	gl_Position = v0;
	Vertex_Color = bones[0].bColor * 0.5;
	EmitVertex();
	
	gl_Position = v4;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v1;
	Vertex_Color = bones[0].bColor;
	EmitVertex();
	
	gl_Position = v3;
	Vertex_Color = bones[0].bColor + vec4(0.8, 0.8, 0.8, 1.0);
	EmitVertex();
	
	EndPrimitive();
}
