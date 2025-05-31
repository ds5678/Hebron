using System.Collections.Generic;

namespace Hebron.Rust
{
	public class RustConversionResult
	{
		public readonly Dictionary<string, string> UnnamedEnumValues = [];
		public readonly Dictionary<string, string> GlobalVariables = [];
		public readonly Dictionary<string, string> Structs = [];
		public readonly Dictionary<string, string> StructDefaults = [];
		public readonly Dictionary<string, string> Functions = [];
	}
}
