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
using ParseStarterProject.Services;

namespace INeedHelp.Client.ViewModels
{
    public class AddRequestViewModel : BaseViewModel
    {
        public string Text { get; set; }
        public string Title { get; set; }

        private ICommand addRequest;
        public ICommand AddRequest
        {
            get
            {
                if(this.addRequest == null)
                {
                    this.addRequest = new RelayCommand(HandleAddRequest);
                }

                return this.addRequest;
            }
        }

        private async void HandleAddRequest(object obj)
        {
            var request = new HelpRequestModel()
                              {
                                  Title = Title,
                                  Text = Text
                              };

            await HelpRequestsPersister.AddRequest(request, AccountManager.CurrentUser.SessionKey);
            NavigationService.Navigate(ViewType.MyRequests);
            NotificationsManager.ShowNotification("Request added");
        }
    }
}
