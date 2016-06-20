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
    /// Логика взаимодействия для EvictTillWindow.xaml
    /// </summary>
    public partial class EvictTillWindow : Window
    {
        public MySqlConnection globalConnection;
        public studentInfo globalWindow;
        public string type = "";

        public EvictTillWindow()
        {
            InitializeComponent();
        }

        public EvictTillWindow(MySqlConnection connection, studentInfo w, string t)
        {
            globalConnection = connection;
            globalWindow = w;
            type = t;
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void evict_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand evict = new MySqlCommand();
            evict.Connection = globalConnection;

            if (type == "S")
            {
                evict.CommandText = "UPDATE Students SET evicted = 1, EvictedTill = '" + ((DateTime)evictDate.SelectedDate).ToString("yyyy-MM-dd H:mm:ss") + "', Room = null WHERE Student_ID = '" + globalWindow.id.Text + "';";
            }
            else
            {
                evict.CommandText = "UPDATE OtherLiving SET evicted = 1, EvictedTill = '" + ((DateTime)evictDate.SelectedDate).ToString("yyyy-MM-dd H:mm:ss") + "', Room = null WHERE ID = '" + globalWindow.id.Text + "';";
            }
            evict.ExecuteNonQuery();
            Common_Functions.delete_FromRoom(globalConnection, globalWindow.roomNumber.Content.ToString());
            globalWindow.Close();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            evict.IsEnabled = false;
        }

        private void evictDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (evictDate.SelectedDate != null && evictDate.SelectedDate > DateTime.Now)
                evict.IsEnabled = true;
        }

    }
}
