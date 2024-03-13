using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Spectral Colour Schemes
/// By Alan Zucconi
/// Website: www.alanzucconi.com
/// Twitter: @AlanZucconi
/// 
/// Example of different spectral colour schemes
/// to convert visible wavelengths of light (400-700 nm) to RGB colours.
/// 
/// The function "Zucconi6" provides the best approximation
/// without including any branching.
/// Its faster version, "Zucconi", is advised for mobile applications.
/// 
/// Read "Improving the Rainbow" for more information
/// http://www.alanzucconi.com/?p=6703
/// </summary>
public static class WavelengthColor
{
	public enum Mode
	{
		Zucconi,
		Zucconi6,
		Jet,
		GPUGems,
		Bruton,
		Spektre
	}
	
	public static Color GetColorFromWaveLength(float w, Mode mode = Mode.Zucconi)
	{
		switch(mode)
		{
			case Mode.Zucconi:
				return Zucconi(w);
			case Mode.Zucconi6:
				return Zucconi6(w);
			case Mode.GPUGems:
				return GPUGems(w);
			case Mode.Jet:
				return Jet(w);
			case Mode.Bruton:
				return Bruton(w);
			case Mode.Spektre:
				return Spektre(w);
		}
		return Zucconi(w);
	}

	internal static vec3 GetVec3FromWaveLength(float w, Mode mode = Mode.Zucconi)
	{
		switch (mode)
		{
			case Mode.Zucconi:
				return zucconi_vec(w);
			case Mode.Zucconi6:
				return zucconi6_vec(w);
			case Mode.GPUGems:
				return gpu_gems_vec(w);
			case Mode.Jet:
				return jet_vec(w);
			case Mode.Bruton:
				return bruton_vec(w);
			case Mode.Spektre:
				return spektre_vec(w);
		}
		return zucconi_vec(w);
	}

