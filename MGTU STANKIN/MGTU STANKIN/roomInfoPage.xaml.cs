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
using System.Collections.ObjectModel;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для roomInfoPage.xaml
    /// </summary>
    public partial class roomInfoPage : Page
    {
        public MySqlConnection globalConnection;
        public ObservableCollection<roomInfo> fullRoomInfo;

        public roomInfoPage()
        {
            InitializeComponent();
        }

        public roomInfoPage(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fullRoomInfo = roomInfo.viewRoomInfo(globalConnection);

            string housing = fullRoomInfo[0].housing;
            int rooms = 0;
            int places = 0;
            int frooms = 0;
            int fplaces = 0;
            int familyRooms = 0;
            int mrooms = 0;
            int fmrooms = 0;
            int femrooms = 0;
            int ffemrooms = 0;
            int _2p = 0;
            int _F2p = 0;
            int _3p = 0;
            int _F3p = 0;
            int _4p = 0;
            int _F4p = 0;

            foreach(roomInfo room in fullRoomInfo)
            {
                rooms += 1;
                places += Convert.ToInt32(room.places_Count);
                fplaces += Convert.ToInt32(room.fPlaces_Count);

                switch(room.places_Count)
                {
                    case "2": _2p += 1; break;
                    case "3": _3p += 1; break;
                    case "4": _4p += 1; break;
                }

                if (room.places_Count == room.fPlaces_Count)
                {
                    frooms += 1;
                    switch(room.places_Count)
                    {
                        case "2": _F2p += 1; break;
                        case "3": _F3p += 1; break;
                        case "4": _F4p += 1; break;
                    }
                }

                switch(room.type)
                {
                    case "c": familyRooms += 1;break;
                    case "m": mrooms += 1; fmrooms += Convert.ToInt32(room.fPlaces_Count); break;
                    case "f": femrooms += 1; ffemrooms += Convert.ToInt32(room.fPlaces_Count); break;
                }      
            }

            housingLabel.Content = housing;
            roomCount.Content = rooms.ToString();
            placesCount.Content = places.ToString();
            emptyRooms.Content = frooms.ToString();
            freePlaces.Content = fplaces.ToString();
            familyRoomsLabel.Content = familyRooms.ToString();
            maleRooms.Content = mrooms.ToString();
            male_fPlaces.Content = fmrooms.ToString();
            femaleRooms.Content = femrooms.ToString();
            female_fPlaces.Content = ffemrooms.ToString();
            places_2rooms.Content = _2p.ToString();
            empty_places_2rooms.Content = _F2p.ToString();
            places_3rooms.Content = _3p.ToString();
            empty_places_3rooms.Content = _F3p.ToString();
            places_4rooms.Content = _4p.ToString();
            empty_places_4rooms.Content = _F4p.ToString();
        }
    }
}
