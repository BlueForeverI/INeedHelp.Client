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
    public class SearchRequestsViewModel : BaseViewModel
    {
        public string QueryText { get; set; }
        public IEnumerable<HelpRequestModel> HelpRequests { get; set; } 

        private ICommand searchLoaded;
        public ICommand SearchLoaded
        {
            get
            {
                if(this.searchLoaded == null)
                {
                    this.searchLoaded = new RelayCommand(HandleSearchLoaded);
                }

                return this.searchLoaded;
            }
        }

        private async void HandleSearchLoaded(object obj)
        {
            this.QueryText = obj.ToString();
            OnPropertyChanged("QueryText");

            this.HelpRequests = await HelpRequestsPersister.SearchRequests(this.QueryText, 
                AccountManager.CurrentUser.SessionKey);
            OnPropertyChanged("HelpRequests");
        }
    }
}
