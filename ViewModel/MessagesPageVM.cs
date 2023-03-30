using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursovik_Kocherzhenko.ViewModel
{
    internal class MessagesPageVM : INotifyPropertyChanged
    {
        public MainWindowViewModel mainWindow;
        public MessagesPage messagesPage;

        StackPanel panel;

        public MessagesPageVM(MainWindowViewModel mainWindow, MessagesPage messagesPage) 
        { 
            this.mainWindow = mainWindow;
            this.messagesPage = messagesPage;

            panel = messagesPage.MessagesResult;
            findDialogText = string.Empty;
            UpdateDialogSearch();
        }

        public string findDialogText { get; set; }
        public string FindDialogText
        {
            get { return findDialogText; }
            set
            {
                findDialogText = value;
                UpdateDialogSearch();
                OnPropertyChanged(nameof(FindDialogText));
            }
        }

        private void UpdateDialogSearch()
        {
            panel.Children.Clear();

            List<Message> messages = DataWorker.GetAllMessages(mainWindow.LoggedUser.Id);
            List<int> members= new List<int>();


            foreach(var item in messages)
            {
                if (item.Getter != mainWindow.LoggedUser.Id && !members.Contains(item.Getter))
                {
                    members.Add(item.Getter);
                }
                else if (item.Sender != mainWindow.LoggedUser.Id && !members.Contains(item.Sender))
                {
                    members.Add(item.Sender);
                }
            }

            foreach(var item in members)
            {
                if (DataWorker.GetUserById(item).FullName.Contains(FindDialogText))
                {
                    DockPanel stackPanel = new DockPanel();
                    stackPanel.Children.Add(GetUserImage(item));
                    stackPanel.Children.Add(GetSpeakerName(item));
                    stackPanel.Children.Add(GetOpenDialogButton(item));

                    panel.Children.Add(stackPanel);
                }
            }
        }

        private Image GetUserImage(int getterId)
        {
            Image userImage = new Image();
            userImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(getterId));
            userImage.Height = 30;
            userImage.HorizontalAlignment = HorizontalAlignment.Left;
            return userImage;
        }

        private Button GetSpeakerName(int getterId)
        {
            Button button = new Button();
            button.Content = DataWorker.GetUserById(getterId).FullName;
            button.FontSize = 20;
            button.Margin = new Thickness(20);
            button.FontWeight = FontWeights.DemiBold;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
            button.Command = OpenUserPage;
            button.CommandParameter = getterId;
            return button;
        }

        private Button GetOpenDialogButton(int getterId)
        {
            Button button = new Button();
            button.Content = "Открыть диалог";
            button.Margin = new Thickness(20);
            button.Padding = new Thickness(20);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Command = OpenDialog;
            button.CommandParameter = getterId;
            return button;
        }

        private RelayCommand openDialog;
        public RelayCommand OpenDialog
        {
            get
            {
                return openDialog ?? new RelayCommand((obj) =>
                {
                    mainWindow.GetterId = (int)obj;
                    mainWindow.ChangePage(Pages.Dialog);
                }
                );
            }
        }

        private RelayCommand openUserPage;
        public RelayCommand OpenUserPage
        {
            get
            {
                return openUserPage ?? new RelayCommand((obj) =>
                {
                    mainWindow.GetterId = (int)obj;
                    mainWindow.ChangePage(Pages.User);
                }
                );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
