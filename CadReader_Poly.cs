using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADtoSQL
{
    class CadReader_Poly
    {
        //blockRef for this will almost certainly be changed
        public static void recordPoly(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //might be a more reasonable way to get syleID, form is often a typo
            string styleId = Class1.GetStyleID(fileName);

            polyRec record = new polyRec();

            //add data depending on tag
            foreach(ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if(ar != null && ar.TextString != "")
                {
                    switch (ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "ITEMNUM":
                            record.StyleID = ar.TextString;
                            break;
                        case "PARTDESC":
                            record.PartDesc = ar.TextString;
                            break;
                        case "QTYPER":
                            record.Quantity = Convert.ToDouble(ar.TextString);//this can obviously fail converting text to double
                            break;
                        case "DATE":
                            record.CreateDate = ar.TextString;
                            break;
                        case "POLYDESC1":
                            record.PolyDesc = ar.TextString;
                            break;
                        case "LENGTH1":
                            record.Depth = Convert.ToDouble(ar.TextString);
                            break;
                        case "WIDTH1":
                            record.Width = Convert.ToDouble(ar.TextString);
                            break;
                        case "THICKNESS1":
                            record.Thickness = Convert.ToDouble(ar.TextString);
                            break;
                        case "POLYTYPE1":
                            record.Type = ar.TextString;
                            break;
                        case "QTY1":
                            
                            break;
                        case "SPECIALINST1":

                            break;
                        case "SPECIALINST2":

                            break;
                        case "SPECIALINST3":

                            break;
                        case "SPECIALINST4":

                            break;
                        case "SPECIALINST5":

                            break;

                        //--------Following tags are for next version of poly spec (dynamic)----------------------------------
                        case "PLY_DATE":
                            record.CreateDate = ar.TextString;
                            break;
                        case "PLY_ITEMNUM":
                            record.StyleID = ar.TextString;
                            break;
                        case "PLY_PARTDESC":
                            record.PartDesc = ar.TextString;
                            break;
                        case "PLY_PARTNUM":
                            record.PartNumber = ar.TextString;
                            break;
                        case "PLY_SUBASSEMBLY":
                            if (ar.TextString.ToUpper() == "X")
                                record.SubPart = true;
                            break;
                        case "PLY_TOLERANCE":
                            //Need parameter
                            break;
                        case "PLY_QTYPERSET":
                            record.Quantity = Convert.ToDouble(ar.TextString);//this can obviously fail converting text to double
                            break;                     
                        case "PLY_LENGTH":
                            record.Depth = Convert.ToDouble(ar.TextString);
                            break;
                        case "PLY_WIDTH":
                            record.Width = Convert.ToDouble(ar.TextString);
                            break;
                        case "PLY_THICKNESS":
                            record.Thickness = Convert.ToDouble(ar.TextString);
                            break;
                        case "PLY_POLYTYPE":
                            record.Type = ar.TextString;
                            break;
                        case "PLY_REVISION":
                            record.RevisionInitials = ar.TextString;
                            break;
                        case "PLY_REVISIONDATE":
                            record.RevisedDate = ar.TextString;
                            break;
                        case "PLY_APPROVDATE":
                            record.ApprovDate = ar.TextString;
                            break;
                        case "PLY_APPROVEINITIALS":
                            record.ApprovInitials = ar.TextString;
                            break;
                        case "PLY_SPECIALINST":
                            //need parameter
                            break;
                        case "PLY_CHANGE":
                            //need parameter
                            break;
                        case "":

                            break;
                            #endregion
                    }
                }
            }
            StrdProc_Poly.writetoSQL(record);
        }
    }
}
