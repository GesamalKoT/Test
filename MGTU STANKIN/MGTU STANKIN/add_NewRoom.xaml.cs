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
using System.Data;
using MySql.Data.MySqlClient;
using MySql_Class;
using System.Collections.ObjectModel;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для add_NewRoom.xaml
    /// </summary>
    /// 

    public class Item
    {
        public int incr { get; set; }
        public string room_Number { get; set; }
        public string places_Count { get; set; }

        public  ObservableCollection<Item> GetItems(MySqlConnection connection) // убрал статику
        {
            ObservableCollection<Item> result = new ObservableCollection<Item>();

            MySqlCommand sel_Command = new MySqlCommand();
            sel_Command.Connection = connection;
            sel_Command.CommandText = "SELECT Number, Places FROM Rooms;";
            sel_Command.ExecuteNonQuery();

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sel_Command);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            var myData = dt.Select();

            for (int i = 0; i < myData.Length; i++)
            {
                result.Add(new Item() { incr = i + 1, room_Number = myData[i].ItemArray[0].ToString(), places_Count = myData[i].ItemArray[1].ToString() });
            }

            return result;
        }
    }

    public partial class add_NewRoom : Window
    {

        Item newItem;
        public add_NewRoom()
        {
            InitializeComponent();
        }

        public MySqlConnection globalConnection;

        public add_NewRoom(MySqlConnection connection)
        {
            InitializeComponent();
            room_Table.Items.Clear();
            newItem = new Item();
            globalConnection = connection;
        }

        private void close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void refresh_Table()
        {
            room_Table.ItemsSource = newItem.GetItems(globalConnection);
            //MessageBox.Show(GetItems().ToString());
            /*room_Table.ItemsSource = dt.DefaultView;*/
            
        }

        private void add_Room_Click(object sender, RoutedEventArgs e)
        {            
            if (Housing.Text != "" && Room.Text != "" && Places.SelectedItem != null)
            {
                if (Administrator.add_NewRoom(globalConnection, Housing.Text, Room.Text, Places.SelectedValue.ToString()) == 1)
                    MessageBox.Show("Комната успешно добавлена");
                else
                    MessageBox.Show("Комната с таким номером уже существует");
                Room.Text = "";
                Places.SelectedIndex = -1;
                refresh_Table();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
                               
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refresh_Table();
            string[] settings = System.IO.File.ReadAllLines(@"MySQL_Settings.txt");
            Housing.Text = settings[2];
        }

    }
}
