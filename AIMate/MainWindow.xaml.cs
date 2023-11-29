using AIMate.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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


namespace AIMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private ObservableCollection<ChatItem> chatItems;

        private ObservableCollection<ChatHistory> chatHistory;

        public MainWindow()
        {
            InitializeComponent();

            chatItems = new ObservableCollection<ChatItem>();
            chatListView.ItemsSource = chatItems;

            chatHistory = new ObservableCollection<ChatHistory>();
            chatHistoryListView.ItemsSource = chatHistory;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Button_Maximize_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState != WindowState.Maximized) 
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void New_Chat_Click(object sender, RoutedEventArgs e)
        {
            //Create a new chat item and add it to the ObservableCollection
            chatItems.Insert(0, new ChatItem { Message = "New Chat Message"});

            //Scroll to the newly added chatitem
            chatListView.ScrollIntoView(chatItems[chatItems.Count-1]);
        }

        private void DeleteChatButton_Click(object sender, RoutedEventArgs e) 
        {
            var button = (Button)sender;
            var chatItem = button.DataContext as ChatItem;

            if (chatItem != null) 
            {
                chatItems.Remove(chatItem);
            }
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(inputTextBox.Text == "Enter Your Prompt Here")
            {
                inputTextBox.Text = "";
            }
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(inputTextBox.Text)) 
            {
                inputTextBox.Text = "Enter Your Prompt Here";
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userPrompt = inputTextBox.Text; //The text in the TextBox will be the user prompt

                //Ensure User entered a prompt
                if(string.IsNullOrWhiteSpace(userPrompt) || userPrompt == "Enter Your Prompt Here")
                {
                    MessageBox.Show("Enter a valid prompt");
                    return;
                }

                //Sending user's prompt to the api endpoint
                string palmApiResponse = await callPalmApiEndpoint(userPrompt);

                //process the api responce here
                chatHistory.Add(new ChatHistory { UserPrompt = userPrompt , ModelResponse = palmApiResponse });

                //Clear the inputTextBox
                inputTextBox.Text = "Enter Your Prompt Here";
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occured : {ex.Message}");
            }
        }

        private async Task<string> callPalmApiEndpoint(string userPrompt)
        {
            using (HttpClient client = new HttpClient()) 
            {
                //Enter your api endpoint here
                string apiUrl = "https://generativelanguage.googleapis.com/v1beta3/models/text-bison-001:generateText?key=AIzaSyAQIGFVsiIbfDA37_hidypsrfhyN9GefKU";

                //string jsonBody = $"{{\"prompt\": \"{userPrompt}\"}}";
                string jsonBody = $"{{\"prompt\": {{\"text\": \"{userPrompt}\"}}}}";

                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                //Send a post request to api endpoint
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                //Ensuring the request was successful
                response.EnsureSuccessStatusCode();

                //read and return the response content
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
