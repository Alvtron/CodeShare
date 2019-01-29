using CodeShare.Model;
using CodeShare.Uwp.Controls;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Xaml;
using System;
using System.Threading.Tasks;

namespace CodeShare.Uwp.ViewModels
{
    public class QuestionViewModel : ContentViewModel<Question>
    {
        public QuestionViewModel(Question question)
            : base(question)
        {
            IsUserAuthor = question.User.Equals(AuthService.CurrentUser);
        }

        public override async Task<bool> Refresh()
        {
            if (!(await RestApiService<Question>.Get(Model.Uid) is Question question))
                return false;

            Model = question;
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
