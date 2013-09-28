using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using INeedHelp.Client.Models;

namespace INeedHelp.Client.ViewModels
{
    public class RequestDetailsViewModel : BaseViewModel
    {
        public HelpRequestModel Request { get; set; }
        public string CommentText { get; set; }

        public RequestDetailsViewModel()
        {
            
        }

        private ICommand requestDetailsLoaded;
        public ICommand RequestDetailsLoaded
        {
            get
            {
                if(this.requestDetailsLoaded == null)
                {
                    this.requestDetailsLoaded = new RelayCommand(HandleRequestDetailsLoaded);
                }

                return this.requestDetailsLoaded;
            }
        }

        private ICommand addComment;
        public ICommand AddComment
        {
            get
            {
                if(this.addComment == null)
                {
                    this.addComment = new RelayCommand(HandleAddComment);
                }

                return this.addComment;
            }
        }

        private async void HandleAddComment(object obj)
        {
            if(!string.IsNullOrEmpty(CommentText))
            {
                var comment = new CommentModel()
                                  {
                                      Content = this.CommentText
                                  };

                await HelpRequestsPersister.AddComment(Request.Id, comment,
                                                       AccountManager.CurrentUser.SessionKey);

                var fullRequest =
                    await HelpRequestsPersister.GetRequestById(Request.Id, AccountManager.CurrentUser.SessionKey);
                this.Request = fullRequest;
                this.CommentText = "";
                OnPropertyChanged("Request");
                OnPropertyChanged("CommentText");
            }
        }

        private async void HandleRequestDetailsLoaded(object obj)
        {
            var request = obj as HelpRequestModel;
            var fullRequest = await HelpRequestsPersister.GetRequestById(request.Id, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;
            OnPropertyChanged("Request");
        }
    }
}
