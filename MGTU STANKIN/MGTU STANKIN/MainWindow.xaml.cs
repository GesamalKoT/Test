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
using MySql_Class;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MySqlConnection globalConnection;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] MySQLSettings = System.IO.File.ReadAllLines(@"MySQL_Settings.txt");
            globalConnection = Start.BD_Check(MySQLSettings);
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
  
            switch(Start.enter(user_Name.Text,user_Password.Password, globalConnection))
            {
                case 0: MessageBox.Show("Пользователь не найден"); user_Name.Text = ""; user_Password.Password = ""; break;
                case -1: MessageBox.Show("Пароль не подходит"); user_Password.Password = ""; break;
                case 1: open_AdminWindow(); break;
                case 2: open_CommonWindow(); break;
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            /*room_Selection w1 = new room_Selection(globalConnection, "");
            w1.Show();*/
        }

        private void open_CommonWindow()
        {
            MainForm w2 = new MainForm(globalConnection, 2);
            w2.Show();
            this.Close();
        }

        private void open_AdminWindow()
        {
            MainForm w1 = new MainForm(globalConnection, 1);
            w1.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
