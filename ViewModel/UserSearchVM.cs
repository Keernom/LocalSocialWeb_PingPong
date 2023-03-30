using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursovik_Kocherzhenko.ViewModel
{
    public class UserSearchVM
    {
        private MainWindowViewModel mainWindow;
        private UserSearchPage searchPage;

        public UserSearchVM(MainWindowViewModel mainWindow, UserSearchPage searchPage)
        {
            this.mainWindow = mainWindow;
            this.searchPage = searchPage;

            StackPanel resultContainer = searchPage.UserFindResult;
            List<User> users = DataWorker.GetAllUsers();
            List<FriendRequest> friendRequests = DataWorker.GetAllRequests();

            foreach (var item in users)
            {
                if (IsNotLoggedUser(item) && item.FullName.Contains(mainWindow.FindUserText))
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Vertical;

                    DockPanel userView = new DockPanel();
                    Image userImage = GetUserImage(item);

                    userView.Children.Add(userImage);
                    userView.Children.Add(GetUserName(item));

                    Border border = new Border();
                    border.BorderThickness = new Thickness(.5);
                    border.BorderBrush = Brushes.DarkTurquoise;

                    if (!DataWorker.AreFriends(mainWindow.LoggedUser, item))
                    {
                        if (DataWorker.RequestSent(mainWindow.LoggedUser.Id, item.Id)
                            || DataWorker.RequestSent(item.Id, mainWindow.LoggedUser.Id))
                        {
                            if (friendRequests.Any(fr => fr.GetterId == mainWindow.LoggedUser.Id && fr.UserId == item.Id))
                            {
                                FriendRequest request = friendRequests.FirstOrDefault(fr => fr.GetterId == mainWindow.LoggedUser.Id && fr.UserId == item.Id);
                                userView.Children.Add(GetAcceptRequestButton(request));
                            }
                            else
                            {
                                userView.Children.Add(GetRequestSentLabel());
                                userView.Children.Add(GetRecallRequestButton(item));
                            }
                        }
                        else
                            userView.Children.Add(GetAddFriendButton(item));
                    }
                    else
                        userView.Children.Add(GetInFriendsLabel());

                    userView.Children.Add(GetWriteMessageButton(item.Id));

                    stackPanel.Children.Add(userView);
                    stackPanel.Children.Add(border);

                    resultContainer.Children.Add(stackPanel);
                }
            }
        }

        private Image GetUserImage(User item)
        {
            Image userImage = new Image();
            userImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(item.Id));
            userImage.Height = 30;
            userImage.HorizontalAlignment = HorizontalAlignment.Left;
            return userImage;
        }

        private bool IsNotLoggedUser(User user)
        {
            return (mainWindow.LoggedUser == null || user.Id != mainWindow.LoggedUser.Id);
        }

        private Button GetWriteMessageButton(int friendId)
        {
            Button button = new Button();
            button.Content = "Написать сообщение";
            button.Margin = new Thickness(20);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Command = WriteMessage;
            button.CommandParameter = friendId;
            return button;
        }

        private Button GetUserName(User item)
        {
            Button button = new Button();
            button.Content = item.FullName;
            button.FontSize = 20;
            button.Margin = new Thickness(20);
            button.FontWeight = FontWeights.DemiBold;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
            button.Command = OpenUserPage;
            button.CommandParameter = item.Id;
            return button;
        }

        private TextBlock GetInFriendsLabel()
        {
            TextBlock newUser = new TextBlock();
            newUser.FontSize = 20;
            newUser.Text = "Уже в друзьях";
            newUser.HorizontalAlignment = HorizontalAlignment.Right;
            newUser.Margin = new Thickness(20);
            newUser.FontWeight = FontWeights.DemiBold;
            return newUser;
        }

        private TextBlock GetRequestSentLabel()
        {
            TextBlock newUser = new TextBlock();
            newUser.FontSize = 20;
            newUser.Text = "Запрос отправлен";
            newUser.HorizontalAlignment = HorizontalAlignment.Right;
            newUser.Margin = new Thickness(20);
            newUser.FontWeight = FontWeights.DemiBold;
            return newUser;
        }

        private Button GetAddFriendButton(User item)
        {
            Button addFriendButton = new Button();
            addFriendButton.Command = SendRequest;
            addFriendButton.Content = "Добавить в друзья";
            addFriendButton.Margin = new Thickness(20);
            addFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            addFriendButton.CommandParameter = item;
            return addFriendButton;
        }

        private Button GetRecallRequestButton(User item)
        {
            Button addFriendButton = new Button();
            addFriendButton.Command = RecallRequest;
            addFriendButton.Content = "Отозвать запрос";
            addFriendButton.Margin = new Thickness(20);
            addFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            addFriendButton.CommandParameter = item;
            return addFriendButton;
        }

        private Button GetAcceptRequestButton(FriendRequest request)
        {
            Button addFriendButton = new Button();
            addFriendButton.Content = "Принять запрос";
            addFriendButton.Margin = new Thickness(20);
            addFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            addFriendButton.Command = AcceptRequest;
            addFriendButton.CommandParameter = request;
            return addFriendButton;
        }

        private RelayCommand sendRequest;
        public RelayCommand SendRequest
        {
            get
            {
                return sendRequest ?? new RelayCommand((obj) =>
                {
                    User user = obj as User;
                    DataWorker.CreateFriendRequest(mainWindow.LoggedUser, user.Id);
                    mainWindow.ChangePage(Pages.UserSearch);
                }
                );
            }
        }

        private RelayCommand recallRequest;
        public RelayCommand RecallRequest
        {
            get
            {
                return recallRequest ?? new RelayCommand((obj) =>
                {
                    User user = obj as User;
                    DataWorker.DeleteRequest(mainWindow.LoggedUser.Id, user.Id);
                    mainWindow.ChangePage(Pages.UserSearch);
                }
                );
            }
        }

        private RelayCommand writeMessage;
        public RelayCommand WriteMessage
        {
            get
            {
                return writeMessage ?? new RelayCommand((obj) =>
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

        private RelayCommand acceptRequest;
        public RelayCommand AcceptRequest
        {
            get
            {
                return acceptRequest ?? new RelayCommand((obj) =>
                {
                    FriendRequest request = obj as FriendRequest;
                    DataWorker.AddFriend(request);
                    DataWorker.DeleteRequest(request);
                    mainWindow.ChangePage(Pages.Friends);
                }
                );
            }
        }
    }
}
