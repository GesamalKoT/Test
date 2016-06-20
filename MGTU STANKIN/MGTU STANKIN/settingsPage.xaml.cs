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
using System.Data;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для settingsPage.xaml
    /// </summary>
    public partial class settingsPage : Page
    {
        public MySqlConnection globalConnection;

        public settingsPage()
        {
            InitializeComponent();
        }

        public settingsPage(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlCommand fillData = new MySqlCommand();
            fillData.Connection = globalConnection;
            fillData.CommandText = "SELECT * FROM Settings;";
            fillData.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(fillData);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var myData = dt.Select();

            for(int i = 0 ; i < myData.Length; i++)
            {
                fioString.Text = myData[i].ItemArray[1].ToString();
                //autumnDate.SelectedDate = Convert.ToDateTime()
                switch(myData[i].ItemArray[6].ToString())
                {
                    case "При каждом запуске": checkPeriod.SelectedIndex = 0; break;
                    case "Раз в неделю": checkPeriod.SelectedIndex = 1; break;
                    case "Раз в месяц": checkPeriod.SelectedIndex = 2; break;
                    case "Не проверять": checkPeriod.SelectedIndex = 3; break;
                    default: checkPeriod.SelectedIndex = -1; break;
                }
                housingNumber.Text = myData[i].ItemArray[2].ToString();
                switch(myData[i].ItemArray[3].ToString())
                {
                    case "1 месяц": outTime.SelectedIndex = 0; break;
                    case "6 месяцев": outTime.SelectedIndex = 1; break;
                    case "1 год": outTime.SelectedIndex = 2; break;
                    case "Не удалять информацию": outTime.SelectedIndex = 3; break;
                    default: outTime.SelectedIndex = -1; break;
                }

                if(myData[i].ItemArray[5].ToString() != "")
                    autumnDateString.Text = (Convert.ToDateTime(myData[i].ItemArray[5])).ToLongDateString().ToString();
                if(myData[i].ItemArray[4].ToString() != "")
                    springDateString.Text = (Convert.ToDateTime(myData[i].ItemArray[4])).ToLongDateString().ToString();
            }

        }

        private void autumnDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)autumnDate.SelectedDate;
            string date = dt.ToLongDateString();
            autumnDateString.Text = date;
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand insertSetting = new MySqlCommand();
            insertSetting.Connection = globalConnection;
            DateTime aDate;
            DateTime sDate;
            string APayDate;
            string SPayDate;
            
            if(fioString.Text != "")
            {
                insertSetting.CommandText = "UPDATE Settings SET FIO = '" + fioString.Text + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            if(autumnDate.SelectedDate != null)
            {
                aDate = (DateTime)autumnDate.SelectedDate;
                APayDate = aDate.ToString("yyyy-MM-dd H:mm:ss");
                insertSetting.CommandText = "UPDATE Settings SET AutumnPay = '" + APayDate + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            if(springDate.SelectedDate != null)
            {
                sDate = (DateTime)springDate.SelectedDate;
                SPayDate = sDate.ToString("yyyy-MM-dd H:mm:ss");
                insertSetting.CommandText = "UPDATE Settings SET SpringPay = '" + SPayDate + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            if(checkPeriod.SelectedIndex != -1)
            {
                insertSetting.CommandText = "UPDATE Settings SET CheckPeriod = '" + checkPeriod.SelectedValue.ToString() + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            if(housingNumber.Text != "")
            {
                insertSetting.CommandText = "UPDATE Settings SET Housing = '" + housingNumber.Text + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            if(outTime.SelectedIndex != -1)
            {
                insertSetting.CommandText = "UPDATE Settings SET OutTime = '" + outTime.SelectedValue.ToString() + "' WHERE ID = 1;";
                insertSetting.ExecuteNonQuery();
            }

            this.NavigationService.GoBack();
        }

        private void springDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)springDate.SelectedDate;
            string date = dt.ToLongDateString();
            springDateString.Text = date;
        }

    }
}
