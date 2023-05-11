using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

using System.Data.SqlClient;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using System.Windows.Forms;
using System.Globalization;

namespace ACADtoSQL
{
    //currently hardcoding in login info for server, might need to link to encrypted xml file to have appropriate login info
    //public connString = "Server=SFVALDB01;User Id=engclient;" &
    //"Password=engdata; MultipleActiveResultSets=true;";

    //ask user if this is a comprehensive build
    //a specific set of folders
    //or the curent file

    //use LINQ to find all DWGs in the proposed range
    //filter collection for dwgs inside of a spec folder

    //Open the file find appropriate blockrefs
    //  --find blkrefs in modelspace & paperspace and concat two lists
    
    //depending the blockref identified
    //collect relevant data and run relevant stored procedure


        //TO DO
        //need to adjust code to write to file instead of SQL
        //need to create error log *accounting for typos and missing data in forms
        //when adding to SQL need to compare values for updates and verify
        //  if CAD or SQL has the more correct information
        //
        //needs to work on frame folder, post folder, CB folder,....

    public class Class1
    {
        [CommandMethod("SqlBuildDB")]
        public void Main()
        {
            //ask what files are adding/updating to sql db
            PromptKeywordOptions pko = new PromptKeywordOptions("");
            pko.Message = ("What range to process?");
            pko.Keywords.Add("All");
            pko.Keywords.Add("Specific Folder");
            pko.Keywords.Add("Current");
            pko.Keywords.Add("Dimensions");
            pko.Keywords.Default = "Current";
            pko.AllowNone = false;
            pko.AppendKeywordsToMessage = true;

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            //keep prog bar so user doesnt have a blank screen when processing a large amount of files
            ProgressMeter pm = new ProgressMeter();

            string searchFolder = "";
            IEnumerable<FileInfo> fileNames = null;

            //request from user 
            PromptResult pkeyRes = ed.GetKeywords(pko);
            if (pkeyRes.Status == PromptStatus.OK)
            {
                if (pkeyRes.StringResult.ToUpper() == "ALL")
                {
                    searchFolder = @"Y:\Product Development\Style Specifications";
                    //dig through folder for dwgs
                    fileNames = LINQ(searchFolder);
                }
                else if (pkeyRes.StringResult.ToUpper() == "SPECIFIC")
                {
                    FolderBrowserDialog folder = new FolderBrowserDialog();
                    folder.SelectedPath = @"Y:\Product Development\Style Specifications";
                    folder.RootFolder = Environment.SpecialFolder.Desktop;

                    if (folder.ShowDialog() == DialogResult.OK)
                    { searchFolder = folder.SelectedPath; }

                    //dig through folder for dwgs
                    fileNames = LINQ(searchFolder);
                }
                else if (pkeyRes.StringResult.ToUpper() == "CURRENT")
                {
                    //skip innumeration
                    //pass filelocation directly to later method                    
                    getBlockRefs( new FileInfo(doc.Database.Filename), ed);

                    //need to "add" file info to filenames inumeration
                    //just process fileinfo and do a return or break?
                    //  ^ignores prog bar, but shouldnt matter for only 1 file                    
                }
                else if (pkeyRes.StringResult.ToUpper() == "DIMENSIONS")
                {
                    ExcelReader_Dimensions.getOldDimensions();
                }
            }
            else
                return;

            //*****FOR LATER REFERANCE
            //create a file of data to later be added to SQL instead of directly adding to sql

            //loop through inumeration file(s)
            //start progress meter
            pm.Start("Processing files");
            pm.SetLimit(fileNames.Count());

            foreach(FileInfo fi in fileNames)
            {
                //filter for files inside normal folders
                //ignores any files in "OLD" folders or files in root folders instead of specific spec folder
                if(hasPattfolder(fi.FullName))
                {
                    //method get blkrefs from a dwg
                    getBlockRefs(fi, ed);
                }

                //sleep keeps it from wasting CPU cycles 
                System.Threading.Thread.Sleep(5);
                Autodesk.AutoCAD.ApplicationServices.Core.Application.UpdateScreen();
                pm.MeterProgress();
            }

            //clear meter now that it is finished
            pm.Stop();
            ed.WriteMessage("\nFinshed updating/writing to database");

            //use this message when there is a save location instead of writing to sql
            //ed.WriteMessage("\nFinished writing to:" + saveLoc);

        }


