using ClangSharp;
using System;
using System.Linq;

namespace Hebron.Rust
{
	partial class RustCodeConverter
	{
		private void FillTypeDeclaration(Cursor cursor, string name)
		{
			var declaration = new IndentedStringWriter();
			declaration.WriteLine("#[derive(Debug, Copy, Clone)]");
			declaration.WriteLine("pub struct " + name + " {");
			++declaration.Indent;

			var def = new IndentedStringWriter();
			def.WriteLine("impl std::default::Default for " + name + " {");
			++def.Indent;
			def.WriteLine("fn default() -> Self {");
			++def.Indent;
			def.WriteLine(name + " {");
			++def.Indent;

			foreach (var child in cursor.CursorChildren.Cast<NamedDecl>())
			{
				if (child is RecordDecl)
				{
					continue;
				}

				var asField = (FieldDecl)child;
				var childName = asField.Name.FixSpecialWords();
				var typeInfo = asField.Type.ToTypeInfo();

				if (typeInfo.TypeString.Contains("unnamed "))
				{
					// unnamed struct
					var subName = name + "_unnamed1";
					FillTypeDeclaration(child.CursorChildren[0], subName);

					typeInfo = new TypeInfo(new StructTypeInfo(subName), typeInfo.PointerCount, typeInfo.ConstantArraySizes);
				}

				var expr = "pub " + childName + ":" + ToRustString(typeInfo) + ",";
				declaration.WriteLine(expr);

				expr = childName + ": " + typeInfo.GetDefaltValue() + ",";
				def.WriteLine(expr);
			}

			--declaration.Indent;
			declaration.WriteLine('}');

			--def.Indent;
			def.WriteLine('}');
			--def.Indent;
			def.WriteLine('}');
			--def.Indent;
			def.WriteLine('}');

			Result.Structs[name] = declaration.ToString();
			Result.StructDefaults[name] = def.ToString();
		}

		public void ConvertStructs()
		{
			if (!Parameters.ConversionEntities.HasFlag(ConversionEntities.Structs))
			{
				return;
			}

			Logger.Info("Processing structs...");

			_state = State.Structs;

			foreach (var cursor in TranslationUnit.EnumerateCursors())
			{
				if (cursor.CursorKind != ClangSharp.Interop.CXCursorKind.CXCursor_StructDecl)
				{
					continue;
				}

				var recordDecl = (RecordDecl)cursor;
				var name = recordDecl.GetName().FixSpecialWords();
				if (Parameters.SkipStructs.Contains(name))
				{
					Logger.Info("Skipping.");
					continue;
				}

				Logger.Info($"Generating code for struct {name}");

				FillTypeDeclaration(cursor, name);
			}
		}
	}
}
