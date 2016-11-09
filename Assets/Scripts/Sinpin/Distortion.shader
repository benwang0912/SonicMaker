Shader "Custom/Distortion"
{
	Properties
	{
		_MainTex("Texture (R,G=X,Y Distortion; B=Mask; A=Unused)", 2D) = "white" {}
	_IntensityAndScrolling("Intensity (XY); Scrolling (ZW)", Vector) = (0.1,0.1,1,1)
		_DistanceFade("Distance Fade (X=Near, Y=Far, ZW=Unused)", Float) = (20, 50,0,0)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
		Blend One Zero
		Lighting Off
		Fog{ Mode Off }
		ZWrite Off
		LOD 200
		Cull[_CullMode]

		GrabPass{ "_GrabTexture" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex  : POSITION;	//position means local position
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex  : SV_POSITION;	//screen view position
				fixed4 color : COLOR;		// a=distortion intensity multiplier
				half4 texcoord : TEXCOORD0; // xy=distort uv, zw=mask uv
				half4 screenuv : TEXCOORD1; // xy=screenuv, z=distance dependend intensity, w=depth
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _GrabTexture;
			float4 _IntensityAndScrolling;
			half2 _DistanceFade;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;

				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex); // Apply texture tiling and offset.
				o.texcoord.xy += half2(_Time.gg.x, _Time.gg.x) * _IntensityAndScrolling.zw; // Apply texture scrolling.

				o.texcoord.zw = v.texcoord;	//texcoord.zw stores the distortion mask texture coordinates.

				half4 screenpos = ComputeGrabScreenPos(o.vertex);
				o.screenuv.xy = screenpos.xy / screenpos.w;

				// Calculate distance dependend intensity.
				// Blend intensity linearily between near to far params.
				half depth = length(mul(UNITY_MATRIX_MV, v.vertex));
				o.screenuv.z = saturate((_DistanceFade.y - depth) / (_DistanceFade.y - _DistanceFade.x));
				o.screenuv.w = depth;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half2 distort = tex2D(_MainTex, i.texcoord.xy).xy;

				// distort*2-1 transforms range from 0..1 to -1..1.
				// negative values move to the left, positive to the right.
				half2 offset = (distort.xy * 2 - 1) * _IntensityAndScrolling.xy * i.screenuv.z * i.color.a;

				// The mask intensity represents how strong the distortion should be for this pixel.
				half  mask = tex2D(_MainTex, i.texcoord.zw).b;
				offset *= mask;

				// get screen space position of current pixel
				half2 uv = i.screenuv.xy + offset;

				half4 color = tex2D(_GrabTexture, uv);
				UNITY_OPAQUE_ALPHA(color.a);

				return color;
			}
			ENDCG
		}
	}
}
