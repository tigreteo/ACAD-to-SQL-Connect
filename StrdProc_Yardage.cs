using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct ydgRecord
    {
        //paramter declarations
        public string StyleId ;//try to pull from the folder name to account for typos in the form
        public string FabNum;
        public string RollDirection;
        public string StyleID;
        //public string Initials;
        public string Holder;
        //public string SignDate;//approval date
        public string PhysIN;
        public string PhysYDS;
        public string Yards;
        public string Inches;
        public string WeltNote;
        public double CadYdg;
        public double PhysYdg;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;

        #region  parameters
        //public ydgRecord(
        //    string styleId,
        //    string fabNum,
        //    string rollDirection,
        //    string styleID,
        //    string initials,
        //    string holder,
        //    string signer,
        //    string signDate,
        //    string physIN,
        //    string physYDS,
        //    string yards,
        //    string inches,
        //    string weltNote,
        //    double cadYdg,
        //    double physYdg,
        //    string createDate,
        //    string revisionDate,
        //    string revisionNote)
        //{
        //    StyleId = styleId;
        //    FabNum = fabNum;
        //    RollDirection = rollDirection;
        //    StyleID = styleID;
        //    Initials = initials;
        //    Holder = holder;
        //    Signer = signer;
        //    SignDate = signDate;
        //    PhysIN = physIN;
        //    PhysYDS = physYDS;
        //    Yards = yards;
        //    Inches = inches;
        //    WeltNote = weltNote;
        //    CadYdg = cadYdg;
        //    PhysYdg = physYdg;
        //    CreateDate = createDate;
        //    RevisionDate = revisionDate;
        //    RevisionNote = revisionNote;
            
        //}

        #endregion
    }

    public class StrdProc_Yardage
    {
        public static void writetoSQL(ydgRecord fabRec)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to DB
                sqlDB.Open();

                //create command object for stored procedure
                SqlCommand cmd = new SqlCommand("dbo.YardagesRecord", sqlDB);

                //set up comand as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", fabRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Orientation", fabRec.RollDirection));
                cmd.Parameters.Add(new SqlParameter("@Physical_Yardage", fabRec.PhysYdg));
                cmd.Parameters.Add(new SqlParameter("@CAD_Yardage", fabRec.CadYdg));
                cmd.Parameters.Add(new SqlParameter("@Fabric#", fabRec.FabNum));
                cmd.Parameters.Add(new SqlParameter("@Fabric_Width", ""));
                cmd.Parameters.Add(new SqlParameter("@Fabric_Repeat", ""));
                cmd.Parameters.Add(new SqlParameter("@Welt_Note", fabRec.WeltNote));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(fabRec.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", fabRec.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", fabRec.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(fabRec.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(fabRec.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", fabRec.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", fabRec.RevisonNote));


                //execute the procedure
                cmd.ExecuteNonQuery();
                //a using statment should close it, but good habit to close
                sqlDB.Close();
            }
          
        }
    }




}
