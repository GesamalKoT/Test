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
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для Citizenship.xaml
    /// </summary>
    /// 

    public class CitizenshipItem
    {
        public string shortName { get; set; }
        public string longName { get; set; }

        public ObservableCollection<CitizenshipItem> getItems(MySqlConnection globalConnecton)
        {
            ObservableCollection<CitizenshipItem> resault = new ObservableCollection<CitizenshipItem>();

            MySqlCommand getCitizenship = new MySqlCommand();
            getCitizenship.Connection = globalConnecton;
            getCitizenship.CommandText = "SELECT * FROM Citizenship;";
            getCitizenship.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(getCitizenship);
            DataTable dt = new DataTable();
            da.Fill(dt);
            var myData = dt.Select();

            for(int i = 0; i < myData.Length; i++)
            {
                resault.Add(new CitizenshipItem() { shortName = myData[i].ItemArray[0].ToString(), longName = myData[i].ItemArray[1].ToString() });
            }

            return resault;
        }
    }

    public partial class Citizenship : Window
    {
        public MySqlConnection globalConnection;
        CitizenshipItem newItem;
        ComboBox cBox;

        public Citizenship()
        {
            InitializeComponent();
        }

        public Citizenship(MySqlConnection connection, ComboBox cb)
        {
            InitializeComponent();
            globalConnection = connection;
            citizenshipTable.Items.Clear();
            cBox = cb;
            newItem = new CitizenshipItem();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand addCitizenship = new MySqlCommand();
            addCitizenship.Connection = globalConnection;
            addCitizenship.CommandText = "INSERT INTO Citizenship VALUES ('"+shortC.Text.ToString()+"','"+fullC.Text.ToString()+"');";
            if (shortC.Text != "" && fullC.Text != "")
            {
                addCitizenship.ExecuteNonQuery();
                refreshTable();
                MessageBox.Show("Страна успешно добавлена");
                shortC.Text = "";
                fullC.Text = "";
                bnd.bindCombobox(cBox, globalConnection);
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            bnd.bindCombobox(cBox, globalConnection);
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshTable();
        }

        public void refreshTable()
        {
            citizenshipTable.ItemsSource = newItem.getItems(globalConnection);
        }
    }
}
