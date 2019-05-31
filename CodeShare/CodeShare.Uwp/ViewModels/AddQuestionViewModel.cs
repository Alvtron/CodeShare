// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="AddQuestionViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class AddQuestionViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    public class AddQuestionViewModel : DialogViewModel
    {
        /// <summary>
        /// Gets or sets the unfiltered code languages.
        /// </summary>
        /// <value>The unfiltered code languages.</value>
        private List<CodeLanguage> UnfilteredCodeLanguages { get; set; }

        /// <summary>
        /// The filtered code languages
        /// </summary>
        private List<CodeLanguage> _filteredCodeLanguages;
        /// <summary>
        /// Gets or sets the filtered code languages.
        /// </summary>
        /// <value>The filtered code languages.</value>
        public List<CodeLanguage> FilteredCodeLanguages
        {
            get => _filteredCodeLanguages;
            set => SetField(ref _filteredCodeLanguages, value);
        }

        /// <summary>
        /// The title
        /// </summary>
        private string _title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        /// <summary>
        /// The question
        /// </summary>
        private string _question;
        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public string Question
        {
            get => _question;
            set => SetField(ref _question, value);
        }

        /// <summary>
        /// The language
        /// </summary>
        private CodeLanguage _language;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Initialize()
        {
            var codeLanguages = await RestApiService<CodeLanguage>.Get();

            if (codeLanguages == null)
            {
                Logger.WriteLine($"Failed to retrieve code languages from database.");
                return;
            }

            UnfilteredCodeLanguages = new List<CodeLanguage>(codeLanguages);
        }

        /// <summary>
        /// upload question as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> UploadQuestionAsync()
        {
            if (CurrentUser == null || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Question) || _language == null)
            {
                Logger.WriteLine("UploadQuestionAsync: One or more fields are empty or not valid.");
                return false;
            }

            var question = new Question(CurrentUser, Title, Question, _language);

            NavigationService.Lock();

            if (await RestApiService<Question>.Add(question) == false)
            {
                NavigationService.Unlock();
                await NotificationService.DisplayErrorMessage("An error occurred during the upload. No changes where made.");
                return false;
            }

            NavigationService.Unlock();

            NavigationService.Navigate(typeof(QuestionPage), question, question.Name);
            return true;
        }

        /// <summary>
        /// Filters the code language.
        /// </summary>
        /// <param name="query">The query.</param>
        public void FilterCodeLanguage(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            if (UnfilteredCodeLanguages == null)
            {
                return;
            }

            //Check each item in search list if it contains the query
            FilteredCodeLanguages = UnfilteredCodeLanguages?
                .Where(x =>
                (x.Extension != null && x.Extension.ToLower().StartsWith(query.ToLower()))
                || (x.Name != null && x.Name.ToLower().StartsWith(query.ToLower())))
                .ToList();
        }

        /// <summary>
        /// Submits the code language.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void SubmitCodeLanguage(object obj)
        {
            if (obj != null)
            {
                _language = obj as CodeLanguage;
            }
        }
    }
}
