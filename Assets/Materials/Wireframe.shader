Shader "Lines/Colored Blended" {
	SubShader { 
		Pass {
			BindChannels { Bind "Color",color }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite on Cull front Fog { Mode Off }
		} 
	} 
}