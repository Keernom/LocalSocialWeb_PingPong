using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.Model.Data;
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

namespace Kursovik_Kocherzhenko.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region MAINPAGE_SETTINGS

        private MainWindow mainWindow;
        StackPanel logButtons;

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            CurrentPage = new View.GuestPage(this); ;
            this.mainWindow = mainWindow;

            logButtons = mainWindow.UserLogInControl;


            UserLogButtonsUpdate();
        }

        #endregion

        #region NAVIGATIONAL_COMMANDS

        public ICommand LoadLogInPage
        {
            get
            {
                return new RelayCommand((obj) => 
                { 
                    CurrentPage = new View.LogInPage(this);
                    GetterId = 0;
                });
            }
        }

        public ICommand LoadRegistrationPage
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    CurrentPage = new View.RegistrationPage(this);
                    GetterId = 0;
                });
            }
        }

        public ICommand LoadUserPage
        {
            get
            {
                return new RelayCommand((obj) => 
                {
                    if (LoggedUser != null)
                    {
                        GetterId = 0;
                        CurrentPage = new View.UserPage(this);
                    }
                });
            }
        }

        public ICommand LoadNewsPage
        {
            get
            {
                return new RelayCommand((obj) => 
                {
                    if (LoggedUser != null)
                    {
                        CurrentPage = new View.NewsPage(this);
                    }
                });
            }
        }

        public ICommand LoadFriendsPage
        {
            get
            {
                return new RelayCommand((obj) => {
                    if (LoggedUser != null)
                    {
                        CurrentPage = new View.FriendsPage(this);
                        GetterId = 0;
                    }
                        
                });
            }
        }

        public ICommand LoadMessagesPage
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (LoggedUser != null)
                    {
                        CurrentPage = new View.MessagesPage(this);
                        GetterId = 0;
                    }
                        
                });
            }
        }

        public ICommand LoadGuestPage
        {
            get
            {
                return new RelayCommand((obj) => 
                {
                    LoggedUser = null;
                    GetterId = 0;
                    CurrentPage = new View.GuestPage(this);
                });
            }
        }

        public ICommand LoadWikiPage
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    GetterId = 0;
                    CurrentPage = new View.WikiPage(this);
                });
            }
        }

        #endregion

        #region DATA_PROPERTIES

        private User loggedUser;
        public User LoggedUser 
        {
            get { return loggedUser; }
            set 
            { 
                loggedUser = value;
                UserLogButtonsUpdate();
                OnPropertyChanged(nameof(LoggedUser));
            } 
        }

        private string findUserText;
        public string FindUserText 
        { 
            get {return findUserText;}
            set 
            {
                if (LoggedUser != null)
                {
                    findUserText = value;
                    ChangePage(Pages.UserSearch);
                    OnPropertyChanged(nameof(FindUserText));
                }
            }
        }

        private int getterId;
        public int GetterId
        {
            get { return getterId;}
            set
            {
                getterId = value;
                OnPropertyChanged(nameof(GetterId));
            }
        }

        private int porstId;
        public int PostId
        {
            get { return porstId; }
            set
            {
                porstId = value;
                OnPropertyChanged(nameof(PostId));
            }
        }

        #endregion

        public void ChangePage(Pages newPage)
        {
            switch (newPage)
            {
                case Pages.Guest:
                    CurrentPage = new View.GuestPage(this); ;
                    break;
                case Pages.LogIn:
                    CurrentPage = new View.LogInPage(this);
                    break;
                case Pages.Registration:
                    CurrentPage = new View.RegistrationPage(this);
                    break;
                case Pages.User:
                    CurrentPage = new View.UserPage(this); ;
                    break;
                case Pages.UserSearch:
                    CurrentPage = new View.UserSearchPage(this); ;
                    break;
                case Pages.News:
                    CurrentPage = new View.NewsPage(this); ;
                    break;
                case Pages.Friends:
                    CurrentPage = new View.FriendsPage(this); ;
                    break;
                case Pages.Messages:
                    CurrentPage = new View.MessagesPage(this); ;
                    break;
                case Pages.Dialog:
                    CurrentPage = new View.DialogPage(this); ;
                    break;
                case Pages.Comment:
                    CurrentPage = new View.CommentPage(this); ;
                    break;
                case Pages.Wiki:
                    CurrentPage = new View.WikiPage(this); ;
                    break;
            }
        }

        private void UserLogButtonsUpdate()
        {
            logButtons.Children.Clear();
            if (LoggedUser == null)
            {
                logButtons.Children.Add(GetLogInButton());
                logButtons.Children.Add(GetRegisrtationButton());
            }
            else
            {
                logButtons.Children.Add(GetExitButton());
            }
        }

        private Button GetExitButton()
        {
            Button exitButton = new Button();
            exitButton.FontSize = 20;
            exitButton.Content = "Выход";
            exitButton.Command = LoadGuestPage;
            exitButton.Background = new SolidColorBrush(Colors.Transparent);
            exitButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            return exitButton;
        }

        private Button GetRegisrtationButton()
        {
            Button regButton = new Button();
            regButton.FontSize = 20;
            regButton.Content = "Регистрация";
            regButton.Command = LoadRegistrationPage;
            //regButton.Background = new SolidColorBrush(Color.FromArgb(0, 66, 194, 252));
            regButton.Background = new SolidColorBrush(Colors.Transparent);
            regButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            return regButton;
        }

        private Button GetLogInButton()
        {
            Button logInButton = new Button();
            logInButton.FontSize = 20;
            logInButton.Content = "Вход";
            logInButton.Command = LoadLogInPage;
            logInButton.Background = new SolidColorBrush(Colors.Transparent);
            logInButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            logInButton.Margin = new Thickness(0, 0, 20, 0);

            return logInButton;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
