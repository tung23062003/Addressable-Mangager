﻿namespace UIWidgets
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Color in HSV format.
	/// </summary>
	[Serializable]
	public struct ColorHSV : IEquatable<ColorHSV>
	{
		const int HueMax = 360;

		/// <summary>
		/// Hue.
		/// </summary>
		[SerializeField]
		public int H;

		/// <summary>
		/// Saturation.
		/// </summary>
		[SerializeField]
		public int S;

		/// <summary>
		/// Value.
		/// </summary>
		[SerializeField]
		public int V;

		/// <summary>
		/// Alpha.
		/// </summary>
		[SerializeField]
		public byte A;

		/// <summary>
		/// Color to use in shader.
		/// </summary>
		/// <value>The color.</value>
		public readonly Color ShaderColor => new Color(H / (float)HueMax, S / 255f, V / 255f, A / 255f);

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgets.ColorHSV"/> struct.
		/// </summary>
		/// <param name="h">Hue.</param>
		/// <param name="s">Saturation.</param>
		/// <param name="v">Value.</param>
		/// <param name="a">Alpha.</param>
		public ColorHSV(int h, int s, int v, byte a)
		{
			H = h;
			S = s;
			V = v;
			A = a;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgets.ColorHSV"/> struct.
		/// </summary>
		/// <param name="color">Color.</param>
		public ColorHSV(Color color)
		{
			float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
			float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
			var delta = max - min;

			H = 0;
			if (delta == 0)
			{
				H = 0;
			}
			else if (max == color.r && color.g >= color.b)
			{
				H = Mathf.RoundToInt(60 * ((color.g - color.b) / delta));
			}
			else if (max == color.r && color.g < color.b)
			{
				H = Mathf.RoundToInt(60 * ((color.g - color.b) / delta)) + HueMax;
			}
			else if (max == color.g)
			{
				H = Mathf.RoundToInt(60 * ((color.b - color.r) / delta)) + 120;
			}
			else if (max == color.b)
			{
				H = Mathf.RoundToInt(60 * ((color.r - color.g) / delta)) + 240;
			}

			if (H < 0)
			{
				H += HueMax;
			}

			S = (max == 0f) ? 0 : Mathf.RoundToInt((1 - (min / max)) * 255);
			V = Mathf.RoundToInt(max * 255);
			A = (byte)Mathf.RoundToInt(color.a * 255);
		}

		/// <summary>
		/// ColorHSV can be converted to Color32.
		/// </summary>
		/// <param name="color">Color HSV.</param>
		/// <returns>Color.</returns>
		public static Color32 ToColor32(ColorHSV color) => (Color)color;

		/// <summary>
		/// ColorHSV can be implicitly converted to Color32.
		/// </summary>
		/// <param name="color">Color.</param>
		public static implicit operator Color32(ColorHSV color) => (Color)color;

		/// <summary>
		/// ColorHSV can be converted to Color.
		/// </summary>
		/// <param name="color">Color HSV.</param>
		/// <returns>Color.</returns>
		public static Color ToColor(ColorHSV color) => color;

		/// <summary>
		/// ColorHSV can be implicitly converted to Color.
		/// </summary>
		/// <param name="color">Color.</param>
		public static implicit operator Color(ColorHSV color)
		{
			var hue = Mathf.Abs((color.H / (float)HueMax) % 1f);
			var saturation = Mathf.Abs((color.S / 256f) % 1f);
			var value = Mathf.Abs((color.V / 256f) % 1f);

			hue = Mathf.Clamp(hue, 0.001f, 0.999f);
			saturation = Mathf.Clamp(saturation, 0.001f, 0.999f);
			value = Mathf.Clamp(value, 0.001f, 0.999f);

			var h6 = hue * 6f;
			if (h6 == 6f)
			{
				h6 = 0f;
			}

			var ihue = (int)h6;
			var p = value * (1f - saturation);
			var q = value * (1f - (saturation * (h6 - ihue)));
			var t = value * (1f - (saturation * (1f - (h6 - ihue))));

			return ihue switch
			{
				0 => new Color(value, t, p, color.A / 255f),
				1 => new Color(q, value, p, color.A / 255f),
				2 => new Color(p, value, t, color.A / 255f),
				3 => new Color(p, q, value, color.A / 255f),
				4 => new Color(t, p, value, color.A / 255f),
				_ => new Color(value, p, q, color.A / 255f),
			};
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public readonly override bool Equals(object obj) => (obj is ColorHSV color) && Equals(color);

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="other">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public readonly bool Equals(ColorHSV other) => H == other.H && S == other.S && V == other.V && A == other.A;

		/// <summary>
		/// Hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode() => H ^ S ^ V ^ A.GetHashCode();

		/// <summary>
		/// Compare specified colors.
		/// </summary>
		/// <param name="color1">First color.</param>
		/// <param name="color2">Second color.</param>
		/// <returns>true if the colors are equal; otherwise, false.</returns>
		public static bool operator ==(ColorHSV color1, ColorHSV color2) => color1.Equals(color2);

		/// <summary>
		/// Compare specified colors.
		/// </summary>
		/// <param name="color1">First color.</param>
		/// <param name="color2">Second color.</param>
		/// <returns>true if the colors not equal; otherwise, false.</returns>
		public static bool operator !=(ColorHSV color1, ColorHSV color2) => !color1.Equals(color2);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0101:Array allocation for params parameter", Justification = "Required.")]
		public override string ToString() => string.Format("HSVA({0}, {1}, {2}, {3})", H.ToString(), S.ToString(), V.ToString(), A.ToString());

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		/// <param name="format">Format.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0101:Array allocation for params parameter", Justification = "Required.")]
		public string ToString(string format)
		{
			return string.Format("HSVA({0}, {1}, {2}, {3})", H.ToString(format), S.ToString(format), V.ToString(format), A.ToString(format));
		}

		static float LerpHue(float a, float b, float t)
		{
			if (a > b)
			{
				(b, a) = (a, b);
				t = 1 - t;
			}

			var delta = b - a;
			return (delta > 180)
				? (a + HueMax + (t * (b - a - HueMax))) % HueMax
				: a + (t * delta);
		}

		/// <summary>
		/// Linearly interpolates between colors a and b by t.
		/// Hue will use direct interpolation.
		/// </summary>
		/// <param name="a">Color a.</param>
		/// <param name="b">Color b.</param>
		/// <param name="t">Float for combining a and b.</param>
		/// <returns>Interpolated color.</returns>
		public static ColorHSV Lerp(ColorHSV a, ColorHSV b, float t)
		{
			t = Mathf.Clamp01(t);
			var h = a.H + ((b.H - a.H) * t);
			var s = a.S + ((b.S - a.S) * t);
			var v = a.V + ((b.V - a.V) * t);
			var alpha = a.A + ((b.A - a.A) * t);

			return new ColorHSV((int)h, (int)s, (int)v, (byte)alpha);
		}

		/// <summary>
		/// Linearly interpolates between colors a and b by t.
		/// Hue will use nearest interpolation: 1..270 will be equivalent 1..-90.
		/// </summary>
		/// <param name="a">Color a.</param>
		/// <param name="b">Color b.</param>
		/// <param name="t">Float for combining a and b.</param>
		/// <returns>Interpolated color.</returns>
		public static ColorHSV LerpAlternative(ColorHSV a, ColorHSV b, float t)
		{
			t = Mathf.Clamp01(t);
			var h = LerpHue(a.H, b.H, t);
			var s = a.S + ((b.S - a.S) * t);
			var v = a.V + ((b.V - a.V) * t);
			var alpha = a.A + ((b.A - a.A) * t);

			return new ColorHSV((int)h, (int)s, (int)v, (byte)alpha);
		}

		/// <summary>
		/// Linearly interpolates between colors a and b by t.
		/// </summary>
		/// <param name="a">Color a</param>
		/// <param name="b">Color b</param>
		/// <param name="t">Float for combining a and b</param>
		/// <returns>Interpolated color.</returns>
		public static Color Lerp(Color a, Color b, float t)
		{
			return Lerp(new ColorHSV(a), new ColorHSV(b), t);
		}

		/// <summary>
		/// Linearly interpolates between colors a and b by t.
		/// Hue will use nearest interpolation: 1..270 is equivalent to 1..-90.
		/// </summary>
		/// <param name="a">Color a</param>
		/// <param name="b">Color b</param>
		/// <param name="t">Float for combining a and b</param>
		/// <returns>Interpolated color.</returns>
		public static Color LerpAlternative(Color a, Color b, float t)
		{
			return LerpAlternative(new ColorHSV(a), new ColorHSV(b), t);
		}

		/// <summary>
		/// Validate channels values.
		/// </summary>
		public void Validate()
		{
			H = Mathf.Clamp(H, 0, HueMax);
			S = Mathf.Clamp(S, 0, 255);
			V = Mathf.Clamp(V, 0, 255);
			A = (byte)Mathf.Clamp(A, 0, 255);
		}
	}
}