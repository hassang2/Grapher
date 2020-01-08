Shader "Plot" {
 
	Properties {
		_Color ("Color", Color) = (0, 0, 0, 0.5)
	}
 
	SubShader {
		Color [_Color]
		Lighting Off
        ZWrite On
        Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue" = "Transparent"}

		Pass {
		}
	}
 
}

