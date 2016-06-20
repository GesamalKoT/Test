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
using System.Runtime.InteropServices;
using System.Diagnostics;
using io = System.IO;
using excel = Microsoft.Office.Interop.Excel;

using Application = Microsoft.Office.Interop.Excel.Application;

namespace MGTU_STANKIN
{
    /// <summary>
    /// Логика взаимодействия для reportsPage.xaml
    /// </summary>
    public partial class reportsPage : Page
    {
        public MySqlConnection globalConnection;
        private Application application;
        private excel.Workbook workBook;
        private excel.Worksheet worksheet;

        public reportsPage()
        {
            InitializeComponent();
        }

        public reportsPage(MySqlConnection connection)
        {
            globalConnection = connection;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            hideL();
            makeReport.IsEnabled = false;
            date.SelectedDate = DateTime.Now.Date;
            housingDate.SelectedDate = DateTime.Now.Date;
        }

        private void hideL()
        {
            bgFaculty.Visibility = Visibility.Hidden;
            bgLivers.Visibility = Visibility.Hidden;
            faculty.Visibility = Visibility.Hidden;
            evicted.Visibility = Visibility.Hidden;
            evictedLabel.Visibility = Visibility.Hidden;
            nonPayed.Visibility = Visibility.Hidden;
            nonPayedLabel.Visibility = Visibility.Hidden;
            ITC.Visibility = Visibility.Hidden;
            MTO.Visibility = Visibility.Hidden;
            FEM.Visibility = Visibility.Hidden;
            COIG.Visibility = Visibility.Hidden;
            Aspr.Visibility = Visibility.Hidden;
            date.Visibility = Visibility.Hidden;
            dateLabel.Visibility = Visibility.Hidden;
            insideUse.Visibility = Visibility.Hidden;
            housingDate.Visibility = Visibility.Hidden;
            housingDateLabel.Visibility = Visibility.Hidden;
            housingInside.Visibility = Visibility.Hidden; 
        }

        private void viewL()
        {
            //bgFaculty.Visibility = Visibility.Visible;
            bgLivers.Visibility = Visibility.Visible;
            faculty.Visibility = Visibility.Visible;
            evicted.Visibility = Visibility.Visible;
            evictedLabel.Visibility = Visibility.Visible;
            nonPayed.Visibility = Visibility.Visible;
            nonPayedLabel.Visibility = Visibility.Visible;
            /*ITC.Visibility = Visibility.Visible;
            MTO.Visibility = Visibility.Visible;
            FEM.Visibility = Visibility.Visible;
            COIG.Visibility = Visibility.Visible;
            Aspr.Visibility = Visibility.Visible;*/
            date.Visibility = Visibility.Visible;
            dateLabel.Visibility = Visibility.Visible;
            insideUse.Visibility = Visibility.Visible;
            housingDate.Visibility = Visibility.Hidden;
            housingDateLabel.Visibility = Visibility.Hidden;
            housingInside.Visibility = Visibility.Hidden; 
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (map.IsChecked == true)
                hideL();
            makeReport.IsEnabled = true;
        }

        private void livers_Click(object sender, RoutedEventArgs e)
        {
            if (livers.IsChecked == true)
                viewL();
            makeReport.IsEnabled = true;
        }

        private void dormitory_Click(object sender, RoutedEventArgs e)
        {
            if (dormitory.IsChecked == true)
            {
                hideL();
                housingDate.Visibility = Visibility.Visible;
                housingDateLabel.Visibility = Visibility.Visible;
                housingInside.Visibility = Visibility.Visible; 
            }
            makeReport.IsEnabled = true;
        }

        private void faculty_Click(object sender, RoutedEventArgs e)
        {
            if(faculty.IsChecked == true)
            {
                ITC.Visibility = Visibility.Visible;
                MTO.Visibility = Visibility.Visible;
                FEM.Visibility = Visibility.Visible;
                COIG.Visibility = Visibility.Visible;
                Aspr.Visibility = Visibility.Visible;
                bgFaculty.Visibility = Visibility.Visible;
            }
            else
            {
                ITC.Visibility = Visibility.Hidden;
                MTO.Visibility = Visibility.Hidden;
                FEM.Visibility = Visibility.Hidden;
                COIG.Visibility = Visibility.Hidden;
                Aspr.Visibility = Visibility.Hidden;
                bgFaculty.Visibility = Visibility.Hidden;
            }
        }

