
Shader "MainAxis" {
	SubShader { 
		Pass {
			BindChannels { Bind "Color",color }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On Cull Front Fog { Mode Off }
		} 
	} 
}