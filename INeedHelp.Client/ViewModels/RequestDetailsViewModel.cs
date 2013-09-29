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
using Windows.System;

namespace INeedHelp.Client.ViewModels
{
    public class RequestDetailsViewModel : BaseViewModel
    {
        public HelpRequestModel Request { get; set; }
        public string CommentText { get; set; }

        public int CommentsCount
        {
            get { return Request.Comments.Count(); }
        }

        public int HelpersCount
        {
            get { return Request.Helpers.Count(); }
        }

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

        private ICommand viewOnMap;
        public ICommand ViewOnMap
        {
            get
            {
                if(this.viewOnMap == null)
                {
                    this.viewOnMap = new RelayCommand(HandleViewOnMap);
                }

                return this.viewOnMap;
            }
        }

        private async void HandleViewOnMap(object obj)
        {
            if (Request.Coordinates != null)
            {
                var uriString = string.Format("bingmaps:?cp={0}~{1}",
                                              Request.Coordinates.Latitude, Request.Coordinates.Longitude);
                await Launcher.LaunchUriAsync(new Uri(uriString));
            }
            else
            {
                ErrorMessage = "The request has no coordinates";
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
            OnPropertyChanged("CommentsCount");
            OnPropertyChanged("HelpersCount");
        }
    }
}
