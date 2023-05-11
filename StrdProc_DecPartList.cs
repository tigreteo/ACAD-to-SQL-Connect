using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct partList
    {
        public string StyleID;
        public string PartNumber;
        public string Quantity;
        public string Part_Description;
        public string Category;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }
    public class StrdProc_DecPartList
    {
        public static void writetoSQL(partList parts)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //Use first command to create/upate the record of the Cardboard Spec
                //create command obj !!**Need to write stored procedure+
                SqlCommand cmd = new SqlCommand("dbo.DevPartsListRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", parts.StyleID));
                cmd.Parameters.Add(new SqlParameter("@PartNumber", parts.PartNumber));
                cmd.Parameters.Add(new SqlParameter("@Quantity", parts.Quantity));
                cmd.Parameters.Add(new SqlParameter("@Part_Description", parts.Part_Description));
                cmd.Parameters.Add(new SqlParameter("@Category", parts.Category));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(parts.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", parts.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", parts.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(parts.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(parts.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", parts.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", parts.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();

                sqlDB.Close();
            }
        }
    }
}
