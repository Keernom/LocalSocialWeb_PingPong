using Kursovik_Kocherzhenko.Model;
using Kursovik_Kocherzhenko.Model.Data;
using Kursovik_Kocherzhenko.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursovik_Kocherzhenko.View
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage(MainWindowViewModel mainWindow)
        {
            InitializeComponent();
            DataContext = new RegisrtationPageVM(mainWindow);
        }
    }
}
