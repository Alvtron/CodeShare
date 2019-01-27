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
        private string _title;
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public Guid? CodeLanguageUid { get; set; }

        public CodeLanguage CodeLanguage { get; set; }

        public User User { get; set; }

        public Guid UserUid { get; set; }

        public Comment Solution { get; set; }

        public Guid? SolutionUid { get; set; }

        public DateTime? DateTimeSolved { get; set; }

        public ICollection<QuestionLog> Logs { get; set; } = new ObservableCollection<QuestionLog>();

        [NotMapped, JsonIgnore]
        public bool IsValid => UserUid != Guid.Empty && !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Text) && CodeLanguageUid != null && CodeLanguageUid != Guid.Empty;
        [NotMapped, JsonIgnore]
        public bool IsSolved => Solution != null;

        public Question()
        {
        }

        public Question(User user, string title, string text, CodeLanguage codeLanguage)
        {
            UserUid = user.Uid;
            Title = title;
            Text = text;
            CodeLanguageUid = codeLanguage.Uid;
            Logs.Add(new QuestionLog(this, user, "created this"));
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public new void SetBanner(User user, Banner banner)
        {
            base.SetBanner(user, banner);
            Logs.Add(new QuestionLog(this, user, "uploaded", banner));
        }

        public new void AddVideo(User user, Video video)
        {
            base.AddVideo(user, video);

            Logs.Add(new QuestionLog(this, user, "added", video));
        }

        public new void Reply(Comment comment)
        {
            base.Reply(comment);
            Logs.Add(new QuestionLog(this, comment.User, "added", comment));
        }

        public new void AddScreenshot(Screenshot screenshot, User user)
        {
            base.AddScreenshot(screenshot, user);
            Logs.Add(new QuestionLog(this, user, "added", screenshot));
        }
    }
}