        private void settlementMap()
        {
            application = new Application { DisplayAlerts = false };
            const string template = "TestExcel.xlsx";
            string mounth = "";
            int floor = 0;
            int line = 5;
            int count = 0;
            string cell = "";

            workBook = application.Workbooks.Open(io.Path.Combine(Environment.CurrentDirectory, template));
            worksheet = workBook.ActiveSheet as excel.Worksheet;

           /* switch (DateTime.Now.Month.ToString())
            {
                case "1": mounth = "января"; break;
                case "2": mounth = "февраля"; break;
                case "3": mounth = "марта"; break;
                case "4": mounth = "апреля"; break;
                case "5": mounth = "мая"; break;
                case "6": mounth = "июня"; break;
                case "7": mounth = "июля"; break;
                case "8": mounth = "августа"; break;
                case "9": mounth = "сентября"; break;
                case "10": mounth = "октября"; break;
                case "11": mounth = "ноября"; break;
                case "12": mounth = "декабря"; break;
            }*/
            

            MySqlCommand selRoom = new MySqlCommand();
            selRoom.Connection = globalConnection;
            selRoom.CommandText = "SELECT * FROM Rooms;";
            selRoom.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter(selRoom);
            DataTable dt = new DataTable();
            da.Fill(dt);
            var myDataRooms = dt.Select();

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT * FROM Students;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter da1 = new MySqlDataAdapter(selST);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            var myDataST = dt1.Select();

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT * FROM OtherLiving;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter da2 = new MySqlDataAdapter(selOL);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            var myDataOL = dt2.Select();

            worksheet.Range["E2"].Value = "Карта расселения проживающих";
            worksheet.Range["E2"].HorizontalAlignment = TextAlignment.Center; //выравнивание по центру исправить

            MySqlCommand selH = new MySqlCommand("SELECT Housing, FIO FROM Settings WHERE ID = 1;", globalConnection);
            selH.ExecuteNonQuery();
            
            MySqlDataAdapter daH = new MySqlDataAdapter(selH);
            DataTable dtH = new DataTable();
            daH.Fill(dtH);
            var myDataH = dtH.Select();

            worksheet.Range["E3"].Value = "в общежитии № " + myDataH[0].ItemArray[0].ToString() + "";
            worksheet.Range["E3"].HorizontalAlignment = TextAlignment.Center;

            worksheet.Range["E4"].Value = "МГТУ Станкин";

            excel.Range excelcells;
            excelcells = worksheet.get_Range("E2", "E4");
            excelcells.HorizontalAlignment = excel.Constants.xlCenter;

            for(int i = 0; i < myDataRooms.Length; i++)
            {
                count = 0;
                if (floor != (Convert.ToInt32(myDataRooms[i].ItemArray[1]) / 100 == 0 ? 1 : Convert.ToInt32(myDataRooms[i].ItemArray[1]) / 100 ))
                {
                    line += 1;
                    floor = (Convert.ToInt32(myDataRooms[i].ItemArray[1]) / 100 == 0 ? 1 : Convert.ToInt32(myDataRooms[i].ItemArray[1]) / 100 );
                    cell = "A" + line.ToString();
                    worksheet.Range[cell].Value = floor.ToString() + " этаж";
                    line += 1;
                }

                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = "комната " + myDataRooms[i].ItemArray[1].ToString();
                line += 1;

                for (int k = 0; k < myDataST.Length; k++)
                {
                    try
                    {
                        if (Convert.ToInt32(myDataST[k].ItemArray[7]) == Convert.ToInt32(myDataRooms[i].ItemArray[1]))
                        {
                            cell = "C" + line.ToString();
                            worksheet.Range[cell].Value = (count + 1).ToString() + ". " + myDataST[k].ItemArray[0].ToString() + " " + myDataST[k].ItemArray[1].ToString() + " " + myDataST[k].ItemArray[3].ToString();
                            line += 1;
                            count += 1;
                            myDataST[k].Delete();
                            if (count == Convert.ToInt32(myDataRooms[i].ItemArray[2]) - Convert.ToInt32(myDataRooms[i].ItemArray[3]))
                                break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                for (int k = 0; k < myDataOL.Length; k++)
                {
                    try
                    {
                        if (Convert.ToInt32(myDataOL[k].ItemArray[5]) == Convert.ToInt32(myDataRooms[i].ItemArray[1]))
                        {
                            cell = "C" + line.ToString();
                            worksheet.Range[cell].Value = (count + 1).ToString() + ". " + myDataOL[k].ItemArray[1].ToString() + " " + myDataOL[k].ItemArray[2].ToString() + " " + myDataOL[k].ItemArray[0].ToString();
                            line += 1;
                            count += 1;
                            if (count == Convert.ToInt32(myDataRooms[i].ItemArray[2]) - Convert.ToInt32(myDataRooms[i].ItemArray[3]))
                                break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if(count != Convert.ToInt32(myDataRooms[i].ItemArray[2]))
                {
                    int p = count;
                    for (int s = 0; s < Convert.ToInt32(myDataRooms[i].ItemArray[2]) - p; s++ )
                    {
                        cell = "C" + line.ToString();
                        worksheet.Range[cell].Value = (count + 1).ToString() + ". " + "пусто";
                        line += 1;
                        count += 1;
                    }
                }
                line += 1;
            }

            cell = "E" + (line + 4).ToString();
            worksheet.Range[cell].Value = "Зав. общежитием________________" + myDataH[0].ItemArray[1].ToString();

            application.Visible = true;
            //TopMost = true;
        }

        private void liversReport()
        {
            int line = 2;
            string cell = "";
            string mounth = "";
            excel.Range excelcells;

            switch (DateTime.Now.Month.ToString())
            {
                case "1": mounth = "января"; break;
                case "2": mounth = "февраля"; break;
                case "3": mounth = "марта"; break;
                case "4": mounth = "апреля"; break;
                case "5": mounth = "мая"; break;
                case "6": mounth = "июня"; break;
                case "7": mounth = "июля"; break;
                case "8": mounth = "августа"; break;
                case "9": mounth = "сентября"; break;
                case "10": mounth = "октября"; break;
                case "11": mounth = "ноября"; break;
                case "12": mounth = "декабря"; break;
            }

            application = new Application { DisplayAlerts = false };
            const string template = "TestExcel.xlsx";

            workBook = application.Workbooks.Open(io.Path.Combine(Environment.CurrentDirectory, template));
            worksheet = workBook.ActiveSheet as excel.Worksheet;
            

            MySqlCommand selSettings = new MySqlCommand("SELECT Housing, FIO FROM Settings WHERE ID = 1;", globalConnection);
            selSettings.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selSettings);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataS = dtS.Select();

            if(insideUse.IsChecked == false && faculty.IsChecked == false)
            {
                worksheet.Range["B2"].Value = "Утверждаю";
                worksheet.Range["B3"].Value = "Проректор по АХР";

                worksheet.Range["B5"].Value = "_________________________";
                worksheet.Range["D6"].Value = "Золотарев И.В.";
                line = 8;
            }

            if(faculty.IsChecked == false)
            {
                cell = "C" + line.ToString();
                worksheet.Range[cell].Value = "Список проживающих в общежитии № " + myDataS[0].ItemArray[0].ToString() + ", ";
                
                line += 1;
                excelcells = worksheet.get_Range(cell, cell);
                excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                /*cell = "B" + line.ToString();
                worksheet.Range[cell].Value = "";*/
            }
            else
            {
                line = facultyfill(application, workBook, line, myDataS[0].ItemArray[0].ToString());
            }
            if (nonPayed.IsChecked == false && faculty.IsChecked == false)
            {
                if (date.SelectedDate == DateTime.Now.Date)
                {
                    cell = "C" + line.ToString();
                    worksheet.Range[cell].Value = "по состоянию на " + DateTime.Now.Day.ToString() + " " + mounth + " " + DateTime.Now.Year.ToString() + " г.";
                    excelcells = worksheet.get_Range(cell, cell);
                    excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                    //worksheet.Cells[cell].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    line += 1;
                }
                else
                {
                    cell = "C" + line.ToString();
                    worksheet.Range[cell].Value = "которые будут проживать с " + date.SelectedDate.Value.Day.ToString() + " " + mounth + " " + date.SelectedDate.Value.Year.ToString();
                    excelcells = worksheet.get_Range(cell, cell);
                    excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                    //worksheet.Cells[cell].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    line += 1;
                }
            }
            else
            {
                if(date.SelectedDate == DateTime.Now.Date && faculty.IsChecked == false)
                {
                    cell = "C" + line.ToString();
                    worksheet.Range[cell].Value = "которые не оплатили проживание";
                    excelcells = worksheet.get_Range(cell, cell);
                    excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                    line += 1;
                    cell = "C" + line.ToString();
                    worksheet.Range[cell].Value = "по состоянию на " + DateTime.Now.Day.ToString() + " " + mounth + " " + DateTime.Now.Year.ToString() + " г.";
                    excelcells = worksheet.get_Range(cell, cell);
                    excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                    //worksheet.Cells[cell].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    line += 1;
                }
                else
                {
                    if (faculty.IsChecked == false)
                    {
                        cell = "C" + line.ToString();
                        worksheet.Range[cell].Value = "у которых закончиться срок оплаты";
                        excelcells = worksheet.get_Range(cell, cell);
                        excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                        line += 1;
                        cell = "C" + line.ToString();
                        worksheet.Range[cell].Value = "к " + date.SelectedDate.Value.Day.ToString() + " " + mounth + " " + date.SelectedDate.Value.Year.ToString() + " г.";
                        excelcells = worksheet.get_Range(cell, cell);
                        excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                        line += 1;
                    }
                    //worksheet.Cells[cell].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }
            }

            if(faculty.IsChecked == false && evicted.IsChecked == false && nonPayed.IsChecked == false && date.SelectedDate == DateTime.Now.Date)
                excelLivingClass.fullReport_withoutE(application, workBook, worksheet, globalConnection, line);
            if (faculty.IsChecked == false && evicted.IsChecked == false && nonPayed.IsChecked == false && date.SelectedDate != DateTime.Now.Date)
                excelLivingClass.fullReport_Date(application, workBook, worksheet, globalConnection, line, Convert.ToDateTime(date.SelectedDate));
            if (faculty.IsChecked == false && evicted.IsChecked == false && nonPayed.IsChecked == true && (date.SelectedDate == DateTime.Now.Date || date.SelectedDate != DateTime.Now.Date))
                excelLivingClass.fullReport_nonPayed(application, workBook, worksheet, globalConnection, line, (DateTime)date.SelectedDate);
            if (faculty.IsChecked == false && evicted.IsChecked == true && nonPayed.IsChecked == false && date.SelectedDate == DateTime.Now.Date)
                excelLivingClass.fullReport_evicted(application, workBook, worksheet, globalConnection, line);
            if (faculty.IsChecked == false && evicted.IsChecked == true && nonPayed.IsChecked == false && date.SelectedDate != DateTime.Now.Date)
                excelLivingClass.fullReport_EvictedDate(application, workBook, worksheet, globalConnection, line, (DateTime)date.SelectedDate);
            if (faculty.IsChecked == true && evicted.IsChecked == false && nonPayed.IsChecked == false && date.SelectedDate == DateTime.Now.Date)
                line += 1;

            application.Visible = true;//конец
        }

        private int facultyfill (excel.Application application, excel.Workbook workBook, int line, string housingNumber)
        {
            string[] facultyList = new string[5];
            int k = 0;
            excel.Sheets sheet;
            excel.Worksheet newWorkSheet;
            string sheetName = "";

            if (ITC.IsChecked == true)
            {
                facultyList[k] = "ИТС";
                k += 1;
            }
            if (MTO.IsChecked == true)
            {
                facultyList[k] = "МТО";
                k += 1;
            }
            if(FEM.IsChecked == true)
            {
                facultyList[k] = "ФЭМ";
                k += 1;
            }
            if(COIG.IsChecked == true)
            {
                facultyList[k] = "ЦОИГ";
                k += 1;
            }
            if(Aspr.IsChecked == true)
            {
                facultyList[k] = "Аспирантура";
                k += 1;
            }

            try
            {
                sheetName = facultyList[0];
                application.Sheets[1].Name = sheetName;

                for (int i = 0; i < k; i++)
                {
                    if (i < k - 1)
                    {
                        sheetName = facultyList[k - (i + 1)];
                        application.Sheets.Add().Name = sheetName;
                    }
                }

                sheet = application.ActiveWorkbook.Worksheets;

                line = excelLivingClass.facultyHeader(application, line, housingNumber, k, (bool)insideUse.IsChecked, (bool)nonPayed.IsChecked, (DateTime)date.SelectedDate, (bool)evicted.IsChecked, globalConnection);

                /*for(int i = 0; i < k; i++)
                {
                    newWorkSheet = (excel.Worksheet)sheet.get_Item(i + 1);
                    excelLivingClass.facultyFullReport(application, newWorkSheet, globalConnection, line);
                }*/

                return line;
            }
            catch
            {
                MessageBox.Show("Выберете хотя бы один факультет");
                return 0;
            }
        }

        private void dormitoryReport()
        {
            int line = 1;
            string cell = "";
            string mounth = "";
            excel.Range excelcells;
            string[] resault = new string[11];

            application = new Application { DisplayAlerts = false };
            const string template = "TestExcel.xlsx";

            workBook = application.Workbooks.Open(io.Path.Combine(Environment.CurrentDirectory, template));
            worksheet = workBook.ActiveSheet as excel.Worksheet;

            switch (housingDate.SelectedDate.Value.Month.ToString())
            {
                case "1": mounth = "января"; break;
                case "2": mounth = "февраля"; break;
                case "3": mounth = "марта"; break;
                case "4": mounth = "апреля"; break;
                case "5": mounth = "мая"; break;
                case "6": mounth = "июня"; break;
                case "7": mounth = "июля"; break;
                case "8": mounth = "августа"; break;
                case "9": mounth = "сентября"; break;
                case "10": mounth = "октября"; break;
                case "11": mounth = "ноября"; break;
                case "12": mounth = "декабря"; break;
            }
            if ((bool)housingInside.IsChecked == false)
            {
                worksheet.Range["B2"].Value = "Утверждаю";
                worksheet.Range["B3"].Value = "Проректор по АХР";

                worksheet.Range["B5"].Value = "_________________________";
                worksheet.Range["D6"].Value = "Золотарев И.В.";
                line = 8;
            }
            else
                line = 2;
            MySqlCommand selH = new MySqlCommand("SELECT Housing, FIO FROM Settings WHERE ID = 1;", globalConnection);
            selH.ExecuteNonQuery();
            
            MySqlDataAdapter daH = new MySqlDataAdapter(selH);
            DataTable dtH = new DataTable();
            daH.Fill(dtH);
            var myDataH = dtH.Select();

            cell = "C" + line.ToString();
            if (housingDate.SelectedDate == DateTime.Now.Date)
            {
                worksheet.Range[cell].Value = "Отчет о состоянии общежития № " + myDataH[0].ItemArray[0].ToString();
                line += 1;
                cell = "C" + line.ToString();
                worksheet.Range[cell].Value = "на " + housingDate.SelectedDate.Value.Day + " " + mounth + " " + date.SelectedDate.Value.Year + " г.";
                excelcells = worksheet.get_Range("C" + (line - 1).ToString(), "C" + line.ToString());
                excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                line += 2;
            }
            else
            {
                worksheet.Range[cell].Value = "Отчет о предпологаемом состоянии";
                line += 1;
                cell = "C" + line.ToString();
                worksheet.Range[cell].Value = "общежития № " + myDataH[0].ItemArray[0].ToString() + " на " + housingDate.SelectedDate.Value.Day + " " + mounth + " " + date.SelectedDate.Value.Year + " г.";
                excelcells = worksheet.get_Range("C" + (line - 1).ToString(), "C" + line.ToString());
                excelcells.HorizontalAlignment = excel.Constants.xlCenter;
                line += 4;
            }

            resault = excelLivingClass.dormitoryReport(globalConnection, (DateTime)housingDate.SelectedDate);

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "1. Количество комнат";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[0];
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "- 2х местных";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[1];
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "- 3х местных";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[2];
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "- 4х местных";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[3];
            line += 1;

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "2. Общее количество мест";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[4];
            line += 1;

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "3. Количество проживающих";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[5];
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "- студентов";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[6];
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "- иных проживающих";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[7];
            line += 1;

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "4. Количество свободных мест";
            cell = "F" + line.ToString();
            worksheet.Range[cell].Value = resault[8];
            line += 1;

            if (date.SelectedDate == DateTime.Now.Date)
            {
                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = "5. Пустых комнат";
                cell = "F" + line.ToString();
                worksheet.Range[cell].Value = resault[9];
                line += 1;

                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = "6. Семейных комнат";
                cell = "F" + line.ToString();
                worksheet.Range[cell].Value = resault[10];
                line += 1;
            }
            application.Visible = true;
        }

        private void makeReport_Click(object sender, RoutedEventArgs e)
        {
            if (map.IsChecked == true)
                settlementMap();
            if (livers.IsChecked == true)
                liversReport();
            if (dormitory.IsChecked == true) 
                dormitoryReport();
        }

        private void nonPayed_Click(object sender, RoutedEventArgs e)
        {
            if(nonPayed.IsChecked == true)
            {
                evicted.IsEnabled = false;
                evicted.IsChecked = false;
            }
            else
            {
                evicted.IsEnabled = true;
            }
        }
    }
}
