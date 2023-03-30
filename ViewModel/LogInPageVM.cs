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
using System.Windows.Media;

namespace Kursovik_Kocherzhenko.ViewModel
{
    public class LogInPageVM : INotifyPropertyChanged
    {
        private MainWindowViewModel mainWindow;

        public LogInPageVM(MainWindowViewModel mainWindow) 
        {
            this.mainWindow = mainWindow;
        }

        public string UserLogin { get; set; }
        public string UserPassword { get; set; }

        private RelayCommand logInUser;
        public RelayCommand LogInUser
        {
            get
            {
                return logInUser ?? new RelayCommand((obj) =>
                {
                    string resultStr = "Неверный логин или пароль!";
                    Page page = (Page)obj;

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        bool checkIsExist = db.Users.Any(x => x.Login == UserLogin);

                        if (checkIsExist)
                        {
                            User userToLogIn = db.Users.FirstOrDefault(u => u.Login == UserLogin);

                            if (UserPassword == userToLogIn.Password)
                            {
                                mainWindow.LoggedUser = userToLogIn;
                                resultStr = "Велком, " + mainWindow.LoggedUser.Login + "!";
                                mainWindow.ChangePage(Pages.User);
                            }
                            else
                            {
                                SetRedBlockControll(page, "UserLoginBlock");
                                SetRedBlockControll(page, "UserPasswordBlock");
                                MessageBox.Show(resultStr);
                            }
                        }
                        else
                        {
                            SetRedBlockControll(page, "UserLoginBlock");
                            SetRedBlockControll(page, "UserPasswordBlock");
                            MessageBox.Show(resultStr);
                        }
                    }
                }
                );
            }
        }

        private void SetRedBlockControll(Page pg, string blockName)
        {
            Border block = pg.FindName(blockName) as Border;
            block.BorderBrush = Brushes.Red;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
