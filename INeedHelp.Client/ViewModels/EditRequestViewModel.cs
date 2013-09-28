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
    public class EditRequestViewModel : BaseViewModel
    {
        public HelpRequestModel Request { get; set; }

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

        private async void HandleEditRequestLoaded(object obj)
        {
            var request = obj as HelpRequestModel;
            var fullRequest = await HelpRequestsPersister.GetRequestById(request.Id, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;
            OnPropertyChanged("Request");
        }
    }
}
