using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class Spectrum : ICloneable
{
	public static float[] defaultWaveLengths = { 380, 400, 420, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620, 640, 660, 680, 700 };
	public float[] wavelengths = defaultWaveLengths;
	public float[] values = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	public Spectrum()
	{
	}
	public Spectrum(Spectrum s) : this((float[])s.wavelengths.Clone(), (float[])s.values.Clone())
	{
	}
	public Spectrum(float[] wavelengths)
	{
		this.wavelengths=wavelengths;
		this.values = new float[wavelengths.Length];
	}
	public Spectrum(float[] wavelengths, float[] values)
	{
		this.wavelengths=wavelengths;
		this.values = values;
	}
	public static Spectrum FromColor(Color color, WavelengthColor.Mode mode = WavelengthColor.Mode.Zucconi)
	{
		return WavelengthColor.GetRedSpectrum(mode) * color.r + 
			WavelengthColor.GetGreenSpectrum(mode) * color.g +
			WavelengthColor.GetBlueSpectrum(mode) * color.b;
	}

	public Color ToColor(WavelengthColor.Mode mode = WavelengthColor.Mode.Zucconi)
	{
		vec3 vec = new vec3();
		//float max = GetMaxValue();
		Spectrum brightness = WavelengthColor.GetBrightnessSpectrum(wavelengths, mode);
		float sum = 0;
		float sumBright = 0;
		float sumR = 0;
		float sumG = 0;
		float sumB = 0;
		for (int i = 0; i < wavelengths.Length; i++)
		{
			var bright = brightness.values[i];
			if (bright == 0)
				continue;
			var wlc = WavelengthColor.GetVec3FromWaveLength(wavelengths[i]);
			sumBright += bright;
			sum += values[i];
			sumR += wlc.x;
			sumG += wlc.y;
			sumB += wlc.z;
			vec += wlc * values[i];
		}
		vec.x /= sumR;
		vec.y /= sumG;
		vec.z /= sumB;
		//if(sum != 0)
		//	return (vec/sum).ToRescaledColor();
		return vec.ToColor();
	}
	
	public float GetValue(float wavelength)
	{
		int i = 0;
		while (i < wavelengths.Length && wavelengths[i] <= wavelength)
		{
			if (wavelengths[i] == wavelength)
				return values[i];
			i++;
		}
		if (i>=wavelengths.Length)
			return values[wavelengths.Length-1];
		if (i == 0)
			return values[0];

		float t = (wavelength - wavelengths[i-1]) / (wavelengths[i]-wavelengths[i-1]);
		return values[i-1] + t * (values[i]-values[i-1]);
	}

	public float GetMaxValue()
	{
		return values.Max();
	}
	
	public Spectrum Resampled()
	{
		return Resampled(defaultWaveLengths);
	}

	/// <summary>
	/// Resample the spectrum with the provided <see cref="wavelengths"/>
	/// </summary>
	/// <param name="wavelengths">the wavelengths to resample with</param>
	/// <returns>the resampled copy of the spectrum</returns>
	public Spectrum Resampled(float[] wavelengths)
	{
		Spectrum res = new Spectrum(wavelengths);
		for(int i = 0; i < wavelengths.Length;i++)
		{
			res.values[i] = GetValue(wavelengths[i]);
		}
		return res;

	}

	/// <summary>
	/// Remove heading and trailing 0 values from the spectrum
	/// </summary>
	/// <returns>this</returns>
	public Spectrum Trim()
	{
		List<float> wavelengths = new List<float>();
		List<float> values = new List<float>();
		var interval = GetNonNullInterval();
		for (int i = 0; i<this.wavelengths.Length; i++)
		{
			if (this.wavelengths[i] >= interval.Item1 && this.wavelengths[i] <= interval.Item2)
			{
				wavelengths.Add(this.wavelengths[i]);
				values.Add(this.values[i]);
			}
		}
		this.wavelengths = wavelengths.ToArray();
		this.values = values.ToArray();
		return this;
	}

	/// <summary>
	/// Get a trimmed version of the spectrum (see <see cref="Trim"/>)
	/// </summary>
	public Spectrum Trimmed
	{
		get
		{
			var s=new Spectrum(this);
			s.Trim();
			return s;
		}
	}

	/// <summary>
	/// Clone the spectrum
	/// </summary>
	/// <returns>the clone</returns>
	public object Clone()
	{
		return new Spectrum((float[])wavelengths.Clone(), (float[])values.Clone());
	}

	public Tuple<float, float> GetNonNullInterval()
	{
		float min = -1;
		float max = -1;
		for(int i = 0; i < wavelengths.Length;i++)
		{
			if (min == -1 && values[i] > 0)
			{
				min = wavelengths[i];
			}
			if (values[i] > 0)
			{
				max = wavelengths[i];
			}
		}
		return new Tuple<float, float>(min, max);
	}
	public Tuple<float, float> GetInterval()
	{
		return new Tuple<float, float>(wavelengths[0], wavelengths[wavelengths.Length-1]);
	}

	#region Operators
	public static Spectrum operator +(Spectrum s1, Spectrum s2)
	{
		var s = new Spectrum(s1.wavelengths);
		for (int i = 0; i<s.values.Length; i++)
		{
			s.values[i] = s1.GetValue(s.wavelengths[i])+s2.GetValue(s.wavelengths[i]);
		}
		return s;
	}
	public static Spectrum operator *(Spectrum s1, Spectrum s2)
	{
		var s = new Spectrum(s1.wavelengths);
		for (int i = 0; i<s.values.Length; i++)
		{
			s.values[i] = s1.GetValue(s.wavelengths[i])*s2.GetValue(s.wavelengths[i]);
		}
		return s;
	}
	public static Spectrum operator /(Spectrum s1, Spectrum s2)
	{
		var s=new Spectrum(s1.wavelengths);
		for(int i=0;i<s.values.Length;i++)
		{
			s.values[i] = s1.GetValue(s.wavelengths[i])/s2.GetValue(s.wavelengths[i]);
		}
		return s;
	}
	
	public static Spectrum operator *(Spectrum s, float scale)
	{
		Spectrum ret=new Spectrum(s.wavelengths);
		for (int i = 0; i<s.values.Length; i++)
		{
			ret.wavelengths[i] = s.wavelengths[i];
			ret.values[i] = s.values[i] * scale;
		}
		return ret;
	}
	public static Spectrum operator /(Spectrum s, float scale)
	{
		Spectrum ret = new Spectrum(s.wavelengths);
		for (int i = 0; i<s.values.Length; i++)
		{
			ret.wavelengths[i] = s.wavelengths[i];
			ret.values[i] = s.values[i] / scale;
		}
		return ret;
	}
	#endregion
	


	
}
