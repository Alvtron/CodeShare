using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class DialogViewModel : ObservableObject
    {
        private bool _canClose;
        public bool CanClose
        {
            get => _canClose;
            set => SetField(ref _canClose, value);
        }
    }
}