        //gets creates an inumeration of the targeted folder sent to it
        //need to ignore the Test folder
        private static IEnumerable<FileInfo> LINQ(string searchFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(searchFolder);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            //filter for dwgs
            fileList =
                from file in fileList
                where file.Extension == ".dwg"
                orderby file.DirectoryName
                select file;

            return fileList;
        }

        //!!!!!!!!!!!!!!!
        //need a better solution than this filter
        private static bool hasPattfolder(string pathName)
        {
            string styleID = "ID not Found";
            //string pathName = Path.GetDirectoryName(doc.Name);
            string[] styleIDparts = pathName.Split('\\');
            styleID = styleIDparts[styleIDparts.Length - 2];

            if (styleID.ToUpper() == "PATTERN" ||
                styleID.ToUpper() == "PATTERNS" ||
                styleID.ToUpper() == "FABRIC" ||
                styleID.ToUpper() == "FABRICS"||
                styleID.ToUpper() == "CARDBOARD"||
                styleID.ToUpper() == "SEWING" ||
                styleID.ToUpper() == "POLY" )
                return true;
            else
                return false;
        }

        private static void getBlockRefs(FileInfo fi, Editor ed)
        {
            Database sideDb = new Database(false, true);
            using (sideDb)
            {
                try { sideDb.ReadDwgFile(fi.FullName, FileShare.Read, false, ""); }
                catch (System.Exception)
                {
                    ed.WriteMessage("\nUnable to read drawing file.");
                    return;
                }

                using (Transaction tr = sideDb.TransactionManager.StartTransaction())
                {
                    BlockTable bt = sideDb.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                    //list all the block refs from model/paperspace
                    IEnumerable<BlockReference> blkRefList = getModalSpaceBlockRefs(sideDb, bt, btr);
                    IEnumerable<BlockReference> blkRefList2 = GetPaperSpaceBlockReferences(sideDb);

                    //check all blkrefs in database and write out all pertinant data
                    foreach (BlockReference blkRef in blkRefList.Concat(blkRefList2))
                    { writeToDB(blkRef, tr, fi.FullName, ed); }
                }
            }
        }

        //grab all blkrefs from model space
        private static IEnumerable<BlockReference> getModalSpaceBlockRefs(Database db, BlockTable bt, BlockTableRecord btr)
        {
            Transaction tr = db.TransactionManager.TopTransaction;
            if (tr == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NotTopTransaction);

            foreach (ObjectId msId in btr)
            {
                if (msId.ObjectClass.DxfName.ToUpper() == "INSERT")
                {
                    BlockReference blkRef = tr.GetObject(msId, OpenMode.ForRead) as BlockReference;
                    yield return blkRef;
                }
            }
        }

        //grab all blkrefs from paper spaces
        private static IEnumerable<BlockReference> GetPaperSpaceBlockReferences(Database db)
        {
            Transaction tr = db.TransactionManager.TopTransaction;
            if (tr == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NotTopTransaction);

            RXClass rxc = RXClass.GetClass(typeof(BlockReference));
            DBDictionary layouts = (DBDictionary)tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead);
            foreach (var entry in layouts)
            {
                if (entry.Key != "Model")
                {
                    Layout lay = (Layout)tr.GetObject(entry.Value, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(lay.BlockTableRecordId, OpenMode.ForRead);
                    foreach (ObjectId id in btr)
                    {
                        if (id.ObjectClass == rxc)
                        {
                            yield return (BlockReference)tr.GetObject(id, OpenMode.ForRead);
                        }
                    }
                }
            }
        }

        //should styleID not be found in spec, grab from the file location
        public static string GetStyleID(string pathName)
        {
            string styleID = "ID not Found";
            //string pathName = Path.GetDirectoryName(doc.Name);
            string[] styleIDparts = pathName.Split('\\');
            styleID = styleIDparts[styleIDparts.Length - 3];
            return styleID;
        }

        //get a styleID out of the DWG name
        public static string getStyleIDFromName(string name)
        {
            string[] nameParts = name.Split(' ');

            return nameParts[0];
        }

