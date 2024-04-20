//using ColorHelper;
using ColorMine.ColorSpaces;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Threading;

namespace RgbController
{
	internal enum RGBMode
	{
		TrueHSV,
		PowerConsciousHSV,
		Sine
	}

	internal class Rainbow
	{
		private readonly byte[] lights = { 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 15, 17, 18, 20, 22, 24, 26, 28, 30, 32, 35, 37, 39, 42, 44, 47, 49, 52, 55, 58, 60, 63, 66, 69, 72, 75, 78, 81, 85, 88, 91, 94, 97, 101, 104, 107, 111, 114, 117, 121, 124, 127, 131, 134, 137, 141, 144, 147, 150, 154, 157, 160, 163, 167, 170, 173, 176, 179, 182, 185, 188, 191, 194, 197, 200, 202, 205, 208, 210, 213, 215, 217, 220, 222, 224, 226, 229, 231, 232, 234, 236, 238, 239, 241, 242, 244, 245, 246, 248, 249, 250, 251, 251, 252, 253, 253, 254, 254, 255, 255, 255, 255, 255, 255, 255, 254, 254, 253, 253, 252, 251, 251, 250, 249, 248, 246, 245, 244, 242, 241, 239, 238, 236, 234, 232, 231, 229, 226, 224, 222, 220, 217, 215, 213, 210, 208, 205, 202, 200, 197, 194, 191, 188, 185, 182, 179, 176, 173, 170, 167, 163, 160, 157, 154, 150, 147, 144, 141, 137, 134, 131, 127, 124, 121, 117, 114, 111, 107, 104, 101, 97, 94, 91, 88, 85, 81, 78, 75, 72, 69, 66, 63, 60, 58, 55, 52, 49, 47, 44, 42, 39, 37, 35, 32, 30, 28, 26, 24, 22, 20, 18, 17, 15, 13, 12, 11, 9, 8, 7, 6, 5, 4, 3, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		private readonly byte[] HSVlights = { 0, 4, 8, 13, 17, 21, 25, 30, 34, 38, 42, 47, 51, 55, 59, 64, 68, 72, 76, 81, 85, 89, 93, 98, 102, 106, 110, 115, 119, 123, 127, 132, 136, 140, 144, 149, 153, 157, 161, 166, 170, 174, 178, 183, 187, 191, 195, 200, 204, 208, 212, 217, 221, 225, 229, 234, 238, 242, 246, 251, 255 };

		private readonly byte[] HSVpower = { 0, 2, 4, 6, 8, 11, 13, 15, 17, 19, 21, 23, 25, 28, 30, 32, 34, 36, 38, 40, 42, 45, 47, 49, 51, 53, 55, 57, 59, 62, 64, 66, 68, 70, 72, 74, 76, 79, 81, 83, 85, 87, 89, 91, 93, 96, 98, 100, 102, 104, 106, 108, 110, 113, 115, 117, 119, 121, 123, 125, 127, 130, 132, 134, 136, 138, 140, 142, 144, 147, 149, 151, 153, 155, 157, 159, 161, 164, 166, 168, 170, 172, 174, 176, 178, 181, 183, 185, 187, 189, 191, 193, 195, 198, 200, 202, 204, 206, 208, 210, 212, 215, 217, 219, 221, 223, 225, 227, 229, 232, 234, 236, 238, 240, 242, 244, 246, 249, 251, 253, 255 };

		private readonly OpenRGBClient RGBClient;

		public Rainbow(OpenRGBClient client)
		{
			RGBClient = client;
		}

		// the real HSV rainbow
		private Color[] TrueHSV(int angle)
		{
			byte red;
			byte green;
			byte blue;

			if (angle < 60)
			{
				red = 255;
				green = HSVlights[angle];
				blue = 0;
			}
			else
			{
				if (angle < 120)
				{
					red = HSVlights[120 - angle];
					green = 255;
					blue = 0;
				}
				else
				{
					if (angle < 180)
					{
						red = 0;
						green = 255;
						blue = HSVlights[angle - 120];
					}
					else
					{
						if (angle < 240)
						{
							red = 0;
							green = HSVlights[240 - angle];
							blue = 255;
						}
						else
						{
							if (angle < 300)
							{
								red = HSVlights[angle - 240];
								green = 0;
								blue = 255;
							}
							else
							{
								red = 255;
								green = 0;
								blue = HSVlights[360 - angle];
							}
						}
					}
				}
			}

			return new Color[] { new Color(red, green, blue) };
		}

		// the 'power-conscious' HSV rainbow
		private Color[] PowerHSV(int angle)
		{
			byte red;
			byte green;
			byte blue;
			if (angle < 120)
			{
				red = HSVpower[120 - angle];
				green = HSVpower[angle];
				blue = 0;
			}
			else
			{
				if (angle < 240)
				{
					red = 0;
					green = HSVpower[240 - angle];
					blue = HSVpower[angle - 120];
				}
				else
				{
					red = HSVpower[angle - 240];
					green = 0;
					blue = HSVpower[360 - angle];
				}
			}

			return new Color[] { new Color(red, green, blue) };
		}

		// sine wave rainbow
		private Color[] SineLED(int angle)
		{
			return new Color[]
			{
				new Color(lights[(angle + 120) % 360],
				lights[angle],
				lights[(angle + 240) % 360])
			};
		}

		public void Loop(int deviceId, RGBMode mode = RGBMode.TrueHSV)
		{
			for (int i = 0; i < 360; i++)
			{
				var color = new Hsl
				{
					H = i,
					S = 0.5,
					L = 0.5
				}.ToRgb();
				RGBClient.UpdateLeds(deviceId, new Color[] { new Color(Convert.ToByte(color.R), Convert.ToByte(color.G), Convert.ToByte(color.B)) });
				Thread.Sleep(30);
				if (i == 359)
					i = -1;
				//var c = ColorConverter.HslToRgb(new HSL(i, 5, 5));
				//new Colorspace.ColorUtil().
				//do something with the color
			}

			for (int k = 0; k < 360; k++)
			{

				// uncomment the mode (or modes) you need below.
				// with all six PWM outputs connected you may use 2 modes, change one 0 to 1.		


				RGBClient.UpdateLeds(deviceId, GetNextColor(mode, k));
				Thread.Sleep(30);
				if (k == 359)
					k = -1;
			}
		}

		private Color[] GetNextColor(RGBMode mode, int k)
		{
			return mode switch
			{
				RGBMode.TrueHSV => TrueHSV(k),
				RGBMode.PowerConsciousHSV => PowerHSV(k),
				RGBMode.Sine => SineLED(k),
				_ => throw new ArgumentException(),
			};
		}
	}
}
