using ClangSharp;
using System;

namespace Hebron.Rust
{
	public class CursorProcessResult
	{
		public Cursor Info { get; }

		public TypeInfo TypeInfo { get; }

		public required string Expression { get; set; }

		public bool IsPointer => TypeInfo.IsPointer;

		public bool IsArray => TypeInfo.IsArray;

		public bool IsPrimitiveNumericType => TypeInfo.IsPrimitiveNumericType();

		public string RustType { get; }

		public CursorProcessResult(RustCodeConverter codeConverter, Cursor cursor)
		{
			Info = cursor ?? throw new ArgumentNullException(nameof(cursor));
			TypeInfo = Info.Handle.Type.ToTypeInfo();
			RustType = codeConverter.ToRustString(TypeInfo);
		}
	}
}
