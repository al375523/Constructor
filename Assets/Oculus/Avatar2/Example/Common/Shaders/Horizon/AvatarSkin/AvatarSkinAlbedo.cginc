﻿#ifndef AVATAR_SKIN_ALBEDO_CGINC
#define AVATAR_SKIN_ALBEDO_CGINC

// BlendScreen and BlendMultiply correspond to screen and multiply blend modes in image editing applications like Photoshop.
// More information here: https://en.wikipedia.org/wiki/Blend_modes#Multiply_and_Screen

half3 BlendScreen(half3 base, half3 blend) {
	return 1. - ((1. - base) * (1. - blend));
}

half3 BlendScreen(half3 base, half3 blend, half opacity) {
	return lerp(base, BlendScreen(base, blend), opacity);
}

half3 BlendMultiply(half3 base, half3 blend, half opacity) {
	return lerp(base, base * blend, opacity);
}

half3 SkinAlbedo(half4 mainTex, half4 stubbleColor, half stubbleMask) {
  return lerp(BlendMultiply(mainTex.rgb, stubbleColor.rgb, stubbleMask), 
              BlendScreen(mainTex.rgb, stubbleColor.rgb, stubbleMask), 
              step(.5, stubbleColor.a));
}

#endif
