#version 400 core

in vec2 pass_textureCoords;
in vec3 surfaceNormal;
in vec3 toLight;
in vec3 toCamera;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 lightColor;
uniform float shineDamper;
uniform float reflectivity;

void main(void)
{
    vec3 surfaceNormalUnitVector = normalize(surfaceNormal);
    vec3 toLightUnitVector = normalize(toLight);
    
    // Dot Product sees how parallel/perpendicular the vectors are
    // The closer to parallel the normal vector and light vector are, the brighter the surface will be
    float dotProd = dot(surfaceNormalUnitVector, toLightUnitVector);
    
    // Make sure brightness is between 0.2 (ambient light) and 1
    float brightness = max(dotProd, 0.2);
    
    vec3 diffuse = brightness * lightColor;
    
    vec3 toCameraUnitVector = normalize(toCamera);
    vec3 lightDirection = -1 * toLightUnitVector;
    vec3 reflectedLightDirection = reflect(lightDirection, surfaceNormalUnitVector);
    float specular = dot(reflectedLightDirection, toCameraUnitVector);
    specular = max(specular, 0.0);
    float damping = pow(specular, shineDamper);
    vec3 finalSpecular = damping * reflectivity * lightColor;
    
    out_Color = vec4(diffuse, 1.0) * texture(textureSampler, pass_textureCoords) + vec4(finalSpecular, 1.0);

}