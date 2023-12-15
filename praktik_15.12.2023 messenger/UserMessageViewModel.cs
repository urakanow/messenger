using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace praktik_15._12._2023_messenger
{
    internal class UserMessageViewModel : INotifyPropertyChanged
    {
        public User currentUser { get; }
        private ObservableCollection<UserMessage> messages;
        public ObservableCollection<UserMessage> Messages { get => messages; set { messages = value; OnPropertyChanged(nameof(messages)); } }
        static private string[] messageColors = { "#FFFCA590", "#FFFFFFA0", "#FFCDEFF1", "#FFF8E7B4" };
        static private int messageCount = 0;

        public UserMessageViewModel(User currentUser)
        {
            this.currentUser = currentUser;
            messages = new ObservableCollection<UserMessage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) { if (PropertyChanged != null) Application.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))); }

        public void AddMessage(User user, string messageText, StackPanel NotesContainer, ScrollViewer MyScrollViewer)
        {
            MessageControl messageControl = new MessageControl(messageColors);

            messageControl.Content = messageText;

            int colorIndex = messageCount % messageColors.Length;
            messageControl.MyBorder.Background = (Brush)new BrushConverter().ConvertFromString(messageColors[colorIndex]);

            messageControl.Margin = new Thickness(10, 10, 0, 0);
            messageControl.Width = 170;
            messageControl.HorizontalAlignment = HorizontalAlignment.Right;

            messageCount++;

            HorizontalAlignment horizontalAlignment;
            if (user == currentUser) horizontalAlignment = HorizontalAlignment.Left;
            else horizontalAlignment = HorizontalAlignment.Right;
            UserMessage message = new UserMessage(user, horizontalAlignment) { Message = messageText };

            messages.Add(message);

            NotesContainer.Children.Add(messageControl);

            MyScrollViewer.ScrollToBottom();
        }

    }
}
