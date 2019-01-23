using CodeShare.Uwp.Services;

namespace CodeShare.Uwp.ViewModels
{
    public class AppSettingsViewModel : BaseViewModel
    {
        public AppSettings AppSettings { get; } = new AppSettings();
    }
}
