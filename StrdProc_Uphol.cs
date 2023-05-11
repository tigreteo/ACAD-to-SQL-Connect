using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct UpholRec
    {
        public string StyleID;
        public string Uphol_Area;
        public string Total_Cotton;
        public string Roll_Poly;
        public string Templates;
        public string Sketches;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }
    public class StrdProc_Uphol
    {
        public static void writetoSQL(UpholRec Uphol)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //Create command object
                SqlCommand cmd = new SqlCommand("dbo.UpholRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", Uphol.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Uphol_Area", Uphol.Uphol_Area));
                cmd.Parameters.Add(new SqlParameter("@Total_Cotton", Uphol.Total_Cotton));
                cmd.Parameters.Add(new SqlParameter("@Roll_Poly", Uphol.Roll_Poly));
                cmd.Parameters.Add(new SqlParameter("@Templates", Uphol.Templates));
                cmd.Parameters.Add(new SqlParameter("@Sketches", Uphol.Sketches));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(Uphol.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", Uphol.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", Uphol.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(Uphol.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(Uphol.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", Uphol.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", Uphol.RevisonNote));


                //execute the procedure
                cmd.ExecuteNonQuery();
                //a using statment should close it, but good habit to close
                sqlDB.Close();
            }
        }
    }
}
