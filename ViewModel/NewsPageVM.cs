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
    class NewsPageVM
    {
        public string ItemText { get; set; }

        private MainWindowViewModel mainWindow;
        private NewsPage newsPage;

        private StackPanel panel;
        private List<New> news;
        public NewsPageVM(MainWindowViewModel mainWindow, NewsPage newsPage)
        {
            this.mainWindow = mainWindow;
            this.newsPage = newsPage;

            panel = newsPage.UserNewsResult;
            news = DataWorker.GetAllNews(mainWindow.LoggedUser.Id);
            UpdatePostView();
        }

        private void UpdatePostView()
        {
            panel.Children.Clear();

            foreach (var item in news)
            {
                Post post = DataWorker.GetPostById(item.Id);

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

                likesPanel.Children.Add(likes);
                likesPanel.Children.Add(likesCount);

                commentsPanel.Children.Add(comments);
                commentsPanel.Children.Add(commentsCount);

                underPostInfo.Children.Add(likesPanel);
                underPostInfo.Children.Add(commentsPanel);

                StackPanel authorInfoPanel = new StackPanel();
                authorInfoPanel.Orientation = Orientation.Horizontal;
                authorInfoPanel.HorizontalAlignment = HorizontalAlignment.Left;

                Image postAuthorImage = new Image();
                postAuthorImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(post.AuthorId));
                postAuthorImage.Height = 30;
                postAuthorImage.HorizontalAlignment = HorizontalAlignment.Left;
                postAuthorImage.Margin = new Thickness(10, 25, 10, 0);

                authorInfoPanel.Children.Add(postAuthorImage);
                authorInfoPanel.Children.Add(GetUserName(post.AuthorId));

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
            commentsPanel.HorizontalAlignment = HorizontalAlignment.Left;
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
            likesCount.VerticalAlignment = VerticalAlignment.Center;
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
            textBlock.Text = post.Text + " - " + DataWorker.GetUserById(post.AuthorId).Login;
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
            border.Margin = new Thickness(10, 25, 10, 0);
            return border;
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
                    mainWindow.PostId = postId;
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

    }
}
