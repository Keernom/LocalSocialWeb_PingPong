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
    public class CommentPageVM : INotifyPropertyChanged
    {
        public string PostText { get; set; }

        public string commentText;
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(nameof(CommentText));
            }
        }

        private MainWindowViewModel mainWindow;
        private CommentPage commentPage;

        private StackPanel commentViewPanel;
        private StackPanel likesPanel;
        private StackPanel commentsPanel;

        private Post currentPost;

        public CommentPageVM(MainWindowViewModel mainWindow, CommentPage commentPage)
        {
            this.mainWindow = mainWindow;
            this.commentPage = commentPage;

            commentViewPanel = commentPage.PostCommentResult;
            likesPanel = commentPage.LikesPanel;
            commentsPanel= commentPage.CommentsPanel;

            PostText = DataWorker.GetPostById(mainWindow.PostId).Text;

            UpdatePostView();
            UpdateCommentsView();
        }

        private void UpdateCommentsView()
        {
            commentViewPanel.Children.Clear();

            List<Comment> comments = DataWorker.GetAllComments(mainWindow.PostId);

            foreach(var item in comments)
            {
                StackPanel commPan = new StackPanel();
                commPan.Orientation = Orientation.Horizontal;
                commPan.HorizontalAlignment= HorizontalAlignment.Left;

                Border line = new Border();
                line.BorderBrush = Brushes.DarkTurquoise;
                line.BorderThickness = new Thickness(.5);
                line.VerticalAlignment= VerticalAlignment.Center;
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.Padding = new Thickness(0, 0, 20, 0);

                Border border = new Border();
                border.Background = new SolidColorBrush(Colors.LightGray);
                border.BorderBrush = Brushes.DarkTurquoise;
                border.BorderThickness = new Thickness(2);
                border.CornerRadius = new CornerRadius(15);
                border.Margin = new Thickness(0, 10, 0, 10);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.Text;
                textBlock.FontSize = 20;
                textBlock.Padding = new Thickness(5);

                border.Child = textBlock;

                commPan.Children.Add(line);
                commPan.Children.Add(border);

                StackPanel authorInfoPanel = new StackPanel();
                authorInfoPanel.Orientation = Orientation.Horizontal;
                authorInfoPanel.HorizontalAlignment = HorizontalAlignment.Left;

                Image postAuthorImage = new Image();
                postAuthorImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(item.UserId));
                postAuthorImage.Height = 30;
                postAuthorImage.HorizontalAlignment = HorizontalAlignment.Left;
                postAuthorImage.Margin = new Thickness(10, 25, 10, 0);

                authorInfoPanel.Children.Add(postAuthorImage);
                authorInfoPanel.Children.Add(GetUserName(item.UserId));

                commentViewPanel.Children.Add(authorInfoPanel);
                commentViewPanel.Children.Add(commPan);
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
            button.VerticalAlignment= VerticalAlignment.Bottom;
            return button;
        }

        #region POST_VIEW

        private void UpdatePostView()
        {
            currentPost = DataWorker.GetPostById(mainWindow.PostId);

            Image postAuthorImage = commentPage.postAuthorImage;
            postAuthorImage.Source = new BitmapImage(DataWorker.GetImageSourceByUserId(currentPost.AuthorId));
            postAuthorImage.Height = 30;
            postAuthorImage.HorizontalAlignment = HorizontalAlignment.Left;
            postAuthorImage.Margin = new Thickness(10, 10, 10, 0);

            Button authorName = commentPage.postAuthorName;
            authorName.Content = DataWorker.GetUserNameAndSurname(currentPost.AuthorId);
            authorName.FontSize = 20;
            authorName.FontWeight = FontWeights.DemiBold;
            authorName.Background = Brushes.Transparent;
            authorName.BorderThickness = new Thickness(0);
            authorName.Command = OpenUserPage;
            authorName.CommandParameter = currentPost.AuthorId;
            authorName.VerticalAlignment = VerticalAlignment.Bottom;

            likesPanel.Children.Clear();
            commentsPanel.Children.Clear();

            Button likes = GetLikeLabel(currentPost);
            TextBlock likesCount = GetLikesCountTextBlock(currentPost);

            likesPanel.Children.Add(likes);
            likesPanel.Children.Add(likesCount);

            Button comments = GetCommentButton(currentPost);
            TextBlock commentsCount = GetCommentsCountText(currentPost);

            commentsPanel.Children.Add(comments);
            commentsPanel.Children.Add(commentsCount);
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

        private static TextBlock GetCommentsCountText(Post post)
        {
            TextBlock commentsCount = new TextBlock();
            commentsCount.Text = post.CommentsCount.ToString();
            commentsCount.FontSize = 20;
            commentsCount.VerticalAlignment = VerticalAlignment.Center;
            commentsCount.Foreground = Brushes.DarkTurquoise;
            return commentsCount;
        }

        #endregion

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

        private RelayCommand sendComment;
        public RelayCommand SendComment
        {
            get
            {
                return sendComment ?? new RelayCommand((obj) =>
                {
                    DataWorker.CreateComment(mainWindow.PostId, mainWindow.LoggedUser.Id, CommentText);
                    CommentText = string.Empty;
                    UpdatePostView();
                    UpdateCommentsView();
                    commentPage.Scroller.ScrollToEnd();
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
