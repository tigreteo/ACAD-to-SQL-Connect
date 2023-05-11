using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;



namespace ACADtoSQL
{
    //needs to pull data from Excel rather than CAD
    class ExcelReader_Dimensions
    {
        //intially migrate data over from excel first
        public static void getOldDimensions()
        {
            //Currently hardcoding file loc
            //@"Y:\Customers\Style Info\Styles and Dimensions.XLS"
            //Open excel file
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = xlApp.Workbooks.Open(@"C:\Users\thenderson\Desktop\Test Copy of Styles and Dimensions.xls", null, true);
            //Workbook wb = xlApp.Workbooks.Open(@"Y:\Customers\Style Info\Styles and Dimensions.XLS", null, true);
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            if(ws == null)
            { Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.", null, true); }

            //hold of all data
            Range all = ws.UsedRange;
            #region column headers in worksheet
            //column headers are as follows
            //(1)   StyleID
            //(2)   Overall Width
            //(3)   Overall Depth
            //(4)   Overall Height
            //(5)   Height to Frame
            //(6)   Arm Height
            //(7)   Seat Height to Seam
            //(8)   Seat Height to Crown
            //(9)   Seat Width
            //(10)  Seat Depth
            //(11)  Arm Width
            //(12)  Diagonal
            //(13)  Back Height
            //(14)  Comments
            //(15)  Product Wieght in lbs
            //(16)  Cartoned Weight
            //(17)  Cartoned Width
            //(18)  Cartoned Depth
            //(19)  Cartoned Height
            //(20)  Leg Part#
            //(21)  Accent Pillows
            //(22)  Type|Accent Pillow
            //(23)  Are Legs Removable
            //(24)  Legs Attached in Shipping
            //(25)  How are legs packed
            #endregion

            //first of data starts on row 3
            for (int iRow =3; iRow < all.Rows.Count; iRow++)
            {
                FinishedDiminsions record = new FinishedDiminsions();

                //load each row into a record
                //Instead need to pass all of this to the user window for review

                var dimsWindow = new StylesAndDImsUser();
                #region load excel data to the window             
                dimsWindow.StyleId.Text = getCellString(iRow, 1, all);
                dimsWindow.TB_Overall_Width.Text = getCellString(iRow, 2, all);
                dimsWindow.TB_Overall_Depth.Text = getCellString(iRow, 3, all);
                dimsWindow.TB_Overall_Height.Text = getCellString(iRow, 4, all);
                dimsWindow.TB_Height_to_Frame.Text = (getCellString(iRow, 5, all));
                dimsWindow.TB_Arm_Height.Text = (getCellString(iRow, 6, all));
                dimsWindow.TB_Seat_Height_Seam.Text = (getCellString(iRow, 7, all));
                dimsWindow.TB_Seat_Height_Crown.Text = (getCellString(iRow, 8, all));
                dimsWindow.TB_Seat_Width.Text = (getCellString(iRow, 9, all));
                dimsWindow.TB_Seat_Depth.Text = (getCellString(iRow, 10, all));
                dimsWindow.TB_Arm_Width.Text = (getCellString(iRow, 11, all));
                dimsWindow.TB_Diagonal.Text = (getCellString(iRow, 12, all));
                dimsWindow.TB_Back_Height.Text = (getCellString(iRow, 13, all));
                dimsWindow.TB_Comment.Text = getCellString(iRow, 14, all);
                dimsWindow.TB_UnCartoned_Weight.Text = (getCellString(iRow, 15, all));
                dimsWindow.TB_Cartoned_Weight.Text = (getCellString(iRow, 16, all));
                dimsWindow.TB_Cartoned_Width.Text = (getCellString(iRow, 17, all));
                dimsWindow.TB_Cartoned_Depth.Text = (getCellString(iRow, 18, all));
                dimsWindow.TB_Cartoned_Height.Text = (getCellString(iRow, 19, all));
                //getCellValue(iRow, 20, all); leg type only tracked early on, stored in BoM anyway
                dimsWindow.TB_Accent_Pillows.Text = (getCellString(iRow, 21, all));
                dimsWindow.TB_Pillow_Type.Text = getCellString(iRow, 22, all);
                dimsWindow.TB_Posts_Removable.Text = (getCellString(iRow, 23, all));
                dimsWindow.TB_Posts_Attached_in_Shipping.Text = (getCellString(iRow, 24, all));
                //dimsWindow = (getCellString(iRow, 25, all)); looks like it used to use a code QS

                #endregion
                dimsWindow.ShowDialog();
            }
            wb.Close();
            xlApp.Visible = false;
        }

        private static string getCellString(int row, int column, Range rng)
        {
            var cellValue = "";
            try
            { cellValue = (string)(rng.Cells[row, column] as Range).Value; }
            catch (NullReferenceException)
            { cellValue = null; }
            //catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            //{ 
            //DateTime dt = (DateTime)(rng.Cells[row, column] as Range).Value;
            //  cellValue = dt.ToString();
            //}
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                var dt = (rng.Cells[row, column] as Range).Value;
                cellValue = dt.ToString();
            }

            return cellValue;
        }

        private static double StringToDoubleWithError(string cellValue)
        {
            try
            {return Convert.ToDouble(cellValue);}
            catch
            {
                return -1; //if negative one, then perhaps just flag so it can be fixed by hand
            }
        }

        //user entered data to fill out both SQL and maintain old Excel Sheet
    }


}
