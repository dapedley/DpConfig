//*************************************************************************************
// DpConfig.cs
//*************************************************************************************
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

namespace DpUtilities;

//*************************************************************************************
// Class DpConfig
//*************************************************************************************
public partial class DpConfig : IDisposable
{
	public IConfigurationRoot Config { get; private set; }

	//-------------------------------------------------------------------------------------
	// Constructor - Creates a Configuration containing both environmental variables and an
	// appsettings.json file.
	// The public property 'Config' is available, after construction, to the main program
	// for it to extract any other data it requires.
	// If 'withLogging' is true (or not set), creates a logger from the "Serilog" section
	// of the appsettings.json data. The logger is accessible via the static (ang globally
	// available) class Log.
	// Depends uopon a valid "Serilog" section having been set up in an appsettings.json
	// file.
	// Called from a main program by:
	//	using Serilog;
	//	using DpUtilities;
	//	... and in the main program ...
	//	using DpConfig dpConfig = new ([true/false]);	// true or absent to enable logging
	// The using is required to ensure that logging is properly flushed and disposed of.
	//-------------------------------------------------------------------------------------

	public DpConfig (bool withLogging = true, string settingsPath = null)
	{
		//*** Set up configuration unless it already exists
		if (Config is null) { SetupConfig (settingsPath); }

		//*** If logging is required, set it up
		if (withLogging) { SetupLogging (); }
	}

	//-------------------------------------------------------------------------------------
	// SetupConfig
	//-------------------------------------------------------------------------------------
	public IConfigurationRoot SetupConfig (string settingsPath = null)
	{
		//*** Does appsettings.json exist?
		bool haveAppSettings = File.Exists ("appSettings.json");

		//*** Is another settings path specified which is not appsettings.json?
		bool haveOtherSettings = settingsPath != null && File.Exists (settingsPath) 
																			&& settingsPath.ToLower() != "appsettings.json";

		try {
			//*** Creates a Configuration instance containing environment variables
			IConfigurationBuilder builder = new ConfigurationBuilder ()
				.SetBasePath (Directory.GetCurrentDirectory ())
				.AddEnvironmentVariables ();

			//*** If appsettings.json exists, add it
			if (haveAppSettings) { builder.AddJsonFile ("appsettings.json"); }

			//*** If another settings file is specified, add that
			if (haveOtherSettings) { builder.AddJsonFile (settingsPath); }

			//*** and build the configuration
			Config = builder.Build ();
		}
		catch (Exception ex) {
			throw new ArgumentException ($"Configuration Error - {ex.Message}");
		}

		return Config;
	}

	//-------------------------------------------------------------------------------------
	// SetupLogging
	//-------------------------------------------------------------------------------------
	public void SetupLogging ()
	{
		try {
			//*** Then creates a logger from the "Serilog" section of the appsettings.json
			Log.Logger = new LoggerConfiguration ()
			.ReadFrom.Configuration (Config)
			.CreateLogger ();
		}
		catch (Exception ex) {
			throw new Exception ($"Logger Configuration Error - {ex.Message}");
		}

		//*** Finally writes a header/separator to the log 
		string sep = new('*', 80);
		string now = $"{DateTime.Now:yyyy-MMM-dd HH:mm:ss.fff}";
		Log.Information ("\n\n{@Sep}\nLogging Started at {Now:}\n", 
			 sep, now);
	}

	//-------------------------------------------------------------------------------------
	// Dispose
	//-------------------------------------------------------------------------------------
	public void Dispose ()
	{
		Log.Information ("*** Logging Ended ***");
		Log.CloseAndFlush ();
		GC.SuppressFinalize (this);
	}
}