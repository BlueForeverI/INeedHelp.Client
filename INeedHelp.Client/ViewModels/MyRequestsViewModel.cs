using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using INeedHelp.Client.Models;

namespace INeedHelp.Client.ViewModels
{
    public class MyRequestsViewModel : BaseViewModel
    {
        public IEnumerable<HelpRequestModel> HelpRequests { get; set; }

        public bool RequestsLoading { get; set; }
        public bool RequestsVisible { get { return !RequestsLoading; } }

        public MyRequestsViewModel()
        {
            try
            {
                OnPropertyChanged("RequestsLoading");
                OnPropertyChanged("RequestsVisible");
                LoadHelpRequests();
            }
            catch (Exception)
            {

            }
        }

        private async void LoadHelpRequests()
        {
            this.HelpRequests = await HelpRequestsPersister.GetRequestsByUser(AccountManager.CurrentUser.SessionKey);
            OnPropertyChanged("HelpRequests");
            OnPropertyChanged("RequestsLoading");
            OnPropertyChanged("RequestsVisible");
        }
    }
}
