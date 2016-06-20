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
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using MySql_Class;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для room_Selection.xaml
    /// </summary>
   
   


    public partial class room_Selection : Window
    {
        public MySqlConnection globalConnection;
        public string Sex;
        public string studentID;

        public Label parentLabel { get; set; }
        
        public room_Selection()
        {
            InitializeComponent();
        }

        public room_Selection(MySqlConnection connection, string S, string ID)
        {
            globalConnection = connection;
            Sex = S;
            studentID = ID;
            InitializeComponent();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            bool srcFreeRooms = false;
            bool srcFamilyRooms = false;
            if (only_FreeRooms.IsChecked == true)
                srcFreeRooms = true;
            if (only_FamilyRooms.IsChecked == true)
                srcFamilyRooms = true;

            if (places_Count.SelectedIndex == -1)
                if (srcFreeRooms == true && srcFamilyRooms == true)
                    room_Info.ItemsSource = RoomItem.src_OnlyFamilyRooms(globalConnection); 
                else
                {
                    if (srcFamilyRooms == true)
                        room_Info.ItemsSource = RoomItem.src_OnlyFamilyRooms(globalConnection);
                    if (srcFreeRooms == true)
                        room_Info.ItemsSource = RoomItem.src_OnlyFreeRooms(globalConnection);
                }
            else
                if (srcFreeRooms == true && srcFamilyRooms == true)
                    room_Info.ItemsSource = RoomItem.src_OnlyFamilyRooms(globalConnection);
                else
                {
                    if (srcFamilyRooms == true)
                        room_Info.ItemsSource = RoomItem.src_OnlyFamilyRooms(globalConnection);
                    if (srcFreeRooms == true)
                        room_Info.ItemsSource = RoomItem.src_OnlyFreeRooms(globalConnection, Convert.ToInt32(places_Count.SelectedItem));
                    if (srcFamilyRooms == false && srcFreeRooms == false)
                        room_Info.ItemsSource = RoomItem.getItems(globalConnection,Sex, Convert.ToInt32(places_Count.SelectedItem));
                }
            if (srcFamilyRooms == false && srcFreeRooms == false && places_Count.SelectedIndex == -1)
                room_Info.ItemsSource = RoomItem.getItems(globalConnection,Sex);
            if (roomNumber.Text.ToString() != "")
            {
                var mydt = RoomItem.src_ByNumber(globalConnection, roomNumber.Text.ToString(), Sex);
                room_Info.ItemsSource = mydt;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            room_Info.ItemsSource = RoomItem.getItems(globalConnection, Sex);
        }

        private void room_Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selRoom = room_Info.SelectedItem;
            string gender = Sex;
            string _selRoom = ((RoomItem)selRoom).room_Numb;
            string _fplaces = ((RoomItem)selRoom).Fplaces_Con;
            if (SetFamily.IsChecked == true) gender = "c";
            if (Convert.ToInt32(_fplaces) != 0)
            {
                Common_Functions.update_RoomInfo(globalConnection, _selRoom, gender);
                Common_Functions.update_StudentRoomInfo(globalConnection, _selRoom, studentID);
                Common_Functions.update_OtherLivingRoomInfo(globalConnection, _selRoom, studentID);
                //room_Info.ItemsSource = RoomItem.getItems(globalConnection, Sex);
                parentLabel.Content = _selRoom;
                this.Close();
            }
            else
                MessageBox.Show("В выбранной комнате нет свободных мест, выберете другую комнату");
        }

        private void srcClear_Click(object sender, RoutedEventArgs e)
        {
            only_FamilyRooms.IsChecked = false;
            only_FreeRooms.IsChecked = false;
            roomNumber.Text = "";
            places_Count.SelectedIndex = -1;
            room_Info.ItemsSource = RoomItem.getItems(globalConnection, Sex);
        }

        private void only_FreeRooms_Click(object sender, RoutedEventArgs e)
        {
            only_FamilyRooms.IsChecked = false;
            roomNumber.Text = "";
        }

        private void only_FamilyRooms_Click(object sender, RoutedEventArgs e)
        {
            only_FreeRooms.IsChecked = false;
            roomNumber.Text = "";
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var selRoom = room_Info.SelectedItem;
            string gender = Sex;
            string _selRoom = ((RoomItem)selRoom).room_Numb;
            if (SetFamily.IsChecked == true) gender = "c";
            Common_Functions.update_RoomInfo(globalConnection, _selRoom, gender);
            Common_Functions.update_StudentRoomInfo(globalConnection, _selRoom, studentID);
            //room_Info.ItemsSource = RoomItem.getItems(globalConnection, Sex);
            parentLabel.Content = _selRoom;
            this.Close();
        }

        private void SetFamily_Click(object sender, RoutedEventArgs e)
        {
            if (SetFamily.IsChecked == true)
                room_Info.ItemsSource = Common_Functions.select_RoomForFamily(globalConnection);
            else
                room_Info.ItemsSource = RoomItem.getItems(globalConnection, Sex); 
        }
    }
}
