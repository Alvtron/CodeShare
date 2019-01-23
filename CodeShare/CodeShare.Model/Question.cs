using Newtonsoft.Json;
using System;
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
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
