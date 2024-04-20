using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using OpenRGB;
using OpenRGB.NET;
using OpenRGB.NET.Enums;
using OpenRGB.NET.Models;

namespace RgbController
{
	public static class Program
	{
		#region Nested classes to support running as service
		public const string ServiceName = "MyService";

		public class Service : ServiceBase
		{
			public Service()
			{
				ServiceName = Program.ServiceName;
			}

			protected override void OnStart(string[] args)
			{
				Program.Start(args);
			}

			protected override void OnStop()
			{
				Program.Stop();
			}
		}
		#endregion

		static void Main(string[] args)
		{
			if (!Environment.UserInteractive)
				// running as service
				using (var service = new Service())
					ServiceBase.Run(service);
			else
			{
				// running as console app
				Start(args);

				Console.WriteLine("Press any key to stop...");
				Console.ReadKey(true);

				Stop();
			}
		}
		//mobo violet 30,1,55
		//ram violet 25,5,115
		private static void Start(string[] args)
		{
			// onstart code here
			using var client = new OpenRGBClient(name: "RgbControllerNET", autoconnect: true, timeout: 1000);

			var deviceCount = client.GetControllerCount();
			var devices = client.GetAllControllerData();
			var rams = devices.Select((x, i) => new { Position = i, Device = x }).Where(x => x.Device.Type == DeviceType.Dram);
			var mobo = devices.Select((x, i) => new { Position = i, Device = x }).FirstOrDefault(x => x.Device.Type == DeviceType.Motherboard);
			var coolers = devices.Select((x, i) => new { Position = i, Device = x }).Where(x => x.Device.Type == DeviceType.Cooler);
			var gpu = devices.Select((x, i) => new { Position = i, Device = x }).Where(x => x.Device.Type == DeviceType.Gpu);
			var strips = devices.Select((x, i) => new { Position = i, Device = x }).Where(x => x.Device.Type == DeviceType.Ledstrip);
			var unknown = devices.Select((x, i) => new { Position = i, Device = x }).Where(x => x.Device.Type == DeviceType.Unknown);
			if (mobo != null)
			{
				var moboRainbow = new Rainbow(client);
				moboRainbow.Loop(mobo.Position, RGBMode.Sine);
			}
			//foreach (var ram in rams)
			//{
			//	var modeId = ram.Device.Modes
			//		.Select((x, i) => new { Position = i, Mode = x })
			//		.FirstOrDefault(x => x.Mode.Name == "Chase Fade")
			//		?.Position;
			//	if (!modeId.HasValue)
			//		continue;
			//	//var leds = Enumerable.Range(0, ram.Device.Leds.Length)
			//	//    .Select(_ => new Color(55, 0, 245))
			//	//    .ToArray();
			//	var leds = new Color[5]
			//	{
			//		new Color(55, 0, 245),
			//		new Color(65, 50, 250),
			//		new Color(95, 134, 254),
			//		new Color(60, 169, 251),
			//		new Color(34, 211, 255)
			//	};
			//	var leds2 = new Color[5]
			//	{
			//		new Color(34, 211, 255),
			//		new Color(55, 0, 245),
			//		new Color(65, 50, 250),
			//		new Color(95, 134, 254),
			//		new Color(60, 169, 251)
			//	};
			//	var leds3 = new Color[5]
			//	{
			//		new Color(60, 169, 251),
			//		new Color(34, 211, 255),
			//		new Color(55, 0, 245),
			//		new Color(65, 50, 250),
			//		new Color(95, 134, 254),
			//	};
			//	var leds4 = new Color[5]
			//	{
			//		new Color(95, 134, 254),
			//		new Color(60, 169, 251),
			//		new Color(34, 211, 255),
			//		new Color(55, 0, 245),
			//		new Color(65, 50, 250),
			//	};
			//	var leds5 = new Color[5]
			//	{
			//		new Color(65, 50, 250),
			//		new Color(95, 134, 254),
			//		new Color(60, 169, 251),
			//		new Color(34, 211, 255),
			//		new Color(55, 0, 245),
			//	};
			//	var veryLeds = new Color[5][]
			//	{
			//		leds,
			//		leds2,
			//		leds3,
			//		leds4,
			//		leds5
			//	};
			//	client.SetMode(ram.Position, 2 /*modeId.Value*/);
			//	var pos = 0;
			//	for (var j = 0; j < 10; j++)
			//	{
			//		Thread.Sleep(50);
			//		if (pos > 4)
			//			pos = 0;
			//		client.UpdateLeds(ram.Position, veryLeds[pos]);
			//		client.UpdateLeds(ram.Position, veryLeds[pos]);
			//		pos++;
			//	}
			//}
			//for (int i = 0; i < devices.Length; i++)
			//{
			//    var leds = Enumerable.Range(0, devices[i].Colors.Length)
			//        .Select(_ => new Color(255, 0, 0))
			//        .ToArray();
			//    client.UpdateLeds(i, leds);
			//}


		}

		private static void Stop()
		{
			// onstop code here
		}
	}
}
