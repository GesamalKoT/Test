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
    /// Логика взаимодействия для studentSerch.xaml
    /// </summary>
    /// 



    public partial class studentSerch : Page
    {
        public MySqlConnection globalConnection;
        public int livingSearch = -1;
        public int searchClass = 0;

        public studentSerch()
        {
            InitializeComponent();
        }

        public studentSerch(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
            bnd.bindCombobox(Citizenship, globalConnection);
        }

        public studentSerch(MySqlConnection connection, int sc_Cl)
        {
            globalConnection = connection;
            searchClass = sc_Cl;
            InitializeComponent();
            bnd.bindCombobox(Citizenship, globalConnection);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearFilters.Visibility = Visibility.Visible;
            switch(Search.SelectedIndex)
            {
                case 0: livingParam.Visibility = Visibility.Visible; livingTable.Visibility = Visibility.Visible ; roomParam.Visibility = Visibility.Hidden; livingPrepare(); break;
                case 1: livingParam.Visibility = Visibility.Hidden; roomPrepare(); break;
                case 2: livingParam.Visibility = Visibility.Hidden; roomParam.Visibility = Visibility.Hidden; arcTable.Visibility = Visibility.Visible; archivePrepare(); break;
            }
        }

        public void livingPrepare()
        {
            CheckBox1.Visibility = Visibility.Visible;
            CheckBox2.Visibility = Visibility.Visible;

            LivingLabel1.Visibility = Visibility.Visible;
            livingLabel2.Visibility = Visibility.Visible;

            livingLabel3.Visibility = Visibility.Visible;
            livingLabel4.Visibility = Visibility.Visible;

            studentFac.Visibility = Visibility.Visible;
            Form.Visibility = Visibility.Visible;

            roomLable1.Visibility = Visibility.Hidden;
            roomLabel2.Visibility = Visibility.Hidden;
            roomLabel3.Visibility = Visibility.Hidden;

            stayLimitLabel1.Visibility = Visibility.Hidden;
            stayLimitLabel2.Visibility = Visibility.Hidden;
            stayLimitComboBox.Visibility = Visibility.Hidden;
            stayLimitLabel3.Visibility = Visibility.Hidden;

            checkBox3.Visibility = Visibility.Hidden;

            livingTable.Visibility = Visibility.Visible;
            livingTable.ItemsSource = LivingItem.showAllLiving(globalConnection);
            evictedP.Visibility = Visibility.Visible;

            type.Visibility = Visibility.Hidden;
            roomParam.Visibility = Visibility.Hidden;
            

            roomLabel4.Visibility = Visibility.Hidden;
            roomPlaces.Visibility = Visibility.Hidden;

            roomTable.Visibility = Visibility.Hidden;

            arcTable.Visibility = Visibility.Hidden;
            arcParam.Visibility = Visibility.Hidden;

            arcDate1.Visibility = Visibility.Hidden;
            arcDate2.Visibility = Visibility.Hidden;
            arcLabel1.Visibility = Visibility.Hidden;
            arcLabel2.Visibility = Visibility.Hidden;

            serchString.Visibility = Visibility.Hidden;
            Citizenship.Visibility = Visibility.Hidden;
            livingParam.SelectedIndex = -1;
        }

        public void roomPrepare()
        {
            livingTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Visible;
            arcTable.Visibility = Visibility.Hidden;

            LivingLabel1.Visibility = Visibility.Hidden;
            livingLabel2.Visibility = Visibility.Hidden;
            livingLabel3.Visibility = Visibility.Hidden;
            livingLabel4.Visibility = Visibility.Hidden;

            stayLimitComboBox.Visibility = Visibility.Hidden;
            stayLimitLabel1.Visibility = Visibility.Hidden;
            stayLimitLabel2.Visibility = Visibility.Hidden;
            stayLimitLabel3.Visibility = Visibility.Hidden;

            roomLable1.Visibility = Visibility.Visible;
            roomLabel2.Visibility = Visibility.Visible;

            CheckBox1.Visibility = Visibility.Visible;
            CheckBox2.Visibility = Visibility.Visible;

            //type.Visibility = Visibility.Visible;
            roomParam.Visibility = Visibility.Visible;
            roomParam.SelectedIndex = -1;

            roomTable.ItemsSource = RoomItem.getItems(globalConnection);

            studentFac.Visibility = Visibility.Hidden;
            Form.Visibility = Visibility.Hidden;

            checkBox3.Visibility = Visibility.Visible;

            roomLabel3.Visibility = Visibility.Visible;
            arcParam.Visibility = Visibility.Hidden;

            arcDate1.Visibility = Visibility.Hidden;
            arcDate2.Visibility = Visibility.Hidden;
            arcLabel1.Visibility = Visibility.Hidden;
            arcLabel2.Visibility = Visibility.Hidden;

            evictedP.Visibility = Visibility.Hidden;

            serchString.Visibility = Visibility.Hidden;
            Citizenship.Visibility = Visibility.Hidden;
            roomParam.SelectedIndex = -1;
        }

        public void archivePrepare()
        {
            livingTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            arcTable.Visibility = Visibility.Visible;

            LivingLabel1.Visibility = Visibility.Hidden;
            livingLabel2.Visibility = Visibility.Hidden;
            livingLabel3.Visibility = Visibility.Hidden;
            livingLabel4.Visibility = Visibility.Hidden;

            stayLimitComboBox.Visibility = Visibility.Hidden;
            stayLimitLabel1.Visibility = Visibility.Hidden;
            stayLimitLabel2.Visibility = Visibility.Hidden;
            stayLimitLabel3.Visibility = Visibility.Hidden;

            roomLable1.Visibility = Visibility.Hidden;
            roomLabel2.Visibility = Visibility.Hidden;

            CheckBox1.Visibility = Visibility.Hidden;
            CheckBox2.Visibility = Visibility.Hidden;

            evictedP.Visibility = Visibility.Hidden;

            //type.Visibility = Visibility.Visible;
            roomParam.Visibility = Visibility.Hidden;
            roomParam.SelectedIndex = -1;

            studentFac.Visibility = Visibility.Hidden;
            Form.Visibility = Visibility.Hidden;

            checkBox3.Visibility = Visibility.Hidden;

            roomLabel3.Visibility = Visibility.Hidden;

            arcTable.ItemsSource = archive.viewArchive(globalConnection);
            arcParam.Visibility = Visibility.Visible;

            arcDate1.Visibility = Visibility.Hidden;
            arcDate2.Visibility = Visibility.Hidden;
            arcLabel1.Visibility = Visibility.Hidden;
            arcLabel2.Visibility = Visibility.Hidden;

            serchString.Visibility = Visibility.Hidden;
            Citizenship.Visibility = Visibility.Hidden;
            arcParam.SelectedIndex = -1;
        }

        private void showCombobox()
        {
            CheckBox1.Visibility = Visibility.Visible;
            CheckBox2.Visibility = Visibility.Visible;

            LivingLabel1.Visibility = Visibility.Visible;
            livingLabel2.Visibility = Visibility.Visible;

        }

        private void hideCombobox()
        {
            CheckBox1.Visibility = Visibility.Hidden;
            CheckBox2.Visibility = Visibility.Hidden;

            LivingLabel1.Visibility = Visibility.Hidden;
            livingLabel2.Visibility = Visibility.Hidden;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (searchClass == 0)
            {
                serchString.Visibility = Visibility.Hidden;

                CheckBox1.Visibility = Visibility.Hidden;
                CheckBox2.Visibility = Visibility.Hidden;

                LivingLabel1.Visibility = Visibility.Hidden;
                livingLabel2.Visibility = Visibility.Hidden;
                livingLabel3.Visibility = Visibility.Hidden;
                livingLabel4.Visibility = Visibility.Hidden;

                livingParam.Visibility = Visibility.Hidden;
                roomParam.Visibility = Visibility.Hidden;

                Citizenship.Visibility = Visibility.Hidden;

                studentFac.Visibility = Visibility.Hidden;
                Form.Visibility = Visibility.Hidden;

                livingTable.Visibility = Visibility.Hidden;
                clearFilters.Visibility = Visibility.Hidden;

                roomLable1.Visibility = Visibility.Hidden;
                roomLabel2.Visibility = Visibility.Hidden;

                roomTable.Visibility = Visibility.Hidden;
                type.Visibility = Visibility.Hidden;

                checkBox3.Visibility = Visibility.Hidden;
                roomLabel3.Visibility = Visibility.Hidden;

                roomLabel4.Visibility = Visibility.Hidden;
                roomPlaces.Visibility = Visibility.Hidden;

                arcTable.Visibility = Visibility.Hidden;
                arcParam.Visibility = Visibility.Hidden;

                stayLimitLabel1.Visibility = Visibility.Hidden;
                stayLimitLabel2.Visibility = Visibility.Hidden;
                stayLimitComboBox.Visibility = Visibility.Hidden;
                stayLimitLabel3.Visibility = Visibility.Hidden;

                evictedP.Visibility = Visibility.Hidden;

                arcDate1.Visibility = Visibility.Hidden;
                arcDate2.Visibility = Visibility.Hidden;
                arcLabel1.Visibility = Visibility.Hidden;
                arcLabel2.Visibility = Visibility.Hidden;
            }

            if(searchClass == 1)
            {
                serchString.Visibility = Visibility.Hidden;

                CheckBox1.Visibility = Visibility.Visible;
                CheckBox2.Visibility = Visibility.Visible;

                LivingLabel1.Visibility = Visibility.Visible;
                livingLabel2.Visibility = Visibility.Visible;
                livingLabel3.Visibility = Visibility.Visible;
                livingLabel4.Visibility = Visibility.Visible;

                livingParam.Visibility = Visibility.Visible;
                roomParam.Visibility = Visibility.Hidden;

                Citizenship.Visibility = Visibility.Hidden;

                studentFac.Visibility = Visibility.Visible;
                Form.Visibility = Visibility.Visible;

                livingTable.Visibility = Visibility.Visible;
                clearFilters.Visibility = Visibility.Visible;

                roomLable1.Visibility = Visibility.Hidden;
                roomLabel2.Visibility = Visibility.Hidden;

                roomTable.Visibility = Visibility.Hidden;
                type.Visibility = Visibility.Hidden;

                checkBox3.Visibility = Visibility.Hidden;
                roomLabel3.Visibility = Visibility.Hidden;

                roomLabel4.Visibility = Visibility.Hidden;
                roomPlaces.Visibility = Visibility.Hidden;

                arcTable.Visibility = Visibility.Hidden;
                arcParam.Visibility = Visibility.Hidden;

                stayLimitLabel1.Visibility = Visibility.Hidden;
                stayLimitLabel2.Visibility = Visibility.Hidden;
                stayLimitComboBox.Visibility = Visibility.Hidden;
                stayLimitLabel3.Visibility = Visibility.Hidden;

                arcDate1.Visibility = Visibility.Hidden;
                arcDate2.Visibility = Visibility.Hidden;
                arcLabel1.Visibility = Visibility.Hidden;
                arcLabel2.Visibility = Visibility.Hidden;

                evictedP.Visibility = Visibility.Visible;
                evictedP.IsEnabled = false;

                CheckBox1.IsChecked = true;
                Search.SelectedIndex = 0;
                livingParam.SelectedIndex = -1;
                livingTable.ItemsSource = SearchFunctions.commonSearch(globalConnection, true, false, "", "", false);
            }

            if(searchClass == 2)
            {
                serchString.Visibility = Visibility.Hidden;

                CheckBox1.Visibility = Visibility.Visible;
                CheckBox2.Visibility = Visibility.Visible;

                LivingLabel1.Visibility = Visibility.Hidden;
                livingLabel2.Visibility = Visibility.Visible;
                livingLabel3.Visibility = Visibility.Visible;
                livingLabel4.Visibility = Visibility.Visible;

                livingParam.Visibility = Visibility.Visible;
                roomParam.Visibility = Visibility.Hidden;

                Citizenship.Visibility = Visibility.Hidden;

                studentFac.Visibility = Visibility.Visible;
                Form.Visibility = Visibility.Visible;

                livingTable.Visibility = Visibility.Visible;
                clearFilters.Visibility = Visibility.Visible;

                roomLable1.Visibility = Visibility.Hidden;
                roomLabel2.Visibility = Visibility.Hidden;

                roomTable.Visibility = Visibility.Hidden;
                type.Visibility = Visibility.Hidden;

                checkBox3.Visibility = Visibility.Hidden;
                roomLabel3.Visibility = Visibility.Hidden;

                roomLabel4.Visibility = Visibility.Hidden;
                roomPlaces.Visibility = Visibility.Hidden;

                arcTable.Visibility = Visibility.Hidden;
                arcParam.Visibility = Visibility.Hidden;

                stayLimitLabel1.Visibility = Visibility.Visible;
                stayLimitLabel2.Visibility = Visibility.Visible;
                stayLimitComboBox.Visibility = Visibility.Visible;
                stayLimitLabel3.Visibility = Visibility.Visible;

                arcDate1.Visibility = Visibility.Hidden;
                arcDate2.Visibility = Visibility.Hidden;
                arcLabel1.Visibility = Visibility.Hidden;
                arcLabel2.Visibility = Visibility.Hidden;

                evictedP.Visibility = Visibility.Visible;

                stayLimitComboBox.IsEnabled = false;
                CheckBox1.IsChecked = true;
                Search.SelectedIndex = 0;
                livingParam.SelectedIndex = 6;
                livingTable.ItemsSource = SearchFunctions.search_byStayLimit(globalConnection, "", true, false, "", "");
            }

            if(searchClass == 3)
            {
                serchString.Visibility = Visibility.Hidden;

                CheckBox1.Visibility = Visibility.Visible;
                CheckBox2.Visibility = Visibility.Visible;

                LivingLabel1.Visibility = Visibility.Visible;
                livingLabel2.Visibility = Visibility.Visible;
                livingLabel3.Visibility = Visibility.Visible;
                livingLabel4.Visibility = Visibility.Visible;

                livingParam.Visibility = Visibility.Visible;
                roomParam.Visibility = Visibility.Hidden;

                Citizenship.Visibility = Visibility.Hidden;

                studentFac.Visibility = Visibility.Visible;
                Form.Visibility = Visibility.Visible;

                livingTable.Visibility = Visibility.Visible;
                clearFilters.Visibility = Visibility.Visible;

                roomLable1.Visibility = Visibility.Hidden;
                roomLabel2.Visibility = Visibility.Hidden;

                roomTable.Visibility = Visibility.Hidden;
                type.Visibility = Visibility.Hidden;

                checkBox3.Visibility = Visibility.Hidden;
                roomLabel3.Visibility = Visibility.Hidden;

                roomLabel4.Visibility = Visibility.Hidden;
                roomPlaces.Visibility = Visibility.Hidden;

                arcTable.Visibility = Visibility.Hidden;
                arcParam.Visibility = Visibility.Hidden;

                stayLimitLabel1.Visibility = Visibility.Hidden;
                stayLimitLabel2.Visibility = Visibility.Hidden;
                stayLimitComboBox.Visibility = Visibility.Hidden;
                stayLimitLabel3.Visibility = Visibility.Hidden;

                arcDate1.Visibility = Visibility.Hidden;
                arcDate2.Visibility = Visibility.Hidden;
                arcLabel1.Visibility = Visibility.Hidden;
                arcLabel2.Visibility = Visibility.Hidden;

                evictedP.Visibility = Visibility.Visible;
                evictedP.IsEnabled = true;
                evictedP.IsChecked = true;

                CheckBox1.IsChecked = false;
                Search.SelectedIndex = 0;
                livingParam.SelectedIndex = -1;
                livingTable.ItemsSource = SearchFunctions.search_exEvicted(globalConnection);
                //livingTable.ItemsSource = SearchFunctions.commonSearch(globalConnection, true, false, "", "", false);
            }
        }

        private void livingParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(livingParam.SelectedIndex)
            {
                case (0): clearFilter(); serchString.Visibility = Visibility.Visible; Citizenship.Visibility = Visibility.Hidden; hideCombobox(); LivingLabel1.Visibility = Visibility.Hidden; CheckBox1.Visibility = Visibility.Hidden; livingLabel3.Visibility = Visibility.Hidden; studentFac.Visibility = Visibility.Hidden; livingLabel4.Visibility = Visibility.Hidden; Form.Visibility = Visibility.Hidden; livingSearch = 0; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Hidden; break;
                case (1): clearFilter(); serchString.Visibility = Visibility.Visible; Citizenship.Visibility = Visibility.Hidden; showCombobox(); livingLabel2.Visibility = Visibility.Hidden; CheckBox2.Visibility = Visibility.Hidden; livingLabel3.Visibility = Visibility.Visible; studentFac.Visibility = Visibility.Visible; livingLabel4.Visibility = Visibility.Visible; Form.Visibility = Visibility.Visible; livingSearch = 1; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Visible; break;
                case (2): clearFilter(); serchString.Visibility = Visibility.Visible; Citizenship.Visibility = Visibility.Hidden; hideCombobox(); livingLabel3.Visibility = Visibility.Hidden; studentFac.Visibility = Visibility.Hidden; livingLabel4.Visibility = Visibility.Hidden; Form.Visibility = Visibility.Hidden; livingSearch = 2; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Hidden; break;
                case (3): clearFilter(); serchString.Visibility = Visibility.Visible; Citizenship.Visibility = Visibility.Hidden; hideCombobox(); livingLabel3.Visibility = Visibility.Hidden; studentFac.Visibility = Visibility.Hidden; livingLabel4.Visibility = Visibility.Hidden; Form.Visibility = Visibility.Hidden; livingSearch = 3; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Hidden; break;
                case (4): clearFilter(); Citizenship.Visibility = Visibility.Visible; serchString.Visibility = Visibility.Hidden; showCombobox(); livingLabel3.Visibility = Visibility.Visible; studentFac.Visibility = Visibility.Visible; livingLabel4.Visibility = Visibility.Visible; Form.Visibility = Visibility.Visible; livingSearch = 4; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Visible; break;
                case (5): clearFilter(); serchString.Visibility = Visibility.Visible; Citizenship.Visibility = Visibility.Hidden; hideCombobox(); livingLabel3.Visibility = Visibility.Hidden; studentFac.Visibility = Visibility.Hidden; livingLabel4.Visibility = Visibility.Hidden; Form.Visibility = Visibility.Hidden; livingSearch = 5; stayLimitComboBox.Visibility = Visibility.Hidden; stayLimitLabel1.Visibility = Visibility.Hidden; stayLimitLabel2.Visibility = Visibility.Hidden; stayLimitLabel3.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Hidden; break;
                case (6): clearFilter(); serchString.Visibility = Visibility.Hidden; Citizenship.Visibility = Visibility.Hidden; showCombobox(); livingLabel3.Visibility = Visibility.Visible; studentFac.Visibility = Visibility.Visible; livingLabel4.Visibility = Visibility.Visible; Form.Visibility = Visibility.Visible; livingSearch = 6; stayLimitComboBox.Visibility = Visibility.Visible; stayLimitLabel1.Visibility = Visibility.Visible; stayLimitLabel2.Visibility = Visibility.Visible; stayLimitLabel3.Visibility = Visibility.Visible; LivingLabel1.Visibility = Visibility.Hidden; evictedP.Visibility = Visibility.Hidden; break; 
            }
        }

        public void searchButton_Click(object sender, RoutedEventArgs e)
        {
            switch(Search.SelectedIndex)
            {
                case (0): livingSerch(); break;
                case (1): roomSearch(); break;
                case (2): arcSearch(); break;
            }
        }

        private void livingSerch()
        {
            switch(livingParam.SelectedIndex)
            {
                case (-1): commonSearch(); break;
                case (0): livingTable.ItemsSource = SearchFunctions.search_byFIOall(globalConnection, serchString.Text, (bool)CheckBox1.IsChecked); break;
                case (1): courseSearch(); break;
                case (2): try { livingTable.ItemsSource = SearchFunctions.search_ByID(globalConnection, serchString.Text.ToString()); }
                    catch { MessageBox.Show("Неверный формат номера студенческого билета");} break;
                case (3): livingTable.ItemsSource = SearchFunctions.search_ByRoomNumber(globalConnection, serchString.Text.ToString()); break;
                case (4): if (Citizenship.SelectedIndex != -1) citizenSearch(); break;
                case (5): livingTable.ItemsSource = SearchFunctions.search_ByDecreeNumber(globalConnection, serchString.Text.ToString()); break;
                case (6): searchByStayLimit(); break;
            }
        }

        private void searchByStayLimit()
        {
            string faculty = "";
            string form = "";
            string sl = "";

            if(stayLimitComboBox.SelectedIndex != -1)
                sl = stayLimitComboBox.SelectedValue.ToString();
            if (studentFac.SelectedIndex != -1)
                faculty = studentFac.SelectedValue.ToString();
            if (Form.SelectedIndex != -1)
                form = Form.SelectedValue.ToString();

            livingTable.ItemsSource = SearchFunctions.search_byStayLimit(globalConnection, sl, (bool)CheckBox1.IsChecked, (bool)CheckBox2.IsChecked, faculty, form);
        }

        private void roomSearch()
        {
            switch(roomParam.SelectedIndex)
            {
                case (-1): roomTable.ItemsSource = SearchFunctions.commonRoomSearch(globalConnection, (bool)CheckBox1.IsChecked, (bool)CheckBox2.IsChecked, (bool)checkBox3.IsChecked); break;
                case (0): roomTable.ItemsSource = SearchFunctions.searchRoomByNumber(globalConnection, serchString.Text); break;
                case (1): roomTable.ItemsSource = SearchFunctions.searchRoom_ByPlaces(globalConnection, serchString.Text, (bool)CheckBox1.IsChecked, (bool)CheckBox2.IsChecked, (bool)checkBox3.IsChecked); break;
                case (2): roomTable.ItemsSource = SearchFunctions.searchRoom_ByFplaces(globalConnection, serchString.Text, fPlaces()); break;
                case (3): if (type.SelectedIndex != -1) roomTypeSearch(); break;
            }
        }

        private void arcSearch()
        {
            switch(arcParam.SelectedIndex)
            {
                case (-1): arcTable.ItemsSource = archive.viewArchive(globalConnection); break;
                case (0): if (serchString.Text != "") arcTable.ItemsSource = archive.search_ByFIO(globalConnection, serchString.Text); break;
                case (1): if (serchString.Text != "") arcTable.ItemsSource = archive.search_ByID(globalConnection, serchString.Text); break;
                case (2): arcDateSearch(); break;
                case (3): if (serchString.Text != "") arcTable.ItemsSource = archive.search_ByDecree(globalConnection, serchString.Text); break;
            }
        }
        
        private void arcDateSearch()
        {
            DateTime first;
            DateTime second;

            if (arcDate1.SelectedDate == null)
                first = DateTime.MinValue;
            else
                first = (DateTime)arcDate1.SelectedDate;

            if (arcDate2.SelectedDate == null)
                second = DateTime.MaxValue;
            else
                second = (DateTime)arcDate2.SelectedDate;
            arcTable.ItemsSource = archive.search_ByDate(globalConnection, first, second);
        }

        private void roomTypeSearch()
        {
            string t = "";

            switch(type.SelectedValue.ToString())
            {
                case ("Мужская"): t = "m"; break;
                case ("Женская"): t = "f"; break;
                case ("Семейная"): t = "c"; break;
            }

            roomTable.ItemsSource = SearchFunctions.searchRoom_ByType(globalConnection, t, (bool)CheckBox2.IsChecked, (bool)checkBox3.IsChecked);
        }

        private string fPlaces()
        {
            string s = "";
            if (roomPlaces.SelectedIndex == -1)
                s = "0";
            else
                s = roomPlaces.SelectedValue.ToString();

            return s;
        }

        private void citizenSearch()
        {
            string faculty = "";
            string form = "";

            if (studentFac.SelectedValue != null)
                faculty = studentFac.SelectedValue.ToString();
            if (Form.SelectedValue != null)
                form = Form.SelectedValue.ToString();

            livingTable.ItemsSource = SearchFunctions.search_ByCitizenship(globalConnection, SearchFunctions.convertToFullCitizenship(globalConnection, Citizenship.Text.ToString()), (bool)CheckBox1.IsChecked, (bool)CheckBox2.IsChecked, faculty, form, (bool)evictedP.IsChecked);
        }

        private void commonSearch()
        {
            string faculty = "";
            string form = "";

            if (studentFac.SelectedValue != null)
                faculty = studentFac.SelectedValue.ToString();
            if (Form.SelectedValue != null)
                form = Form.SelectedValue.ToString();

            livingTable.ItemsSource = SearchFunctions.commonSearch(globalConnection, (bool)CheckBox1.IsChecked, (bool)CheckBox2.IsChecked, faculty, form, (bool)evictedP.IsChecked);
        }

        private void courseSearch()
        {
            string faculty = "";
            string form = "";

            if (studentFac.SelectedValue != null)
                faculty = studentFac.SelectedValue.ToString();
            if (Form.SelectedValue != null)
                form = Form.SelectedValue.ToString();

            livingTable.ItemsSource = SearchFunctions.search_byCourse(globalConnection, serchString.Text.ToString(), (bool)CheckBox1.IsChecked, faculty, form, (bool)evictedP.IsChecked);
        }

        private void CheckBox2_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBox2.IsChecked)
            {
                studentFac.IsEnabled = false;
                Form.IsEnabled = false;
                studentFac.SelectedIndex = -1;
                Form.SelectedIndex = -1;
                checkBox3.IsChecked = false;
            }
            else
            {
                studentFac.IsEnabled = true;
                Form.IsEnabled = true;
            }

        }

        private void clearFilters_Click(object sender, RoutedEventArgs e)
        {
            clearFilter();
        }

        private void clearFilter()
        {
            serchString.Text = "";
            CheckBox1.IsChecked = false;
            CheckBox2.IsChecked = false;

            Citizenship.SelectedIndex = -1;

            studentFac.SelectedIndex = -1;
            studentFac.IsEnabled = true;

            Form.SelectedIndex = -1;
            Form.IsEnabled = true;

            stayLimitComboBox.SelectedIndex = -1;

            checkBox3.IsChecked = false;
            roomPlaces.SelectedIndex = -1;
            Citizenship.SelectedIndex = -1;

            type.SelectedIndex = -1;

            stayLimitComboBox.IsEnabled = true;
            evictedP.IsEnabled = true;
            evictedP.IsChecked = false;
        }

        private void livingTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selST = livingTable.SelectedItem;
                string _selST = ((LivingItem)selST).id;

                studentInfo w1 = new studentInfo(globalConnection, _selST, this);
                w1.ShowDialog();
            }
            catch
            { }
        }

        private void checkBox3_Click(object sender, RoutedEventArgs e)
        {
            if((bool)checkBox3.IsChecked)
            {
                CheckBox1.IsChecked = false;
                CheckBox2.IsChecked = false;
            }
        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBox1.IsChecked)
            {
                checkBox3.IsChecked = false;
                stayLimitComboBox.IsEnabled = false;
                evictedP.IsEnabled = false;
            }
            else
            {
                stayLimitComboBox.IsEnabled = true;
                evictedP.IsEnabled = true;
            }
        }

        private void roomParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(roomParam.SelectedIndex)
            {
                case (-1): clearFilter(); type.Visibility = Visibility.Hidden; CheckBox1.Visibility = Visibility.Visible; roomLable1.Visibility = Visibility.Visible; CheckBox2.Visibility = Visibility.Visible; roomLabel2.Visibility = Visibility.Visible; checkBox3.Visibility = Visibility.Visible; roomLabel3.Visibility = Visibility.Visible; serchString.Visibility = Visibility.Hidden; roomLabel4.Visibility = Visibility.Hidden; roomPlaces.Visibility = Visibility.Hidden; break;
                case (0): clearFilter(); type.Visibility = Visibility.Hidden; CheckBox1.Visibility = Visibility.Hidden; roomLable1.Visibility = Visibility.Hidden; CheckBox2.Visibility = Visibility.Hidden; roomLabel2.Visibility = Visibility.Hidden; checkBox3.Visibility = Visibility.Hidden; roomLabel3.Visibility = Visibility.Hidden; serchString.Visibility = Visibility.Visible; roomLabel4.Visibility = Visibility.Hidden; roomPlaces.Visibility = Visibility.Hidden; break;
                case (1): clearFilter(); type.Visibility = Visibility.Hidden; CheckBox1.Visibility = Visibility.Visible; roomLable1.Visibility = Visibility.Visible; CheckBox2.Visibility = Visibility.Visible; roomLabel2.Visibility = Visibility.Visible; checkBox3.Visibility = Visibility.Visible; roomLabel3.Visibility = Visibility.Visible; serchString.Visibility = Visibility.Visible; roomLabel4.Visibility = Visibility.Hidden; roomPlaces.Visibility = Visibility.Hidden; break;
                case (2): clearFilter(); type.Visibility = Visibility.Hidden; CheckBox1.Visibility = Visibility.Hidden; roomLable1.Visibility = Visibility.Hidden; CheckBox2.Visibility = Visibility.Hidden; roomLabel2.Visibility = Visibility.Hidden; checkBox3.Visibility = Visibility.Hidden; roomLabel3.Visibility = Visibility.Hidden; serchString.Visibility = Visibility.Visible; roomLabel4.Visibility = Visibility.Visible; roomPlaces.Visibility = Visibility.Visible; break;
                case (3): clearFilter(); type.Visibility = Visibility.Visible; CheckBox1.Visibility = Visibility.Hidden; roomLable1.Visibility = Visibility.Hidden; CheckBox2.Visibility = Visibility.Visible; roomLabel2.Visibility = Visibility.Visible; checkBox3.Visibility = Visibility.Visible; roomLabel3.Visibility = Visibility.Visible; roomLabel4.Visibility = Visibility.Hidden; roomPlaces.Visibility = Visibility.Hidden; serchString.Visibility = Visibility.Hidden; break;
            }
        }

        private void stayLimitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stayLimitComboBox.SelectedIndex != -1)
            {
                CheckBox1.IsEnabled = false;
                CheckBox1.IsChecked = false;
            }
            else
                CheckBox1.IsEnabled = true;
        }

        private void arcParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(arcParam.SelectedIndex)
            {
                case (-1): clearFilter(); serchString.Visibility = Visibility.Hidden; arcLabel2.Visibility = Visibility.Hidden; arcLabel1.Visibility = Visibility.Hidden; arcDate1.Visibility = Visibility.Hidden; arcDate2.Visibility = Visibility.Hidden; break;
                case (0): clearFilter(); serchString.Visibility = Visibility.Visible; arcLabel2.Visibility = Visibility.Hidden; arcLabel1.Visibility = Visibility.Hidden; arcDate1.Visibility = Visibility.Hidden; arcDate2.Visibility = Visibility.Hidden; break;
                case (1): clearFilter(); serchString.Visibility = Visibility.Visible; arcLabel2.Visibility = Visibility.Hidden; arcLabel1.Visibility = Visibility.Hidden; arcDate1.Visibility = Visibility.Hidden; arcDate2.Visibility = Visibility.Hidden; break;
                case (2): clearFilter(); serchString.Visibility = Visibility.Hidden; arcLabel2.Visibility = Visibility.Visible; arcLabel1.Visibility = Visibility.Visible; arcDate1.Visibility = Visibility.Visible; arcDate2.Visibility = Visibility.Visible; break;
                case (3): clearFilter(); serchString.Visibility = Visibility.Visible; arcLabel2.Visibility = Visibility.Hidden; arcLabel1.Visibility = Visibility.Hidden; arcDate1.Visibility = Visibility.Hidden; arcDate2.Visibility = Visibility.Hidden; break;
            }
        }

        private void arcDate2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (arcDate1.SelectedDate == null)
                arcDate1.SelectedDate = arcDate2.SelectedDate;
        }

        private void arcTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selST = arcTable.SelectedItem;
                string _selST = ((archive)selST).id;

                arcInfo w1 = new arcInfo(globalConnection, _selST);
                w1.ShowDialog();
            }
            catch
            { }
        }

        private void evictedP_Click(object sender, RoutedEventArgs e)
        {
            if (evictedP.IsChecked == true)
            {
                CheckBox1.IsEnabled = false;
                //CheckBox2.IsEnabled = false;
            }
            else
            {
                CheckBox1.IsEnabled = true;
                //CheckBox2.IsEnabled = true;
            }
        }
    }
}
