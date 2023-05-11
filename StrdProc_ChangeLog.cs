using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct changeLog
    {
        public string StyleID;
        public string Notes;
        public string Category;
        public string ChangeNumber;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }
    public class StrdProc_ChangeLog
    {
        public static void writetoSQL(changeLog log)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //Use first command to create/upate the record of the Cardboard Spec
                //create command obj !!**Need to write stored procedure+ in SQL still
                SqlCommand cmd = new SqlCommand("dbo.ChangeLog", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", log.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", log.Notes));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", log.Category));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", log.ChangeNumber));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(log.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", log.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", log.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(log.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(log.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", log.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", log.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();

                sqlDB.Close();
            }
        }
    }
}
