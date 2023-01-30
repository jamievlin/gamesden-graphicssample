#version 450

layout(location = 0)
in vec3 vtxInPos;

layout(location = 1)
in vec3 vtxInNormal;

out vec3 outNormal;

// uniforms
uniform mat4 projViewModelMatrix;
uniform mat4 modelInverseTransposeMatrix;

void main()
{
    vec4 transformedNormal = modelInverseTransposeMatrix * vec4(vtxInNormal, 0.0f);
    outNormal = normalize(transformedNormal.xyz);
    gl_Position = projViewModelMatrix * vec4(vtxInPos, 1.0f);
}