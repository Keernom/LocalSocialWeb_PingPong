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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Kursovik_Kocherzhenko.ViewModel
{
    internal class DialogPageVM
    {
        private MainWindowViewModel mainWindow;
        private DialogPage dialogPage;

        public string MessageText { get;set; }

        public DialogPageVM(MainWindowViewModel mainWindow, DialogPage dialogPage)
        {
            this.mainWindow = mainWindow;
            this.dialogPage = dialogPage;

            Button getterName = dialogPage.GetterName;
            getterName.Content = DataWorker.GetUserNameAndSurname(mainWindow.GetterId);
            getterName.FontSize = 20;
            getterName.FontWeight = FontWeights.DemiBold;
            getterName.Background = Brushes.Transparent;
            getterName.BorderThickness = new Thickness(0);
            getterName.Command = OpenUserPage;
            getterName.CommandParameter = mainWindow.GetterId;

            System.Windows.Controls.Image image = dialogPage.GetterImage;
            image.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(mainWindow.GetterId));
            image.Height = 30;
            image.Margin = new Thickness(0, 0, 10, 0);
            image.HorizontalAlignment = HorizontalAlignment.Left;

            ScrollViewer scrollViewer = dialogPage.Scroller;
            scrollViewer.ScrollToEnd();

            DockPanel resultContainer = dialogPage.DialogResult;
            List<Message> messages = DataWorker.GetAllMessages(mainWindow.LoggedUser.Id, mainWindow.GetterId);

            foreach(var item in messages)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = Brushes.DarkTurquoise;
                border.CornerRadius = new CornerRadius(15);
                border.Background = new SolidColorBrush(Colors.LightGray);
                border.Margin = new Thickness(10, 25, 10, 0);

                TextBlock text = new TextBlock();
                text.Text = item.Text;
                text.FontSize = 20;

                if (item.Sender == mainWindow.LoggedUser.Id)
                    border.HorizontalAlignment= HorizontalAlignment.Right;
                else
                    border.HorizontalAlignment= HorizontalAlignment.Left;

                text.FontWeight = FontWeights.DemiBold;
                text.Padding = new Thickness(10);

                TextBlock date = new TextBlock();
                date.Text = item.Date;
                date.FontSize = 15;
                date.HorizontalAlignment = border.HorizontalAlignment;
                date.Margin = new Thickness(0, 0, 0, 10);

                border.Child = text;

                DockPanel.SetDock(border, Dock.Top);
                DockPanel.SetDock(date, Dock.Top);
                resultContainer.Children.Add(border);
                resultContainer.Children.Add(date);
            }
        }

        private Button GetUserName(int userId)
        {
            Button button = new Button();
            
            return button;
        }

        private RelayCommand sendMessage;
        public RelayCommand SendMessage
        {
            get
            {
                return sendMessage ?? new RelayCommand((obj) =>
                {
                    DataWorker.CreateMessage(DateTime.Now.ToString(), mainWindow.LoggedUser.Id, mainWindow.GetterId, MessageText);
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
    }
}
