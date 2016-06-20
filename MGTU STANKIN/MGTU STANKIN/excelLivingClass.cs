using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql_Class;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
using io = System.IO;
using excel = Microsoft.Office.Interop.Excel;

namespace MGTU_STANKIN
{
    static class excelLivingClass
    {
        public static void fullReport_withoutE(excel.Application application, excel.Workbook workBook, excel.Worksheet worksheet, MySqlConnection globalConnection, int line)
        {
            string cell = "";

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID, Room FROM Students WHERE evicted = 0 order by Room;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daST = new MySqlDataAdapter(selST);
            DataTable dtST = new DataTable();
            daST.Fill(dtST);
            var myDataST = dtST.Select();

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT Surname, Name, ID, Room FROM OtherLiving WHERE evicted = 0 order by Room;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daOL = new MySqlDataAdapter(selOL);
            DataTable dtOL = new DataTable();
            daOL.Fill(dtOL);
            var myDataOL = dtOL.Select();

            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Студенты";
            line += 2;

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "Комната";
            cell = "D" + line.ToString();
            worksheet.Range[cell].Value = "Проживающий";
            line += 2;

            for(int i = 0; i < myDataST.Length; i++)
            {
                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = myDataST[i].ItemArray[3].ToString();

                cell = "D" + line.ToString();
                worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();

                line += 1;
            }

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Иные проживающие";
            line += 2;

            cell = "B" + line.ToString();
            worksheet.Range[cell].Value = "Комната";
            cell = "D" + line.ToString();
            worksheet.Range[cell].Value = "Проживающий";
            line += 2;

            for (int i = 0; i < myDataOL.Length; i++ )
            {
                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = myDataOL[i].ItemArray[3].ToString();

                cell = "D" + line.ToString();
                worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString();

                line += 1;
            }           
        }

        public static void fullReport_Date(excel.Application application, excel.Workbook workBook, excel.Worksheet worksheet, MySqlConnection globalConnection, int line, DateTime toDate)
        {
            string cell = "";
            string date = toDate.ToString("yyyy-MM-dd H:mm:ss");
            int counter = 1;

            MySqlCommand selSt = new MySqlCommand();
            selSt.Connection = globalConnection;
            selSt.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTill, StayLimit FROM Students ORDER BY Surname;";
            selSt.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selSt);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT Surname, Name, ID, evicted, EvictedTill, StayLimit FROM Otherliving ORDER BY Surname;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daO = new MySqlDataAdapter(selOL);
            DataTable dtO = new DataTable();
            daO.Fill(dtO);
            var myDataOL = dtO.Select();

            line += 2;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Студенты";
            line += 2;

            for(int i = 0; i < myDataST.Length; i++)
            {
                if (Convert.ToDateTime(myDataST[i].ItemArray[5]) >= Convert.ToDateTime(date))
                    if ((Convert.ToBoolean(myDataST[i].ItemArray[3]) == true && Convert.ToDateTime(myDataST[i].ItemArray[4]) < Convert.ToDateTime(date)) || Convert.ToBoolean(myDataST[i].ItemArray[3]) == false)
                    {
                        cell = "B" + line.ToString();
                        worksheet.Range[cell].Value = counter.ToString();

                        cell = "D" + line.ToString();
                        worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                        line += 1;
                        counter += 1;
                    }
            }

            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Иные проживающие";
            line += 2;

            counter = 1;

            for (int i = 0; i < myDataOL.Length; i++)
            {
                 if (Convert.ToDateTime(myDataOL[i].ItemArray[5]) >= Convert.ToDateTime(date))
                     if ((Convert.ToBoolean(myDataOL[i].ItemArray[3]) == true && Convert.ToDateTime(myDataOL[i].ItemArray[4]) < Convert.ToDateTime(date)) || Convert.ToBoolean(myDataOL[i].ItemArray[3]) == false)
                     {
                         cell = "B" + line.ToString();
                         worksheet.Range[cell].Value = counter.ToString();

                         cell = "D" + line.ToString();
                         worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString();
                         line += 1;
                         counter += 1;
                     }
            }
        }

