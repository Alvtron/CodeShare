using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeShare.Model
{
    public class Code : Content, IClassifiable
    {
        #region Properties

        public User User { get; set; }

        public Guid? UserUid { get; set; }

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

        private ObservableCollection<CodeFile> _files = new ObservableCollection<CodeFile>();
        public ObservableCollection<CodeFile> Files
        {
            get => _files;
            set => SetField(ref _files, value);
        }

        public ObservableCollection<CodeLog> Logs { get; set; } = new ObservableCollection<CodeLog>();

        [NotMapped, JsonIgnore]
        public bool Valid => Uid != Guid.Empty && !string.IsNullOrWhiteSpace(Name);

        #endregion
        #region Constructors

        public Code()
        {
        }

        public Code(string name, IEnumerable<CodeFile> files, User author)
        {
            Name = name;
            Files = new ObservableCollection<CodeFile>(files);
            UserUid = author.Uid;
            Created = DateTime.Now;
            Logs.Add(new CodeLog(this, author, "created this"));
        }

        #endregion
        #region Methods

        public override string ToString() => Name;

        public void AddFile(CodeFile file, User user)
        {
            if (file == null)
                throw new NullReferenceException("File was null.");
            if (user == null)
                throw new NullReferenceException("User was null.");
            if (Files == null)
                Files = new ObservableCollection<CodeFile>();

            Files.Add(file);

            Logs.Add(new CodeLog(this, user, $"added", file));
        }

        public override void SetBanner(User user, Banner banner)
        {
            SetBanner(banner);
            Logs.Add(new CodeLog(this, user, "uploaded", banner));
        }

        public override void AddScreenshot(User user, Screenshot screenshot)
        {
            AddScreenshot(screenshot);
            Logs.Add(new CodeLog(this, user, "added", screenshot));
        }

        public override void AddVideo(User user, Video video)
        {
            AddVideo(video);

            Logs.Add(new CodeLog(this, user, "added", video));
        }

        public override void Reply(User user, Reply comment)
        {
            Reply(comment);
            Logs.Add(new CodeLog(this, user, "added", comment));
        }

        #endregion
    }
}
