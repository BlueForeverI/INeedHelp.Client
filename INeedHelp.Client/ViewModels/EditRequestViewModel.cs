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
using ParseStarterProject.Services;

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

        private ICommand saveChanges;
        public ICommand SaveChanges
        {
            get
            {
                if(this.saveChanges == null)
                {
                    this.saveChanges = new RelayCommand(HandleSaveChanges);
                }

                return this.saveChanges;
            }
        }

        private async void HandleSaveChanges(object obj)
        {
            await HelpRequestsPersister.EditRequest(Request, AccountManager.CurrentUser.SessionKey);
            NavigationService.Navigate(ViewType.MyRequests);
            NotificationsManager.ShowNotification("Request details saved");
        }

        private async void HandleAddHelper(object obj)
        {
            int id = (int) obj;
            await HelpRequestsPersister.AddHelper(Request.Id, id, AccountManager.CurrentUser.SessionKey);
            LoadRequest(Request.Id);
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

        private void HandleEditRequestLoaded(object obj)
        {
            var request = obj as HelpRequestModel;
            LoadRequest(request.Id);
        }

        private async void LoadRequest(int requestId)
        {
            var fullRequest = await HelpRequestsPersister.GetRequestById(requestId, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;

            OnPropertyChanged("Request");
            OnPropertyChanged("CommentsCount");
            OnPropertyChanged("HelpersCount");
            OnPropertyChanged("SuggestedHelpers");
        }
    }
}
