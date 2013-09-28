using System;
using System.Collections;
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
    public class EditRequestViewModel : BaseViewModel
    {
        public HelpRequestModel Request { get; set; }
        public int CommentsCount { get { return Request.Comments.Count(); } }

        private ICommand editRequestLoaded;
        public ICommand EditRequestLoaded
        {
            get
            {
                if (this.editRequestLoaded == null)
                {
                    this.editRequestLoaded = new RelayCommand(HandleEditRequestLoaded);
                }

                return this.editRequestLoaded;
            }
        }

        private ICommand addHelper;
        public ICommand AddHelper
        {
            get
            {
                if(this.addHelper == null)
                {
                    this.addHelper = new RelayCommand(HandleAddHelper);
                }

                return this.addHelper;
            }
        }

        private async void HandleAddHelper(object obj)
        {
            int id = (int) obj;
            await HelpRequestsPersister.AddHelper(Request.Id, id, AccountManager.CurrentUser.SessionKey);
            var fullRequest = await HelpRequestsPersister.GetRequestById(Request.Id, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;

            OnPropertyChanged("Request");
            OnPropertyChanged("CommentsCount");
            OnPropertyChanged("HelpersCount");
            OnPropertyChanged("SuggestedHelpers");
        }

        public int HelpersCount
        {
            get { return Request.Helpers.Count(); }
        }

        public IEnumerable<UserModel> SuggestedHelpers
        {
            get
            {
                return Request.Comments.Select(c => c.User)
                    .Where(u => !Request.Helpers.Any(h => h.Id == u.Id)).Distinct(new UserModelComparer());
            }
        }

        private async  void HandleEditRequestLoaded(object obj)
        {
            var request = obj as HelpRequestModel;
            var fullRequest = await HelpRequestsPersister.GetRequestById(request.Id, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;

            OnPropertyChanged("Request");
            OnPropertyChanged("CommentsCount");
            OnPropertyChanged("HelpersCount");
            OnPropertyChanged("SuggestedHelpers");
        }
    }
}
