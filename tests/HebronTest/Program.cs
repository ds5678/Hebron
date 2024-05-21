using Hebron.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hebron
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = new RoslynConversionParameters
            {
                Defines = new string[]
                {
                    //"CRNLIB_SUPPORT_ATI_COMPRESS",
                    //"CRNLIB_SUPPORT_SQUISH",
                },
                InputPath = @"path\repos\Texture2DDecoder\Texture2DDecoderNative\crunch\crnlib.h",
                SkipGlobalVariables = new string[]
				{
                    //"stbi__g_failure_reason"
                },
                SkipFunctions = new string[]
				{
                    //"stbi__err",
                    //"stbi_failure_reason"
                },
            };


//            var result = TextCodeConverter.Convert(parameters.InputPath, parameters.Defines);


            var result = RoslynCodeConverter.Convert(parameters);

            var cls = ClassDeclaration("Crunch")
                .AddModifiers(Token(SyntaxKind.UnsafeKeyword), Token(SyntaxKind.PartialKeyword));

            foreach (var pair in result.NamedEnums)
            {
                cls = cls.AddMembers(pair.Value);
            }

            foreach (var pair in result.UnnamedEnumValues)
            {
                cls = cls.AddMembers(pair.Value);
            }

            foreach (var pair in result.Delegates)
            {
                cls = cls.AddMembers(pair.Value);
            }

            foreach (var pair in result.Structs)
            {
                cls = cls.AddMembers(pair.Value);
            }

            foreach (var pair in result.GlobalVariables)
            {
                cls = cls.AddMembers(pair.Value);
            }

            foreach (var pair in result.Functions)
            {
                cls = cls.AddMembers(pair.Value);
            }

            var ns = NamespaceDeclaration(ParseName("CrunchSharp")).AddMembers(cls);

            string s;
            using (var sw = new StringWriter())
            {
                ns.NormalizeWhitespace().WriteTo(sw);

                s = sw.ToString();
            }

            //s = s.Replace("stbi__jpeg j = (stbi__jpeg)(stbi__malloc((ulong)(sizeof(stbi__jpeg))))", "var j = new stbi__jpeg()");

            File.WriteAllText(@"path\repos\TextureDecoder\AssetRipper.TextureDecoder\CrunchSharp\Crunch.Generated.cs", s);
        }
    }
}