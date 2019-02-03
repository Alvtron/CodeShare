using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeShare.Model
{
    public class Question : Content
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public CodeLanguage CodeLanguage { get; set; }
        public Guid? CodeLanguageUid { get; set; }
        public User User { get; set; }
        public Guid? UserUid { get; set; }
        public Reply Solution { get; set; }
        public Guid? SolutionUid { get; set; }
        public DateTime? DateTimeSolved { get; set; }
        public ICollection<QuestionLog> Logs { get; set; } = new ObservableCollection<QuestionLog>();

        [NotMapped, JsonIgnore]
        public bool IsValid => UserUid != Guid.Empty && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Text) && CodeLanguageUid != null && CodeLanguageUid != Guid.Empty;
        [NotMapped, JsonIgnore]
        public bool IsSolved => Solution != null;

        public Question()
        {
        }

        public Question(User user, string title, string text, CodeLanguage codeLanguage)
        {
            UserUid = user.Uid;
            Name = title;
            Text = text;
            CodeLanguageUid = codeLanguage.Uid;
            Logs.Add(new QuestionLog(this, user, "created this"));
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public override void SetBanner(User user, Banner banner)
        {
            SetBanner(banner);
            Logs.Add(new QuestionLog(this, user, "uploaded", banner));
        }

        public override void AddScreenshot(User user, Screenshot screenshot)
        {
            AddScreenshot(screenshot);
            Logs.Add(new QuestionLog(this, user, "added", screenshot));
        }

        public override void AddVideo(User user, Video video)
        {
            AddVideo(video);

            Logs.Add(new QuestionLog(this, user, "added", video));
        }

        public override void Reply(User user, Reply comment)
        {
            Reply(comment);
            Logs.Add(new QuestionLog(this, user, "added", comment));
        }
    }
}