	#region Get primary colors spectrum
	/// <summary>
	/// Get the red spectrum
	/// </summary>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the red spectrum</returns>
	public static Spectrum GetRedSpectrum(Mode mode = Mode.Zucconi)
	{
		return GetRedSpectrum(new Spectrum(Spectrum.defaultWaveLengths), mode);
	}
	/// <summary>
	/// Get the red spectrum
	/// </summary>
	/// <param name="wavelengths">the wavelengths to sample</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the red spectrum</returns>
	public static Spectrum GetRedSpectrum(float[] wavelengths, Mode mode = Mode.Zucconi)
	{
		return GetRedSpectrum(new Spectrum(wavelengths), mode);
	}
	/// <summary>
	/// Get the red spectrum
	/// </summary>
	/// <param name="s">the spectrum (using its wavelengths, values will be overwritten)</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the red spectrum (aka s)</returns>
	/// <returns></returns>
	public static Spectrum GetRedSpectrum(Spectrum s, Mode mode = Mode.Zucconi)
	{
		for(int i=0;i<s.wavelengths.Length;i++)
		{
			s.values[i] = GetColorFromWaveLength(s.wavelengths[i]).r;
		}
		return s;
	}
	/// <summary>
	/// Get the green spectrum
	/// </summary>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the green spectrum</returns>
	public static Spectrum GetGreenSpectrum(Mode mode = Mode.Zucconi)
	{
		return GetGreenSpectrum(new Spectrum(Spectrum.defaultWaveLengths), mode);
	}
	/// <summary>
	/// Get the green spectrum
	/// </summary>
	/// <param name="wavelengths">the wavelengths to sample</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the green spectrum</returns>
	public static Spectrum GetGreenSpectrum(float[] wavelengths, Mode mode = Mode.Zucconi)
	{
		return GetGreenSpectrum(new Spectrum(wavelengths), mode);
	}
	/// <summary>
	/// Get the green spectrum
	/// </summary>
	/// <param name="s">the spectrum (using its wavelengths, values will be overwritten)</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the green spectrum (aka s)</returns>
	/// <returns></returns>
	public static Spectrum GetGreenSpectrum(Spectrum s, Mode mode = Mode.Zucconi)
	{
		for (int i = 0; i<s.wavelengths.Length; i++)
		{
			s.values[i] = GetColorFromWaveLength(s.wavelengths[i]).g;
		}
		return s;
	}
	/// <summary>
	/// Get the blue spectrum
	/// </summary>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the blue spectrum</returns>
	public static Spectrum GetBlueSpectrum(Mode mode = Mode.Zucconi)
	{
		return GetBlueSpectrum(new Spectrum(Spectrum.defaultWaveLengths), mode);
	}
	/// <summary>
	/// Get the blue spectrum
	/// </summary>
	/// <param name="wavelengths">the wavelengths to sample</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the blue spectrum</returns>
	public static Spectrum GetBlueSpectrum(float[] wavelengths, Mode mode = Mode.Zucconi)
	{
		return GetBlueSpectrum(new Spectrum(wavelengths), mode);
	}
	/// <summary>
	/// Get the blue spectrum
	/// </summary>
	/// <param name="s">the spectrum (using its wavelengths, values will be overwritten)</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the blue spectrum (aka s)</returns>
	public static Spectrum GetBlueSpectrum(Spectrum s, Mode mode = Mode.Zucconi)
	{
		for (int i = 0; i<s.wavelengths.Length; i++)
		{
			s.values[i] = GetColorFromWaveLength(s.wavelengths[i]).b;
		}
		return s;
	}
	/// <summary>
	/// Get the brightness spectrum
	/// </summary>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the blue spectrum</returns>
	public static Spectrum GetBrightnessSpectrum(Mode mode = Mode.Zucconi)
	{
		return GetBrightnessSpectrum(new Spectrum(Spectrum.defaultWaveLengths), mode);
	}
	/// <summary>
	/// Get the brightness spectrum
	/// </summary>
	/// <param name="wavelengths">the wavelengths to sample</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the brightness spectrum</returns>
	public static Spectrum GetBrightnessSpectrum(float[] wavelengths, Mode mode = Mode.Zucconi)
	{
		return GetBrightnessSpectrum(new Spectrum(wavelengths), mode);
	}
	/// <summary>
	/// Get the brightness spectrum
	/// </summary>
	/// <param name="s">the spectrum (using its wavelengths, values will be overwritten)</param>
	/// <param name="mode">algorithm to use</param>
	/// <returns>the brightness spectrum (aka s)</returns>
	public static Spectrum GetBrightnessSpectrum(Spectrum s, Mode mode = Mode.Zucconi)
	{
		for (int i = 0; i<s.wavelengths.Length; i++)
		{
			var val = GetVec3FromWaveLength(s.wavelengths[i]);
			//TODO: other brightness computation???
			s.values[i] = (val.x + val.y + val.z)/3;
		}
		return s;
	}
	#endregion

	internal static float saturate(float x)
	{
		return Math.Min(1.0f, Math.Max(0.0f, x));
	}

	internal static vec3 saturate(vec3 x)
	{
		return new vec3(saturate(x.x), saturate(x.y), saturate(x.z));
	}

	// --- Spectral Zucconi --------------------------------------------
	private static vec3 bump3y(vec3 x, vec3 yoffset)
	{
		vec3 y = new vec3(1f, 1f, 1f) - x * x;
		y = saturate(y-yoffset);
		return y;
	}
	/// <summary>
	/// --- Spectral Zucconi --------------------------------------------
	/// By Alan Zucconi
	/// Based on GPU Gems: https://developer.nvidia.com/sites/all/modules/custom/gpugems/books/GPUGems/gpugems_ch08.html
	/// But with values optimised to match as close as possible the visible spectrum
	/// Fits this: https://commons.wikimedia.org/wiki/File:Linear_visible_spectrum.svg
	/// With weighter MSE (RGB weights: 0.3, 0.59, 0.11)
	/// </summary>
	/// <param name="w">wavelength</param>
	/// <returns>color</returns>
	public static Color Zucconi(float w)
	{
		return zucconi_vec(w).ToColor();
	}

