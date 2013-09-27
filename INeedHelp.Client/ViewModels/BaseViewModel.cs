using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeedHelp.Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string successMessage;
        private string errorMessage;

        public string SuccessMessage
        {
            get { return this.successMessage; }
            set { this.successMessage = value; OnPropertyChanged("SuccessMessage"); }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
