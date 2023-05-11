using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace ACADtoSQL
{
    class CadReader_Posts
    {
        public static void recordPosts(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //alternative way to store styleID
            string styleId = Class1.GetStyleID(fileName);

            postsRec record = new postsRec();

            //add data to record
            foreach(ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if(ar != null && ar.TextString != "")
                {
                    switch(ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes
                        case "SF_POSTS_PARTID":
                            record.PartID = ar.TextString;
                            break;
                        case "SF_PARTDESC":
                            
                            break;
                        case "SF_POSTS_WIDTH":
                            record.Width = Convert.ToDouble(ar.TextString); //might need a verification so it doesnt break throwing a string into a double
                            break;
                        case "SF_POSTS_DEPTH":
                            record.Depth = Convert.ToDouble(ar.TextString);
                            break;
                        case "SF_POSTS_HEIGHT":
                            record.Height = Convert.ToDouble(ar.TextString);
                            break;
                        case "SF_POSTS_PITCH":
                            record.Pitch = Convert.ToDouble(ar.TextString);
                            break;
                        case "SF_POSTS_HANGBOLT":
                            if (ar.TextString.ToUpper() == "X")
                                record.Hanger = true;
                            break;
                        case "SF_POSTS_TAPER":
                            record.Taper = Convert.ToDouble(ar.TextString);
                            break;
                        case "SF_POSTS_TURNED":
                            if (ar.TextString.ToUpper() == "X")
                                record.Turned = true;
                            break;
                        case "SF_POSTS_CREATEINITIALS":
                            record.CreateInitials = ar.TextString;
                            break;
                        case "SF_POSTS_CREATE_DATE":
                            record.CreateDate = ar.TextString;
                            break;
                        case "SF_POSTS_APPROVEINITIALS":
                            record.ApprovInitials = ar.TextString;
                            break;
                        case "SF_POSTS_APPROVE_FECHA":
                            record.ApprovDate = ar.TextString;
                            break;
                        case "SF_POSTS_MATERIAL":
                            //not a real tag on spec, but possibly one to add
                            break;
                        case "":

                            break;
                            #endregion
                    }
                }
            }
            StrdProc_Posts.writeToSQL(record);
        }
    }
}
