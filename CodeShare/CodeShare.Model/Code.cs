using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeShare.Model
{
    public class Code : Content, IClassifiable
    {
        #region Properties

        public User User { get; set; }

        public Guid UserUid { get; set; }

        public CodeLanguage CodeLanguage { get; set; }

        private string _version;
        public string Version
        {
            get => _version;
            set => SetField(ref _version, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        private string _about;
        public string About
        {
            get => _about;
            set => SetField(ref _about, value);
        }

        [NotMapped, JsonIgnore]
        public bool Valid => Uid != Guid.Empty && !string.IsNullOrWhiteSpace(Name);

        #endregion
        #region Constructors

        public Code()
        {
        }

        public Code(string name, IEnumerable<File> files, User author)
        {
            Name = name;
            Files = new ObservableCollection<File>(files);
            UserUid = author.Uid;
            Created = DateTime.Now;
            Logs.Add(new ContentLog(true, $"was created by", author.Uid));
        }

        #endregion
        #region Methods

        public override string ToString() => Name;

        #endregion
    }
}
