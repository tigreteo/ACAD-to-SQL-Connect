using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADtoSQL
{
    class CadReader_FabLayout
    {
        public static void writeYardage(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;


            //hold onto specific data from the attributes 
            //replace with a Parameters_yardage datatype  
            #region initialize parameters         
            //********************NOT BEING USED
            string styleId = Class1.GetStyleID(fileName);//try to pull from the folder name to account for typos in the form
            string holder= "";
            string[] comment = { "FL_COMMENT1", "FL_COMMENT2", "FL_COMMENT3", "FL_NOTE1", "FL_NOTE2", "FL_NOTE3" };
            DateTime latestRevision = new DateTime();
            string revision = "";

            #endregion

            ydgRecord record = new ydgRecord();

            //look for certain tags
            foreach (ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if (ar != null && ar.TextString != "")
                {
                    switch (ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "FL_ITEMNUM":
                            record.StyleID = ar.TextString;
                            break;
                        case "FL_RAILROAD":
                            record.RollDirection = "RR";
                            break;
                        case "FL_CUTRITE":
                            record.RollDirection = "CR";
                            break;
                        case "FL_DATE":
                            record.CreateDate = ar.TextString;//creation date
                            break;
                        case "FL_INITIALS":
                            record.CreateInitials = ar.TextString;//creater of spec
                            break;
                        case "FL_YARDS":
                            record.PhysYDS = ar.TextString;//CAD yardage
                            break;
                        case "FL_INCHES":
                            record.PhysIN = ar.TextString;//CAD yardage
                            break;
                        case "FL_YARDS2":
                            record.Yards = ar.TextString;//Physical yardage
                            break;
                        case "FL_INCHES2":
                            record.Inches = ar.TextString;//Physical yardage
                            break;
                        case "FL_FAB_NUMBER":
                            record.FabNum = ar.TextString;
                            break;
                        case "FL_APPROVAL":
                            record.ApprovInitials = ar.TextString;//Approver
                            break;
                        case "FL_APPROVEFECHA":
                            record.ApprovDate = ar.TextString;//Approval Date
                            break;

                            //need revision note and revision date
                            #endregion
                    }
                    //get fabric number, selfwelt, or separate welt
                    //loop through array of tag names comment 1,2,3;note 1,2,3                    
                    for (int i = 0; i < 6; i++)
                    {
                        if (ar.Tag.ToUpper() == comment[i])
                        {   //pass data to method and check for keywords
                            holder = holder + Class1.commentReader(ar.TextString);
                        }                        
                    }

                    //keep latest revision note
                    //compare the dates, the latest (newest date) is kept
                    if(ar.Tag.Contains("REVFECHA"))
                    {
                        try {
                            //convertDate 
                            var sysFormatDate = DateTime.Parse(ar.TextString);
                            //compare dates
                            int result = DateTime.Compare(sysFormatDate, latestRevision);
                            if(result>0)
                            {
                                latestRevision = sysFormatDate;
                                record.RevisedDate = ar.TextString;
                                revision = ar.Tag.ToString().Remove(0, ar.Tag.ToString().Count() - 1);
                            }
                        }
                        catch {}
                    }
                    //record the revision note
                    if(ar.Tag.ToUpper() == "FL_REVISIONS" + revision)
                    { record.RevisonNote = ar.TextString; }
                }
            }
            //get inches
            record.CadYdg =  Class1.ydFieldsConvert(record.Yards, record.Inches);
            record.PhysYdg = Class1.ydFieldsConvert(record.PhysYDS, record.PhysIN);

            //update welt note if possible
            if (holder.Contains("Selfwelt"))
            { record.WeltNote = "SelfWelt"; }
            else if (holder.Contains("Separate"))
            { record.WeltNote = "Separate"; }
            else if (holder.Contains("Bias"))
            { record.WeltNote = "Bias"; }

            //!!!!!!!!!!!!!!!!!!
            //This would be the break between writing to sql procedure or intermediate file

            //run stored procedure to pass all relevant data
            StrdProc_Yardage.writetoSQL(record);

        }
    }
}
