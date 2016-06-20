using System;
using System.Collections.Generic;
using System.Linq;
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
using MySql.Data.MySqlClient;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для admin_Functions.xaml
    /// </summary>
    public partial class admin_Functions : Page
    {
        public admin_Functions()
        {
            InitializeComponent();
        }

        public MySqlConnection globalConnection;

        public admin_Functions(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void add_NewUser_Click(object sender, RoutedEventArgs e)
        {
            add_NewUser us = new add_NewUser(globalConnection);
            us.ShowDialog();
        }

        private void delete_User_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Room_Click(object sender, RoutedEventArgs e)
        {
            add_NewRoom w1 = new add_NewRoom(globalConnection);
            w1.ShowDialog();
        }

    }
}
