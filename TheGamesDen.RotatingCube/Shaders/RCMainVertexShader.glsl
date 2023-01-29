#version 450

layout(location = 0)
in vec3 vtxInPos;

layout(location = 1)
in vec3 vtxInNormal;

out vec3 outNormal;

// uniforms
uniform mat4 projViewModelMatrix;

void main()
{
    outNormal = vtxInNormal;
    gl_Position = projViewModelMatrix * vec4(vtxInPos, 1.0f);
}