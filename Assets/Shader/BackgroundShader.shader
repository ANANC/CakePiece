Shader "Custom/BackgroundShader"
{
	Properties
	{
		_BaseColor("BaseColor", Color) = (1,1,1,1)

		_Enable_0("Enable_0",Float) = 1
		_Texture_0("Texture_0", 2D) = "white" {}
		_Color_0("Color_0", Color) = (1,1,1,1)
		_Position_0("Position_0",Vector) = (0,0,0,0)
		_Radius_0("Radius_0",Float) = 1

		_Enable_1("Enable_1",Float) = 1
		_Texture_1("Texture_1", 2D) = "white" {}
		_Color_1("Color_1", Color) = (1,1,1,1)
		_Position_1("Position_1",Vector) = (0,0,0,0)
		_Radius_1("Radius_1",Float) = 1
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _BaseColor;

			float _Enable_0;
			fixed4 _Color_0;
			sampler2D _Texture_0;
			float4 _Position_0;
			float _Radius_0;

			float _Enable_1;
			fixed4 _Color_1;
			sampler2D _Texture_1;
			float4 _Position_1;
			float _Radius_1;


			struct a2v {
				float4 vertex:POSITION;
				float2 uv:TEXCOORD0;
			};

			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}

			fixed DrawCircle(float2 screenUV, float2 pos, float radius, out fixed4 color,out float alpha)
			{
				screenUV -= pos;
				float lengthValue = length(screenUV);
				color = step(lengthValue, radius);
				alpha = smoothstep(0, 1, lengthValue);
			}

			fixed4 frag(v2f i) :SV_Target
			{
				fixed2 screenUV = i.uv;

				float _c0length = length(screenUV - _Position_0);
				fixed4 _c0 = step(_c0length, _Radius_0) * (1 - _Color_0);
				_c0 *= smoothstep(_Radius_0, _Radius_0 - 0.4f, _c0length);
				_c0 *= _Enable_0;

				float _c1length = length(screenUV - _Position_1);
				fixed4 _c1 = step(_c1length, _Radius_1) * (1 - _Color_1);
				_c1 *= smoothstep(_Radius_1, _Radius_1 - 0.4f, _c1length);
				_c1 *= _Enable_1;

				fixed4 color = _BaseColor * (1-(_c0 + _c1));

				return color;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
