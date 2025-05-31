using ClangSharp;
using ClangSharp.Interop;

namespace Hebron
{
	public static class TextCodeConverter
	{
		public static string Convert(string inputPath, string[] defines, string[] additionalIncludeFolders)
		{
			var translationUnit = Utility.Compile(inputPath, defines, additionalIncludeFolders);

			var writer = new IndentedStringWriter();
			foreach (var cursor in translationUnit.EnumerateCursors())
			{
				DumpCursor(writer, cursor);
			}

			return writer.Result;
		}

		private static void DumpCursor(IndentedStringWriter writer, Cursor cursor)
		{
			var line = $"// {cursor.CursorKindSpelling}- {cursor.Spelling} - {clang.getTypeSpelling(clang.getCursorType(cursor.Handle))}";

			var addition = string.Empty;

			switch (cursor.CursorKind)
			{
				case CXCursorKind.CXCursor_UnaryExpr:
				case CXCursorKind.CXCursor_UnaryOperator:
					{
						addition = $"Unary Operator: {cursor.Handle.UnaryOperatorKind} ({cursor.Handle.UnaryOperatorKindSpelling})";
					}
					break;
				case CXCursorKind.CXCursor_BinaryOperator:
					{
						addition = $"Binary Operator: {cursor.Handle.BinaryOperatorKind} ({cursor.Handle.BinaryOperatorKindSpelling})";
					}
					break;
				case CXCursorKind.CXCursor_IntegerLiteral:
				case CXCursorKind.CXCursor_FloatingLiteral:
				case CXCursorKind.CXCursor_CharacterLiteral:
				case CXCursorKind.CXCursor_StringLiteral:
				case CXCursorKind.CXCursor_CXXBoolLiteralExpr:
					addition = $"Literal: {cursor.Handle.GetLiteralString()}";
					break;
			}

			if (!string.IsNullOrEmpty(addition))
			{
				line += " [" + addition + "]";
			}

			writer.WriteLine(line);

			writer.Indent++;
			foreach(var child in cursor.CursorChildren)
			{
				DumpCursor(writer, child);
			}
			writer.Indent--;
		}
	}
}
