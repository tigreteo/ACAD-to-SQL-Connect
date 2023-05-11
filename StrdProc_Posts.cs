using System;
using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    public struct postsRec
    {
        public string PartID;
        public double Height;
        public double Width;
        public double Depth;
        public string ConnectionType; //hanger bolt, screw holes, bolts holes...
        public string ProfileShape; //turned, square....
        public double Taper; //0 if straight
        public double Pitch;
        public double Price; //? maybe keep in different table
        public bool Hanger;
        public bool Turned;
        //typical data status info
        public string CreateDate;
        public string CreateInitials;
        public string ApprovInitials;
        public string ApprovDate;
        public string RevisedDate;
        public string RevisionInitials;
        public string RevisonNote;
    }

    public class StrdProc_Posts
    {
        public static void writeToSQL(postsRec post)
        {
            using (SqlConnection sqlDB = SQLConnect.openSQL())
            {
                //open connection to db
                sqlDB.Open();

                //create command obj !!**Need to write stored procedure+
                SqlCommand cmd = new SqlCommand("dbo.PostsRecord", sqlDB);

                //set up command as stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StyleID", post.PartID));
                cmd.Parameters.Add(new SqlParameter("@Height", post.Height));
                cmd.Parameters.Add(new SqlParameter("@Width", post.Width));
                cmd.Parameters.Add(new SqlParameter("@Depth", post.Depth));
                cmd.Parameters.Add(new SqlParameter("@ConnectionType", post.ConnectionType));
                cmd.Parameters.Add(new SqlParameter("@ProfileShape", post.ProfileShape));
                cmd.Parameters.Add(new SqlParameter("@Taper", post.Taper));
                cmd.Parameters.Add(new SqlParameter("@Pitch", post.Pitch));
                cmd.Parameters.Add(new SqlParameter("@Price", post.Price));
                //typcial information to give status of file
                cmd.Parameters.Add(new SqlParameter("@Creation_Date", Class1.sysDateToSqlDate(post.CreateDate)));
                cmd.Parameters.Add(new SqlParameter("@Creation_Initials", post.CreateInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Initials", post.ApprovInitials));
                cmd.Parameters.Add(new SqlParameter("@Approval_Date", Class1.sysDateToSqlDate(post.ApprovDate)));
                cmd.Parameters.Add(new SqlParameter("@Revised_Date", Class1.sysDateToSqlDate(post.RevisedDate)));
                cmd.Parameters.Add(new SqlParameter("@Revision_Initials", post.RevisionInitials));
                cmd.Parameters.Add(new SqlParameter("@Revision_Note", post.RevisonNote));

                //execute the procedure
                cmd.ExecuteNonQuery();
                sqlDB.Close();
            }
        }
    }
}
