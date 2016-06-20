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
using System.Windows.Shapes;
using MySql_Class;
using MySql.Data.MySqlClient;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для add_NewUser.xaml
    /// </summary>
    public partial class add_NewUser : Window
    {
        public MySqlConnection globalConnection;

        public add_NewUser()
        {
            InitializeComponent();
        }
        
        public add_NewUser(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (user_Name.Text != "" && user_Password.Password != "")
                if (Administrator.add_NewUser(user_Name.Text, user_Password.Password, globalConnection) == 1)
                {
                    user_Name.Text = "";
                    user_Password.Password = "";
                    MessageBox.Show("Пользователь успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем уже существует");
                    user_Name.Text = "";
                    user_Password.Password = "";
                }
            else
                MessageBox.Show("Заполните все поля");

        }

    }
}
