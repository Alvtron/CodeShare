﻿using CodeShare.Model;
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
    public class AddCodeViewModel : DialogViewModel
    {
        private CodeLanguage[] CodeLanguages { get; set; }

        private bool _changed;
        public bool Changed
        {
            get => _changed;
            set => SetField(ref _changed, value);
        }

        private Code _code;
        public Code Code
        {
            get => _code;
            set => SetField(ref _code, value);
        }

        private RelayCommand _addCodeFromFileCommand;
        public ICommand AddCodeFromFileCommand => _addCodeFromFileCommand = _addCodeFromFileCommand ?? new RelayCommand(async param => await AddCodeFromFileAsync());

        public AddCodeViewModel()
        {
            if (AuthService.CurrentUser == null)
            {
                NavigationService.GoBack();
                return;
            }

            Code = new Code { UserUid = AuthService.CurrentUser.Uid };
        }

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
                    Code.AddFile(codeFile, AuthService.CurrentUser);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }

            Changed = true;
        }

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
