using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
namespace ACADtoSQL
{
    //Need to get a change log number
    class CadReader_ChangeForm
    {
        public static void writeChanges(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //data holders to get block attributes we want
            string styleId = Class1.GetStyleID(fileName);

            changeLog log = new changeLog();

            //add data depending on tag
            foreach (ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if (ar != null && ar.TextString != "")
                {
                    switch (ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "CF_ITEM":
                            log.StyleID = ar.TextString;
                            break;
                        case "CATEGORY":
                            log.Category = ar.TextString;
                            break;
                        case "BY":
                            log.CreateInitials = ar.TextString;
                            //log.RevisionInitials = ar.TextString;
                            break;
                        case "DATE":
                            log.CreateDate = ar.TextString;
                            //log.RevisedDate = ar.TextString;
                            break;
                        case "CHANGENUM":
                            log.ChangeNumber = ar.TextString;
                            break;
                        case "DESC1":
                            break;
                        case "DESC2":
                            break;
                        case "DESC3":
                            break;
                        case "DESC4":
                            break;
                        case "DESC5":
                            break;
                        case "CHANGE_DESCRIPTION":
                            break;

                        #endregion
                    }
                }
            }

            StrdProc_ChangeLog.writetoSQL(log);
        }
    }
}
