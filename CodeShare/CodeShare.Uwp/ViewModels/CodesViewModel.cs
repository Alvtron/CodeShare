using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeShare.Uwp.ViewModels
{
    public class CodesViewModel : ObservableObject
    {
        private ObservableCollection<Code> _codes;
        public ObservableCollection<Code> Codes
        {
            get => _codes;
            set => SetField(ref _codes, value);
        }

        public CodesViewModel(IEnumerable<Code> codes)
        {
            if (codes == null)
            {
                throw new ArgumentNullException("Codes was null");
            }

            Codes = new ObservableCollection<Code>(codes);
        }
    }
}
