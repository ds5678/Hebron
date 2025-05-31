using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Hebron.Rust
{
	internal static partial class RustUtility
	{
		private static readonly HashSet<string> _specialWords = ["type", "in", "final"];

		public static string FixSpecialWords(this string name)
		{
			if (_specialWords.Contains(name))
			{
				name = "_" + name + "_";
			}

			return name;
		}

		public static string ApplyCast(this string expr, string type)
		{
			if (string.IsNullOrEmpty(expr))
			{
				return expr;
			}

			if (type.StartsWith("["))
			{
				// Don't cast to array types
				return expr;
			}

			var lastCast = string.Empty;
			var dexpr = expr.Deparentize();

			var m = CastRegex().Match(dexpr);
			if (m.Success)
			{
				lastCast = m.Groups[2].Value;
			}

			if (!string.IsNullOrEmpty(lastCast) && string.CompareOrdinal(lastCast, type) == 0)
			{
				return expr;
			}

			return (expr.Parentize() + " as " + type).Parentize();
		}

		internal static string GetExpression(this CursorProcessResult? cursorProcessResult)
		{
			return cursorProcessResult != null ? cursorProcessResult.Expression : string.Empty;
		}

		public static string UpdateNativeCall(this string functionName)
		{
			if (functionName.IsNativeFunctionName())
			{
				return "c_runtime::" + functionName;
			}

			return functionName;
		}

		public static string GetDefaltValue(this BaseTypeDescriptor type)
		{
			if (type is PrimitiveTypeInfo asPrimitive)
			{
				switch (asPrimitive.PrimitiveType)
				{
					case PrimitiveType.Boolean:
						return "false";
					case PrimitiveType.Byte:
						return "0";
					case PrimitiveType.Sbyte:
						return "0";
					case PrimitiveType.UShort:
						return "0";
					case PrimitiveType.Short:
						return "0";
					case PrimitiveType.Float:
						return "0.0f32";
					case PrimitiveType.Double:
						return "0.0f64";
					case PrimitiveType.Int:
						return "0";
					case PrimitiveType.Uint:
						return "0";
					case PrimitiveType.Long:
						return "0";
					case PrimitiveType.ULong:
						return "0";
				}
			}

			if (type is StructTypeInfo asStruct)
			{
				return asStruct.StructName + "::default()";
			}

			throw new Exception($"Unable to create default value for type {type.ToString()}");
		}

		public static string GetDefaltValue(this TypeInfo type)
		{
			if (type.IsArray)
			{
				var sb = new StringBuilder();
				if (type.ConstantArraySizes.Length > 0)
				{
					sb.Append(new string('[', type.ConstantArraySizes.Length));
					sb.Append(GetDefaltValue(type.TypeDescriptor));
					for (var i = 0; i < type.ConstantArraySizes.Length; ++i)
					{
						sb.Append(';');
						sb.Append(type.ConstantArraySizes[i]);
						sb.Append(']');
					}

					return sb.ToString();
				}
			}

			if (type.IsPointer)
			{
				return "std::ptr::null_mut()";
			}

			return GetDefaltValue(type.TypeDescriptor);
		}

		public static string SizeOfExpr(this string type)
		{
			return "std::mem::size_of::<" + type + ">() as u64";
		}

		public static string ToRustTypeName(this string type)
		{
			return type switch
			{
				"bool" => "bool",
				"unsigned char" => "u8",
				"char" => "i8",
				"unsigned short" => "u16",
				"short" => "i16",
				"float" => "f32",
				"double" => "f64",
				"int" or "long" => "i32",
				"unsigned int" or "unsigned long" => "u32",
				"long long" => "i64",
				"unsigned long long" => "u64",
				_ => type,
			};
		}

		[GeneratedRegex(@"(.+)\s*as\s*(\w+)")]
		private static partial Regex CastRegex();
	}
}
