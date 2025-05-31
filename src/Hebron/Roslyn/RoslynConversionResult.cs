using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Hebron.Roslyn
{
	public class RoslynConversionResult
	{
		public readonly Dictionary<string, EnumDeclarationSyntax> NamedEnums = [];
		public readonly Dictionary<string, FieldDeclarationSyntax> UnnamedEnumValues = [];
		public readonly Dictionary<string, DelegateDeclarationSyntax> Delegates = [];
		public readonly Dictionary<string, FieldDeclarationSyntax> GlobalVariables = [];
		public readonly Dictionary<string, TypeDeclarationSyntax> Structs = [];
		public readonly Dictionary<string, MethodDeclarationSyntax> Functions = [];
	}
}
