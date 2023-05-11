using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct patRec
    {
        public string StyleID;
        public string Complete;

        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }

    public class StrdProc_Pattern
    {
        public static void writetoSQL(patRec pattern)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //create command obj
                SqlCommand cmd = new SqlCommand("dbo.PatternRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", pattern.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Complete", pattern.Complete));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(pattern.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", pattern.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", pattern.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(pattern.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(pattern.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", pattern.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", pattern.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }

    }
}
