using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Documents;
using CodeShare.Model;
using CodeShare.Model.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeBlock : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(CodeBlock), new PropertyMetadata(default(string), CodeChanged));

        private static void CodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CodeBlock codeBlock)) return;
            codeBlock.CodeTextBlock.Text = e.NewValue as string;
            codeBlock.UpdateLineNumbers();
            codeBlock.ColorFormat();
        }

        public string Code
        {
            get => (string) GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        private RelayCommand _copyCodeCommand;
        public ICommand CopyCodeCommand => _copyCodeCommand = _copyCodeCommand ?? new RelayCommand(parameter => CopyCode());

        public CodeBlock()
        {
            InitializeComponent();
        }

        private void CopyCode()
        {
            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(Code);
            Clipboard.SetContent(dataPackage);
        }

        private void UpdateLineNumbers()
        {
            LineNumbers.Text = "";
            if (string.IsNullOrWhiteSpace(Code)) return;
            LineNumbers.Text = GenerateLineNumbers(Code.Split('\n').Length);
        }

        private string GenerateLineNumbers(int numberOfLines)
        {
            var lineNumbers = "";

            for (int i = 1; i <= numberOfLines; i++)
                lineNumbers += $"{i}\n";

            return lineNumbers;
        }

        private void ColorFormat()
        {
            if (string.IsNullOrWhiteSpace(Code)) return;
            foreach (var keyWord in KeyWords)
            {
                var startIndexes = Code.AllIndexesOf(keyWord);
                foreach (var index in startIndexes)
                {
                    var startPointer = CodeTextBlock.ContentStart.GetPositionAtOffset(index, LogicalDirection.Forward);
                    var endPointer = CodeTextBlock.ContentStart.GetPositionAtOffset(index + keyWord.Length, LogicalDirection.Forward);

                    CodeTextBlock.Select(startPointer, endPointer);
                    var kek = CodeTextBlock.Inlines;
                }
            }
        }

        public static List<string> KeyWords = new List<string>
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
