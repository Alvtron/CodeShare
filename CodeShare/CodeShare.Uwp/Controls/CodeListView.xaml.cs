using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeListView : UserControl
    {
        public static readonly DependencyProperty CodesProperty = DependencyProperty.Register("Codes", typeof(IEnumerable<Code>), typeof(CodeListView), new PropertyMetadata(new List<Code>()));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(CodeListView), new PropertyMetadata(null));
        public static readonly DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(CodeListView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsSearchBoxEnabledProperty = DependencyProperty.Register("IsSearchBoxEnabled", typeof(bool), typeof(CodeListView), new PropertyMetadata(false));

        private ObservableCollection<Code> FilteredCodes { get; set; } = new ObservableCollection<Code>();

        public IEnumerable<Code> Codes
        {
            get => GetValue(CodesProperty) as IEnumerable<Code>;
            set
            {
                SetValue(CodesProperty, value);
                SearchByQuery(SearchQuery);
            }
        }
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public string SearchQuery
        {
            get => GetValue(SearchQueryProperty) as string;
            set
            {
                SearchByQuery(value);
                SetValue(SearchQueryProperty, value);
            }
        }

        public bool IsSearchBoxEnabled
        {
            get => (bool)GetValue(IsSearchBoxEnabledProperty);
            set => SetValue(IsSearchBoxEnabledProperty, value);
        }

        public CodeListView()
        {
            InitializeComponent();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Code code))
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that code. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(CodePage), code, $"{code.Name}");
        }

        private void UpdateHeader()
        {
            Header = FilteredCodes.Count == 1
                ? $"{FilteredCodes.Count} result"
                : $"{FilteredCodes.Count} results";
        }

        private void UpdateListView(IEnumerable<Code> codes)
        {
            FilteredCodes.Clear();
            foreach (var code in codes)
            {
                FilteredCodes.Add(code);
            }
            UpdateHeader();
        }

        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                UpdateListView(Codes);
                return;
            }

            query = query?.ToLower();

            UpdateListView(Codes?.Where(u => u.Name.ToLower().StartsWith(query)));
        }
    }
}
