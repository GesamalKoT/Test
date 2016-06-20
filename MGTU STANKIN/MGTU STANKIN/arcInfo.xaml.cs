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
using System.Data;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для arcInfo.xaml
    /// </summary>
    public partial class arcInfo : Window
    {
        public MySqlConnection globalConnection;
        public string _ID;

        public arcInfo()
        {
            InitializeComponent();
        }

        public arcInfo (MySqlConnection connection, string id)
        {
            globalConnection = connection;
            _ID = id;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlCommand search = new MySqlCommand();
            search.Connection = globalConnection;
            search.CommandText = "SELECT * FROM Archive WHERE Student_ID = '" + _ID + "';";
            search.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(search);
            DataTable dt = new DataTable();
            da.Fill(dt);
            var myData = dt.Select();

            surname.Content = myData[0].ItemArray[0].ToString();
            name.Content = myData[0].ItemArray[1].ToString();
            patronymic.Content = myData[0].ItemArray[2].ToString();
            ID.Content = myData[0].ItemArray[3].ToString();

            if (myData[0].ItemArray[5].ToString() == "")
                decree.Text = "";
            else
                decree.Text = myData[0].ItemArray[5].ToString();

            if (myData[0].ItemArray[6].ToString() == "")
                decreeDate.SelectedDate = null;
            else
                decreeDate.SelectedDate = Convert.ToDateTime(myData[0].ItemArray[6]);
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            string date = "";
            DateTime dt = DateTime.Now;
            string dec = "";

            if (decree.Text != "")
                dec = decree.Text;

            if (decreeDate.SelectedDate == null)
                date = "";
            else
            {
                dt = (DateTime)decreeDate.SelectedDate;
                date = dt.ToString("yyyy-MM-dd H:mm:ss");
            }

            MySqlCommand update = new MySqlCommand();
            update.Connection = globalConnection;
            update.CommandText = "UPDATE Archive SET Decree = '" + dec + "', Date = '" + date + "' WHERE Student_ID = '" + ID.Content.ToString() + "';";
            update.ExecuteNonQuery();

            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand delete = new MySqlCommand();
            delete.Connection = globalConnection;
            delete.CommandText = "DELETE FROM Archive WHERE Student_ID = '" + ID.Content.ToString() + "';";
            delete.ExecuteNonQuery();

            this.Close();
        }
    }
}
