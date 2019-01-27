﻿using CodeShare.Uwp.Utilities;
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
using System.Threading.Tasks;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeBlock : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(CodeBlock), new PropertyMetadata(default(string)));

        public string Code
        {
            get => (string) GetValue(CodeProperty);
            set
            {
                SetValue(CodeProperty, value);
                UpdateCode(value);
            }
        }

        private RelayCommand _copyCodeCommand;
        public ICommand CopyCodeCommand => _copyCodeCommand = _copyCodeCommand ?? new RelayCommand(parameter => CopyCode());

        private bool LockChangeExecution { get; set; }

        public CodeBlock()
        {
            InitializeComponent();
        }

        private void UpdateCode(string text)
        {
            if (!LockChangeExecution)
            {
                LockChangeExecution = true;

                var inlines = CodeEditor.SyntaxHighlight(text, Colors.DodgerBlue);
                
                CodeTextBlock.Inlines.Clear();

                foreach (var inline in inlines)
                {
                    CodeTextBlock.Inlines.Add(inline);
                }

                UpdateLineNumbers();

                LockChangeExecution = false;
            }
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
    }
}
