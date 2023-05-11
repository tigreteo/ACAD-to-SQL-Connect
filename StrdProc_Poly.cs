using System;
using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct polyRec
    {
        public string StyleID;
        public string PartName;
        public string PartNumber;
        public double Depth;
        public double Width;
        public double Thickness;
        public string Type; //density|compression, fiber....
        public double Price; //may belong in a diff table
        public double Quantity;
        public string PartDesc;
        public string PolyDesc;
        public bool SubPart;
        public bool Bevel;
        public string Instructions;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }


    public  class StrdProc_Poly
    {
        public static void writetoSQL(polyRec plyRec)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //create command obj
                SqlCommand cmd = new SqlCommand("dbo.PolyRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@styleID", plyRec.StyleID));
                cmd.Parameters.Add(new SqlParameter("@Quantity", plyRec.Quantity));
                cmd.Parameters.Add(new SqlParameter("@PartNumber", plyRec.PartNumber));
                cmd.Parameters.Add(new SqlParameter("@SubPart", plyRec.SubPart));//there isnt a bool in SQL need to convert bool to 1 or 0
                cmd.Parameters.Add(new SqlParameter("@Part_Description", plyRec.PartDesc));
                cmd.Parameters.Add(new SqlParameter("@Poly_Description", plyRec.PolyDesc));
                cmd.Parameters.Add(new SqlParameter("@Depth", plyRec.Depth));
                cmd.Parameters.Add(new SqlParameter("@Width", plyRec.Width));
                cmd.Parameters.Add(new SqlParameter("@Height", plyRec.Thickness));
                cmd.Parameters.Add(new SqlParameter("@Density_Compression_Weight", plyRec.Type));
                //cmd.Parameters.Add(new SqlParameter("@PartType", plyRec.));
                cmd.Parameters.Add(new SqlParameter("@Bevel", plyRec.Bevel));//there isnt a bool in SQL need to convert bool to 1 or 0
                //cmd.Parameters.Add(new SqlParameter("@Category", plyRec.));

                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(plyRec.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", plyRec.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", plyRec.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(plyRec.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(plyRec.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", plyRec.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", plyRec.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }
    }
}
