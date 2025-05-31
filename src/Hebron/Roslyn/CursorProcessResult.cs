using ClangSharp;
using System;

namespace Hebron.Roslyn
{
	public class CursorProcessResult
	{
		public Cursor Info { get; }

		public TypeInfo TypeInfo { get; }

		public required string Expression { get; set; }

		public bool IsPointer => TypeInfo.IsPointer;

		public bool IsArray => TypeInfo.IsArray;

		public string CsType { get; }

		public bool IsClass { get; }

		public CursorProcessResult(RoslynCodeConverter roslynCodeConverter, Cursor cursor)
		{
			ArgumentNullException.ThrowIfNull(cursor);

			Info = cursor;
			TypeInfo = Info.Handle.Type.ToTypeInfo();
			CsType = roslynCodeConverter.ToRoslynString(TypeInfo);
			IsClass = roslynCodeConverter.IsClass(TypeInfo.TypeName);
		}
	}
}
