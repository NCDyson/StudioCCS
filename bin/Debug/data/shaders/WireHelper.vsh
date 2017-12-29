#version 330 core
layout(location=0) in vec3 Position;

uniform mat4 Matrix;
uniform vec4 Color;
uniform float Scale;
uniform int Bill;
out vec4 FragmentBaseColor;

void main(){
	mat4 mv = Matrix;
	vec3 scaledPos = Position * Scale;
	//vec4 scaledPos = Matrix * vec4(Position, 1.0);
	if(Bill == 0)
	{
		gl_Position = mv * vec4(scaledPos, 1.0);
	}
	else
	{
		//mv[0][0] = Scale;
		//mv[0][1] = 0.0;
		//mv[0][2] = 0.0;
		//mv[0][3] = 0.0;
		
		//mv[1][0] = 0.0;
		//mv[1][1] = Scale;
		//mv[1][2] = 0.0;
		//mv[1][3] = 0.0;
	
		//mv[2][0] = 0.0;
		//mv[2][1] = 0.0;
		//mv[2][2] = Scale;
		//mv[2][3] = 0.0;
		
		//mv[3][3] = 1.0;
		//gl_Position = mv * vec4(Position, 1.0);
		//gl_Position /= gl_Position.w;
		
		vec3 camRight = vec3(mv[0][0], mv[1][0], mv[2][0]);
		vec3 camUp = vec3(mv[0][1], mv[1][1], mv[2][1]);
		vec3 pos = camUp * scaledPos;- camUp * scaledPos.y;
		//vec3 pos = scaledPos  + camRight + camUp;
		gl_Position = mv * vec4(pos, 1.0);
		//gl_Position /= gl_Position.w;
	}
	
	//gl_Position = vec4(pos, 1.0);
	FragmentBaseColor = Color;
}