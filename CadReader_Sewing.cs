using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace ACADtoSQL
{
    class CadReader_Sewing
    {
        public static void recordSpec(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //alternative approach to getting styleID
            string styleId = Class1.GetStyleID(fileName);

            SewPnlRecord record = new SewPnlRecord();

            foreach(ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if(ar != null && ar.TextString != "")
                {
                    switch(ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "SEW_PANELNAME":
                            record.PanelName = ar.TextString;
                            break;
                        case "SEW_PANEL_BOXINGLENGTH":
                            
                            break;
                        case "SEW_PANEL_SEAM":
                            record.SeamLength = Convert.ToDouble(ar.TextString);
                            break;
                        case "SEW_PANEL_PAIR":
                            
                            break;
                        case "SEW_PANEL_QTY":
                                        
                            break;
                        case "SEW_PANEL_NOTES":

                            break;
                            // current sew spec
                        case "SEW_ITEMNUM":
                            record.StyleID = ar.TextString;
                            break;
                        case "SEW_ITEMDESC":
                            
                            break;
                        case "SEW_AUTHOR":
                            record.CreateInitials = ar.TextString;
                            break;
                        case "SEW_APPROVAL":
                            record.ApprovInitials = ar.TextString;
                            break;
                        case "SEW_APPROVEDATE":
                            record.ApprovDate = ar.TextString;
                            break;
                        case "SEW_DATE":
                            record.CreateDate = ar.TextString;
                            break;
                        case "SEW_PAGENUM":
                            
                            break;
                        case "SEW_TOTALPAGENUM":
                            
                            break;
                            //4 of each
                        case "SEW_REVFECHA1":


                            break;
                        case "SEW_INITIAL1":


                            break;
                        case "SEW_REVNUM1":


                            break;
                        case "SEW_COMMENT1":


                            break;
                            #endregion
                    }
                }
            }
            StrdProc_Sewing.writeToSQL(record);
        }
    }
}
