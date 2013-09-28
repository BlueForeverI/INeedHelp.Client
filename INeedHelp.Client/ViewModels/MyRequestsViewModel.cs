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

        public MyRequestsViewModel()
        {
            LoadHelpRequests();
        }

        private async void LoadHelpRequests()
        {
            this.HelpRequests = await HelpRequestsPersister.GetRequestsByUser(AccountManager.CurrentUser.SessionKey);
            OnPropertyChanged("HelpRequests");
        }
    }
}
