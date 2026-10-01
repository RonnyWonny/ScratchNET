#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 transform = mat4(1.0);
uniform mat4 projection = mat4(1.0);

void main(void) {
	gl_Position	= vec4(aPosition, 1.0f) * transform * projection;
	texCoord = aTexCoord;
}