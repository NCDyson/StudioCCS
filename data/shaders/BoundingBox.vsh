#version 330 core
layout(location=0) in vec3 Min;
layout(lacation=1) in vec3 Max;

uniform mat4 Matrix;
uniform vec4 Color;


out BBox
{
	vec3 bMin;
	vec3 bMax;
} box;

void main(){
	gl_Position = vec4(0.0, 0.0, 0.0, 1.0);
	box.bMin = Min;
	box.bMax = Max;
}

