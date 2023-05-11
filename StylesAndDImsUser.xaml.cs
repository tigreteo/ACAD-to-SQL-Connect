using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Xaml;

namespace ACADtoSQL
{
    /// <summary>
    /// Interaction logic for StylesAndDImsUser.xaml
    /// </summary>
    public partial class StylesAndDImsUser : Window
    {
        
        public StylesAndDImsUser()
        {
            InitializeComponent();
        }

        //need to change status label to show full string when cursor is in the textblock


        public void Verify_Click(object sender, RoutedEventArgs e)
        {
            //verify each entry and warn user of invalid data
            List<string> errors = new List<string>();
            errors.Add("The following field(s) failed:");
            string result = "";
            bool pass = true;

            List<System.Windows.Controls.TextBox> fieldsDouble = new List<System.Windows.Controls.TextBox>();
            List<System.Windows.Controls.TextBox> fieldsString = new List<System.Windows.Controls.TextBox>();
            List<System.Windows.Controls.TextBox> fieldsDate = new List<System.Windows.Controls.TextBox>();
            #region fields we're working with
            fieldsDouble.Add(TB_Overall_Width);
            fieldsDouble.Add(TB_Overall_Depth);
            fieldsDouble.Add(TB_Overall_Height);
            fieldsDouble.Add(TB_Height_to_Frame);
            fieldsDouble.Add(TB_Arm_Height);
            fieldsDouble.Add(TB_Seat_Height_Seam);
            fieldsDouble.Add(TB_Seat_Height_Crown);
            fieldsDouble.Add(TB_Seat_Width);
            fieldsDouble.Add(TB_Seat_Depth);
            fieldsDouble.Add(TB_Arm_Width);
            fieldsDouble.Add(TB_Diagonal);
            fieldsDouble.Add(TB_Back_Height);
            fieldsDouble.Add(TB_UnCartoned_Weight);
            fieldsDouble.Add(TB_OSB_Pitch);
            fieldsDouble.Add(TB_ISB_Pitch);
            fieldsDouble.Add(TB_Cartoned_Width);
            fieldsDouble.Add(TB_Cartoned_Depth);
            fieldsDouble.Add(TB_Cartoned_Height);
            fieldsDouble.Add(TB_Cartoned_Weight);
            fieldsDouble.Add(TB_Accent_Pillows);          
            fieldsDouble.Add(TB_Back_Cushions);
            fieldsDouble.Add(TB_Posts_Attached_in_Shipping);
            fieldsDouble.Add(TB_Posts_Removable);
            fieldsDouble.Add(TB_Mattress_Opening_Width);
            fieldsDouble.Add(TB_Mattress_Opening_Depth);
            fieldsDouble.Add(TB_SideRail_Width);
            fieldsDouble.Add(TB_SideRail_Depth);
            fieldsDouble.Add(TB_SideRail_Height);
            fieldsDouble.Add(TB_Height_to_Slats_Bot);
            fieldsDouble.Add(TB_Height_to_SideRail_Bot);
            fieldsDouble.Add(TB_UnderBed_Opening_Width);
            fieldsDouble.Add(TB_UnderBed_Opening_Depth);

            fieldsString.Add(TB_Pillow_Type);
            fieldsString.Add(TB_Comment);
            fieldsString.Add(TB_Initials);
            fieldsString.Add(TB_Revision);
            fieldsString.Add(TB_Rev_Intials);

            fieldsDate.Add(TB_Date);
            fieldsDate.Add(TB_Rev_Date);
            #endregion

            //every verification that fails, add to string builder
            #region verify data in fields is valid types
            foreach(System.Windows.Controls.TextBox tb in fieldsDouble)
            {
                try { Convert.ToDouble(tb.Text); }
                catch
                { errors.Add(tb.Name); pass = false; }         
            }
            //checking for valid date
            foreach(System.Windows.Controls.TextBox tb in fieldsDate)
            {
                try { Convert.ToDateTime(tb.Text); }
                catch
                { errors.Add(tb.Name); pass = false; }
            }
            //checking for valid string - not sure what would fail this
            foreach(System.Windows.Controls.TextBox tb in fieldsString)
            {
                try { Convert.ToString(tb.Text); }
                catch
                { errors.Add(tb.Name); pass = false; }
            }
            #endregion

            //if fail, pass fails to status label
            if (!pass)
            {
                //display to the list for the user
                displayList.ItemsSource = errors;
                displayList.Items.Refresh();
            }

            //pass data back to sql and close window
            if(pass)
            {
                FinishedDiminsions record = new FinishedDiminsions();
                #region load data into a Styles&Dim
                //load all parts of record with verified data
                record.StyleID = StyleId.Text;
                record.Width = Convert.ToDouble(TB_Overall_Width.Text);
                record.Depth = Convert.ToDouble(TB_Overall_Depth.Text);
                record.Height = Convert.ToDouble(TB_Overall_Height.Text);
                record.Height2Frame = Convert.ToDouble(TB_Height_to_Frame.Text);
                record.ArmHeight = Convert.ToDouble(TB_Arm_Height.Text);
                record.Height2Seam = Convert.ToDouble(TB_Seat_Height_Seam.Text);
                record.Height2Crown = Convert.ToDouble(TB_Seat_Height_Crown.Text);
                record.SeatWidth = Convert.ToDouble(TB_Seat_Width.Text);
                record.SeatDepth = Convert.ToDouble(TB_Seat_Depth.Text);
                record.ArmWidth = Convert.ToDouble(TB_Arm_Width.Text);
                record.Diagonal = Convert.ToDouble(TB_Diagonal.Text);
                record.BackHeight = Convert.ToDouble(TB_Back_Height.Text);
                record.Comments = TB_Comment.Text;//Have to sanitize text data
                record.ProductWeight = Convert.ToDouble(TB_UnCartoned_Weight.Text);
                record.PackagedWeight = Convert.ToDouble(TB_Cartoned_Weight.Text);
                record.CartonedWidth = Convert.ToDouble(TB_Cartoned_Width.Text);
                record.CartonedDepth = Convert.ToDouble(TB_Cartoned_Depth.Text);
                record.CartonedHeight = Convert.ToDouble(TB_Cartoned_Height.Text);
                //
                record.AccentPillows = Convert.ToDouble(TB_Accent_Pillows.Text);
                record.PillowType = TB_Pillow_Type.Text; //Have to sanitize text data
                record.RemovableLegs = Convert.ToDouble(TB_Posts_Removable.Text);
                record.ShipAttachedLegs = Convert.ToDouble(TB_Posts_Attached_in_Shipping.Text);
                //record.LegShipType = ; ***what is this again?
                #endregion

                //call method to stored procedure to add to table
                StrdProc_DimsAndWeights.writeToSQL(record);
                Close();
            }
        }
    }

    ////use to keep selected text box ontop of others
    //public class TabZIndexConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var tabItem = values[0] as TabItem;
    //        var tabControl = values[1] as TabControl;
    //        if (tabItem == null || tabControl == null) return Binding.DoNothing;

    //        var count = (int)values[2];

    //        var index = tabControl.ItemContainerGenerator.IndexFromContainer(tabItem);

    //        return count - index;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }
    //}
}
