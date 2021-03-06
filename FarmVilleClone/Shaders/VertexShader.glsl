#version 400 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec2 textureCoords;
layout(location = 2) in vec3 normal;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;
out vec3 toLight;
out vec3 toCamera;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 lightPosition;

uniform float usesFakeLighting;

void main(void)
{
    vec4 worldPosition = transformationMatrix * vec4(position, 1.0);
    gl_Position = projectionMatrix * viewMatrix * worldPosition;
    pass_textureCoords = textureCoords;
    
    vec3 actualNormal = normal;
    if (usesFakeLighting > 0.5) {
        actualNormal = vec3(0, 1, 0);
    }
    
    surfaceNormal = (transformationMatrix * vec4(normal, 0.0)).xyz;
    toLight = lightPosition - worldPosition.xyz;
    
    toCamera = (inverse(viewMatrix) * vec4(0,0,0,1)).xyz - worldPosition.xyz;
}