using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
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
    }
}
