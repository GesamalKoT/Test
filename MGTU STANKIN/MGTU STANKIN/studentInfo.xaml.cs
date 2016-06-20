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
    /// Логика взаимодействия для studentInfo.xaml
    /// </summary>
    public partial class studentInfo : Window
    {
        public MySqlConnection globalConnection;
        public string studentID;
        public string gender = "";
        public studentSerch page;
        public string type = "";

        public studentInfo()
        {
            InitializeComponent();
        }
        
        public studentInfo(MySqlConnection connection, string id, studentSerch s)
        {
            globalConnection = connection;
            studentID = id;
            page = s;
            InitializeComponent();
            bnd.bindCombobox(citizenship, globalConnection);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] fill = SearchFunctions.fill_ID(globalConnection, studentID);
            selectRoom.Visibility = Visibility.Hidden;

            switch(fill[0])
            {
                case "S": fillST(fill); type = "S"; break;
                case "O": fillOL(fill); type = "O"; break;
                case "SE": fill_e_ST(fill); type = "SE"; break;
                case "OE": fill_e_OL(fill); type = "OE"; break;
            }

            /*if (fill[0] == "S")
            {
                fillST(fill);
                type = "S";
            }
            else
                if (fill[0] == "O")
                {
                    fillOL(fill);
                    type = "O";
                }*/
        }

        public void clos()
        {
            
        }

        private void fillST(string[] res)
        {
            infoLabel.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Hidden;
            faculty.IsEnabled = true;
            course.IsEnabled = true;
            form.IsEnabled = true;
            id.IsEnabled = true;

            gender = res[5];

            surname.Text = res[1];
            name.Text = res[2];
            patronymic.Text = res[3];
            id.Text = res[4];
            course.Text = res[7];
            roomNumber.Content = res[8];
            phone.Text = res[10];
            decree.Text = res[11];

            faculty.SelectedValue = res[6];
            form.SelectedValue = res[13];
            citizenship.SelectedValue = SearchFunctions.convertToShortCitizenship(globalConnection, res[15]);

            date.SelectedDate = Convert.ToDateTime(res[9]);
            decreeDate.SelectedDate = Convert.ToDateTime(res[12]);
            stayLimit.SelectedDate = Convert.ToDateTime(res[14]);
            inspire.Visibility = Visibility.Hidden;
            evictionDate.Visibility = Visibility.Hidden;
            evictionDateLabel.Visibility = Visibility.Hidden;
        }

        private void fill_e_ST(string[] res)
        {
            infoLabel.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Hidden;
            faculty.IsEnabled = true;
            course.IsEnabled = true;
            form.IsEnabled = true;
            id.IsEnabled = true;

            gender = res[5];

            surname.Text = res[1];
            name.Text = res[2];
            patronymic.Text = res[3];
            id.Text = res[4];
            course.Text = res[7];
            roomNumber.Content = "-";
            phone.Text = res[10];
            decree.Text = res[11];

            faculty.SelectedValue = res[6];
            form.SelectedValue = res[13];
            citizenship.SelectedValue = SearchFunctions.convertToShortCitizenship(globalConnection, res[15]);

            date.SelectedDate = Convert.ToDateTime(res[9]);
            decreeDate.SelectedDate = Convert.ToDateTime(res[12]);
            stayLimit.SelectedDate = Convert.ToDateTime(res[14]);

            evictionDate.SelectedDate = Convert.ToDateTime(res[17]);

            evictTill.Visibility = Visibility.Hidden;

            roomChange.Visibility = Visibility.Hidden;
            inspire.Visibility = Visibility.Visible;
            evictionDate.Visibility = Visibility.Visible;
            evictionDateLabel.Visibility = Visibility.Visible;
        }

        private void fillOL(string[] res)
        {
            infoLabel.Visibility = Visibility.Visible;
            info.Visibility = Visibility.Visible;
            faculty.IsEnabled = false;
            course.IsEnabled = false;
            form.IsEnabled = false;
            id.IsEnabled = false;

            gender = res[5];

            id.Text = res[1];
            surname.Text = res[2];
            name.Text = res[3];
            patronymic.Text = res[4];
            roomNumber.Content = res[6];
            decree.Text = res[8];
            info.Text = res[12];
            phone.Text = res[13];

            citizenship.SelectedValue = SearchFunctions.convertToShortCitizenship(globalConnection, res[11]);

            date.SelectedDate = Convert.ToDateTime(res[7]);
            decreeDate.SelectedDate = Convert.ToDateTime(res[9]);
            stayLimit.SelectedDate = Convert.ToDateTime(res[10]);
            inspire.Visibility = Visibility.Hidden;
            evictionDate.Visibility = Visibility.Hidden;
            evictionDateLabel.Visibility = Visibility.Hidden;
        }

        private void fill_e_OL(string[] res)
        {
            infoLabel.Visibility = Visibility.Visible;
            info.Visibility = Visibility.Visible;
            faculty.IsEnabled = false;
            course.IsEnabled = false;
            form.IsEnabled = false;
            id.IsEnabled = false;

            gender = res[5];

            id.Text = res[1];
            surname.Text = res[2];
            name.Text = res[3];
            patronymic.Text = res[4];
            roomNumber.Content = res[6];
            decree.Text = res[8];
            info.Text = res[12];
            phone.Text = res[13];

            citizenship.SelectedValue = SearchFunctions.convertToShortCitizenship(globalConnection, res[11]);

            date.SelectedDate = Convert.ToDateTime(res[7]);
            decreeDate.SelectedDate = Convert.ToDateTime(res[9]);
            stayLimit.SelectedDate = Convert.ToDateTime(res[10]);

            evictionDate.SelectedDate = Convert.ToDateTime(res[13]);

            evictTill.Visibility = Visibility.Hidden;
            inspire.Visibility = Visibility.Visible;
            evictionDate.Visibility = Visibility.Visible;
            evictionDateLabel.Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (date.SelectedDate > stayLimit.SelectedDate)
                {
                    MessageBox.Show("Срок проживания заканчивается раньше конца оплаченного периода проживания");
                    date.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    if (id.IsEnabled == true)
                        Common_Functions.update_StudentData(globalConnection, studentID, name.Text, surname.Text, patronymic.Text, id.Text, faculty.SelectedValue.ToString(), course.Text, phone.Text, decree.Text, form.SelectedValue.ToString(), citizenship.SelectedValue.ToString(), (DateTime)date.SelectedDate, (DateTime)decreeDate.SelectedDate, (DateTime)stayLimit.SelectedDate);
                    else
                        Common_Functions.update_OtherLivingData(globalConnection, id.Text, name.Text, surname.Text, patronymic.Text, (DateTime)date.SelectedDate, decree.Text, (DateTime)decreeDate.SelectedDate, (DateTime)stayLimit.SelectedDate, citizenship.SelectedValue.ToString(), info.Text, phone.Text);
                    page.searchButton_Click(sender, e);
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Невозможно изменить информацию о проживающем");
            }
        }

        private void roomChange_Click(object sender, RoutedEventArgs e)
        {
            selectRoom.Visibility = Visibility.Visible;
            roomChange.Visibility = Visibility.Hidden;
            roomNumber.Visibility = Visibility.Hidden;
            change.IsEnabled = false;
            Close.IsEnabled = false;
            evictionBT.IsEnabled = false;
            evictTill.IsEnabled = false;

            Common_Functions.delete_FromRoom(globalConnection, roomNumber.Content.ToString());
        }

        private void selectRoom_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            if (gender == "Мужской") s = "m";
            if (gender == "Женский") s = "f";

            room_Selection rs = new room_Selection(globalConnection, s, studentID);
            rs.parentLabel = this.roomNumber;
            rs.ShowDialog();

            roomNumber.Visibility = Visibility.Visible;
            roomChange.Visibility = Visibility.Visible;
            selectRoom.Visibility = Visibility.Hidden;
            change.IsEnabled = true;
            Close.IsEnabled = true;
            evictionBT.IsEnabled = true;
            evictTill.IsEnabled = true;
        }

        private void evictionBT_Click(object sender, RoutedEventArgs e)
        {
            /*string type = "";

            if (id.IsEnabled == true)
                type = "S";
            else
                type = "O";*/

            eviction w1 = new eviction(globalConnection, type, this, id.Text, page);
            w1.ShowDialog();
        }

        private void evictTill_Click(object sender, RoutedEventArgs e)
        {
            EvictTillWindow w1 = new EvictTillWindow(globalConnection, this, type);
            w1.ShowDialog();
        }

        private void inspire_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand inspire = new MySqlCommand();
            inspire.Connection = globalConnection;
            if (faculty.IsEnabled == true)
                inspire.CommandText = "UPDATE Students SET evicted = false, EvictedTill = null WHERE Student_ID = '" + id.Text + "';";
            else
                inspire.CommandText = "UPDATE OtherLiving SET evicted = false, EvictedTill = null WHERE ID = '" + id.Text + "';";
            inspire.ExecuteNonQuery();
            selectRoom_Click(sender, e);
            this.Close();
        }
    }
}
