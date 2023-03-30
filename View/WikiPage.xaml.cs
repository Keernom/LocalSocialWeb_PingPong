using Kursovik_Kocherzhenko.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for WikiPage.xaml
    /// </summary>
    public partial class WikiPage : Page
    {
        public WikiPage(MainWindowViewModel mainWindow)
        {
            InitializeComponent();
            DataContext = new WikiPageVM(mainWindow);
        }
    }
}
