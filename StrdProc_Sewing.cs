using System;
using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct SewPnlRecord
    {
        public string StyleID;
        public string PanelName;
        public double SeamLength;
        public string SeamType; //TopStich, SingleNeedle, DoubleNeedle, Welt....
        public double PullLength;
        public string PullType;
        public double ZipperLength;
        public double VelcroLength;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;

    }

    public class StrdProc_Sewing
    {
        public static void writeToSQL(SewPnlRecord panel)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //create command obj !!**Need to write stored procedure+
                SqlCommand cmd = new SqlCommand("dbo.SewingRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", panel.StyleID));
                cmd.Parameters.Add(new SqlParameter("@PanelName", panel.PanelName));
                cmd.Parameters.Add(new SqlParameter("@SeamLength", panel.SeamLength));
                cmd.Parameters.Add(new SqlParameter("@SeamType", panel.SeamType));
                cmd.Parameters.Add(new SqlParameter("@PullLength", panel.PullLength));
                cmd.Parameters.Add(new SqlParameter("@PullType", panel.PullType));
                cmd.Parameters.Add(new SqlParameter("@ZipperLength", panel.ZipperLength));
                cmd.Parameters.Add(new SqlParameter("@VelcroLength", panel.VelcroLength));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(panel.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", panel.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", panel.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(panel.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(panel.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", panel.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", panel.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }
    }
}
