using AIMate.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace AIMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private ObservableCollection<ChatItem> chatItems;

        public MainWindow()
        {
            InitializeComponent();

            chatItems = new ObservableCollection<ChatItem>();
            chatListView.ItemsSource = chatItems;
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
    }
}
