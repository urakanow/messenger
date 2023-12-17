using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
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
        private UserMessageViewModel userMessageViewModel;
        private User user;
        //static string responseData = "";

        //static string ClientServer = "client";

        public MainWindow(User user)
        {
            InitializeComponent();

            this.user = user;
            userMessageViewModel = new UserMessageViewModel(user);

            if (user.Username == "server")
            {
                Task.Run(async () => await GetMessageClient(userMessageViewModel, this.user, NotesContainer, MyScrollViewer));
            }
            else
            {
                Task.Run(async () => await GetMessageServer(userMessageViewModel, this.user, NotesContainer, MyScrollViewer));
            }
        }

        static async Task GetMessageClient(UserMessageViewModel userMessageViewModel, User user, StackPanel NotesContainer, ScrollViewer MyScrollViewer)
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
                                string responseData = Encoding.UTF8.GetString(buffer, 0, byteCount);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    // Update the UI with the received message
                                    userMessageViewModel.AddMessage(user, responseData, NotesContainer, MyScrollViewer);
                                });

                                //ClientServer = "server";
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
                    //Task.Run(async () =>
                    //{
                    //    try
                    //    {
                    //        while (true)
                    //        {

                    //            byte[] data = Encoding.UTF8.GetBytes(clientMessage);
                    //            await stream.WriteAsync(data, 0, data.Length);

                    //            ClientServer = "client";
                    //        }
                    //    }
                    //    catch (IOException)
                    //    {
                    //        // Handle IOException when the server disconnects
                    //        MessageBox.Show("Server disconnected.");
                    //    }
                    //    finally
                    //    {
                    //        client.Close();
                    //    }
                    //});

                    //// Keep the main thread alive
                    //await Task.Delay(-1);
                }
            }
        }

        static async Task GetMessageServer(UserMessageViewModel userMessageViewModel, User user, StackPanel NotesContainer, ScrollViewer MyScrollViewer)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            Console.WriteLine("Server has started on 127.0.0.1:8888");
            Console.WriteLine("Waiting for a connection...");

            TcpClient client = await server.AcceptTcpClientAsync();
            Console.WriteLine("A client connection");

            NetworkStream stream = client.GetStream();

            // Start a task to continuously read from the client
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        byte[] buffer = new byte[1024];
                        int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string data = Encoding.UTF8.GetString(buffer, 0, byteCount);
                        userMessageViewModel.AddMessage(user, data, NotesContainer, MyScrollViewer);
                        if (string.IsNullOrEmpty(data))
                        {
                            break; // Exit the loop if the received data is empty (client disconnected)
                        }

                        string time = DateTime.Now.ToLongTimeString();
                        string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                        //Console.WriteLine($"Received at {time} from {clientIP}: {data}");

                    }
                }
                catch (IOException)
                {
                    // Handle IOException when the client disconnects
                    Console.WriteLine("Client disconnected.");
                }
                finally
                {
                    server.Stop();
                }
            });
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageControl messageControl = new MessageControl(messageColors);
            //string clientMessage = EnterTextBox.Text;
            ////GetMessageClient(clientMessage);

            //string username = "ur1nda";
            //User server = new User() { Username = username };

            //string username2 = "ur1nda";
            //User client = new User() { Username = username2 };

            //UserMessageViewModel userMessageViewModel = new UserMessageViewModel(server);


            //if (ClientServer == "client")
            //{
            //userMessageViewModel.AddMessage(client, EnterTextBox.Text, NotesContainer, MyScrollViewer);

            //    DataContext = userMessageViewModel;


            //}
            //else if (ClientServer == "server")
            //{
            //    userMessageViewModel.AddMessage(server, responseData, NotesContainer, MyScrollViewer);

            //    DataContext = userMessageViewModel;
            //}

            if (string.IsNullOrWhiteSpace(EnterTextBox.Text)) return;

            SendMessage(EnterTextBox.Text);
            userMessageViewModel.AddMessage(user, EnterTextBox.Text, NotesContainer, MyScrollViewer);
        }

        public static async void SendMessage(string responseString)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            Console.WriteLine("Server has started on 127.0.0.1:8888");
            Console.WriteLine("Waiting for a connection...");

            TcpClient client = await server.AcceptTcpClientAsync();
            Console.WriteLine("A client connection");

            NetworkStream stream = client.GetStream();

            Task.Run(async () =>
            {
                try
                {

                    //Console.WriteLine("\nYour response: ");
                    //string responseString = Console.ReadLine();
                    byte[] response = Encoding.UTF8.GetBytes(responseString);
                    await stream.WriteAsync(response, 0, response.Length);
                }
                catch (IOException)
                {
                    // Handle IOException when the client disconnects
                    Console.WriteLine("Client disconnected.");
                }
                finally
                {
                    client.Close();
                }
            });
        }
    }
}
