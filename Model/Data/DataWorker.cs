using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.Model;
using static System.Net.Mime.MediaTypeNames;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media;
using System.Windows.Documents;
using System.IO;

namespace Kursovik_Kocherzhenko.Model.Data
{
    internal static class DataWorker
    {
        #region USER_METHODS 

        public static string CreateUser(string fullName, string login, string password)
        {
            string result = "Пользователь " + login + " уже существует.";
            
            using(ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(x => x.Login == login);

                if (!checkIsExist)
                {
                    User newUser = new User
                    {
                        FullName = fullName,
                        Login = login,
                        Password = password
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    result = "Пользователь " + login + " добавлен"!;
                }
            }

            return result;
        }

        public static User GetUserById(int id)
        {
            User user = new User();

            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Users.FirstOrDefault(user => user.Id == id);
            }

            return user;
        }

        public static User GetUserByLogin(string login)
        {
            User user = new User();

            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Users.FirstOrDefault(user=> user.Login == login);
            }

            return user;
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (ApplicationContext db = new ApplicationContext())
            {
                users = db.Users.ToList();
            }
            return users;
        }

        public static string GetUserNameAndSurname(int userId)
        {
            User user = GetUserById(userId);

            string[] fio = user.FullName.Split(' ');
            
            string nameAndSurname = fio[0] + " " + fio[1];

            return nameAndSurname;
        }

        #endregion

        #region FRIEND_REQUEST_METHODS

        public static void CreateFriendRequest(User user, int getterId)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                FriendRequest newFR = new FriendRequest
                {
                    UserId = user.Id,
                    GetterId = getterId
                };