        public static void fullReport_nonPayed(excel.Application application, excel.Workbook workBook, excel.Worksheet worksheet, MySqlConnection globalConnection, int line, DateTime toDate)
        {
            string payDate = toDate.ToString("yyyy-MM-dd H:mm:ss");
            string cell = "";
            int counter = 1;
            

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID FROM Students WHERE evicted = 0 AND Date < '" + payDate + "' ORDER BY Surname;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT Surname, Name, ID FROM OtherLiving WHERE evicted = 0 AND Date < '" + payDate + "' ORDER BY Surname;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daO = new MySqlDataAdapter(selOL);
            DataTable dtO = new DataTable();
            daO.Fill(dtO);
            var myDataOL = dtO.Select();

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Студенты";
            line += 2;

            for(int i = 0; i < myDataST.Length; i++)
            {
                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = counter;

                cell = "D" + line.ToString();
                worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();

                line += 1;
                counter += 1;
            }

            counter = 1;
            line += 1;

            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Иные проживающие";
            line += 2;

            string cell1 = "D" + line.ToString();

            for(int i = 0; i < myDataOL.Length; i++)
            {
                cell = "B" + line.ToString();
                worksheet.Range[cell].Value = counter;

                cell = "D" + line.ToString();
                worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString();

                line += 1;
                counter += 1;
            }
            
            worksheet.get_Range(cell1,cell).EntireColumn.AutoFit();
        }

