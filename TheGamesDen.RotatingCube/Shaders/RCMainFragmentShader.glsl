#version 450

in vec3 outNormal;

out vec4 outColor;

void main()
{
    // Again, we have to normalize the normal vector we got from the vertex shader,
    // since interpolation of multiple normalized vectors are not always unit length.
    // (for example, consider A = (0, 1, 0) and B = (0, 0, 1). Is (1/2)A + (1/2)B unit length?
    
    // The answer is no, since (1/2)A + (1/2)B is (0, 0.5, 0.5) which has length sqrt(0.25 + 0.25) = sqrt(0.5)
    // which is approximately length 0.7071.
    vec3 normalizedNormal = normalize(outNormal);
    
    // This is a simple light calculation using a fixed color and a fix direction vector using a Lambertian diffuse model
    // where the surface's brightness is determined by how perpendicular the surface is to the light's direction.
    // 
    // Note that there are also other light calculation models, many of which look much better - for example,
    // one of my favorite - Cook-Torrance model.
    // See https://graphicscompendium.com/gamedev/15-pbr for more details
    float diffuseLightIntensity = clamp(dot(normalizedNormal, vec3(0.75, -0.5, 1)), 0, 1);
    
    // Here, we are simply calculating what the pixel will look like and pass that to outColor.
    // The extra step in between (outColorDiffuse) is needed since our calculation is in vec3 (without the alpha channel)
    // while we are required to supply a vec4 color (RGB + Alpha) for the render target
    vec3 outColorDiffuse = diffuseLightIntensity * vec3(1.0f, 0.0f, 0.0f);
    outColor = vec4(outColorDiffuse, 1.0f);
}