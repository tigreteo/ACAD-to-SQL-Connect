using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ACADtoSQL
{
    // OUT OF DATE, use CaadReader_CardBoard instead
    class CadReader_CB
    {
        //pass a blockref to method, search through for relevent data and write it to a file
        public static void writeToFile(BlockReference blkRef, Transaction tr,string fileName)
        {
            //try to verify the block name but be sure to account for erased blockrefs
            //because this is a side base this should never be an issue...
            string blockName = "";
            try { blockName = blkRef.Name; }
            catch (System.Exception e)
            { return; }

            if (blockName == "CardboardForm")
            {
                AttributeCollection atts = blkRef.AttributeCollection;


                //!!!!!!!!!!!!REPLACE WITH DEFINED OBJECT
                //try to build a string from the data
                //StyleID | partName | Date | W | L | thickness | grain |Quantity |description?
                cbRecord cbRec = new cbRecord();

                //look for certain tags
                foreach (ObjectId id in atts)
                {
                    AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                    if (ar != null && ar.TextString != "")
                    {
                        switch (ar.Tag.ToUpper())
                        {
                            case "CS_ITEMNUM":
                                cbRec.StyleId = ar.TextString;
                                break;
                            case "CS_DATE":
                                cbRec.CreateDate = ar.TextString;
                                break;
                            case "CS_LENGTH":
                                Double.TryParse(ar.TextString,out cbRec.Length);
                                break;
                            case "CS_WIDTH":
                                Double.TryParse(ar.TextString, out cbRec.Width);
                                break;
                            case "CS_THICKNESS":
                                Double.TryParse(ar.TextString, out cbRec.Thickness);
                                break;
                            case "CS_DIR":
                                cbRec.Grain = ar.TextString;
                                break;
                            case "CS_QUANTITY":
                                Double.TryParse(ar.TextString, out cbRec.Quantity);                    
                                break;
                            case "CS_REVFECHA":
                                cbRec.RevisedDate = ar.TextString;
                                break;
                            case "CHANGE1":
                                cbRec.RevisonNote = ar.TextString;
                                break;
                        }
                    }
                }

                //get styleID from doc to pass along
                cbRec.PartNumber  = Class1.getStyleIDFromName(Path.GetFileNameWithoutExtension(fileName));


                //send record to stored procedure
                StrdProc_CardBoard.writetoSQL(cbRec);

            }
        }

    }
}
