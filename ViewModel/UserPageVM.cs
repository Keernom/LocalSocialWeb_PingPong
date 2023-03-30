using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.View;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursovik_Kocherzhenko.ViewModel
{
    public class UserPageVM : INotifyPropertyChanged
    {
        public string UserName { get; }
        public string UserSurname { get; }
        public string UserMiddlename { get; }

        public string RandomRating { get; set; }

        public string postText;
        public string PostText
        {
            get { return postText; }
            set
            {
                postText = value;
                OnPropertyChanged(nameof(PostText));
            }
        }

        private MainWindowViewModel mainWindow;
        private UserPage userPage;
        private StackPanel panel;
        private Image avatarImage;

        private int UserId;
        string currentUserInfoDir;
        public UserPageVM(MainWindowViewModel mainWindow, UserPage userPage)
        {
            this.mainWindow = mainWindow;
            this.userPage = userPage;

            if (mainWindow.GetterId == 0)
            {
                UserId = mainWindow.LoggedUser.Id;
            }
            else
            {
                UserId = mainWindow.GetterId;
            }

            var fullNameDivided = GetFullNameDivided();
            UserSurname = fullNameDivided[0];
            UserName = fullNameDivided[1];
            UserMiddlename = fullNameDivided[2];

            SetRating();

            UpdateAvatar();

            panel = userPage.UserPostResult;
            UpdatePostView();
        }

        private void SetRating()
        {
            Random random = new Random();

            RandomRating = random.Next(1, 100).ToString();
        }

        private void UpdateAvatar()
        {
            avatarImage = userPage.AvatarImage;

            avatarImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(UserId));
        }

        private void UpdatePostView()
        {
            panel.Children.Clear();

            List<Post> posts = DataWorker.GetAllPosts(UserId);
            
            for (int i = posts.Count-1; i >= 0; i--)
            {
                Post post = posts[i];
                Border border = GetParentBorder();
                TextBlock textBlock = GetPostTextBlock(post);
                border.Child = textBlock;

                StackPanel underPostInfo = GetPostInfoPanel();

                StackPanel likesPanel = GetLikesPanel();
                Button likes = GetLikeLabel(post);
                TextBlock likesCount = GetLikesCountTextBlock(post);
                
                StackPanel commentsPanel = GetCommentPanel();
                Button comments = GetCommentButton(post);
                TextBlock commentsCount = GetCommentsCountText(post);

                StackPanel authorInfoPanel = new StackPanel();
                authorInfoPanel.Orientation = Orientation.Horizontal;
                authorInfoPanel.HorizontalAlignment = HorizontalAlignment.Left;

                Image postAuthorImage = new Image();
                postAuthorImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(post.AuthorId));
                postAuthorImage.Height = 30;
                postAuthorImage.HorizontalAlignment = HorizontalAlignment.Left;
                postAuthorImage.Margin = new Thickness(10, 25, 10, 0);

                TextBlock authorName = new TextBlock();
                authorName.Text = DataWorker.GetUserNameAndSurname(post.AuthorId);
                authorName.HorizontalAlignment = HorizontalAlignment.Left;
                authorName.VerticalAlignment= VerticalAlignment.Bottom;
                authorName.FontSize = 20;
                authorName.FontWeight = FontWeights.DemiBold;

                authorInfoPanel.Children.Add(postAuthorImage);
                authorInfoPanel.Children.Add(GetUserName(post.AuthorId));

                likesPanel.Children.Add(likes);
                likesPanel.Children.Add(likesCount);

                commentsPanel.Children.Add(comments);
                commentsPanel.Children.Add(commentsCount);

                underPostInfo.Children.Add(likesPanel);
                underPostInfo.Children.Add(commentsPanel);

                panel.Children.Add(authorInfoPanel);
                panel.Children.Add(border);
                panel.Children.Add(underPostInfo);
            }
        }

        private Button GetUserName(int userId)
        {
            Button button = new Button();
            button.Content = DataWorker.GetUserNameAndSurname(userId);
            button.FontSize = 20;
            button.FontWeight = FontWeights.DemiBold;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
            button.Command = OpenUserPage;
            button.CommandParameter = userId;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            return button;
        }

        private static TextBlock GetCommentsCountText(Post post)
        {
            TextBlock commentsCount = new TextBlock();
            commentsCount.Text = post.CommentsCount.ToString();
            commentsCount.FontSize = 20;
            commentsCount.VerticalAlignment = VerticalAlignment.Center;
            commentsCount.Foreground = Brushes.DarkTurquoise;
            return commentsCount;
        }

        private static StackPanel GetCommentPanel()
        {
            StackPanel commentsPanel = new StackPanel();
            commentsPanel.Orientation = Orientation.Horizontal;
            commentsPanel.HorizontalAlignment= HorizontalAlignment.Left;
            commentsPanel.Margin = new Thickness(10, 0, 0, 0);
            return commentsPanel;
        }

        private Button GetCommentButton(Post post)
        {
            Button comments = new Button();
            comments.Content = "✎";
            comments.FontSize = 20;
            comments.Foreground = Brushes.DarkTurquoise;
            comments.Background = Brushes.Transparent;
            comments.BorderThickness = new Thickness(0);
            comments.Command = OpenComments;
            comments.CommandParameter = post.Id;
            return comments;
        }

        private StackPanel GetPostInfoPanel()
        {
            StackPanel underPostInfo = new StackPanel();
            underPostInfo.Orientation = Orientation.Horizontal;
            underPostInfo.Margin = new Thickness(10, 0, 10, 0);
            return underPostInfo;
        }

        private TextBlock GetLikesCountTextBlock(Post post)
        {
            TextBlock likesCount = new TextBlock();
            likesCount.Text = post.LikesCount.ToString();
            likesCount.FontSize = 20;
            likesCount.VerticalAlignment= VerticalAlignment.Center;
            likesCount.Foreground = Brushes.DarkTurquoise;
            return likesCount;
        }

        private Button GetLikeLabel(Post post)
        {
            Button likes = new Button();
            likes.Content = DataWorker.IsPostLiked(post, mainWindow.LoggedUser.Id) ? "♥" : "♡";
            likes.FontSize = 30;
            likes.Foreground = Brushes.DarkTurquoise;
            likes.Background = Brushes.Transparent;
            likes.BorderThickness = new Thickness(0);
            likes.Command = LikeIncrease;
            likes.CommandParameter = post;

            return likes;
        }

        private StackPanel GetLikesPanel()
        {
            StackPanel likesPanel = new StackPanel();
            likesPanel.Orientation = Orientation.Horizontal;
            likesPanel.HorizontalAlignment = HorizontalAlignment.Left;
            return likesPanel;
        }

        private TextBlock GetPostTextBlock(Post post)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = post.Text;
            textBlock.FontSize = 20;
            textBlock.Margin = new Thickness(10);
            return textBlock;
        }

        private Border GetParentBorder()
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.DarkTurquoise;
            border.CornerRadius = new CornerRadius(15);
            border.Background = new SolidColorBrush(Colors.LightGray);
            border.Margin = new Thickness(10, 5, 10, 0);
            return border;
        }

        public string[] GetFullNameDivided()
        {
            User CurrentUser = DataWorker.GetUserById(UserId);
            return CurrentUser == null ? null : CurrentUser.FullName.Split(' ');
        }

        private RelayCommand addPost;
        public RelayCommand AddPost
        {
            get
            {
                return addPost ?? new RelayCommand((obj) =>
                {
                    if (PostText != null)
                    {
                        
                        DataWorker.CreatePost(UserId, mainWindow.LoggedUser.Id, PostText);
                        PostText = string.Empty;
                        if (UserId == mainWindow.LoggedUser.Id)
                        {
                            Post post = DataWorker.GetAllPosts(UserId).Last();
                            DataWorker.CreateNew(post.Id, UserId);
                        }
                        UpdatePostView();
                    }
                    else
                        MessageBox.Show("Ошибка! Пустое поле ввода!");
                }
                );
            }
        }

        private RelayCommand likeIncrease;
        public RelayCommand LikeIncrease
        {
            get
            {
                return likeIncrease ?? new RelayCommand((obj) =>
                {
                    Post post = (Post)obj;
                    DataWorker.LikesCountChange(post, mainWindow.LoggedUser.Id);
                    UpdatePostView();
                }
                );
            }
        }

        private RelayCommand openComments;
        public RelayCommand OpenComments
        {
            get
            {
                return openComments ?? new RelayCommand((obj) =>
                {
                    int postId = (int)obj;
                    mainWindow.PostId= postId;
                    mainWindow.ChangePage(Pages.Comment);
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
