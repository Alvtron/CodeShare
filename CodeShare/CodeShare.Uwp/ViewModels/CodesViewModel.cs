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
    public class CodesViewModel : BaseViewModel
    {
        private IList<Code> _codes = new List<Code>();
        public IList<Code> Codes
        {
            get => _codes;
            set => SetField(ref _codes, value);
        }

        public CodesViewModel()
        {
            InitializeSearchList();
        }

        private void InitializeSearchList()
        {
            var allCodes = RestApiService<Code>.Get().Result;

            if (allCodes == null)
                return;

            Codes = new List<Code>(allCodes);
        }
    }
}
