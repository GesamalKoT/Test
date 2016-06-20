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
using System.Threading;
using System.ComponentModel;
using MySql_Class;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для Functions.xaml
    /// </summary>
    public partial class Functions : Page
    {

        public string[] errorMessages;
        public int messageCounter = 0;
        //private BackgroundWorker backgroundWorker;
        public Functions()
        {
            InitializeComponent();
        }
      
        public MySqlConnection globalConnection;
        public int access;

        public Functions(MySqlConnection connection, int a)
        {
            globalConnection = connection;
            access = a;
            InitializeComponent();
            //backgroundWorker = ((BackgroundWorker)this.FindResource("backgroungWorker"));
        }

        private void add_Student_Click(object sender, RoutedEventArgs e)
        {
            add_Student ad_st = new add_Student(globalConnection);
            this.NavigationService.Navigate(ad_st);
        }

        private void admin_Functions_Click(object sender, RoutedEventArgs e)
        {
            testLabel1.Content = "";
            admin_Functions ad_func = new admin_Functions(globalConnection);
            this.NavigationService.Navigate(ad_func);
            
        }

        private void archiveDelete(string message)
        {
            bool f = false;
            string dateString = "";

            if (message == "Не удалять информацию")
                f = false;
            else
            {
                f = true;
                DateTime aDate = DateTime.Now;

                switch(message)
                {
                    case "1 месяц": aDate = DateTime.Now.AddMonths(-1); break;
                    case "6 месяцев": aDate = DateTime.Now.AddMonths(-6); break;
                    case "1 год": aDate = DateTime.Now.AddYears(-1); break;
                }

                dateString = aDate.ToString("yyyy-MM-dd H:mm:ss");
            }

            if(f == true)
            {
                MySqlCommand deleteArc = new MySqlCommand();
                deleteArc.Connection = globalConnection;
                deleteArc.CommandText = "DELETE FROM Archive WHERE Date < '" + dateString + "';";
                deleteArc.ExecuteNonQuery();
            }
        }

        private string dateCheck(string aDate, string sDate)
        {
            string resaultMessage = "";
            if (aDate != "" && sDate != "")
            {
                DateTime aDateCheck = Convert.ToDateTime(aDate);
                DateTime sDateCheck = Convert.ToDateTime(sDate);

                if (DateTime.Now.AddDays(7) >= aDateCheck && DateTime.Now < aDateCheck)
                {
                    TimeSpan ts = aDateCheck - DateTime.Now;
                    int daysCouns = ts.Days;
                    resaultMessage = "Срок оплаты проживания за весенний семестр наступает через " + daysCouns + " дней";
                }

                if (DateTime.Now.AddDays(7) >= sDateCheck && DateTime.Now < sDateCheck)
                {
                    TimeSpan ts = sDateCheck - DateTime.Now;
                    int daysCouns = ts.Days;
                    resaultMessage = "Срок оплаты проживания за осенний семестр наступает через " + daysCouns + " дней";
                }

                MySqlCommand dateChange = new MySqlCommand();
                dateChange.Connection = globalConnection;

                if(sDateCheck < DateTime.Now)
                {
                    dateChange.CommandText = "UPDATE Settings SET SpringPay = '" + sDateCheck.AddYears(1).ToString("yyyy-MM-dd H:mm:ss") + "' WHERE ID = 1;";
                    dateChange.ExecuteNonQuery();
                }

                if(aDateCheck < DateTime.Now)
                {
                    dateChange.CommandText = "UPDATE Settings SET AutumnPay = '" + aDateCheck.AddYears(1).ToString("yyyy-MM-dd H:mm:ss") + "' WHERE ID = 1;";
                    dateChange.ExecuteNonQuery();
                }
            }

            return resaultMessage;
        }


        private void initionalTest()
        {
            Action action = () =>
                {
                    bool f = true;
                    bool f2 = true;

                    MySqlCommand settings = new MySqlCommand();
                    settings.Connection = globalConnection;
                    settings.CommandText = "SELECT * FROM Settings";
                    settings.ExecuteNonQuery();
                    string outTimeMessage = "";
                    string sDateCheck = "";
                    string aDateCheck = "";

                    MySqlDataAdapter sDA = new MySqlDataAdapter(settings);
                    DataTable sDT = new DataTable();
                    sDA.Fill(sDT);
                    var mySData = sDT.Select();
                    outTimeMessage = mySData[0].ItemArray[3].ToString();
                    sDateCheck = mySData[0].ItemArray[4].ToString();
                    aDateCheck = mySData[0].ItemArray[5].ToString();

                    if (mySData[0].ItemArray[6].ToString() == "Не проверять")
                        f = false;
                    else
                    {
                        f = true;
                        if(mySData[0].ItemArray[6].ToString() != "При каждом запуске")
                        {
                            DateTime checkDate = DateTime.Now;
                            switch(mySData[0].ItemArray[6].ToString())
                            {
                                case "Раз в неделю": checkDate = DateTime.Now.AddDays(-7); break;
                                case "Раз в месяц": checkDate = DateTime.Now.AddMonths(-1); break;
                                default: break;
                            }

                            if (checkDate != DateTime.Now)
                            {
                                DateTime lastCheck = (DateTime)mySData[0].ItemArray[7];
                                if (lastCheck <= checkDate)
                                    f2 = true;
                                else
                                    f2 = false;
                            }
                        }
                        else
                        {
                            f2 = true;
                        }
                    }

                    if (f == true && f2 == true)
                    {
                        string message1 = "Есть проживающие, не оплатившие проживание";
                        string message2 = "Есть проживающие, у которых закончился период проживания";
                        string message3 = "";
                        string message4 = "";
                        bool fl1 = false;
                        bool fl2 = false;
                        string today = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

                        archiveDelete(outTimeMessage);
                        message3 = dateCheck(aDateCheck,sDateCheck);

                        MySqlCommand search = new MySqlCommand();
                        search.Connection = globalConnection;
                        search.CommandText = "SELECT * FROM Students;";
                        search.ExecuteNonQuery();

                        MySqlDataAdapter da = new MySqlDataAdapter(search);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        var myData = dt.Select();

                        for (int i = 0; i < myData.Length; i++)
                        {
                            if (myData[i].ItemArray[1].ToString() == "FAKE")
                            {
                                Common_Functions.delete_FromRoom(globalConnection, myData[i].ItemArray[7].ToString());
                                MySqlCommand deleteFake = new MySqlCommand();
                                deleteFake.Connection = globalConnection;
                                deleteFake.CommandText = "DELETE FROM Students WHERE Name = 'FAKE';";
                                deleteFake.ExecuteNonQuery();

                                deleteFake.CommandText = "DELETE FROM OtherLiving WHERE Name = 'FAKE';";
                                deleteFake.ExecuteNonQuery();

                                continue;
                            }
                            if (Convert.ToDateTime(myData[i].ItemArray[8]) < Convert.ToDateTime(today))
                                fl1 = true;
                            if (Convert.ToDateTime(myData[i].ItemArray[13]) < Convert.ToDateTime(today))
                                fl2 = true;
                            if (Convert.ToBoolean(myData[i].ItemArray[15]) == true)
                                if (Convert.ToDateTime(myData[i].ItemArray[16]) <= Convert.ToDateTime(today)) 
                                    message4 = "Есть временно выселенные проживающие ожидающие заселения";
                        }

                        if (fl1 == false || fl2 == false || message4 == "")
                        {
                            search.CommandText = "SELECT * FROM OtherLiving;";
                            search.ExecuteNonQuery();

                            MySqlDataAdapter da1 = new MySqlDataAdapter(search);
                            DataTable dt1 = new DataTable();
                            da1.Fill(dt1);
                            var myData1 = dt1.Select();

                            for (int i = 0; i < myData1.Length; i++)
                            {
                                if (Convert.ToDateTime(myData1[i].ItemArray[6]) < Convert.ToDateTime(today))
                                    fl1 = true;
                                if (Convert.ToDateTime(myData1[i].ItemArray[9]) < Convert.ToDateTime(today))
                                    fl2 = true;
                                if(Convert.ToBoolean(myData1[i].ItemArray[13]) == true)
                                    if(Convert.ToDateTime(myData1[i].ItemArray[14]) <= Convert.ToDateTime(today))
                                        message4 = "Есть временно выселенные проживающие ожидающие заселения";
                                if (fl1 == true && fl2 == true && message4 != "")
                                    break;
                            }
                        }

                        //message3 = evictedCheck();

                        if (fl1 == true && fl2 == false && message3 == "" && message4 == "")
                            testLabel1.Content = message1;
                        if (fl1 == true && fl2 == false && message3 == "" && message4 != "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message4;
                        }
                        if (fl1 == false && fl2 == false && message3 != "" && message4 == "")
                            testLabel1.Content = message3;
                        if (fl1 == false && fl2 == false && message3 != "" && message4 != "")
                        {
                            testLabel1.Content = message3;
                            testLabel2.Content = message4;
                        }
                        if(fl1 == false && fl2 == true && message3 != "" && message4 == "")
                        {
                            testLabel1.Content = message2;
                            testLabel2.Content = message3;
                        }
                        if (fl1 == false && fl2 == true && message3 != "" && message4 != "")
                        {
                            testLabel1.Content = message2;
                            testLabel2.Content = message3;
                            testLabel3.Content = message4;
                        }
                        if(fl1 == true && fl2 == false && message3 != "" && message4 == "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message3;
                        }
                        if (fl1 == true && fl2 == false && message3 != "" && message4 != "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message3;
                            testLabel3.Content = message4;
                        }
                        if (fl1 == true && fl2 == true && message3 == "" && message4 == "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message2;
                        }
                        if (fl1 == true && fl2 == true && message3 == "" && message4 != "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message2;
                            testLabel3.Content = message4;
                        }
                        if(fl1 == true && fl2 == true && message3 != "" && message4 == "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message2;
                            testLabel3.Content = message3;
                        }
                        if (fl1 == true && fl2 == true && message3 != "" && message4 != "")
                        {
                            testLabel1.Content = message1;
                            testLabel2.Content = message2;
                            testLabel3.Content = message3;
                            testLabel4.Content = message4;
                        }
                        if (fl1 == false && fl2 == true && message3 == "" && message4 == "")
                            testLabel1.Content = message2;
                        if (fl1 == false && fl2 == true && message3 == "" && message4 != "")
                        {
                            testLabel1.Content = message2;
                            testLabel2.Content = message4;
                        }
                        if (fl1 == false && fl2 == false && message3 == "" && message4 != "")
                            testLabel1.Content = message4;
                        if (fl1 == false && fl2 == false && message3 == "" && message4 == "")
                            testLabel1.Content = "Сообщений нет";
                        DateTime todayCheckDate = DateTime.Now;
                        string cDate = todayCheckDate.ToString("yyyy-MM-dd H:mm:ss");

                        MySqlCommand insertDate = new MySqlCommand();
                        insertDate.Connection = globalConnection;
                        insertDate.CommandText = "UPDATE Settings SET LastCheckDate = '" + cDate + "' WHERE ID = 1;";
                        insertDate.ExecuteNonQuery();
                    } 
                };
            this.Dispatcher.Invoke(action);
        }

        /*private void backgroundWorker_DoWork(CancellationToken ct)
        {
            Action action = () =>
            {

                string message = "Есть проживающие, не оплатившие проживание";
                string today = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

                MySqlCommand searchNonPayedST = new MySqlCommand("SELECT * FROM Students WHERE Date < '" + today + "';", globalConnection);
                searchNonPayedST.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(searchNonPayedST);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var myData = dt.Select();

                if (myData.Length != 0) { testLabel1.Content = message; }
                else
                {
                    searchNonPayedST.CommandText = "SELECT * FROM OtherLiving WHERE Date < '" + today + "';";
                    searchNonPayedST.ExecuteNonQuery();

                    MySqlDataAdapter da1 = new MySqlDataAdapter(searchNonPayedST);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    var myData1 = dt1.Select();

                    if (myData1.Length != 0) { testLabel1.Content = message; }
                }
            };
            ct.ThrowIfCancellationRequested();
            this.Dispatcher.Invoke(action);
        }*/

      /*  private void tt(CancellationToken ct)
        {
            Action action = () =>
            {

                string message = "Есть проживающие, у которых закончился период проживания";
                string today = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

                MySqlCommand searchNonPayedST = new MySqlCommand("SELECT * FROM Students WHERE StayLimit < '" + today + "';", globalConnection);
                searchNonPayedST.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(searchNonPayedST);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var myData = dt.Select();

                if (myData.Length != 0) { if (testLabel1.Content.ToString() == "") testLabel1.Content = message; else testLabel2.Content = message; }
                else
                {
                    searchNonPayedST.CommandText = "SELECT * FROM OtherLiving WHERE StayLimit < '" + today + "';";
                    searchNonPayedST.ExecuteNonQuery();

                    MySqlDataAdapter da1 = new MySqlDataAdapter(searchNonPayedST);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    var myData1 = dt1.Select();

                    if (myData1.Length != 0) { if (testLabel1.Content.ToString() == "") testLabel1.Content = message; else testLabel2.Content = message; }
                }
                if (testLabel1.Content.ToString() == "" && testLabel2.Content.ToString() == "")
                {
                    testLabel1.Content = "Сообщений нет";
                }
            };
            ct.ThrowIfCancellationRequested();
            this.Dispatcher.Invoke(action);
        }*/

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Run(() => backgroundWorker_DoWork(5));  
            if (testLabel1.Content.ToString() == "")
            {
                Task t = Task.Run(() => initionalTest());
                //Task cwt = t.ContinueWith(task => tt(CancellationToken.None));
            }

            switch(access)
            {
                case 1: admin_Functions.Visibility = Visibility.Visible; break;
                case 2: admin_Functions.Visibility = Visibility.Hidden; break;
            }
        }

        private void search_Student_Click(object sender, RoutedEventArgs e)
        {
            studentSerch search_st = new studentSerch(globalConnection);
            this.NavigationService.Navigate(search_st);
        }

        private void livInfo_Click(object sender, RoutedEventArgs e)
        {
            livingInfoPage infoPage = new livingInfoPage(globalConnection);
            this.NavigationService.Navigate(infoPage);
        }

        private void roomInfo_Click(object sender, RoutedEventArgs e)
        {
            roomInfoPage infoPage = new roomInfoPage(globalConnection);
            this.NavigationService.Navigate(infoPage);
        }

        private void testLabel1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int k = 0;

            if (testLabel1.Content.ToString() == "Есть проживающие, у которых закончился период проживания")
                k = 2;
            if (testLabel1.Content.ToString() == "Есть проживающие, не оплатившие проживание")
                k = 1;
            if (testLabel1.Content.ToString() == "Есть временно выселенные проживающие ожидающие заселения")
                k = 3;
            studentSerch search_st = new studentSerch(globalConnection, k);
            this.NavigationService.Navigate(search_st);
        }

        private void testLabel2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int k = 0;
            if (testLabel2.Content.ToString() == "Есть проживающие, у которых закончился период проживания")
                k = 2;
            if (testLabel2.Content.ToString() == "Есть временно выселенные проживающие ожидающие заселения")
                k = 3;
            studentSerch search_st = new studentSerch(globalConnection, k);
            this.NavigationService.Navigate(search_st);
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsPage w1 = new settingsPage(globalConnection);
            this.NavigationService.Navigate(w1);
        }

        private void reports_Click(object sender, RoutedEventArgs e)
        {
            reportsPage w1 = new reportsPage(globalConnection);
            this.NavigationService.Navigate(w1);
        }

        private void testLabel3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int k = 0;
            if (testLabel3.Content.ToString() == "Есть временно выселенные проживающие ожидающие заселения")
                k = 3;
            studentSerch search_st = new studentSerch(globalConnection, k);
            this.NavigationService.Navigate(search_st);
        }

        private void testLabel4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int k = 0;
            if (testLabel3.Content.ToString() == "Есть временно выселенные проживающие ожидающие заселения")
                k = 3;
            studentSerch search_st = new studentSerch(globalConnection, k);
            this.NavigationService.Navigate(search_st);
        }
    }

 /*   public class theradStart
    {

        public MySqlConnection globalConnection;
        public string[] messages;
        public int messageCounter = 0;

        public theradStart(MySqlConnection connection)
        {
            globalConnection = connection;
        }

        public void dbCheck()
        {
            string tooday = DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss");
            Action _searchNonPayedAction = () =>
            {
                string message = "Есть проживающие, не оплатившие проживание";

                MySqlCommand searchNonPayedST = new MySqlCommand("SELECT * FROM Students WHERE Date < '" + tooday + "';", globalConnection);
                searchNonPayedST.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(searchNonPayedST);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var myData = dt.Select();

                if (myData.Length != 0) { _nonPayed(message); }
                else
                {
                    searchNonPayedST.CommandText = "SELECT * FROM OtherLiving WHERE Date < '" + tooday + "';";
                    searchNonPayedST.ExecuteNonQuery();

                    MySqlDataAdapter da1 = new MySqlDataAdapter(searchNonPayedST);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    var myData1 = dt1.Select();

                    if (myData1.Length != 0) { _nonPayed(message); }
                }
            };
        }

        public event Action<string> _nonPayed;
    }*/
}
