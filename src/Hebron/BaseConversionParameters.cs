using System;

namespace Hebron
{
	[Flags]
	public enum ConversionEntities
	{
		Enums = 1,
		GlobalVariables = 2,
		Structs = 4,
		Functions = 8,
		All = Enums | GlobalVariables | Structs | Functions
	}

	public class BaseConversionParameters
	{
		public ConversionEntities ConversionEntities = ConversionEntities.All;

		public required string InputPath { get; set; }
		public string[] Defines { get; set; }
		public string[] AdditionalIncludeDirectories { get; set; }

		/// <summary>
		/// Names of enums to skip
		/// </summary>
		public string[] SkipEnums { get; set; }

		/// <summary>
		/// Names of structs to skip
		/// </summary>
		public string[] SkipStructs { get; set; }

		/// <summary>
		/// Names of global variables to skip
		/// </summary>
		public string[] SkipGlobalVariables { get; set; }

		/// <summary>
		/// Names of functions to skip
		/// </summary>
		public string[] SkipFunctions { get; set; }
		
		public BaseConversionParameters()
		{
			AdditionalIncludeDirectories = [];
			Defines = [];
			SkipEnums = [];
			SkipStructs = [];
			SkipGlobalVariables = [];
			SkipFunctions = [];
		}
	}
}
