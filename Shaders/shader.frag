#version 330

uniform vec3 Lamb1;
uniform vec3 Ldiff1;
uniform vec3 Lamb2;
uniform vec3 Ldiff2;

uniform vec4 Lpos1;
uniform vec4 Lpos2;
uniform vec3 Lspec1;
uniform vec3 Lspec2;

const vec3 Kamb = vec3(0.6, 0.6, 0.2);
const vec3 Kdiff = vec3(0.6, 0.6, 0.2);
const vec3 Kspec = vec3(0.3, 0.3, 0.3);
const float Kshi = 30.0;

in vec3 Idiff;
in vec4 P;
in vec3 N;
out vec4 fragment;

void main()
{
	vec3 V = -normalize(P.xyz);

	vec3 L = normalize((Lpos1 * P.w - P * Lpos1.w).xyz);
	vec3 L2 = normalize((Lpos2 * P.w - P * Lpos2.w).xyz);
	vec3 H = normalize(L + V);
	vec3 H2 = normalize(L2 + V);
	vec3 Ispec = vec3(0.0);
	vec3 Idiff = vec3(0.0);

	vec3 Iamb = Kamb * Lamb1;
	vec3 Iamb2 = Kamb * Lamb2;
	Idiff += max(dot(N, L), 0.0) * Kdiff * Ldiff1 + Iamb;
	Idiff += max(dot(N, L2), 0.0) * Kdiff * Ldiff2 + Iamb2;

	Ispec += pow(max(dot(N, H), 0.0), Kshi) * Kspec * Lspec1;
	Ispec += pow(max(dot(N, H2), 0.0), Kshi) * Kspec * Lspec2;
	fragment = vec4(Idiff + Ispec, 1.0);
}