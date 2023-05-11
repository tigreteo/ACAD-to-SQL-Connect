using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace ACADtoSQL
{
    class CadReader_Frame
    {
        public static void writeFrameData(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //get a relatively more reliable copy of the styleID
            string styleId = Class1.GetStyleID(fileName);

            frameRec frame = new frameRec();

            foreach (ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if (ar != null && ar.TextString != "")
                {
                    switch (ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "BOM_GROUP":
                            break;
                        case "BOM_STYLE":
                            break;
                        case "BOM_MATERIAL":
                            frame.Plywood = ar.TextString;
                            break;
                        case "BOM_POST":
                            break;
                        case "BOM_TNUT":
                            break;
                        case "BOM_INITIALS":
                            frame.CreateInitials = ar.TextString;
                            break;
                        case "BOM_DATE":
                            frame.CreateDate = ar.TextString;
                            break;
                            #endregion
                    }
                }
            }
        }
    }
}
