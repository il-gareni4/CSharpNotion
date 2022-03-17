using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
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

        public CodeBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Caption = blockValue?.Properties?.Caption?.ElementAt(0)[0].GetString() ?? "";
            WrapCode = blockValue?.Format?.CodeWrap ?? false;
            Language = CodeBlockLanguageExtensions.GetLanguageByString(blockValue?.Properties?.Language?.ElementAt(0)[0]) ?? CodeBlockLanguage.JavaScript;
        }

        public CodeBlock SetCaption(string caption)
        {
            if (caption == Caption) return this;
            Dictionary<string, object?> args = new() { { "caption", new string[][] { new string[] { caption } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Caption = caption
            );
            return this;
        }

        public CodeBlock SetWrapCode(bool wrapCode)
        {
            if (wrapCode == WrapCode) return this;
            Dictionary<string, object?> args = new() { { "code_wrap", wrapCode } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => WrapCode = wrapCode
            );
            return this;
        }

        public CodeBlock SetLanguage(CodeBlockLanguage language)
        {
            if (language == Language) return this;
            Dictionary<string, object?> args = new() { { "language", new string[][] { new string[] { language.ToCodeNameString() } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Language = language
            );
            return this;
        }

        public override CodeBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }
    }
}