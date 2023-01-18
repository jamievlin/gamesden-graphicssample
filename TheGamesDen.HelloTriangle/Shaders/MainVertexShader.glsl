#version 450

in vec2 vtxInPos;
in vec4 vtxInColor;

out vec4 fragColor;

void main()
{
    gl_Position = vec4(vtxInPos, 0.f, 1.f);
    
    // The Fragment shader does not have direct access to vertex attribuites
    // (in this, vtxInColor), hence we need to forward vtxInColor as fragColor
    // for the rasterizer, and hence the fragment shader
    fragColor = vtxInColor;
}