                db.FriendRequests.Add(newFR);
                db.SaveChanges();
            }
        }

        public static List<FriendRequest> GetAllRequests()
        {
            List<FriendRequest> friendRequests= new List<FriendRequest>();
            using (ApplicationContext db = new ApplicationContext())
            {
                friendRequests = db.FriendRequests.ToList();
            }
            return friendRequests;
        }

        public static void DeleteRequest(FriendRequest request)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.FriendRequests.Remove(request);
                db.SaveChanges();
            }
        }

        public static void DeleteRequest(int initiatorId, int getterId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                FriendRequest request = db.FriendRequests.FirstOrDefault(fr => fr.UserId == initiatorId && fr.GetterId == getterId);
                db.FriendRequests.Remove(request);
                db.SaveChanges();
            }
        }

        public static bool RequestSent(int userId, int getterId)
        {
            bool result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.FriendRequests.Any(fr => fr.UserId == userId && fr.GetterId == getterId);
            }
            return result;
        }

        #endregion

        #region FRIEND_METHODS

        public static void AddFriend(FriendRequest request)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Friend newFriend = new Friend
                {
                    UserId = request.UserId,
                    FriendId = request.GetterId
                };

                db.Friends.Add(newFriend);
                db.SaveChanges();
            }
        }

        public static void DeleteFriend(Friend friend)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Friends.Remove(friend);
                db.SaveChanges();
            }
        }

        public static List<Friend> GetAllFriends()
        {
            List<Friend> friends= new List<Friend>();
            using (ApplicationContext db = new ApplicationContext())
            {
                friends = db.Friends.ToList();
            }
            return friends;
        }

        public static bool AreFriends(User user, User anotherUser)
        {
            bool result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.Friends.Any(f => (f.UserId == user.Id && f.FriendId == anotherUser.Id) ||
                    (f.UserId == anotherUser.Id && f.FriendId == user.Id));
            }
            return result;
        }

        #endregion

        #region MESSAGE_METHODS

        public static void CreateMessage(string date, int senderId, int getterId, string text)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Message newMessage = new Message
                {
                    Date= date,
                    Sender = senderId,
                    Getter = getterId,
                    Text = text
                };

                db.Messages.Add(newMessage);
                db.SaveChanges();
            }
        }

        public static List<Message> GetAllMessages(int senderId, int getterId)
        {
            List<Message> messages = new List<Message>();
            using (ApplicationContext db = new ApplicationContext())
            {
                messages = db.Messages.Where(m => (m.Sender == senderId && m.Getter == getterId) ||
                                                  (m.Sender == getterId && m.Getter == senderId)).ToList();
            }
            return messages;
        }

        public static List<Message> GetAllMessages(int senderId)
        {
            List<Message> messages = new List<Message>();
            using (ApplicationContext db = new ApplicationContext())
            {
                messages = db.Messages.Where(m => m.Sender == senderId || m.Getter == senderId).ToList();
            }
            return messages;
        }

        #endregion

        #region POST_METHODS

        public static void CreatePost(int userId, int authorId,string text)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Post newPost = new Post
                {
                    UserId = userId,
                    AuthorId = authorId,
                    Text = text,
                    LikesCount= 0,
                    CommentsCount = 0
                };

                db.Posts.Add(newPost);
                db.SaveChanges();
            }
        }

        public static List<Post> GetAllPosts(int userId)
        {
            List<Post> posts = new List<Post>();
            using (ApplicationContext db = new ApplicationContext())
            {
                posts = db.Posts.Where(p => p.UserId == userId).ToList();
            }
            return posts;
        }

        public static Post GetPostById(int postId)
        {
            Post post = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                post = db.Posts.FirstOrDefault(p => p.Id == postId);
            }
            return post;
        }

        public static void LikesCountChange(Post post, int likerId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Post currentPost = db.Posts.FirstOrDefault(p => p.Id == post.Id);

                if (!IsPostLiked(post, likerId))
                {
                    Like like = new Like
                    {
                        UserId = likerId,
                        PostId = post.Id,
                    };

                    db.Likes.Add(like);
                    currentPost.LikesCount++;
                }
                else
                {
                    Like like = db.Likes.FirstOrDefault(l => l.UserId == likerId && l.PostId == post.Id);
                    db.Likes.Remove(like);
                    currentPost.LikesCount--;
                }
                
                db.SaveChanges();
            }
        }

        public static bool IsPostLiked(Post post, int likerId)
        {
            bool result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.Likes.Any(l => l.UserId == likerId && l.PostId == post.Id);
            }
            return result;
        }
        #endregion

        #region COMMENT_METHODS

        public static void CreateComment(int postId, int userId, string text)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Comment newComment = new Comment
                {
                    UserId = userId,
                    PostId = postId,
                    Text = text
                };

                db.Posts.FirstOrDefault(p => p.Id == postId).CommentsCount++;
                
                db.Comments.Add(newComment);
                db.SaveChanges();
            }
        }

        public static List<Comment> GetAllComments(int postId) 
        {
            List<Comment> comments = new List<Comment>();
            using (ApplicationContext db = new ApplicationContext())
            {
                comments = db.Comments.Where(c => c.PostId == postId).ToList();
            }
            return comments;
        }

        #endregion

        #region NEWS_METHODS

        public static void CreateNew(int postId, int authorId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                New newItem = new New
                {
                    PostId = postId
                };

                db.News.Add(newItem);   
                db.SaveChanges();

                New currentNew = db.News.FirstOrDefault(n => n.PostId == postId);

                Check newCheck = new Check
                {
                    UserId = authorId,
                    NewsId = currentNew.Id
                };

                db.Checks.Add(newCheck);
                db.SaveChanges();
            }
        }

        public static List<New> GetAllNews(int viewerId)
        {
            List<New> list = new List<New>();
            using (ApplicationContext db = new ApplicationContext())
            {
                list = db.News.Where(n => !db.Checks.Any(c => c.UserId == viewerId && c.NewsId == n.Id)).ToList();

                foreach (var item in list)
                {
                    Check newCheck = new Check
                    {
                        UserId = viewerId,
                        NewsId = item.Id
                    };

                    db.Checks.Add(newCheck);
                    db.SaveChanges();
                }
            }
            return list;
        }

        #endregion

        #region PHOTOS_METHODS

        private const string userInfoName = @".\UserInfo";
        const string avatarName = "def_avatar.png";

        public static Uri GetImageSourceByUserId(int userId)
        {
            string currentUserInfoDir = Path.Join(@".\UserInfo", DataWorker.GetUserById(userId).Login);

            DirectoryInfo directoryInfo = new DirectoryInfo(currentUserInfoDir);

            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(currentUserInfoDir);
                string fullDefAvatarPath = Path.Join(userInfoName, avatarName);

                string fullUserAvatarPath = Path.Join(currentUserInfoDir, avatarName);

                File.Copy(fullDefAvatarPath, fullUserAvatarPath);
            }

            DirectoryInfo freshInfo = new DirectoryInfo(currentUserInfoDir);

            if (freshInfo.Exists)
            {
                string[] pathParts = freshInfo.FullName.Split('\\');

                string finalPath = string.Empty;

                for (int i = 0; i < pathParts.Length; i++)
                {
                    finalPath += pathParts[i] + "/";
                }

                finalPath += avatarName;
                Uri source = new Uri(finalPath);
                return source;
            }

            return new Uri(string.Empty);
        }

        #endregion
    }

}
