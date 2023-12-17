using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        static private int messageCount = 0;
        static private string[] messageColors = { "#FFFCA590", "#FFFFFFA0", "#FFCDEFF1", "#FFF8E7B4" };
        static string responseData = "";

        static string ClientServer = "client";

        public MainWindow()
        {
            InitializeComponent();
        }

        static async Task GetAndSendMessage(string clientMessage)
        {
            MessageControl messageControl = new MessageControl(messageColors);

            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync("25.45.123.152", 8888);

                using (NetworkStream stream = client.GetStream())
                {
                    // Start a task to continuously read from the server
                    Task.Run(async () =>
                    {
                        try
                        {
                            while (true)
                            {
                                byte[] buffer = new byte[1024];
                                int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                                responseData = Encoding.UTF8.GetString(buffer, 0, byteCount);

                                ClientServer = "server";
                            }
                        }
                        catch (IOException)
                        {
                            // Handle IOException when the server disconnects
                            MessageBox.Show("Server disconnected.");
                        }
                        finally
                        {
                            client.Close();
                        }
                    });

                    // Start a task to continuously write to the server
                    Task.Run(async () =>
                    {
                        try
                        {
                            while (true)
                            {
                                byte[] data = Encoding.UTF8.GetBytes(clientMessage);
                                await stream.WriteAsync(data, 0, data.Length);

                                ClientServer = "client";
                            }
                        }
                        catch (IOException)
                        {
                            // Handle IOException when the server disconnects
                            MessageBox.Show("Server disconnected.");
                        }
                        finally
                        {
                            client.Close();
                        }
                    });

                    // Keep the main thread alive
                    await Task.Delay(-1);
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            MessageControl messageControl = new MessageControl(messageColors);
            string clientMessage = EnterTextBox.Text;
            GetAndSendMessage(clientMessage);

            string username = "ur1nda";
            User server = new User() { Username = username };

            string username2 = "ur1nda";
            User client = new User() { Username = username2 };

            UserMessageViewModel userMessageViewModel = new UserMessageViewModel(server);
            

            if (ClientServer == "client")
            {
                userMessageViewModel.AddMessage(client, EnterTextBox.Text, NotesContainer, MyScrollViewer);

                DataContext = userMessageViewModel;

                
            }
            else if (ClientServer == "server")
            {
                userMessageViewModel.AddMessage(server, responseData, NotesContainer, MyScrollViewer);

                DataContext = userMessageViewModel;
            }
        }
    }
}
