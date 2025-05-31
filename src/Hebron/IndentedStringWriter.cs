using System.CodeDom.Compiler;
using System.IO;

namespace Hebron
{
	public class IndentedStringWriter() : IndentedTextWriter(new StringWriter(), "\t")
	{
		private new StringWriter InnerWriter => (StringWriter)base.InnerWriter;

		public string Result => InnerWriter.ToString();

		public override string ToString() => Result;
	}
}
