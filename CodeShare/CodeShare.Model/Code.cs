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

        private ObservableCollection<File> _files = new ObservableCollection<File>();
        public ObservableCollection<File> Files
        {
            get => _files;
            set => SetField(ref _files, value);
        }
        public IList<File> FilesSorted => Files?.OrderBy(c => c.Name).ToList();

        public ICollection<CodeLog> Logs { get; set; } = new ObservableCollection<CodeLog>();

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
            Logs.Add(new CodeLog(this, author, "created this"));
        }

        #endregion
        #region Methods

        public override string ToString() => Name;

        public void AddFile(File file, User user)
        {
            if (file == null)
                throw new NullReferenceException("File was null.");
            if (user == null)
                throw new NullReferenceException("User was null.");
            if (Files == null)
                Files = new ObservableCollection<File>();

            Files.Add(file);

            Logs.Add(new CodeLog(this, user, $"added", file));
        }

        public new void SetBanner(User user, Banner banner)
        {
            base.SetBanner(user, banner);
            Logs.Add(new CodeLog(this, user, "uploaded", banner));
        }

        public new void AddVideo(User user, Video video)
        {
            base.AddVideo(user, video);

            Logs.Add(new CodeLog(this, user, "added", video));
        }

        public new void Reply(Comment comment)
        {
            base.Reply(comment);
            Logs.Add(new CodeLog(this, comment.User, "added", comment));
        }

        public new void AddScreenshot(Screenshot screenshot, User user)
        {
            base.AddScreenshot(screenshot, user);
            Logs.Add(new CodeLog(this, user, "added", screenshot));
        }

        #endregion
    }
}
