using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursovik_Kocherzhenko.ViewModel
{
    class FriendsPageVm
    {
        private MainWindowViewModel mainWindow;
        private FriendsPage friendsPage;

        public FriendsPageVm(MainWindowViewModel mainWindow, FriendsPage friendsPage) 
        { 
            this.mainWindow = mainWindow;
            this.friendsPage = friendsPage;

            StackPanel resultContainer = friendsPage.FriendsResult;
            List<FriendRequest> friendRequests = DataWorker.GetAllRequests();
            List<Friend> friends = DataWorker.GetAllFriends();

            if (friendRequests.Any(r => r.GetterId == mainWindow.LoggedUser.Id))
            {
                resultContainer.Children.Add(GetRequestHeader());
                foreach (var item in friendRequests)
                {
                    if (mainWindow.LoggedUser.Id == item.GetterId)
                    {
                        int friendId = item.UserId == mainWindow.LoggedUser.Id ? item.GetterId : item.UserId;

                        DockPanel userView = new DockPanel();

                        userView.Children.Add(GetUserImage(friendId));
                        userView.Children.Add(GetUserName(item));
                        userView.Children.Add(GetWriteMessageButton(friendId));
                        userView.Children.Add(GetAddFriendButton(item));
                        userView.Children.Add(GetDeclineFriendButton(item));

                        resultContainer.Children.Add(userView);
                    }
                }
            }

            if (friends.Any(f => f.UserId == mainWindow.LoggedUser.Id || f.FriendId == mainWindow.LoggedUser.Id))
            {
                resultContainer.Children.Add(GetFriendHeader());
                foreach (var item in friends)
                {
                    if (mainWindow.LoggedUser.Id == item.UserId || mainWindow.LoggedUser.Id == item.FriendId)
                    {
                        int friendId = item.UserId == mainWindow.LoggedUser.Id ? item.FriendId : item.UserId;

                        DockPanel userView = new DockPanel();

                        userView.Children.Add(GetUserImage(friendId));
                        userView.Children.Add(GetFriendName(item));
                        userView.Children.Add(GetWriteMessageButton(friendId));
                        userView.Children.Add(GetDeleteFriendButton(item));

                        resultContainer.Children.Add(userView);
                    }
                }
            }

        }

        private Image GetUserImage(int userId)
        {
            Image userImage = new Image();
            userImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(userId));
            userImage.Height = 30;
            userImage.HorizontalAlignment = HorizontalAlignment.Left;
            return userImage;
        }

        private Button GetWriteMessageButton(int friendId)
        {
            Button button= new Button();
            button.Content = "Написать сообщение";
            button.Margin = new Thickness(20);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Command = WriteMessage;
            button.CommandParameter = friendId;
            return button;
        }

        private Button GetFriendName(Friend item)
        {
            Button button = new Button();
            int friendId = item.UserId == mainWindow.LoggedUser.Id ? item.FriendId : item.UserId;

            User user = DataWorker.GetUserById(friendId);
            button.Content = user.FullName;
            button.FontSize = 20;
            button.Margin = new Thickness(20);
            button.FontWeight = FontWeights.DemiBold;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
            button.Command = OpenUserPage;
            button.CommandParameter = user.Id;
            return button;
        }

        private TextBlock GetRequestHeader()
        {
            TextBlock textBlock= new TextBlock();
            textBlock.FontSize = 30;
            textBlock.Text = "Запросы на добавление в друзья ";
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(20);
            return textBlock;
        }

        private TextBlock GetFriendHeader()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 30;
            textBlock.Text = "Друзья ";
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(20);
            return textBlock;
        }

        private Button GetUserName(FriendRequest request)
        {
            User initiator = DataWorker.GetUserById(request.UserId);
        
            Button button = new Button();
            button.Content = initiator.FullName;
            button.FontSize = 20;
            button.Margin = new Thickness(20);
            button.FontWeight = FontWeights.DemiBold;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
            button.Command = OpenUserPage;
            button.CommandParameter = initiator.Id;

            return button;
        }

        private Button GetDeclineFriendButton(FriendRequest request)
        {
            Button declineFriendButton = new Button();
            declineFriendButton.Content = "Отклонить запрос";
            declineFriendButton.Margin = new Thickness(20);
            declineFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            declineFriendButton.Command = DeclineRequest;
            declineFriendButton.CommandParameter = request;
            return declineFriendButton;
        }

        private Button GetAddFriendButton(FriendRequest request)
        {
            Button addFriendButton = new Button();
            addFriendButton.Content = "Принять запрос";
            addFriendButton.Margin = new Thickness(20);
            addFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            addFriendButton.Command = AcceptRequest;
            addFriendButton.CommandParameter = request;
            return addFriendButton;
        }

        private Button GetDeleteFriendButton(Friend friend)
        {
            Button deleteFriendButton = new Button();
            deleteFriendButton.Content = "Удалить из друзей";
            deleteFriendButton.Margin = new Thickness(20);
            deleteFriendButton.HorizontalAlignment = HorizontalAlignment.Right;
            deleteFriendButton.Command = DeleteFriend;
            deleteFriendButton.CommandParameter = friend;
            return deleteFriendButton;
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

        private RelayCommand declineRequest;
        public RelayCommand DeclineRequest
        {
            get
            {
                return declineRequest ?? new RelayCommand((obj) =>
                {
                    FriendRequest request = obj as FriendRequest;
                    DataWorker.DeleteRequest(request);
                    mainWindow.ChangePage(Pages.Friends);
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

        private RelayCommand deleteFriend;
        public RelayCommand DeleteFriend
        {
            get
            {
                return deleteFriend ?? new RelayCommand((obj) =>
                {
                    Friend friend = obj as Friend;
                    DataWorker.DeleteFriend(friend);
                    mainWindow.ChangePage(Pages.Friends);
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
