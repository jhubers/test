using ImageLoader;
using pCOLADnamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MyImageLoader
{
    class ViewModel : INotifyPropertyChanged
    {
        //This class is ment to be used as plug-in for an application to show images as thumbnails in an ItemsControl
        //from a directory, which path has to be set as property of the MainWindow in MainWindow.cls.
        //Or it can be choosen from a FolderBrowserDialog.
        //Right clicking on a cell shows a menu to add, delete, copy or paste images in a directory.
        public static pImage dummy;
        public static string selectedImagePath;
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        //private Model _Model; //for clarity left out
        private DataTable propDataTable;
        public DataTable PropDataTable
        {
            get { return propDataTable; }
            set
            {
                propDataTable = value;
                NotifyPropertyChanged("PropDataTable");
            }
        }
        private readonly DelegateCommand<string> _menuActionAddFromClipBoard;
        public DelegateCommand<string> menuActionAddFromClipBoard
        {
            get { return _menuActionAddFromClipBoard; }
        }
        private readonly DelegateCommand<string> _menuActionCopy;
        public DelegateCommand<string> menuActionCopy
        {
            get { return _menuActionCopy; }
        }
        private readonly DelegateCommand<string> _menuActionDelete;
        public DelegateCommand<string> menuActionDelete
        {
            get { return _menuActionDelete; }
        }

        //constructor
        public ViewModel()
        {
            PropDataTable = new DataTable();
            PropDataTable.Columns.Add("Images", typeof(Item));
            //if you want to use this application on it's own, you'll have to add rows in a loop. E.g. for every subdirectory.
            //For now the application is used as plug-in for just one row with possibly several images.
            DataRow row0 = PropDataTable.NewRow();
            List<string> files = new List<string>();
            //String searchFolder = "D:\\Temp\\Images\\P1";
            //this is when you use this application as plug-in for one row. It is a property you can set to avoid next dialog
            String searchFolder = MainWindow.imagesPath;
            if (searchFolder == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Please choose a folder to add or delete images, and/or right click in Images column...";
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    searchFolder = fbd.SelectedPath;
                }
                else
                {
                    searchFolder = "empty";
                }
            }
            List<pImage> ImageList = new List<pImage>();
            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            if (!searchFolder.Equals("empty"))
            {
                //next function returns a list of strings with only the path names of files, with extension in filters
                files = Functions.GetFilesFrom(searchFolder, filters, false);
                foreach (var imagePath in files)
                {
                    pImage im = new pImage(imagePath);
                    ImageList.Add(im);
                }
                if (files.Count == 0)
                {
                    dummy = dummyFunction();
                    ImageList.Add(dummy);
                }
            }
            else
            {
                //put a button kind of dummy image in the Item.Iml - otherwise the right click ContextMenu doesn't work.
                //since the images in the xaml control are made on the basis of a path, use the TempPath of the computer.
                //However some computers might not like that... Try to use the Resource
                dummy = dummyFunction();
                ImageList.Add(dummy);
            }
            Item i0 = new Item("item 0");
            i0.Iml = ImageList;
            row0[0] = i0;
            PropDataTable.Rows.Add(row0);
            //this runs only when rightclick on an image during execution of the application
            //s is the commandparameter. It should lead to the path of the image that was right clicked
            _menuActionCopy = new DelegateCommand<string>(
            (s) =>
            { /* perform some action*/
                List<string> sourcePaths;
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = true;
                fd.Filter = "Image files (*.jpg; *.jpeg; *. png; *. gif; *. tiff; *. bmp) | *.jpg; *.jpeg; *. png; *. gif; *. tiff; *. bmp";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    sourcePaths = fd.FileNames.ToList();
                    if (sourcePaths.Count == 0)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (searchFolder.Equals("empty"))
                #region fill files and Imagelist
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Please choose a folder where you want to add this image...";
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        searchFolder = fbd.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                    //since searchFolder equals "empty" files is also empty
                    //next function returns a list of strings with only the path names of files, with extension in filters
                    files = Functions.GetFilesFrom(searchFolder, filters, false);
                    //remove the button like image
                    ImageList.Clear();
                    foreach (var imagePath in files)
                    {
                        pImage im = new pImage(imagePath);
                        ImageList.Add(im);
                    }
                }
                #endregion
                //check if there are files with the same name in the target directory
                //store the copy actions in a dictionary and execute it only if there was no cancel
                Dictionary<string, string> CopyDict = new Dictionary<string, string>();
                List<string> targetPaths = new List<string>();
                foreach (pImage pI in i0.Iml)
                {
                    targetPaths.Add(pI.ImagePath);
                }
                //iterate over sourcePaths
                for (int i = sourcePaths.Count - 1; i >= 0; i--)
                {
                    bool fileNamesSame = false;
                    string sourceFileName = Path.GetFileName(sourcePaths[i]);
                    //iterate over targetPaths to check if fileNames are the same
                    for (int j = targetPaths.Count - 1; j >= 0; j--)
                    {
                        string targetFileName = Path.GetFileName(targetPaths[j]);
                        if (sourceFileName == targetFileName)
                        {
                            fileNamesSame = true;
                            continue;
                        }
                    }
                    if (!fileNamesSame)
                    {
                        CopyDict.Add(sourcePaths[i], searchFolder + "\\" + sourceFileName);
                    }
                    else
                    {
                        Dialogue1 D1 = new Dialogue1();
                        string a1 = "";
                        D1.Topmost = true;
                        D1.Question.Content = sourceFileName + " already exists. OK to replace, or please choose another name.";
                        D1.Answer.Text = sourceFileName;
                        D1.Show();
                        D1.Answer.Focus();
                        D1.Answer.SelectAll();
                        //wait for the answer and store it or cancel
                        D1.Closing += (sender, e) =>
                        {
                            var d = sender as Dialogue1;
                            if (d.Canceled == false)
                            {
                                a1 = D1.Answer.Text;
                                var imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                                imageExtensions.Add("jpg");
                                imageExtensions.Add("jpeg");
                                imageExtensions.Add("jpe");
                                imageExtensions.Add("jfif");
                                imageExtensions.Add("png");
                                imageExtensions.Add("bmp");
                                imageExtensions.Add("dib");
                                imageExtensions.Add("rle");
                                imageExtensions.Add("gif");
                                imageExtensions.Add("tif");
                                imageExtensions.Add("tiff");
                                //check if it is a valide image name
                                string extension = Path.GetExtension(a1);
                                if (!imageExtensions.Contains(extension))
                                {
                                    System.Windows.MessageBox.Show("The file extension is not a valid image. Please try again...");
                                    return;
                                }
                                //replace the fileName in dictionary CopyDict
                                CopyDict.Add(sourcePaths[i], searchFolder + "\\" + a1);
                                //i0.Iml[i] = new pImage(a1);//no because you can't cancel later
                            }
                            else
                            {
                                return;
                            }
                        };
                    }
                }
                //now use the dictionary CopyDict to copy the files and update the PropDataTable
                //if the user Canceled one of the renaming you will not get here because of the return;s
                foreach (KeyValuePair<string, string> entry in CopyDict)
                {
                    File.Copy(entry.Key, entry.Value);
                    while (!File.Exists(entry.Value))
                    {
                        Thread.Sleep(1000);
                    }
                    i0.Iml.Add(new pImage(entry.Value));
                }
                //if there is a dummy in the first row, remove it.
                if (i0.Iml[0] == dummy)
                {
                    i0.Iml.RemoveAt(0);
                }
                PropDataTable.AcceptChanges();
            }, //Execute
            (s) => { return true; } //!string.IsNullOrEmpty(_menuItem); } //CanExecute
            );
            _menuActionAddFromClipBoard = new DelegateCommand<string>(
            (s) =>
            { /* perform some action*/
              //System.Windows.MessageBox.Show("Add from clipboard...");                        
                if (searchFolder.Equals("empty"))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Please choose a folder where you want to add this image...";
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        searchFolder = fbd.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                    //since searchFolder equals "empty" files is also empty
                    //next function returns a list of strings with only the path names of files, with extension in filters
                    files = Functions.GetFilesFrom(searchFolder, filters, false);
                    //remove the button like image
                    ImageList.Clear();
                    foreach (var imagePath in files)
                    {
                        pImage im = new pImage(imagePath);
                        ImageList.Add(im);
                    }
                }
                if (System.Windows.Clipboard.GetDataObject() != null)
                {
                    System.Windows.IDataObject data = System.Windows.Clipboard.GetDataObject();
                    if (data.GetDataPresent(System.Windows.DataFormats.Bitmap))
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        if (!searchFolder.Equals("empty"))
                        {
                            saveFileDialog1.CustomPlaces.Add(searchFolder);
                            saveFileDialog1.InitialDirectory = searchFolder;
                        }
                        saveFileDialog1.AddExtension = true;
                        saveFileDialog1.DefaultExt = "Png";
                        saveFileDialog1.ValidateNames = true;
                        saveFileDialog1.Filter = "Png image (*.Png)|*.Png";
                        saveFileDialog1.OverwritePrompt = true;
                        saveFileDialog1.Title = "Please give a name for this file...";
                        saveFileDialog1.ShowDialog();
                        //string toBeDeleted = "";
                        string fp = saveFileDialog1.FileName;
                        if (fp == "")
                        {
                            //searchFolder = "empty";//this is risky in pCOLAD it should be set to the default
                            searchFolder = MainWindow.imagesPath;
                            return;
                        }
                        //if the overwriting a file is chosen, you get an IO error because the file is in use in the display
                        //so you first have to put the image in memory (through converter). 
                        //You also have to update the PropDataTable
                        //because the cells are bound to that and through a converter to the imageList
                        //so first find the right row in PropDataTable, then the Item, and then the image in the Item.Iml (imageList)
                        //hmm, the other way around
                        //but if from the start there is no searchfolder, then files is empty

                        if (files.Contains(fp))
                        {
                            for (int p = ImageList.Count - 1; p >= 0; p--)
                            {
                                if (ImageList[p].ImagePath.Equals(fp))
                                {
                                    //find this Item in PropDataTable
                                    int ri = 0;
                                    for (int i = 0; i < propDataTable.Rows.Count; i++)
                                    {
                                        if (propDataTable.Rows[i][0] == i0)
                                        {
                                            ri = i;
                                            continue;
                                        }
                                    }
                                    //change the imageList property of this Item
                                    i0.Iml[p] = new pImage(fp);
                                    //change the PropDataTable so it updates in the xaml control
                                    PropDataTable.AcceptChanges();//this sets the PropDataTable and runs the propertyChanged notifier
                                }
                            }
                        }
                        if (!fp.Equals(""))
                        {
                            var image = System.Windows.Clipboard.GetImage();
                            using (var fileStream = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(image));
                                encoder.Save(fileStream);
                            }
                            //add the image to the display if it was not a replacement
                            if (!files.Contains(fp))
                            {
                                i0.Iml.Add(new pImage(fp));
                                //if there is a dummy in the first row, remove it.
                                if (i0.Iml[0] == dummy)
                                {
                                    i0.Iml.RemoveAt(0);
                                }
                                //change the PropDataTable so it updates in the xaml control
                                PropDataTable.AcceptChanges();//this sets the PropDataTable and runs the propertyChanged notifier
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("No image in Clipboard !!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Clipboard Empty !!");
                }
            }, //Execute
            (s) => { return true; } //!string.IsNullOrEmpty(_menuItem); } //CanExecute
            );
            _menuActionDelete = new DelegateCommand<string>(
            (s) =>
            { /* perform some action*/
                string fp = selectedImagePath;//is set in code behind
                if (searchFolder.Equals("empty"))//then fp is the buttonlike immage. Abort.
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Please choose a folder where you want to delete an image, and try again...";
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        searchFolder = fbd.SelectedPath;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                //since you might have added files that you want to delete you have to make sure all the displayed files are in the list 
                //next function returns a list of strings with only the path names of files, with extension in filters
                files = Functions.GetFilesFrom(searchFolder, filters, false);
                //remove the button like image
                ImageList.Clear();
                foreach (var imagePath in files)
                {
                    pImage im = new pImage(imagePath);
                    ImageList.Add(im);
                }
                //if deleting a file, you get an IO error because the file is in use in the display
                //so you first have to put the image in memory (through converter). 
                //You also have to update the PropDataTable
                //because the cells are bound to that and through a converter to the imageList
                //so first find the right row in PropDataTable, then the Item, and then the image in the Item.Iml (imageList)
                //hmm, the other way around
                //but if from the start there is no searchfolder, then files is empty
                if (files.Contains(fp))//it doesn't if user chose Cancel all the time
                {
                    for (int p = ImageList.Count - 1; p >= 0; p--)//why? To find the right image with selectyedImagePath.
                    //Why not show a file selector, so you can delete several files at once and use common shortcuts
                    //to permanently delete or put in recycle bin? Because in History you would like to be able to find
                    //the files and most of the time you only want to delete 1 file. It should be just one click.
                    {
                        if (ImageList[p].ImagePath.Equals(fp))
                        {
                            //find this Item in PropDataTable
                            int ri = 0;
                            for (int i = 0; i < propDataTable.Rows.Count; i++)
                            {
                                if (propDataTable.Rows[i][0] == i0)
                                {
                                    ri = i;
                                    continue;
                                }
                            }
                            //File.Delete(fp);//this peremanently deletes the file 
                            string deletedImagesDir = Path.GetDirectoryName(fp).Replace("Images", "Deleted Images");
                            if (!Directory.Exists(deletedImagesDir))
                            {
                                Directory.CreateDirectory(deletedImagesDir);
                            }
                            string movedFp = fp.Replace("Images", "Deleted Images");
                            //int n = 1;
                            while (File.Exists(movedFp))
                            {
                                //just add '(nr)' to filepath. No need to bather the user with dialogues. Also useful for History
                                string delDir = Path.GetDirectoryName(movedFp) + "\\";
                                string fn = Path.GetFileNameWithoutExtension(movedFp);
                                string ext = Path.GetExtension(movedFp);
                                //check if there is a file with same name that already ends with (number)

                                int end = fn.LastIndexOf(")");
                                int begin = fn.LastIndexOf("(");
                                if (end != -1 & begin != -1)
                                {
                                    string number = fn.Substring(begin + 1, end - begin - 1);
                                    int n;
                                    bool isNumeric = int.TryParse(number, out n);
                                    if (isNumeric)
                                    {
                                        //replace the (number) by (number + 1)
                                        number = (n + 1).ToString();
                                        fn = fn.Substring(0, begin + 1) + number + ")";
                                        movedFp = delDir + fn + ext;
                                    }
                                    else
                                    {
                                        movedFp = delDir + fn + "(1)" + ext;
                                    }
                                }
                                else
                                {
                                    movedFp = delDir + fn + "(1)" + ext;
                                }
                            }
                            //if this was the last file in the folder, you have to put pCOLADdummy.bmp back in
                            if (files.Count == 1)
                            {
                                dummy = dummyFunction();
                                i0.Iml[0]=dummy;
                            }
                            else
                            {
                                //change the imageList property of this Item
                                i0.Iml.RemoveAt(p);
                            }
                            //change the PropDataTable so it updates in the xaml control
                            PropDataTable.AcceptChanges();//this sets the PropDataTable and runs the propertyChanged notifier                            
                            File.Move(fp, movedFp);
                        }
                    }
                }
            }, //Execute
            (s) => { return true; } //!string.IsNullOrEmpty(_menuItem); } //CanExecute
            );
        }
        public pImage dummyFunction()
        {
            //in case of Grasshopper next line appFolderPath = C:\Users\jhubers\AppData\Roaming\Grasshopper\Libraries
            string appFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = appFolderPath + "\\pCOLADdummy.bmp";
            if (!File.Exists(path))
            {
                Bitmap b = ImageLoader.Properties.Resources.pCOLADdummy;
                b.Save(path);
            }
            return dummy = new pImage(path);
        }
    }
}

