Shader "Custom/Color Cull Off"
{
	Properties{
		_MaColor("Main Color", Color) = (1,0,0,1)
	}
		SubShader{
		Cull Off
			Pass{
				Material{
					Diffuse[_MaColor]
				}
			Lighting On
		}
	}
}
