using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	public class CfgColor : ConfigItemVersion
	{

		public CfgColor()
		{
			ValueType = ConfigValueType.Color;
		}


		public override void Initialize(string s)
		{
			Color? c = CfgColor.Parse(s);
			if (c.HasValue) VersionValue = c.Value;
			else VersionValue = null;
		}

		public static Color? Parse(string s)
		{
			// #1012FF
			// #FD012045
			// Yellow	-> alpha = 100%

			// 50% Red
			// 100% WindowText
			// 255 Violet
			// FF# ForestGreen

			// (alpha = 100%)
			// 128 64 96	
			// 2F# 18# 01#
			// 19 F4# 70%   
			// 0* 15% 18%   -> HSB

			// 255 128 64 96
			// 50% 50% 10% 60%
			// 255 20* 30% 50%   -> HSB

			Color? c = null;
			string[] split = s.Split(' ');
			switch (split.Length)
			{
				case 1:
					string t = split[0];
					if (t[0] == '#')
					{
						int argb = 0;
						if (int.TryParse(t.Substring(1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out argb))
						{
							c = Color.FromArgb(argb);
						}
					}
					else c = Color.FromName(t);
					break;

				case 2:
					int alpha = GetColorComponent(split[0]);
					Color baseColor = Color.FromName(split[1]);
					c = Color.FromArgb(alpha, baseColor);
					break;

				case 3:
					c = GetColor("255", split[0], split[1], split[2]);
					break;

				case 4:
					c = GetColor(split[0], split[1], split[2], split[3]);
					break;
			}
			return c;
		}

		private static Color GetColor(string alpha, string x, string y, string z)
		{
			Color c;
			int a = GetColorComponent(alpha);
			if (x.EndsWith("*"))
			{
				float hue = GetColorComponentF(x);
				float sat = GetColorComponentF(y);
				float bri = GetColorComponentF(z);
				c = ColorFromAhsb(a, hue, sat, bri);
			}
			else
			{
				int red = GetColorComponent(x);
				int grn = GetColorComponent(y);
				int blu = GetColorComponent(z);
				c = Color.FromArgb(a, red, grn, blu);
			}
			return c;
		}

		private static float GetColorComponentF(string s)
		{
			int i;
			double d;
			GetColorComponent(s, out i, out d);
			return (float)d;
		}

		private static int GetColorComponent(string s)
		{
			int i;
			double d;
			GetColorComponent(s, out i, out d);
			return i;
		}
		
		private static void GetColorComponent(string s, out int i, out double d)
		{
			// hex: eg. 1F#
			// 0..255:  eg. 128
			// 0..100%: eg. 50%
			// 0..360 degrees: eg. 180*
			i = -1;
			d = double.NaN;
			char last = s[s.Length - 1];
			string t = char.IsDigit(last) ? s : s.Remove(s.Length - 1);
			if (last == '#')
			{
				int h;
				if (int.TryParse(t, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out h))
				{
					i = h;
				}
			}
			else
			{
				int h;
				if (int.TryParse(t, out h))
				{
					if (last == '%') i = Convert.ToInt32(255 * (d = h / 100.0));
					else if (last == '*') d = i = h;
					else i = h;
				}
			}
		}

		public override string ToString()
		{
			Color c = (Color)VersionValue;
			return '#' + c.ToArgb().ToString("X8");
		}

		/// <summary>
		/// Insert a 
		/// </summary>
		/// <param name="specific"></param>
		/// <returns></returns>
		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			if (specific == null) return this;

			Color? result = null;
			double? alpha = null;
			switch (specific.ValueType)
			{
					// for numbers: replace the alpha by the specific value.
				case ConfigValueType.Integer:
					result = Color.FromArgb((int)specific.VersionValue, (Color)VersionValue);
					break;
				case ConfigValueType.Decimal:
					alpha = Convert.ToDouble(specific.VersionValue);
					break;
				case ConfigValueType.Real:
					alpha = (double)specific.VersionValue;
					break;
				case ConfigValueType.Color: // blend the two colours by taking the average of each component
					Color c1 = (Color)VersionValue;
					Color c2 = (Color)specific.VersionValue;
					int a = (c1.A + c2.A) >> 1;
					int r = (c1.R + c2.R) >> 1;
					int g = (c1.G + c2.G) >> 1;
					int b = (c1.B + c2.B) >> 1;
					result = Color.FromArgb(a, r, g, b);
					break;
			}
			if (alpha.HasValue) result = Color.FromArgb(Convert.ToInt32(alpha.Value * 255), (Color)VersionValue);
			return result == null ? this : Clone(result);
		}




		// adapted from http://blogs.msdn.com/cjacks/archive/2006/04/12/575476.aspx
		private static Color ColorFromAhsb(int a, float h, float s, float b)
		{
			if (0 == s)
			{
				return Color.FromArgb(a, Convert.ToInt32(b * 255),
					Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
			}

			float fMax, fMid, fMin;
			int iSextant, iMax, iMid, iMin;

			if (0.5 < b)
			{
				fMax = b - (b * s) + s;
				fMin = b + (b * s) - s;
			}
			else
			{
				fMax = b + (b * s);
				fMin = b - (b * s);
			}

			iSextant = (int)Math.Floor(h / 60f);
			if (300f <= h)
			{
				h -= 360f;
			}
			h /= 60f;
			h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
			if (0 == iSextant % 2)
			{
				fMid = h * (fMax - fMin) + fMin;
			}
			else
			{
				fMid = fMin - h * (fMax - fMin);
			}

			iMax = Convert.ToInt32(fMax * 255);
			iMid = Convert.ToInt32(fMid * 255);
			iMin = Convert.ToInt32(fMin * 255);

			switch (iSextant)
			{
				case 1:
					return Color.FromArgb(a, iMid, iMax, iMin);
				case 2:
					return Color.FromArgb(a, iMin, iMax, iMid);
				case 3:
					return Color.FromArgb(a, iMin, iMid, iMax);
				case 4:
					return Color.FromArgb(a, iMid, iMin, iMax);
				case 5:
					return Color.FromArgb(a, iMax, iMin, iMid);
				default:
					return Color.FromArgb(a, iMax, iMid, iMin);
			}
		}

	}
}