	private static vec3 zucconi_vec(float w)
	{
		// w: [400, 700]
		// x: [0,   1]
		float x = saturate((w - 400f)/ 300f);

		vec3 cs = new vec3(3.54541723f, 2.86670055f, 2.29421995f);
		vec3 xs = new vec3(0.69548916f, 0.49416934f, 0.28269708f);
		vec3 ys = new vec3(0.02320775f, 0.15936245f, 0.53520021f);

		return bump3y(cs * (x - xs), ys);
	}

	/// <summary>
	/// --- Spectral Zucconi 6 --------------------------------------------
	/// By Alan Zucconi
	/// Based on GPU Gems: https://developer.nvidia.com/sites/all/modules/custom/gpugems/books/GPUGems/gpugems_ch08.html
	/// But with values optimised to match as close as possible the visible spectrum
	/// Fits this: https://commons.wikimedia.org/wiki/File:Linear_visible_spectrum.svg
	/// With weighter MSE (RGB weights: 0.3, 0.59, 0.11)
	/// Optimised by Alan Zucconi
	/// </summary>
	/// <param name="w">wavelength</param>
	/// <returns>color</returns>
	public static Color Zucconi6(float w)
	{
		return zucconi6_vec(w).ToColor();
	}

	private static vec3 zucconi6_vec(float w)
	{
		// w: [400, 700]
		// x: [0,   1]
		float x = saturate((w - 400f)/ 300f);

		vec3 c1 = new vec3(3.54585104f, 2.93225262f, 2.41593945f);
		vec3 x1 = new vec3(0.69549072f, 0.49228336f, 0.27699880f);
		vec3 y1 = new vec3(0.02312639f, 0.15225084f, 0.52607955f);

		vec3 c2 = new vec3(3.90307140f, 3.21182957f, 3.96587128f);
		vec3 x2 = new vec3(0.11748627f, 0.86755042f, 0.66077860f);
		vec3 y2 = new vec3(0.84897130f, 0.88445281f, 0.73949448f);

		return
			bump3y(c1 * (x - x1), y1) +
			bump3y(c2 * (x - x2), y2);
	}

	/// <summary>
	/// --- MATLAB Jet Colour Scheme ----------------------------------------
	/// </summary>
	/// <param name="w">wavelength</param>
	/// <returns>color</returns>
	public static Color Jet(float w)
	{
		return jet_vec(w).ToColor();
	}

	private static vec3 jet_vec(float w)
	{
		// w: [400, 700]
		// x: [0,   1]
		float x = saturate((w - 400f)/ 300f);
		vec3 c;

		if (x < 0.25)
			c = new vec3(0.0f, 4.0f * x, 1.0f);
		else if (x < 0.5)
			c = new vec3(0.0f, 1.0f, 1.0f + 4.0f * (0.25f - x));
		else if (x < 0.75)
			c = new vec3(4.0f * (x - 0.5f), 1.0f, 0.0f);
		else
			c = new vec3(1.0f, 1.0f + 4.0f * (0.75f - x), 0.0f);

		// Clamp colour components in [0,1]
		return saturate(c);
	}

	// --- GPU Gems -------------------------------------------------------
	private static vec3 bump3(vec3 x)
	{
		vec3 y = new vec3(1f, 1, 1f) - x * x;
		y = new vec3(Math.Max(0f, y.x), Math.Max(0f, y.y), Math.Max(0f, y.z));
		return y;
	}

	/// <summary>
	/// GPU Gems
	/// https://developer.nvidia.com/sites/all/modules/custom/gpugems/books/GPUGems/gpugems_ch08.html
	/// </summary>
	/// <param name="w">wavelength</param>
	/// <returns>color</returns>
	public static Color GPUGems(float w)
	{
		return gpu_gems_vec(w).ToColor();
	}
	private static vec3 gpu_gems_vec(float w)
	{
		// w: [400, 700]
		// x: [0,   1]
		float x = saturate((w - 400f)/ 300f);

		return bump3
		(new vec3
			(
				4f * (x - 0.75f),    // Red
				4f * (x - 0.5f), // Green
				4f * (x - 0.25f) // Blue
			)
		);
	}

