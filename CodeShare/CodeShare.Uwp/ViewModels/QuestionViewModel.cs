using CodeShare.Model;
using CodeShare.Uwp.Controls;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Xaml;
using System;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    public class QuestionViewModel : ContentPageViewModel
    {
        private Question _question;
        public Question Question
        {
            get => _question;
            set
            {
                SetField(ref _question, value);
                IsUserAuthor = value.User.Equals(AuthService.CurrentUser);
            }
        }

        public override async Task<bool> Refresh()
        {
            if (!(await RestApiService<Question>.Get(Question.Uid) is Question question))
                return false;

            Question = question;
            return true;
        }

        public override Task ReportAsync()
        {
            throw new NotImplementedException();
        }

        public override void ViewImage(WebFile image)
        {
            throw new NotImplementedException();
        }

        public override void ViewVideo(Video video)
        {
            throw new NotImplementedException();
        }

        public override void LogClick(ILog log)
        {
            throw new NotImplementedException();
        }
    }
}
