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
using System.Data;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для add_Student.xaml
    /// </summary>
    /// 

    public static class bnd
    {
        public static void bindCombobox(ComboBox comboBoxName, MySqlConnection connection)
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Citizenship;", connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "Citizenship");
            comboBoxName.ItemsSource = ds.Tables[0].DefaultView;
            comboBoxName.DisplayMemberPath = ds.Tables[0].Columns["Name"].ToString();
            comboBoxName.SelectedValuePath = ds.Tables[0].Columns["Name"].ToString();
        }
    }

    public partial class add_Student : Page
    {

        public MySqlConnection globalConnection;
        public int otherLiving = 0;
        public string otherLivingID = "0";

        public add_Student()
        {
            InitializeComponent();
        }

        public add_Student(MySqlConnection connection)
        {
            globalConnection = connection; 
            InitializeComponent();
            bnd.bindCombobox(studentCitizenship, globalConnection);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            if (studentGender.SelectedIndex == 0) s = "f";
            if (studentGender.SelectedIndex == 1) s = "m";

            /*try
            {*/
                Common_Functions.fake_AddStudent(globalConnection, studentID.Text.ToString());
                Common_Functions.fake_addOtherLiving(globalConnection, studentID.Text.ToString());
                room_Selection rs = new room_Selection(globalConnection, s, studentID.Text.ToString());
                rs.parentLabel = this.studentRoom;
                rs.ShowDialog();
                rSelect.Visibility = Visibility.Hidden;
                studentRoom.Visibility = Visibility.Visible;
                RoomClear.Visibility = System.Windows.Visibility.Visible;
            /*}
            catch
            {
                MessageBox.Show("Проживающий с таким номером уже есть");
            }   */         
        }

        private void studentGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(studentGender.SelectedIndex != -1 && studentID.Text != "")
            {
                studentRoom.Visibility = Visibility.Hidden;
                rSelect.Visibility = Visibility.Visible;
                RoomClear.Visibility = Visibility.Hidden;
            }

            /*if ((studentGender.SelectedIndex == -1) && (string.IsNullOrEmpty(studentID.Text.ToString())))
            {
                studentRoom.Visibility = Visibility.Visible;
                rSelect.Visibility = Visibility.Hidden;
            }
                
            else
            {
                studentRoom.Visibility = Visibility.Hidden;
                rSelect.Visibility = Visibility.Visible;
            }*/
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            rSelect.Visibility = Visibility.Hidden;
            infoLabel.Visibility = Visibility.Hidden;
            Info.Visibility = Visibility.Hidden;
            /*MySqlCommand citizenship = new MySqlCommand();
            citizenship.Connection = globalConnection;
            citizenship.CommandText = "SELECT * FROM Citizenship;";
            citizenship.ExecuteNonQuery();
            MySqlDataAdapter adapter = new MySqlDataAdapter(citizenship);
            DataTable dt;
            adapter.Fill(dt);
            studentCitizenship.ItemsSource = dt;*/
        }

        private void RoomClear_Click(object sender, RoutedEventArgs e)
        {
            rSelect.Visibility = Visibility.Visible;
            studentRoom.Visibility = System.Windows.Visibility.Hidden;
            RoomClear.Visibility = System.Windows.Visibility.Hidden;

            Common_Functions.delete_FromRoom(globalConnection, studentRoom.Content.ToString());

        }


        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand clear_Fake = new MySqlCommand();
            clear_Fake.Connection = globalConnection;
            clear_Fake.CommandText = "DELETE FROM Students WHERE Name = 'FAKE';";
            clear_Fake.ExecuteNonQuery();

            MySqlCommand clear_FakeOT = new MySqlCommand();
            clear_FakeOT.Connection = globalConnection;
            clear_FakeOT.CommandText = "DELETE FROM OtherLiving WHERE Name = 'FAKE';";
            clear_FakeOT.ExecuteNonQuery();

            if (studentRoom.Content.ToString() != "")
                Common_Functions.delete_FromRoom(globalConnection,studentRoom.Content.ToString());

            if(otherLiving == 1)
            {
                MySqlCommand deleteotherID = new MySqlCommand();
                deleteotherID.Connection = globalConnection;
                deleteotherID.CommandText = "DELETE FROM OtherLivingID WHERE Id = '"+otherLivingID+"';";
                deleteotherID.ExecuteNonQuery();
            }
            d();
        }

        private void d()
        {
            MySqlCommand impruve = new MySqlCommand();
            impruve.Connection = globalConnection;
            impruve.CommandText = "UPDATE Students SET Gender = 'Мужской' WHERE Gender = 'Male';";
            impruve.ExecuteNonQuery();
        }

        private void addStudent_Click(object sender, RoutedEventArgs e)
        {
            DateTime setDate;
            if (!(bool)studentType.IsChecked)
                if (!contentCheckfull())
                {
                    if (studentDate.SelectedDate == null)
                        setDate = DateTime.Now.AddDays(-1);
                    else
                        setDate = (DateTime)studentDate.SelectedDate;
                    Common_Functions.refresh_StudentData(globalConnection, studentName.Text, studentSurname.Text, studentPatronymic.Text, studentID.Text, studentGender.SelectedValue.ToString(), studentFaculty.SelectedValue.ToString(), studentCourse.Text, studentPhone.Text, Decree.Text, Form.SelectedValue.ToString(), studentCitizenship.SelectedValue.ToString(), setDate, (DateTime)Decree_Date.SelectedDate, (DateTime)studentStayLimit.SelectedDate);
                    deleteFakeOL();
                    clearAllContent();
                    rSelect.Visibility = Visibility.Hidden;
                    infoLabel.Visibility = Visibility.Hidden;
                    Info.Visibility = Visibility.Hidden;
                }
                else ;
            else
            {
                if (!contentCheck())
                {
                    if (studentDate.SelectedDate == null)
                        setDate = DateTime.Now.AddDays(-1);
                    else
                        setDate = (DateTime)studentDate.SelectedDate;
                    Common_Functions.refresh_OtherLivingData(globalConnection, studentID.Text, studentName.Text, studentSurname.Text, studentPatronymic.Text, studentGender.SelectedValue.ToString(), setDate, Decree.Text, (DateTime)Decree_Date.SelectedDate, (DateTime)studentStayLimit.SelectedDate, studentCitizenship.SelectedValue.ToString(), Info.Text, studentPhone.Text);
                    deleteFakeST();
                    clearAllContent();
                    rSelect.Visibility = Visibility.Hidden;
                    infoLabel.Visibility = Visibility.Hidden;
                    Info.Visibility = Visibility.Hidden;
                }
            }
            otherLiving = 0;

        }

        public void clearAllContent()
        {
            studentName.Text = "";
            studentSurname.Text = "";
            studentPatronymic.Text = "";
            studentID.Text = "";
            studentGender.SelectedIndex = -1;
            studentFaculty.SelectedIndex = -1;
            studentCourse.Text = "";
            studentDate.SelectedDate = null;
            studentPhone.Text = "";
            Decree.Text = "";
            Decree_Date.SelectedDate = null;
            Form.SelectedIndex = -1;
            studentStayLimit.SelectedDate = null;
            studentCitizenship.SelectedIndex = -1;
            Info.Text = "";
            studentType.IsChecked = false;

            studentRoom.Content = "";
            studentRoom.Visibility = Visibility.Hidden;
            //rSelect.Visibility = Visibility.Visible;
            RoomClear.Visibility = Visibility.Hidden;

            studentID.IsEnabled = true;
            studentFaculty.IsEnabled = true;
            studentCourse.IsEnabled = true;
            Form.IsEnabled = true;

        }

        public void deleteFakeST()
        {
            MySqlCommand del = new MySqlCommand();
            del.Connection = globalConnection;
            del.CommandText = "DELETE FROM Students WHERE Name = 'FAKE';";
            del.ExecuteNonQuery();
        }

        public void deleteFakeOL()
        {
            MySqlCommand del = new MySqlCommand();
            del.Connection = globalConnection;
            del.CommandText = "DELETE FROM OtherLiving WHERE Name = 'FAKE';";
            del.ExecuteNonQuery();
        }

        public bool contentCheck()
        {
            bool fl = false;
            bool dateErrors = false;
            bool stayLimitErrors = false;

            if (Decree_Date.SelectedDate == null)
            {
                fl = true;
                Decree_Date.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                Decree_Date.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            
            if (studentSurname.Text == "")
            {
                fl = true;
                studentSurname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentSurname.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentName.Text == "")
            {
                fl = true;
                studentName.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentName.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentID.Text == "")
            {
                fl = true;
                studentID.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentID.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            /*if(studentDate.SelectedDate == null)
            {
                fl = true;
                studentDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }*/

            if (Decree.Text == "")
            {
                fl = true;
                Decree.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                Decree.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentGender.SelectedIndex == -1)
            {
                fl = true;
                studentGender.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentGender.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentRoom.Content.ToString() == "")
            {
                fl = true;
                studentRoom.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentRoom.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentStayLimit.SelectedDate == null)
            {
                fl = true;
                stayLimitErrors = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);
                stayLimitErrors = false;
            }

            if (studentCitizenship.SelectedIndex == -1)
            {
                fl = true;
                studentCitizenship.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentCitizenship.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentDate.SelectedDate < DateTime.Now)
            {
                dateErrors = true;
                fl = true;
                studentDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                studentDate.BorderBrush = new SolidColorBrush(Colors.SlateGray);
                dateErrors = false;
            }

            if (studentStayLimit.SelectedDate < DateTime.Now || stayLimitErrors == true)
            {
                fl = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentStayLimit.SelectedDate < studentDate.SelectedDate || stayLimitErrors == true)
            {
                fl = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            return fl;
        }

        public bool contentCheckfull()
        {
            bool fl = false;
            bool dateErrors = false;
            bool stayLimitErrors = false;


            if (Decree_Date.SelectedDate == null)
            {
                fl = true;
                Decree_Date.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                Decree_Date.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            if (Form.SelectedIndex == -1)
            {
                Form.BorderBrush = new SolidColorBrush(Colors.Red);
                fl = true;
            }
            else
                Form.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            if (studentSurname.Text == "")
            {
                fl = true;
                studentSurname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentSurname.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentName.Text == "")
            {
                fl = true;
                studentName.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentName.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentID.Text == "")
            {
                fl = true;
                studentID.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentID.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            
            /*if(studentDate.SelectedDate == null)
            {
                fl = true;
                studentDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }*/

            if (Decree.Text == "")
            {
                fl = true;
                Decree.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                Decree.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentGender.SelectedIndex == -1)
            {
                fl = true;
                studentGender.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentGender.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentFaculty.SelectedIndex == -1)
            {
                fl = true;
                studentFaculty.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentFaculty.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentCourse.Text == "")
            {
                fl = true;
                studentCourse.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentCourse.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentRoom.Content.ToString() == "")
            {
                fl = true;
                studentRoom.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentRoom.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (Form.SelectedIndex == -1)
            {
                fl = true;
                Form.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                Form.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentStayLimit.SelectedDate == null)
            {
                fl = true;
                stayLimitErrors = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);
                stayLimitErrors = false;
            }

            if (studentCitizenship.SelectedIndex == -1)
            {
                fl = true;
                studentCitizenship.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentCitizenship.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentDate.SelectedDate < DateTime.Now)
            {
                dateErrors = true;
                fl = true;
                studentDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                studentDate.BorderBrush = new SolidColorBrush(Colors.SlateGray);
                dateErrors = false;
            }

            if (studentStayLimit.SelectedDate < DateTime.Now || stayLimitErrors == true)
            {
                fl = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            if (studentStayLimit.SelectedDate < studentDate.SelectedDate || stayLimitErrors == true)
            {
                fl = true;
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                studentStayLimit.BorderBrush = new SolidColorBrush(Colors.SlateGray);

            return fl;
        }

        private void studentType_Checked(object sender, RoutedEventArgs e) // при нажатии на checkBox
        {
            if((bool)studentType.IsChecked)
            {
                studentID.IsEnabled = false;
                studentFaculty.IsEnabled = false;
                studentCourse.IsEnabled = false;
                Form.IsEnabled = false;
                studentID.Text = Common_Functions.getNewID(globalConnection, otherLiving);
                otherLivingID = studentID.Text;
                otherLiving = 1;
                infoLabel.Visibility = Visibility.Visible;
                Info.Visibility = Visibility.Visible;
            }
            else
            {
                studentID.IsEnabled = true;
                studentFaculty.IsEnabled = true;
                studentCourse.IsEnabled = true;
                Form.IsEnabled = true;
                //otherLivingID = "0";
                studentID.Text = "";
                infoLabel.Visibility = Visibility.Hidden;
                Info.Visibility = Visibility.Hidden;
            }
        }

        private void addCitizenship_Click(object sender, RoutedEventArgs e)
        {
            Citizenship w1 = new Citizenship(globalConnection, studentCitizenship);
            w1.ShowDialog();
        }

       /* private void studentID_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(studentGender.SelectedIndex != -1)
            {
                studentRoom.Visibility = Visibility.Hidden;
                rSelect.Visibility = Visibility.Visible;
                RoomClear.Visibility = Visibility.Hidden;
            }
        }*/

        private void studentID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (studentGender.SelectedIndex != -1)
            {
                studentRoom.Visibility = Visibility.Hidden;
                rSelect.Visibility = Visibility.Visible;
                RoomClear.Visibility = Visibility.Hidden;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if(studentRoom.Content != "")
            {
                Common_Functions.delete_FromRoom(globalConnection, studentRoom.Content.ToString());
                deleteFakeOL();
                deleteFakeST();
            }
        }
    }
}