        //convert inches with fractions into real numbers
        public static double FractionToDouble(string fraction)
        {
            double result;

            if (double.TryParse(fraction, out result))
            { return result; }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        return (double)a / b;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        return a + (double)b / c;
                    }
                }
            }

            throw new FormatException("Not a valid fraction.");
        }

        //convert yards(string) and inches(string) into inches(double
        public static double ydFieldsConvert(string yards, string inches)
        {
            //get inches
            double yardage_inches = 0;
            #region calculate inches
            //need to validate numbers if numbers are >= 0 then they successfully converted their field
            double ins = -1;
            double yds = -1;
            try
            {
                yds = Convert.ToDouble(yards);
                ins = Class1.FractionToDouble(inches);
            }
            catch
            { }//errors here would suggest non numbers being typed into the field

            if (yds >= 0)
            { yardage_inches = yds * 36; }
            else
            {
                //leave yards at 0 if it was null or blank;
            }

            //if valid, add inches to yards(in inches)
            if (ins >= 0)
            { yardage_inches = yardage_inches + ins; }

            return yardage_inches;
            #endregion
        }

        //convert system date to sql complient date
        public static object sysDateToSqlDate(string sysDate)
        {
            //coming out of cad mm/dd/yy *System format
            //SQL desires yyyy/mm/dd
            try
            {
                var sysFormatDate = DateTime.Parse(sysDate);
                var sqlFormattedDate = sysFormatDate.Date.ToString("yyyy-MM-dd");
                return sqlFormattedDate;
            }
            catch (System.Exception e)
            {
                //add to error log lack of proper date format
                return DBNull.Value;
            }
        }


        //get sent to diff methods based on blockref type
        private static void writeToDB(BlockReference blkRef, Transaction tr, string fileName, Editor ed)
        {
            //try to verify the block name but be sure to account for erased blockrefs
            //because this is a side base this should never be an issue...
            //still there can be odd circumstances leaving a false blkref "ghost"
            string blockName = "";
            try { blockName = blkRef.Name; }
            catch (System.Exception e)
            { return; }//posssible point to write to an error log

            switch (blockName.ToUpper())
            {
                //used in frame assembly notes,
                case "BOM TEMPLATE":
                    break;

                //Cardboard form
                case "CARDBOARDFORM":
                    //CadReader_CB.writeToFile(blkRef, tr, fileName);
                    break;

                //for shipping boxes
                case "CARDBOARD TRAY FORM":
                    break;

                //change form
                case "CHANGE FORM":
                case "CHANGE FORM DYNAMIC":
                    break;

                //fabric layouts need to be added to yardage table
                case "FABRIC LAYOUT":
                case "FABRIC LAYOUT (PLOT)":
                    CadReader_FabLayout.writeYardage(blkRef, tr, fileName, ed);
                    break;

                //generic form for any purchased part
                case "PART FORM":
                    break;

                //Poly Spec
                case "POLY SPECIFICATION":
                case "POLY SPECIFICATION DYNAMIC":
                    break;

                //Post form *not yet signed off to use as spec sheet
                case "POST FORM":
                    break;

                // Sewing SPec
                case "SEWING SPEC":
                    break;

                //not a form we have a stored procedure for                
                default:
                    return;
            }
            

        }

        

        //sort through the mess of comments to only find the kind we'd be interested in as a note
        public static string commentReader(string commentText)
        {
            string returnedText = "";
            char[] delimiterChars = { ' ', ':', '#' };
            //need to deliminate string to break up by spaces into keywords about welt or fab
            string[] mainKeywords = commentText.Split(delimiterChars);
            //loop through that group of words, if they are found then pass them
            //then need to break up those words by : and #, loop through that
            //for fabric need to get the fabric and fabric number from that and return them
            for (int i = 0; i < mainKeywords.Length; i++)
            {
                switch (mainKeywords[i].ToUpper())
                {
                    case "SELFWELT":
                    case "SEPARATE WELT":
                        returnedText = returnedText + "SelfWelt";
                        break;
                    case "SEPARATE":
                        returnedText = returnedText + "Separete Welt";
                        break;
                    case "FAB":
                    case "FABRIC":
                        if ((i + 1) < mainKeywords.Length)
                        {
                            if (mainKeywords[i + 1] != "")
                            //if there is a number after it then add it too
                            { returnedText = returnedText + " -fab#" + mainKeywords[i + 1]; }
                        }
                        else
                        { returnedText = returnedText + " -special fabric"; }
                        break;
                }
            }
            return returnedText;
        }
    }
}
