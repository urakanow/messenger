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

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {

        public string Content
        {
            get { return ContentTextBlock.Text; }
            set { ContentTextBlock.Text = value; }
        }
        private string[] messageColors;

        public MessageControl(string[] colors)
        {
            InitializeComponent();
            this.messageColors = colors;
        }
    }
}
