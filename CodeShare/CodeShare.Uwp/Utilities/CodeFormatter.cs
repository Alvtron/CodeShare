// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeFormatter.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class CodeEditor.
    /// </summary>
    public static class CodeEditor
    {
        /// <summary>
        /// Syntaxes the highlight.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <returns>IEnumerable&lt;Inline&gt;.</returns>
        public static IEnumerable<Inline> SyntaxHighlight(string text, Color color)
        {
            var lines = text.Split("\r\n");

            foreach (var line in lines)
            {
                var words = line.Split(' ');
                foreach (var word in words)
                {
                    if (KeyWords.Any(keyWord => keyWord.Equals(word.Replace("\r", "").Replace("\t", "").Replace(";", ""))))
                    {
                        yield return new Run { Text = word, Foreground = new SolidColorBrush(color) };
                    }
                    else
                    {
                        yield return new Run { Text = word };
                    }
                    yield return new Run { Text = " " };
                }
                yield return new Run { Text = "\r\n" };
            }
        }

        /// <summary>
        /// Gets the key words.
        /// </summary>
        /// <value>The key words.</value>
        public static string[] KeyWords { get; } = new string[]
        {
            "abstract",
            "add",
            "alias",
            "as",
            "ascending",
            "async",
            "await",
            "base",
            "bool",
            "break",
            "by",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "clause)",
            "condition)",
            "const",
            "constraint)",
            "continue",
            "decimal",
            "default",
            "delegate",
            "descending",
            "do",
            "double",
            "dynamic",
            "else",
            "enum",
            "equals",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "from",
            "get",
            "global",
            "goto",
            "group",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "into",
            "is",
            "join",
            "let",
            "lock",
            "long",
            "nameof",
            "namespace",
            "new",
            "null",
            "object",
            "on",
            "operator",
            "orderby",
            "out",
            "override",
            "params",
            "partial",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "remove",
            "return",
            "sbyte",
            "sealed",
            "select",
            "set",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "static using",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "type",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "value",
            "var",
            "virtual",
            "void",
            "volatile",
            "when",
            "where",
            "where",
            "while",
            "yield"
        };
    }
}
