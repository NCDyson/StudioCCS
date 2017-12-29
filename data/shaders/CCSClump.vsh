#version 330
in ivec2 vEndpoints;

uniform mat4 uMatrix;
uniform int selectedID;
uniform samplerBuffer uMatrixList;

out BoneGeo
{
	vec4 bColor;
	int bStartpointID;
	int bEndpointID;
} bone;

void main()
{
	gl_Position = vec4(0.0f, 0.0f, 0.0f, 1.0f);
	bone.bColor = vec4(1.0f, 0.0f, 0.0, 1.0f);
	if(gl_VertexID == selectedID)
	{
		bone.bColor = vec4(0.0f, 1.0f, 1.0f, 1.0f);
	}
	bone.bStartpointID = vEndpoints[0];
	bone.bEndpointID = vEndpoints[1];
}