#version 330

uniform mat4 model;
uniform mat4 projection;
uniform mat3 normalMatrix;

const int Lcount = 2;

in vec4 position;
in vec3 normal;

out vec4 P;
out vec3 N;

void main()
{
	P = position * model;
	N = normalize(normal * normalMatrix);
	
	gl_Position = P * projection;
}