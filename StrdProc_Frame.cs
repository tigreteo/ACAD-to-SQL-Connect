using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct frameRec
    {
        public string StyleID;
        public string Plywood;
        public string Plywood_Yield;
        public string Assembly_Notes;

        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }

    public class StrdProc_Frame
    {
        public static void writetoSQL(frameRec frmRec)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connections to db
                sqlDB.Open();

                //create command obj
                SqlCommand cmd = new SqlCommand("dbo.FrameRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Plywood", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Plywood_Yield", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Assembly_Notes", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("", frmRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("", frmRec.StyleID));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(frmRec.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", frmRec.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", frmRec.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(frmRec.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(frmRec.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", frmRec.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", frmRec.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }
    }
}
