using System.Data;
using System.Data.SqlClient;
using System.Xaml;

namespace ACADtoSQL
{
    public class FinishedDiminsions
    {
        //in this class -1 is the error code for missing data
        public string StyleID { get; set; } = "";
        public double Height { get; set; } = -1;
        public double Width { get; set; } = -1;
        public double Depth { get; set; } = -1;
        public double OSBPitch { get; set; } = -1;
        public double ISBPitch { get; set; } = -1;
        public double Height2Frame { get; set; } = -1;
        public double ArmHeight { get; set; } = -1;
        public double Height2Seam { get; set; } = -1;
        public double Height2Crown { get; set; } = -1;
        public double SeatWidth { get; set; } = -1;
        public double SeatDepth { get; set; } = -1;
        public double ArmWidth { get; set; } = -1;
        public double Diagonal { get; set; } = -1;
        public double BackHeight { get; set; } = -1;
        public double AccentPillows { get; set; } = -1;
        public string PillowType { get; set; } = "None";
        public double BackCushions { get; set; } = -1;
        public string Comments { get; set; } = "No Comment";
        public double ProductWeight { get; set; } = -1; //pounds
        public double PackagedWeight { get; set; } = -1;
        public double CartonedWidth { get; set; } = -1;
        public double CartonedDepth { get; set; } = -1;
        public double CartonedHeight { get; set; } = -1;
        //public string PostNum;
        public double RemovableLegs { get; set; } = -1;  //need front and back data?
        public double ShipAttachedLegs { get; set; } = -1; //legs attached while shipping?
        public double LegShipType { get; set; } = -1; //how ar legs packed
        public double SRHeight { get; set; } = -1; //SeatRail Height
        public double SRLength { get; set; } = -1;
        public double SRThickness { get; set; } = -1;
        public double HeightToSlatBot { get; set; } = -1;
        public double HeightToSideRailBot { get; set; } = -1;
        public double UnderBedOpeningWidth { get; set; } = -1;
        public double UnderBedOpeningDepth { get; set; } = -1;
        //typical data status info
        public string CreateDate { get; set; } = "";
        public string CreateInitials { get; set; } = "";
        public string ApprovInitials { get; set; } = "";
        public string ApprovDate { get; set; } = "";
        public string RevisedDate { get; set; } = "";
        public string RevisionInitials { get; set; } = "";
        public string RevisonNote { get; set; } = "";
        //might use later
        public double FBHeight { get; set; } = -1; //FootboardHeight
        public double FBLength { get; set; } = -1;
        public double FBThickness { get; set; } = -1;
        public double HBHeight { get; set; } = -1; //Heabboard height
        public double HBThickness { get; set; } = -1;
        public string[] MatressOpening { get; set; }
        //need data for special hardware? secional connectors, bed connectors?

    }

    public class StrdProc_DimsAndWeights
    {
        public static void writeToSQL(FinishedDiminsions dims)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //create command obj !!**Need to write stored procedure+  DimensionsRecord
                SqlCommand cmd = new SqlCommand("dbo.DimensionsRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", dims.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Total_Width", dims.Width));
                cmd.Parameters.Add(new SqlParameter("@Total_Depth", dims.Depth));
                cmd.Parameters.Add(new SqlParameter("@Total_Height", dims.Height));
                cmd.Parameters.Add(new SqlParameter("@OSB_Pitch", dims.OSBPitch));
                cmd.Parameters.Add(new SqlParameter("@ISB_Pitch", dims.ISBPitch));
                cmd.Parameters.Add(new SqlParameter("@Height_to_Frame", dims.Height2Frame));
                cmd.Parameters.Add(new SqlParameter("@Arm_Height", dims.ArmHeight));
                cmd.Parameters.Add(new SqlParameter("@Seat_Height_Seam", dims.Height2Seam));
                cmd.Parameters.Add(new SqlParameter("@Seat_Height_Crown", dims.Height2Crown));
                cmd.Parameters.Add(new SqlParameter("@Seat_Width", dims.SeatWidth));
                cmd.Parameters.Add(new SqlParameter("@Seat_Depth", dims.SeatDepth));
                cmd.Parameters.Add(new SqlParameter("@Arm_Width", dims.ArmWidth));
                cmd.Parameters.Add(new SqlParameter("@Diagonal", dims.Diagonal));
                cmd.Parameters.Add(new SqlParameter("@Back_Height", dims.BackHeight));
                cmd.Parameters.Add(new SqlParameter("@Accent_Pillows", dims.AccentPillows));
                cmd.Parameters.Add(new SqlParameter("@Pillow_Type", dims.PillowType));
                cmd.Parameters.Add(new SqlParameter("@Back_Cushions", dims.BackCushions));
                cmd.Parameters.Add(new SqlParameter("@UnCartoned_Weight", dims.ProductWeight));
                cmd.Parameters.Add(new SqlParameter("@Comments", dims.Comments));                
                cmd.Parameters.Add(new SqlParameter("@Cartoned_Width", dims.CartonedWidth));
                cmd.Parameters.Add(new SqlParameter("@Cartoned_Depth", dims.CartonedDepth));
                cmd.Parameters.Add(new SqlParameter("@Cartoned_Height", dims.CartonedHeight));
                cmd.Parameters.Add(new SqlParameter("@Cartoned_Weight", dims.PackagedWeight));              
                cmd.Parameters.Add(new SqlParameter("@Posts_Removable", dims.RemovableLegs));
                cmd.Parameters.Add(new SqlParameter("@Posts_Attached_in_Shipping", dims.ShipAttachedLegs));
                //cmd.Parameters.Add(new SqlParameter("@LegShipType", dims.LegShipType));
                cmd.Parameters.Add(new SqlParameter("@Mattress_Opening_Width", dims.UnderBedOpeningWidth));
                cmd.Parameters.Add(new SqlParameter("@Mattress_Opening_Depth", dims.UnderBedOpeningDepth));
                cmd.Parameters.Add(new SqlParameter("@SideRail_Width", dims.SRThickness));                
                cmd.Parameters.Add(new SqlParameter("@SideRail_Depth", dims.SRLength));
                cmd.Parameters.Add(new SqlParameter("@SideRail_Height", dims.SRHeight));
                cmd.Parameters.Add(new SqlParameter("@Height_to_Slats_Bot", dims.HeightToSlatBot));
                cmd.Parameters.Add(new SqlParameter("@Height_to_SideRail_Bot", dims.HeightToSideRailBot));
                cmd.Parameters.Add(new SqlParameter("@UnderBed_Opening_Width", dims.UnderBedOpeningWidth));
                cmd.Parameters.Add(new SqlParameter("@UnderBed_Opening_Depth", dims.UnderBedOpeningDepth));

                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(dims.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", dims.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", dims.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(dims.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(dims.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", dims.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", dims.RevisonNote));

                ////might use later
                //cmd.Parameters.Add(new SqlParameter("@HBHeight", dims.HBHeight));
                //cmd.Parameters.Add(new SqlParameter("@HBThickness", dims.HBThickness));
                //cmd.Parameters.Add(new SqlParameter("@MatressOpening", dims.MatressOpening));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }
    }
}
