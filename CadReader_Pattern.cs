using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADtoSQL
{
    class CadReader_Pattern
    {
        //blockRef doesnt exist to fill this out yet
        public static void writeYardage(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            AttributeCollection atts = blkRef.AttributeCollection;

            //optional use of styleID
            string styleId = Class1.GetStyleID(fileName);

            patRec record = new patRec();

            //look for certain tags
            foreach (ObjectId id in atts)
            {
                AttributeReference ar = tr.GetObject(id, OpenMode.ForRead) as AttributeReference;
                if (ar != null && ar.TextString != "")
                {
                    switch (ar.Tag.ToUpper())
                    {
                        #region Cases for all form attributes

                        #endregion
                    }
                }
            }
        }
    }
}
