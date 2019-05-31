// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="AddCodeViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class AddCodeViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    public class AddCodeViewModel : DialogViewModel
    {
        /// <summary>
        /// Gets or sets the code languages.
        /// </summary>
        /// <value>The code languages.</value>
        private CodeLanguage[] CodeLanguages { get; set; }

        /// <summary>
        /// The changed
        /// </summary>
        private bool _changed;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AddCodeViewModel"/> is changed.
        /// </summary>
        /// <value><c>true</c> if changed; otherwise, <c>false</c>.</value>
        public bool Changed
        {
            get => _changed;
            set => SetField(ref _changed, value);
        }

        /// <summary>
        /// The code
        /// </summary>
        private Code _code;
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public Code Code
        {
            get => _code;
            set => SetField(ref _code, value);
        }

        /// <summary>
        /// The add code from file command
        /// </summary>
        private RelayCommand _addCodeFromFileCommand;
        /// <summary>
        /// Gets the add code from file command.
        /// </summary>
        /// <value>The add code from file command.</value>
        public ICommand AddCodeFromFileCommand => _addCodeFromFileCommand = _addCodeFromFileCommand ?? new RelayCommand(async param => await AddCodeFromFileAsync());

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCodeViewModel"/> class.
        /// </summary>
        public AddCodeViewModel()
        {
            if (CurrentUser == null)
            {
                NavigationService.GoBack();
                return;
            }

            Code = new Code { UserUid = CurrentUser.Uid };
        }

        /// <summary>
        /// add code from file as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task AddCodeFromFileAsync()
        {
            var files = await StorageUtilities.PickMultipleFiles();

            if (files == null || files.Count == 0)
            {
                return;
            }

            if (CodeLanguages == null)
            {
                await InitializeCodeLanguages();
            }

            foreach (var file in files)
            {
                var fileType = file.FileType.ToLower();
                var codeLanguage = CodeLanguages.FirstOrDefault(cl => cl.Extension.ToLower().Equals(fileType));
                    
                if (codeLanguage == null)
                {
                    Logger.WriteLine($"Rejected file {file.Name}. Extension is not supported.");
                    continue;
                }

                try
                {
                    var codeFile = new CodeFile(codeLanguage, await FileIO.ReadTextAsync(file), file.DisplayName, file.FileType);
                    Code.AddFile(codeFile, CurrentUser);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            Changed = true;
        }

        /// <summary>
        /// Initializes the code languages.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task InitializeCodeLanguages()
        {
            var codeLanguages = await RestApiService<CodeLanguage>.Get();

            if (codeLanguages == null)
            {
                Logger.WriteLine($"Failed to retrieve code languages from database.");
                return;
            }

            CodeLanguages = codeLanguages;
        }

        /// <summary>
        /// upload code as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> UploadCodeAsync()
        {
            if (!Code.Valid)
            {
                Logger.WriteLine("UploadCodeAsync: One or more fields are empty or not valid.");
                return false;
            }

            NavigationService.Lock();

            if (await RestApiService<Code>.Add(Code) == false)
            {
                NavigationService.Unlock();
                Logger.WriteLine("UploadCodeAsync: An error occurred during the upload. No changes where made.");
                return false;
            }

            NavigationService.Unlock();

            NavigationService.Navigate(typeof(CodePage), Code, Code.Name);
            return true;
        }
    }
}
