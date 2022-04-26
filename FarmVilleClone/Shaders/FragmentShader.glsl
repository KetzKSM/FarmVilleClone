#version 400 core

in vec2 pass_textureCoords;
in vec3 surfaceNormal;
in vec3 toLight;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 lightColor;

void main(void)
{
    vec3 surfaceNormalUnitVector = normalize(surfaceNormal);
    vec3 toLightUnitVector = normalize(toLight);
    
    // Dot Product sees how parallel/perpendicular the vectors are
    // The closer to parallel the normal vector and light vector are, the brighter the surface will be
    float dotProd = dot(surfaceNormalUnitVector, toLightUnitVector);
    
    // Make sure brightness is between 0 and 1
    float brightness = max(dotProd, 0.0);
    
    vec3 diffuse = brightness * lightColor;
    
    out_Color = vec4(diffuse, 1.0) * texture(textureSampler, pass_textureCoords);

}