        public static void fullReport_evicted(excel.Application application, excel.Workbook workBook, excel.Worksheet worksheet, MySqlConnection globalConnection, int line)
        {
            int counter = 1;
            string cell = "";
            excel.Range excelcells;

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTiLL FROM Students ORDER BY Surname;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Студенты";
            line += 2;

            for(int i = 0; i < myDataST.Length; i++)
            {
                if(Convert.ToBoolean(myDataST[i].ItemArray[3]) == true)
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = (i + 1).ToString();
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString() + " " + "(выселен до: "+ ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Day).ToString() + "." +  ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Month).ToString()+ "." + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Year).ToString() + " г. )";
                    excelcells = worksheet.get_Range("B" + line.ToString(), "D" + line.ToString());
                    excelcells.Font.Bold = true;
                    line += 1;
                }
                else
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = i + 1;
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                    line += 1;
                }
            }

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT Surname, Name, ID, evicted, EvictedTill FROM OtherLiving ORDER BY Surname;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daO = new MySqlDataAdapter(selOL);
            DataTable dtO = new DataTable();
            daO.Fill(dtO);
            var myDataOL = dtO.Select();

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Иные проживающие";
            line += 2;

            for (int i = 0; i < myDataOL.Length; i++)
            {
                if (Convert.ToBoolean(myDataOL[i].ItemArray[3]) == true)
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = (i + 1).ToString();
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString() + " " + "(выселен до: " + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Day).ToString() + "." + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Month).ToString() + "." + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Year).ToString() + " г. )";
                    excelcells = worksheet.get_Range("B" + line.ToString(), "D" + line.ToString());
                    excelcells.Font.Bold = true;
                    line += 1;
                }
                else
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = i + 1;
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString();
                    line += 1;
                }
            }

        }

        public static void fullReport_EvictedDate(excel.Application application, excel.Workbook workBook, excel.Worksheet worksheet, MySqlConnection globalConnection, int line, DateTime toDate)
        {
            string cell = "";
            string tillDate = toDate.ToString("yyyy-MM-dd H:mm:ss");
            excel.Range excelcells;

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Студенты";
            line += 2;

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTill FROM Students WHERE StayLimit > '" + tillDate + "' ORDER BY Surname;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            for (int i = 0; i < myDataST.Length; i++)
            {
                if(Convert.ToBoolean(myDataST[i].ItemArray[3]) == true && Convert.ToDateTime(myDataST[i].ItemArray[4]) > Convert.ToDateTime(tillDate))
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = (i + 1).ToString();
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString() + " " + "(выселен до: " + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Day).ToString() + "." + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Month).ToString() + "." + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Year).ToString() + " г. )";
                    excelcells = worksheet.get_Range("B" + line.ToString(), "D" + line.ToString());
                    excelcells.Font.Bold = true;
                    line += 1;
                }
                else
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = i + 1;
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                    line += 1;
                }
            }

            line += 1;
            cell = "C" + line.ToString();
            worksheet.Range[cell].Value = "Иные проживающие";
            line += 2;

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            selOL.CommandText = "SELECT Surname, Name, ID, evicted, EvictedTill FROM OtherLiving WHERE StayLimit > '" + tillDate + "' ORDER BY Surname;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daO = new MySqlDataAdapter(selOL);
            DataTable dtO = new DataTable();
            daO.Fill(dtO);
            var myDataOL = dtO.Select();

            for(int i = 0; i < myDataOL.Length; i++)
            {
                if (Convert.ToBoolean(myDataOL[i].ItemArray[3]) == true && Convert.ToDateTime(myDataOL[i].ItemArray[4]) > Convert.ToDateTime(tillDate))
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = (i + 1).ToString();
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString() + " " + "(выселен до: " + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Day).ToString() + "." + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Month).ToString() + "." + ((Convert.ToDateTime(myDataOL[i].ItemArray[4])).Year).ToString() + " г. )";
                    excelcells = worksheet.get_Range("B" + line.ToString(), "D" + line.ToString());
                    excelcells.Font.Bold = true;
                    line += 1;
                }
                else
                {
                    cell = "B" + line.ToString();
                    worksheet.Range[cell].Value = i + 1;
                    cell = "D" + line.ToString();
                    worksheet.Range[cell].Value = myDataOL[i].ItemArray[0].ToString() + " " + myDataOL[i].ItemArray[1].ToString() + " " + myDataOL[i].ItemArray[2].ToString();
                    line += 1;
                }
            }
        }

        public static int facultyHeader(excel.Application app, int line, string housingNumber, int counter, bool inside, bool isNonPayed, DateTime date, bool isEvicted, MySqlConnection globalConnection)
        {
            excel.Sheets sheets = app.ActiveWorkbook.Worksheets;
            excel.Worksheet workSheet;
            int oldLine = line;
            string cell = "";
            string mounth = "";

            switch (date.Month.ToString())
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

            for(int i = 0; i < counter; i++)
            {
                workSheet = (excel.Worksheet)sheets.get_Item(i + 1);
                line = oldLine;
                excel.Range excelcells;

                if(inside == false)
                {
                    cell = "B" + line.ToString();
                    workSheet.Range[cell].Value = "Утверждаю";
                    line += 1;

                    cell = "B" + line.ToString();
                    workSheet.Range[cell].Value = "Проректор по АХР";
                    line += 1;

                    cell = "B" + line.ToString();
                    workSheet.Range[cell].Value = "_________________________";
                    line += 1;

                    cell = "D" + line.ToString();
                    workSheet.Range[cell].Value = "Золотарев И.В.";
                    line = 8;
                }

                cell = "D" + line.ToString();
                workSheet.Range[cell].Value = "Список студентов факультета " + workSheet.Name + ",";
                line += 1;

                cell = "D" + line.ToString();
                workSheet.Range[cell].Value = "проживающих в общежитии № " + housingNumber;
                line += 1;

                if(isNonPayed == true)
                {
                    if (date == DateTime.Now.Date)
                    {
                        cell = "D" + line.ToString();
                        workSheet.Range[cell].Value = "которые не оплатили проживание по состояню на " + DateTime.Now.Day + " " + mounth + " " + DateTime.Now.Year + " г.";
                        line += 2;
                    }
                    else
                    {
                        cell = "D" + line.ToString();
                        workSheet.Range[cell].Value = "у которых заканчивается срок оплаты к " + date.Day + " " + mounth + " " + date.Year + " г.";
                        line += 2;
                    }
                }
                else
                {
                    if (date == DateTime.Now.Date)
                    {
                        cell = "D" + line.ToString();
                        workSheet.Range[cell].Value = "по состояню на " + DateTime.Now.Day + " " + mounth + " " + DateTime.Now.Year + " г.";
                        line += 2;
                    }
                    else
                    {
                        line -= 1;
                        cell = "D" + line.ToString();
                        workSheet.Range[cell].Value = "которые будут проживать в общежитии № " + housingNumber;

                        line += 1;
                        cell = "D" + line.ToString();
                        workSheet.Range[cell].Value = "c " + date.Day + " " + mounth + " " + date.Year + " г.";
                        line += 2;
                    }
                }

                excelcells = workSheet.get_Range("D" + oldLine.ToString(), "D" + (line - 1).ToString());
                excelcells.HorizontalAlignment = excel.Constants.xlCenter;

            }

            for (int i = 0; i < counter; i++ )
            {
                workSheet = (excel.Worksheet)sheets.get_Item(i + 1);
                if (isNonPayed == false && date.Date == DateTime.Now.Date && isEvicted == false)
                    facultyFullReport(app, workSheet, globalConnection, line);
                if (isNonPayed == false && date.Date != DateTime.Now.Date && isEvicted == false)
                    facultyDateReport(app, workSheet, globalConnection, line, date);
                if (isNonPayed == true && date.Date == DateTime.Now.Date && isEvicted == false)
                    facultyNonPayed(app, workSheet, globalConnection, line, DateTime.Now.Date);
                if (isNonPayed == true && date.Date != DateTime.Now.Date && isEvicted == false)
                    facultyNonPayed(app, workSheet, globalConnection, line, date);
                if (isNonPayed == false && date.Date == DateTime.Now.Date && isEvicted == true)
                    facultyEvicted(app, workSheet, globalConnection, line, DateTime.Now.Date);
                if (isNonPayed == false && date.Date != DateTime.Now.Date && isEvicted == true)
                    facultyEvicted(app, workSheet, globalConnection, line, date);
            }
                return line;
        }

        public static void facultyFullReport(excel.Application application, excel.Worksheet workSheet, /*string[] faculty,*/ MySqlConnection globalConnection, int line)
        {
            //application.Sheets.Add();
            //(application.Sheets[2] as excel.Worksheet).Range["A1"].Value = "Привет лист 1";
            string cell = "";

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID FROM Students WHERE Faculty = '" + workSheet.Name + "' AND evicted = 0 ORDER BY Surname;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            line += 1;

            for(int i = 0; i < myDataST.Length; i++)
            {
                cell = "B" + line.ToString();
                workSheet.Range[cell].Value = i + 1;
                cell = "D" + line.ToString();
                workSheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                line += 1;
            }
        }

        public static void facultyDateReport(excel.Application application, excel.Worksheet workSheet, MySqlConnection globalConnection, int line, DateTime date)
        {
            string cell = "";
            string selDate = date.ToString("yyyy-MM-dd");
            
            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTill FROM Students WHERE Faculty = '" + workSheet.Name + "' AND StayLimit > '" + selDate + "' AND (evicted = 0 OR (evicted = 1 AND EvictedTill =< '" + selDate + "')) ORDER BY Surname;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            line += 1;

            for (int i = 0; i < myDataST.Length; i++)
            {
                cell = "B" + line.ToString();
                workSheet.Range[cell].Value = i + 1;
                cell = "D" + line.ToString();
                workSheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                line += 1;
            }
        }

        private static void facultyNonPayed(excel.Application application, excel.Worksheet workSheet, MySqlConnection globalConnection, int line, DateTime date)
        {
            string tooday = date.Date.ToString("yyyy-MM-dd");
            string cell = "";

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            selST.CommandText = "SELECT Surname, Name, Student_ID FROM Students WHERE Faculty = '" + workSheet.Name + "' AND Date < '" + tooday + "' AND evicted = 0;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            for(int i = 0; i < myDataST.Length; i++)
            {
                cell = "B" + line.ToString();
                workSheet.Range[cell].Value = i + 1;
                cell = "D" + line.ToString();
                workSheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                line += 1;
            }
        }

        private static void facultyEvicted(excel.Application application, excel.Worksheet workSheet, MySqlConnection globalConnection, int line, DateTime date)
        {
            string tooday = date.Date.ToString("yyyy-MM-dd");
            string cell = "";
            excel.Range excelcells;

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            if (date.Date == DateTime.Now.Date)
                selST.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTill FROM Students WHERE Faculty = '" + workSheet.Name + "';";
            else
                selST.CommandText = "SELECT Surname, Name, Student_ID, evicted, EvictedTill FROM Students WHERE Faculty = '" + workSheet.Name + "' AND StayLimit > '" + tooday + "';";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            for(int i = 0; i < myDataST.Length; i++)
            {
                if (Convert.ToBoolean(myDataST[i].ItemArray[3]) == true && Convert.ToDateTime(myDataST[i].ItemArray[4]) > Convert.ToDateTime(tooday))
                {
                    cell = "B" + line.ToString();
                    workSheet.Range[cell].Value = (i + 1).ToString();
                    cell = "D" + line.ToString();
                    workSheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString() + " " + "(выселен до: " + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Day).ToString() + "." + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Month).ToString() + "." + ((Convert.ToDateTime(myDataST[i].ItemArray[4])).Year).ToString() + " г. )";
                    excelcells = workSheet.get_Range("B" + line.ToString(), "D" + line.ToString());
                    excelcells.Font.Bold = true;
                    line += 1;
                }
                else
                {
                    cell = "B" + line.ToString();
                    workSheet.Range[cell].Value = i + 1;
                    cell = "D" + line.ToString();
                    workSheet.Range[cell].Value = myDataST[i].ItemArray[0].ToString() + " " + myDataST[i].ItemArray[1].ToString() + " " + myDataST[i].ItemArray[2].ToString();
                    line += 1;
                }
            }
        }

        public static string[] dormitoryReport(MySqlConnection globalConnection, DateTime date)
        {
            string sDate = date.Date.ToString("yyyy-MM-dd");
            string[] resault = new string[11];

            int stCount = 0;
            int olCount = 0;
            int roomCount = 0;
            int placesCount = 0;
            int fPlacesCount = 0;
            int _2p = 0;
            int _3p = 0;
            int _4p = 0;
            int emptRoom = 0;
            int famRoom = 0;
            int k = 0;
            int t = 0;
            bool fl = false;
            int stRoom = 0;

            MySqlCommand selST = new MySqlCommand();
            selST.Connection = globalConnection;
            if(date.Date == DateTime.Now.Date)
                selST.CommandText = "SELECT Room, StayLimit, evicted, EvictedTill FROM Students WHERE evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "') ORDER BY Room;";
            else
                selST.CommandText = "SELECT Room, StayLimit, evicted, EvictedTill FROM Students WHERE StayLimit > '" + sDate + "' AND (evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "')) ORDER BY Room;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daS = new MySqlDataAdapter(selST);
            DataTable dtS = new DataTable();
            daS.Fill(dtS);
            var myDataST = dtS.Select();

            if (date.Date == DateTime.Now.Date)
                selST.CommandText = "SELECT count(*) FROM Students WHERE evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "') ORDER BY Room;";
            else
                selST.CommandText = "SELECT count(*) FROM Students WHERE StayLimit > '" + sDate + "' AND (evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "')) ORDER BY Room;";
            selST.ExecuteNonQuery();

            MySqlDataAdapter daSC = new MySqlDataAdapter(selST);
            DataTable dtSC = new DataTable();
            daSC.Fill(dtSC);
            var STCount = dtSC.Select();

            stCount = Convert.ToInt32(STCount[0].ItemArray[0]);

            MySqlCommand selOL = new MySqlCommand();
            selOL.Connection = globalConnection;
            if (date.Date == DateTime.Now.Date)
                selOL.CommandText = "SELECT Room, StayLimit, evicted, EvictedTill FROM OtherLiving WHERE evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "') ORDER BY Room;";
            else
                selOL.CommandText = "SELECT Room, StayLimit, evicted, EvictedTill FROM OtherLiving WHERE StayLimit > '" + sDate + "' AND (evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "')) ORDER BY Room;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daO = new MySqlDataAdapter(selOL);
            DataTable dtO = new DataTable();
            daO.Fill(dtO);
            var myDataOL = dtO.Select();

            if (date.Date == DateTime.Now.Date)
                selOL.CommandText = "SELECT count(*) FROM OtherLiving WHERE evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "') ORDER BY Room;";
            else
                selOL.CommandText = "SELECT count(*) FROM OtherLiving WHERE StayLimit > '" + sDate + "' AND (evicted = 0 OR (evicted = 1 AND EvictedTill <= '" + sDate + "')) ORDER BY Room;";
            selOL.ExecuteNonQuery();

            MySqlDataAdapter daOC = new MySqlDataAdapter(selOL);
            DataTable dtOC = new DataTable();
            daOC.Fill(dtOC);
            var OLCount = dtOC.Select();

            olCount = Convert.ToInt32(OLCount[0].ItemArray[0]);

            MySqlCommand selR = new MySqlCommand();
            selR.Connection = globalConnection;
            selR.CommandText = "SELECT * FROM Rooms;";
            selR.ExecuteNonQuery();

            MySqlDataAdapter daR = new MySqlDataAdapter(selR);
            DataTable dtR = new DataTable();
            daR.Fill(dtR);
            var myDataR = dtR.Select();

            roomCount = myDataR.Length;

            for (int i = 0; i < myDataR.Length; i++ )
            {
                placesCount += Convert.ToInt32(myDataR[i].ItemArray[2]);
                switch(Convert.ToInt32(myDataR[i].ItemArray[2]))
                {
                    case 2: _2p += 1; break;
                    case 3: _3p += 1; break;
                    case 4: _4p += 1; break;
                }
                if (myDataR[i].ItemArray[4].ToString() == "")
                    emptRoom += 1;
                /*if (date.Date != DateTime.Now.Date)
                {
                   
                    if (Convert.ToInt32(STCount[0].ItemArray[0]) != 0)
                    {
                        try
                        {
                            while (Convert.ToInt32(myDataST[k].ItemArray[0]) == Convert.ToInt32(myDataR[i].ItemArray[1]))
                            {
                                if (Convert.ToDateTime(myDataST[k].ItemArray[1]) < Convert.ToDateTime(sDate) && Convert.ToBoolean(myDataST[k].ItemArray[3]) == false)
                                    fPlacesCount += 1;
                                k += 1;
                                try
                                {
                                    stRoom = Convert.ToInt32(myDataST[k].ItemArray[0]);
                                    fl = true;
                                }
                                catch
                                { break; }
                            }
                        }
                        catch
                        { goto r; }
                    r: if (Convert.ToInt32(OLCount[0].ItemArray[0]) != 0)
                            try
                            {
                                while (Convert.ToInt32(myDataOL[t].ItemArray[0]) == Convert.ToInt32(myDataR[i].ItemArray[1]))
                                {
                                    if (Convert.ToDateTime(myDataOL[t].ItemArray[1]) < Convert.ToDateTime(sDate) && Convert.ToBoolean(myDataOL[t].ItemArray[3]) == false)
                                        fPlacesCount += 1;
                                    t += 1;
                                    try
                                    {
                                        stRoom = Convert.ToInt32(myDataOL[t].ItemArray[0]);
                                        fl = true;
                                    }
                                    catch
                                    { break; }
                                }
                            }
                        catch
                            { goto r1; }
                    r1: if (fl == false && myDataR[i].ItemArray[4].ToString() != "")
                            emptRoom += 1;

                        if (fPlacesCount + Convert.ToInt32(myDataR[i].ItemArray[3]) == Convert.ToInt32(myDataR[i].ItemArray[2]))
                            emptRoom += 1;
                        fPlacesCount = 0;
                        if (myDataR[i].ItemArray[4].ToString() == "c")
                            famRoom += 1;
                    }
                }*/
                
            }

            if (Convert.ToInt32(myDataST[0].ItemArray[0]) == 0)
                emptRoom = roomCount;

            resault[0] = roomCount.ToString();
            resault[1] = _2p.ToString();
            resault[2] = _3p.ToString();
            resault[3] = _4p.ToString();
            resault[4] = placesCount.ToString();
            resault[5] = (stCount + olCount).ToString();
            resault[6] = stCount.ToString();
            resault[7] = olCount.ToString();
            resault[8] = (placesCount - Convert.ToInt32(resault[5])).ToString();
            if (date.Date == DateTime.Now.Date)
            {
                resault[9] = emptRoom.ToString();
                resault[10] = famRoom.ToString();
            }
            else
            {
                resault[9] = "-";
                resault[10] = "-";
            }

                return resault;
        }
    }
}
