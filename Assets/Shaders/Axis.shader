Shader "Axis" {
	SubShader { 
		Pass {
			BindChannels { Bind "Color",color }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite on Cull front Fog { Mode Off }
		} 
	} 
}