#version 330 core

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

uniform vec3 colorOverlay = vec3(1, 1, 1);

void main() {
	outputColor = texture(texture0, texCoord) * vec4(colorOverlay, 1);
}