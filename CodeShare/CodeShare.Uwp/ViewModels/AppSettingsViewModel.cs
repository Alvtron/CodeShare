using CodeShare.Model;
using CodeShare.Uwp.Services;

namespace CodeShare.Uwp.ViewModels
{
    public class AppSettingsViewModel : ObservableObject
    {
        public AppSettings AppSettings { get; } = new AppSettings();
    }
}
