using DpUtilities;

using Microsoft.Extensions.Configuration;

namespace DpConfigTest;

internal class Program
{
	public static IConfigurationRoot Config;
	static void Main (string [] args)
	{

		using DpConfig dpConfig = new (true, 
			@"C:\Dev\_Libraries\Libraries\DpConsoleUtilities\DpConfig\mysettings.json");
		Config = dpConfig.Config;

		Console.WriteLine ("Hello, World!");

		var sec = Config ["TestSection:TestString"];
		Console.WriteLine ($"Test String: {sec}");

		Console.ReadLine ();
	}
}
