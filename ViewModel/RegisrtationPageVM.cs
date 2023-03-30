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

namespace Kursovik_Kocherzhenko.ViewModel
{
    public class RegisrtationPageVM : INotifyPropertyChanged
    {
        public string UserFullName { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }

        private MainWindowViewModel mainWindow;

        public RegisrtationPageVM(MainWindowViewModel mainWindow) 
        {
            this.mainWindow = mainWindow;
        }

        private RelayCommand addNewUser;
        public RelayCommand AddNewUser
        {
            get
            {
                return addNewUser ?? new RelayCommand((obj) =>
                {
                    string resultStr = "";

                    if (UserFullName != null && CanBeDivided(UserFullName) && UserLogin != null && UserPassword != null)
                    {
                        resultStr = DataWorker.CreateUser(UserFullName, UserLogin, UserPassword);
                        mainWindow.LoggedUser = DataWorker.GetUserByLogin(UserLogin);
                        mainWindow.ChangePage(Pages.User);
                    }
                    
                    //MessageBox.Show(resultStr);
                }
                );
            }
        }

        private bool CanBeDivided(string userFN)
        {
            string[] userFnParts = userFN.Split(' ');

            if (userFnParts.Length != 3)
                MessageBox.Show("Ошибка ввода! Введите ФИО через пробел!");

            return userFnParts.Length == 3;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
