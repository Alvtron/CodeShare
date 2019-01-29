using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
