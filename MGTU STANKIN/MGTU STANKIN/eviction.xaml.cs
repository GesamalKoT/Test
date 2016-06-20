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
using MySql.Data.MySqlClient;
using MySql_Class;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для eviction.xaml
    /// </summary>
    public partial class eviction : Window
    {
        public MySqlConnection globalConnection;
        public string type;
        public studentInfo window;
        public studentSerch page;
        public string id;

        public eviction()
        {
            InitializeComponent();
        }

        public eviction(MySqlConnection connection, string t, studentInfo s, string ID, studentSerch p)
        {
            globalConnection = connection;
            type = t;
            id = ID;
            InitializeComponent();
            window = s;
            page = p;
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            switch(type)
            {
                case "S": Common_Functions.evictionST(globalConnection, id, decree.Text, (DateTime)date.SelectedDate); break;
                case "SE": Common_Functions.evictionSTE(globalConnection, id, decree.Text, (DateTime)date.SelectedDate); break;
                case "O": Common_Functions.evictionOL(globalConnection, id, decree.Text, (DateTime)date.SelectedDate); break;
                case "OE": Common_Functions.evictionOLE(globalConnection, id, decree.Text, (DateTime)date.SelectedDate); break;
            }

            /*if (type == "S")
            {
                Common_Functions.evictionST(globalConnection, id, decree.Text, (DateTime)date.SelectedDate);
            }
            else
            {
                Common_Functions.evictionOL(globalConnection, id, decree.Text, (DateTime)date.SelectedDate);
            }*/

            page.searchButton_Click(sender, e);
            window.Close();
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            date.SelectedDate = DateTime.Today;
        }
    }
}
