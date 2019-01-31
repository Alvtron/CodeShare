using CodeShare.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeItem : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(Code), typeof(CodeItem), new PropertyMetadata(new Code()));

        public Code Code
        {
            get => GetValue(CodeProperty) as Code;
            set => SetValue(CodeProperty, value);
        }

        public CodeItem()
        {
            this.InitializeComponent();
        }
    }
}
