Shader "Custom/Mask" {
Properties
{
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Mask ("Culling Mask", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
}
	SubShader
	{
		Tags {"Queue"="Transparent+10"}
		Lighting Off
		ZWrite Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			SetTexture [_Mask] {combine texture}
			SetTexture [_MainTex] {combine texture * previous}
		}
		
	}
	FallBack "Diffuse"
}



