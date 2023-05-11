using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct cbRecord
    {
        public string StyleId;
        public string PartNumber;        
        public double Length;
        public double Width;
        public double Thickness;
        public double Quantity;
        public string Grain;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;

        //#region parameters
        //public cbRecord(
        //    string styleId,
        //    string partNumber,
        //    string createDate,
        //    double length,
        //    double width,
        //    double thickness,
        //    double quantity,
        //    string grain,
        //    string approveDate,
        //    string reviseDate,
        //    string revisenote)
        //{
        //    StyleId = styleId;
        //    PartNumber = partNumber;
        //    CreateDate = createDate;
        //    Length = length;
        //    Width = width;
        //    Thickness = thickness;
        //    Quantity = quantity;
        //    Grain = grain;
        //    ApprovDate = approveDate;
        //    ReviseDate = reviseDate;
        //    ReviseNote = revisenote;
        //}

        //#endregion
    }


    public class StrdProc_CardBoard
    {
        public static void writetoSQL(cbRecord cbRec)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //Use first command to create/upate the record of the Cardboard Spec
                //create command obj !!**Need to write stored procedure+
                SqlCommand cmd = new SqlCommand("dbo.CardboardRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PartNumber", cbRec.PartNumber));
                cmd.Parameters.Add(new SqlParameter("@Length", cbRec.Length));
                cmd.Parameters.Add(new SqlParameter("@Width", cbRec.Width));
                cmd.Parameters.Add(new SqlParameter("@Thickness", cbRec.Thickness));
                cmd.Parameters.Add(new SqlParameter("@Grain", cbRec.Grain));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(cbRec.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", cbRec.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", cbRec.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(cbRec.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(cbRec.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", cbRec.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", cbRec.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();

                sqlDB.Close();                
            }
        }
    }
}
