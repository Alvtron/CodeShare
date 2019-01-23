using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    public class AddQuestionViewModel : DialogViewModel
    {
        private List<CodeLanguage> UnfilteredCodeLanguages { get; set; }

        private List<CodeLanguage> _filteredCodeLanguages;
        public List<CodeLanguage> FilteredCodeLanguages
        {
            get => _filteredCodeLanguages;
            set => SetField(ref _filteredCodeLanguages, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        private string _question;
        public string Question
        {
            get => _question;
            set => SetField(ref _question, value);
        }

        private CodeLanguage _language;

        public async Task Initialize()
        {
            UnfilteredCodeLanguages = new List<CodeLanguage>(await RestApiService<CodeLanguage>.Get());
        }

        public async Task<bool> UploadQuestionAsync()
        {
            if (AuthService.CurrentUser == null || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Question) || _language == null)
            {
                Debug.WriteLine("UploadQuestionAsync: One or more fields are empty or not valid.");
                return false;
            }

            var question = new Question(AuthService.CurrentUser, Title, Question, _language);

            NavigationService.Lock();

            if (await RestApiService<Question>.Add(question) == false)
            {
                NavigationService.Unlock();
                await NotificationService.DisplayErrorMessage("An error occurred during the upload. No changes where made.");
                return false;
            }

            NavigationService.Unlock();

            NavigationService.Navigate(typeof(QuestionPage), question, question.Title);
            return true;
        }

        public void FilterCodeLanguage(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return;

            if (UnfilteredCodeLanguages == null)
                return;

            //Check each item in search list if it contains the query
            FilteredCodeLanguages = UnfilteredCodeLanguages?.Where(x => x.Extension != null && x.Extension.ToLower().Contains(query.ToLower())).ToList();
        }

        public void SubmitCodeLanguage(object obj)
        {
            if (obj != null)
            {
                _language = obj as CodeLanguage;
            }
        }
    }
}
