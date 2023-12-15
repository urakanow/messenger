using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace praktik_15._12._2023_messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string username = "ur1nda";
            User user = new User() { Username = username };

            string username2 = "ur1nda";
            User user2 = new User() { Username = username2 };

            UserMessageViewModel userMessageViewModel = new UserMessageViewModel(user);
            userMessageViewModel.AddMessage(user, "hello");
            userMessageViewModel.AddMessage(user2, "world");

            DataContext = userMessageViewModel;
        }
    }
}
