using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OMDbAPIClient.ViewModel
{
    public interface IMessageBoxService
    {
        void ShowNotification(string message);
        bool AskForConfirmation(string message);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void ShowNotification(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool AskForConfirmation(string message)
        {
            throw new NotImplementedException();
        }
    }
}
