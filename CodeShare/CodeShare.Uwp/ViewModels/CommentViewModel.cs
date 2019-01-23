using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeShare.Model;
using CodeShare.Uwp.Controls;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Xaml;

namespace CodeShare.Uwp.ViewModels
{
    public class CommentViewModel : ContentPageViewModel
    {
        public Comment Comment { get; private set; }
        
        public CommentViewModel(Comment comment)
        {
            Comment = comment;
            IsUserAuthor = comment.User.Equals(AuthService.CurrentUser);
        }

        public override void LogClick(ILog log)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Refresh()
        {
            throw new NotImplementedException();
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
    }
}
