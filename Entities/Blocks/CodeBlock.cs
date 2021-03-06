using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public enum CodeBlockLanguage
    {
        ABAP,
        Arduino,
        Bash,
        BASIC,
        C,
        Clojure,
        CoffeeScript,
        CPlusPlus,
        CSharp,
        CSS,
        Dart,
        Diff,
        Docker,
        Elixir,
        Elm,
        Erlang,
        Flow,
        Fortran,
        FSharp,
        Gherkin,
        GLSL,
        Go,
        GraphQL,
        Groovy,
        Haskell,
        HTML,
        Java,
        JavaScript,
        JSON,
        Julia,
        Kotlin,
        LaTeX,
        Less,
        Lisp,
        LiveScript,
        Lua,
        Makefile,
        Markdown,
        Markup,
        MATLAB,
        Mermaid,
        Nix,
        ObjectiveC,
        OCaml,
        Pascal,
        Perl,
        PHP,
        PlainText,
        PowerShell,
        Prolog,
        Protobuf,
        Python,
        R,
        Reason,
        Ruby,
        Rust,
        Sass,
        Scala,
        Scheme,
        Scss,
        Shell,
        Solidity,
        SQL,
        Swift,
        TypeScript,
        VBNet,
        Verilog,
        VHDL,
        VisualBasic,
        WebAssembly,
        XML,
        YAML
    }

    public static class CodeBlockLanguageExtensions
    {
        public static string ToCodeNameString(this CodeBlockLanguage codeLanguage) => codeLanguage switch
        {
            CodeBlockLanguage.CSharp => "C#",
            CodeBlockLanguage.CPlusPlus => "C++",
            CodeBlockLanguage.FSharp => "F#",
            CodeBlockLanguage.ObjectiveC => "Objective-C",
            CodeBlockLanguage.PlainText => "Plain Text",
            CodeBlockLanguage.VBNet => "VB.Net",
            CodeBlockLanguage.VisualBasic => "Visual Basic",
            _ => codeLanguage.ToString()
        };

        public static CodeBlockLanguage? GetLanguageByString(string? languageString) => languageString switch
        {
            "C#" => CodeBlockLanguage.CSharp,
            "C++" => CodeBlockLanguage.CPlusPlus,
            "F#" => CodeBlockLanguage.FSharp,
            "Objective-C" => CodeBlockLanguage.ObjectiveC,
            "Plain Text" => CodeBlockLanguage.PlainText,
            "VB.Net" => CodeBlockLanguage.VBNet,
            "Visual Basic" => CodeBlockLanguage.VisualBasic,
            null => null,
            _ => Enum.Parse<CodeBlockLanguage>(languageString)
        };
    }

    public class CodeBlock : TitleContainingBlock<CodeBlock>, ICaptionBlock<CodeBlock>
    {
        public string Caption { get; protected set; }
        public bool WrapCode { get; protected set; }
        public CodeBlockLanguage Language { get; protected set; }

        internal CodeBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Caption = blockValue?.Properties?.GetValueOrDefault("caption")?.ElementAt(0)[0].GetString() ?? "";
            WrapCode = blockValue?.Format?.CodeWrap ?? false;
            Language = CodeBlockLanguageExtensions.GetLanguageByString(
                blockValue?.Properties?.GetValueOrDefault("language")?.ElementAt(0)[0].GetString()
            ) ?? CodeBlockLanguage.JavaScript;
        }

        public CodeBlock SetCaption(string caption)
        {
            if (caption == Caption) return this;
            SetProperty("caption", new string[][] { new string[] { caption } }, () => Caption = caption);
            return this;
        }

        public CodeBlock SetWrapCode(bool wrapCode)
        {
            if (wrapCode == WrapCode) return this;
            SetFormat("code_wrap", wrapCode, () => WrapCode = wrapCode);
            return this;
        }

        public CodeBlock SetLanguage(CodeBlockLanguage language)
        {
            if (language == Language) return this;
            SetProperty("language", new string[][] { new string[] { language.ToCodeNameString() } }, () => Language = language);
            return this;
        }

        public override CodeBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }
    }
}