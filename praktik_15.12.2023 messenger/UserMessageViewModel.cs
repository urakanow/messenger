using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace praktik_15._12._2023_messenger
{
    internal class UserMessageViewModel : INotifyPropertyChanged
    {
        public User currentUser { get; }
        private ObservableCollection<UserMessage> messages;
        public ObservableCollection<UserMessage> Messages { get => messages; set {  messages = value; OnPropertyChanged(nameof(messages)); } }

        public UserMessageViewModel(User currentUser)
        {
            this.currentUser = currentUser;
            messages = new ObservableCollection<UserMessage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) { if (PropertyChanged != null) Application.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))); }

        public void AddMessage(User user, string messageText) {
            HorizontalAlignment horizontalAlignment;
            if (user == currentUser) horizontalAlignment = HorizontalAlignment.Left;
            else horizontalAlignment = HorizontalAlignment.Right;
            UserMessage message = new UserMessage(user, horizontalAlignment) { Message = messageText };

            messages.Add(message);
        }

    }
}
