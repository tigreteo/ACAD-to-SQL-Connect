using System;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace ACADtoSQL
{
    class CadReader_CardBoard
    {
        public static void pullCBData(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;
            cbRecord record = new cbRecord();

            //get syleID, can come from filepath, fileName, or from the specSheet?????
            //Cardboard spec doesn't inclue a styleID number, may need to revise spec
            string styleID = Class1.getStyleIDFromName(Path.GetFileNameWithoutExtension(fileName));

            //seek specific data for specific tags
            foreach (ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if(ar != null && ar.TextString != "")
                {
                    switch(ar.Tag.ToUpper())
                    {
                        #region Cases for all blkref attributes
                        case "CS_ITEMNUM":
                            record.PartNumber = ar.TextString;
                            break;
                        case "CS_DATE":
                            record.CreateDate = ar.TextString;
                            break;
                        case "CS_LENGTH":
                            record.Length = Convert.ToDouble(ar.TextString);//need error handling for not numbers
                            break;
                        case "CS_WIDTH":
                            record.Width = Convert.ToDouble(ar.TextString);
                            break;
                        case "CS_THICKNESS":
                            record.Thickness = Convert.ToDouble(ar.TextString);
                            break;
                            //quantity only makes sense from a styleID perspective not, as a CB spec 
                        case "CS_QUANTITY":
                            record.Quantity = Convert.ToDouble(ar.TextString);
                            break;
                        case "CS_DIRECTION":
                            record.Grain = ar.TextString;
                            break;
                        case "CS_APPROVED":
                            record.ApprovDate = ar.TextString;
                            break;
                        case "CS_CHNGNUM1":
                            record.CreateDate = ar.TextString;
                            break;
                        case "CHANGE1":
                            record.RevisedDate = ar.TextString;
                            break;

                            //add comment control
                            #endregion
                    }

                    //need to have code for comment
                    //need to be sure to have the latest change note/date
                }
            }

            //if there is an intermediary save loc, it goes here
            //  >>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<

            StrdProc_CardBoard.writetoSQL(record);
        }
    }
}
