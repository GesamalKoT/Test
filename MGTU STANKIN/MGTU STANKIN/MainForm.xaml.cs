using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public MySqlConnection globalConnection;
        public int access = 0;

        public MainForm(MySqlConnection connection, int a)
        {
            globalConnection = connection;
            access = a;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Run(() => nonPayed(globalConnection));
            Functions func = new Functions(globalConnection, access);
            Frame.NavigationService.Navigate(func);
        }

        private void Frame_ContentRendered(object sender, EventArgs e)
        {
            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

    }
}
