using CodeShare.Model;
using CodeShare.Model.Extensions;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class ModelSettingsViewModel<T> : ObservableObject where T : IEntity, INotifyPropertyChanged, new()
    {
        private T _modelOriginal;

        private T _modelChanged;
        public T Model
        {
            get => _modelChanged;
            set
            {
                _modelOriginal = value.Clone();
                SetField(ref _modelChanged, value);
            }
        }

        private bool _isModelChanged;
        public bool IsModelChanged
        {
            get => _isModelChanged;
            set => SetField(ref _isModelChanged, value);
        }

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand = _saveCommand ?? new RelayCommand(async param => await SaveChangesAsync());

        private RelayCommand _resetCommand;
        public ICommand ResetCommand => _resetCommand = _resetCommand ?? new RelayCommand(param => Reset());

        public ModelSettingsViewModel(T model)
        {
            Model = model;
            Model.PropertyChanged += (s, e) => ViewModel_ModelChanged(s, e);
        }

        public void ViewModel_ModelChanged(object sender, EventArgs e)
        {
            IsModelChanged = true;
        }

        public async Task SaveChangesAsync()
        {
            NavigationService.Lock();

            if (await UpdateModel())
            {
                Reflection.CopyProperties(_modelChanged, _modelOriginal);
                IsModelChanged = false;
            }

            NavigationService.Unlock();
        }

        public void Reset()
        {
            Reflection.CopyProperties(_modelOriginal, Model);
            IsModelChanged = false;
        }

        private async Task<bool> UpdateModel()
        {
            if (Model == null)
            {
                await NotificationService.DisplayErrorMessage("Some changes are not valid.");
                return false;
            }

            if (await RestApiService<T>.Update(Model, Model.Uid) == false)
            {
                await NotificationService.DisplayErrorMessage("An error occurred during the upload. No changes where made.");
                NavigationService.Unlock();
                return false;
            }

            return true;
        }
    }
}
