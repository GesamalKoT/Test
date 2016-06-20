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
using System.Collections.ObjectModel;
using MySql_Class;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для livingInfoPage.xaml
    /// </summary>
    public partial class livingInfoPage : Page
    {
        public MySqlConnection globalConnection;
        public ObservableCollection<livingST> allST;
        public ObservableCollection<livingOL> allOL;

        public livingInfoPage()
        {
            InitializeComponent();
        }

        public livingInfoPage(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            allST = Common_Functions.getAllST(globalConnection);
            allOL = Common_Functions.getAllOL(globalConnection);

            int stCountF = 0;
            int olCount = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach(livingST st in allST)
            {
                if(st.evictedTillDate < DateTime.Now)
                    stCountF += 1;
                if (st.date < DateTime.Today)
                    nonPayST += 1;
                if (st.gender == "Мужской")
                    men += 1;
                else
                    if (st.gender == "Женский")
                    women += 1;
                if (st.citizenship != "Российская Федерация")
                    forein += 1;
            }

            foreach(livingOL ol in allOL)
            {
                if(ol.evictedTillDate < DateTime.Now)
                    olCount += 1;
            }

            nonSTcount.Content = olCount.ToString();
            stCount.Content = stCountF.ToString();
            commonCount.Content = (olCount + stCountF).ToString();

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (faculty.SelectedIndex != -1)
            {
                if (form.SelectedIndex == -1 && course.Text == "")
                    onlyFaculty(faculty.SelectedValue.ToString());
                if (form.SelectedIndex != -1 && course.Text == "")
                    formFaculty(form.SelectedValue.ToString(), faculty.SelectedValue.ToString());
                if (form.SelectedIndex == -1 && course.Text != "")
                    courseFaculty(course.Text, faculty.SelectedValue.ToString());
                if (form.SelectedIndex != -1 && course.Text != "")
                    fullSelect(form.SelectedValue.ToString(), faculty.SelectedValue.ToString(), course.Text);
            }

        }

        private void fullSelect(string formST, string fucultyST, string courseST)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.faculty == fucultyST && st.course == Convert.ToInt32(courseST) && st.form == formST)
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void courseFaculty(string courseST, string fucultyST)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.faculty == fucultyST && st.course == Convert.ToInt32(courseST))
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void formFaculty(string formST, string fucultyST)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.faculty == fucultyST && st.form == formST)
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }
        
        private void onlyFaculty(string fuc)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach(livingST st in allST)
            {
                if(st.faculty == fuc)
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();

        }

        private void form_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (form.SelectedIndex != -1)
            {
                if (faculty.SelectedIndex == -1 && course.Text == "")
                    onlyForm(form.SelectedValue.ToString());
                if (faculty.SelectedIndex != -1 && course.Text == "")
                    formFaculty(form.SelectedValue.ToString(), faculty.SelectedValue.ToString());
                if (faculty.SelectedIndex == -1 && course.Text != "")
                    formCourse(form.SelectedValue.ToString(), course.Text);
                if (faculty.SelectedIndex != -1 && course.Text != "")
                    fullSelect(form.SelectedValue.ToString(), faculty.SelectedValue.ToString(), course.Text);
            }
        }

        private void formCourse(string form, string courseST)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.form == form && st.course == Convert.ToInt32(courseST))
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void onlyForm(string form)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.form == form)
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void course_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (course.Text != "")
            {
                if (faculty.SelectedIndex == -1 && form.SelectedIndex == -1)
                    onlyCourse(course.Text);
                if (faculty.SelectedIndex != -1 && form.SelectedIndex == -1)
                    courseFaculty(course.Text, faculty.SelectedValue.ToString());
                if (faculty.SelectedIndex == -1 && form.SelectedIndex != -1)
                    formCourse(form.SelectedValue.ToString(), course.Text);
                if (faculty.SelectedIndex != -1 && form.SelectedIndex != -1)
                    fullSelect(form.SelectedValue.ToString(), faculty.SelectedValue.ToString(), course.Text);
            }
        }

        private void onlyCourse(string courseST)
        {
            int stCountF = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if (st.course == Convert.ToInt32(courseST))
                {
                    stCountF += 1;
                    if (st.date < DateTime.Today)
                        nonPayST += 1;
                    if (st.gender == "Мужской")
                        men += 1;
                    else
                        women += 1;
                    if (st.citizenship != "Российская Федерация")
                        forein += 1;
                }
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            form.SelectedIndex = -1;
            faculty.SelectedIndex = -1;
            course.Text = "";

            int stCountF = 0;
            int olCount = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                stCountF += 1;
                if (st.date < DateTime.Today)
                    nonPayST += 1;
                if (st.gender == "Мужской")
                    men += 1;
                else
                    women += 1;
                if (st.citizenship != "Российская Федерация")
                    forein += 1;
            }

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();

        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            /*allST = Common_Functions.getAllST(globalConnection);
            allOL = Common_Functions.getAllOL(globalConnection);*/

            int stCountF = 0;
            int olCount = 0;

            int nonPayST = 0;
            int men = 0;
            int women = 0;
            int forein = 0;

            foreach (livingST st in allST)
            {
                if(st.evictedTillDate < date.SelectedDate && st.livingDate >= date.SelectedDate)
                    stCountF += 1;
                if (st.date < DateTime.Today && st.livingDate >= date.SelectedDate && st.evictedTillDate < date.SelectedDate)
                    nonPayST += 1;
                if (st.gender == "Мужской" && st.livingDate >= date.SelectedDate && st.evictedTillDate < date.SelectedDate)
                    men += 1;
                else
                    if (st.gender == "Женский" && st.livingDate >= date.SelectedDate && st.evictedTillDate < date.SelectedDate)
                        women += 1;
                if (st.citizenship != "Российская Федерация" && st.livingDate >= date.SelectedDate && st.evictedTillDate < date.SelectedDate)
                    forein += 1;
            }

            foreach (livingOL ol in allOL)
            {
                if(ol.evictedTillDate < date.SelectedDate && ol.livingDate >= date.SelectedDate)
                olCount += 1;
            }

            nonSTcount.Content = olCount.ToString();
            stCount.Content = stCountF.ToString();
            commonCount.Content = (olCount + stCountF).ToString();

            stCount1.Content = stCountF.ToString();
            nonPayedST.Content = nonPayST.ToString();
            menCountST.Content = men.ToString();
            womenCountST.Content = women.ToString();
            forenSTCount.Content = forein.ToString();
        }
    }
     
}