	/// <summary>
	/// --- Approximate RGB values for Visible Wavelengths ------------------
	/// by Dan Bruton
	/// http://www.physics.sfasu.edu/astro/color/spectra.html
	/// https://stackoverflow.com/questions/3407942/rgb-values-of-visible-spectrum
	/// </summary>
	/// <param name="w">wavelength</param>
	/// <returns>color</returns>
	public static Color Bruton(float w)
	{
		return bruton_vec(w).ToColor();
	}

	private static vec3 bruton_vec(float w)
	{
		vec3 c;

		if (w >= 380f && w < 440f)
			c = new vec3
			(
				-(w - 440f) / (440f - 380f),
				0.0f,
				1.0f
			);
		else if (w >= 440f && w < 490f)
			c = new vec3
			(
				0.0f,
				(w - 440f) / (490f - 440f),
				1.0f
			);
		else if (w >= 490f && w < 510f)
			c = new vec3
			(0.0f,
				1.0f,
				-(w - 510f) / (510f - 490f)
			);
		else if (w >= 510f && w < 580f)
			c = new vec3
			(
				(w - 510f) / (580f - 510f),
				1.0f,
				0.0f
			);
		else if (w >= 580f && w < 645f)
			c = new vec3
			(
				1.0f,
				-(w - 645f) / (645f - 580f),
				0.0f
			);
		else if (w >= 645f && w <= 780f)
			c = new vec3
			(1.0f,
				0.0f,
				0.0f
			);
		else
			c = new vec3
			(0.0f,
				0.0f,
				0.0f
			);

		return saturate(c);
	}

	/// <summary>
	/// --- Spektre ---------------------------------------------------------
	/// http://stackoverflow.com/questions/3407942/rgb-values-of-visible-spectrum
	/// </summary>
	/// <param name="l">wavelength</param>
	/// <returns>color</returns>
	public static Color Spektre(float w)
	{
		return spektre_vec(w).ToColor();
	}

	private static vec3 spektre_vec(float l)
	{
		float r = 0.0f, g = 0.0f, b = 0.0f;
		if ((l>=400.0)&&(l<410.0)) { float t = (l-400.0f)/(410.0f-400.0f); r=    +(0.33f*t)-(0.20f*t*t); }
		else if ((l>=410.0)&&(l<475.0)) { float t = (l-410.0f)/(475.0f-410.0f); r=0.14f        -(0.13f*t*t); }
		else if ((l>=545.0)&&(l<595.0)) { float t = (l-545.0f)/(595.0f-545.0f); r=    +(1.98f*t)-(t*t); }
		else if ((l>=595.0)&&(l<650.0)) { float t = (l-595.0f)/(650.0f-595.0f); r=0.98f+(0.06f*t)-(0.40f*t*t); }
		else if ((l>=650.0)&&(l<700.0)) { float t = (l-650.0f)/(700.0f-650.0f); r=0.65f-(0.84f*t)+(0.20f*t*t); }
		if ((l>=415.0)&&(l<475.0)) { float t = (l-415.0f)/(475.0f-415.0f); g=             +(0.80f*t*t); }
		else if ((l>=475.0)&&(l<590.0)) { float t = (l-475.0f)/(590.0f-475.0f); g=0.8f +(0.76f*t)-(0.80f*t*t); }
		else if ((l>=585.0)&&(l<639.0)) { float t = (l-585.0f)/(639.0f-585.0f); g=0.82f-(0.80f*t); }
		if ((l>=400.0)&&(l<475.0)) { float t = (l-400.0f)/(475.0f-400.0f); b=    +(2.20f*t)-(1.50f*t*t); }
		else if ((l>=475.0)&&(l<560.0)) { float t = (l-475.0f)/(560.0f-475.0f); b=0.7f -(t)+(0.30f*t*t); }

		return new vec3(r, g, b);
	}

}

