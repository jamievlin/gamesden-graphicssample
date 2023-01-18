#version 450

in vec4 fragColor;

// In some older OpenGL tutorials, gl_FragColor may be used instead, but is no longer recommended.
// If there is only a single output vec4 as target, that target is assumed to be the color of the fragment.
out vec4 outColor;

void main()
{
    // Here, we forward the fragColor we passed from the Vertex Shader (through the rasterizer) to outColor
    outColor = fragColor;
}