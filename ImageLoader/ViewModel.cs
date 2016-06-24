using ImageLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MyImageLoader
{
    class ViewModel : INotifyPropertyChanged
    {
        //This class is ment to be used as plug-in for an application to show images as thumbnails in a data grid
        //from a directory, which path has to be set as property of the MainWindow in MainWindow.cls.
        //Or it can be choosen from a FolderBrowserDialog.
        //Right clicking on a cell shows a menu to add, delete, copy or paste images in a directory.
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
        private readonly DelegateCommand<string> _menuAction;
        public DelegateCommand<string> menuAction
        {
            get { return _menuAction; }
        }

        //constructor
        public ViewModel()
        {
            PropDataTable = new DataTable();
            PropDataTable.Columns.Add("Images", typeof(Item));
            //if you want to use this application on it's own, you'll have to add rows in a loop. E.g. for every subdirectory.
            //For now the application is used as plug-in for just one row.
            DataRow row0 = PropDataTable.NewRow();
            List<string> files = new List<string>();
            //String searchFolder = "D:\\Temp\\Images\\P1"; 
            //this is when you use this application as plug-in for one row. It is a property you can set to avoid next dialog
            String searchFolder = MainWindow.imagesPath;
            if (searchFolder == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Please choose a folder with images, or right click in Images column...";
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
            }
            else
            {
                //put a button kind of dummy image in the Item.Iml - otherwise the right click ContextMenu doesn't work.
                //since the images in the xaml control are made on the basis of a path, use the TempPath of the computer.
                //However some computers might not like that... Try to use the Resource
                string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string path = Path.Combine(Directory.GetParent(appFolderPath).Parent.FullName, "pCOLADdummy.bmp");
                pImage dummy = new pImage(path);
                ImageList.Add(dummy);
            }
            Item i0 = new Item("item 0");
            i0.Iml = ImageList;
            row0[0] = i0;
            PropDataTable.Rows.Add(row0);
            //this runs only when rightclick on an image during execution of the application
            _menuAction = new DelegateCommand<string>(
            (s) =>
            { /* perform some action*/
                switch (s)
                {
                    case "Copy...":
                        System.Windows.MessageBox.Show("You choose Copy...");
                        break;
                    case "Add from clipboard...":
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
                                break;
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
                                //if the overwriting a file is chosen, you get an IO error because the file is in use in the display
                                //so you first have to remove it from there (through converter put in memory). 
                                //You also have to update the PropDataTable
                                //because the cells are bound to that and through a converter to the imageList
                                //so first find the right row in PropDataTable, then the Item, and then the image in the Item.Iml (imageList)
                                //hmm, the other way around
                                //but if from the start theres is no searchfolder, then files is empty

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
                        break;
                    case "Delete...":
                        System.Windows.MessageBox.Show("You choose Delete...");
                        break;
                    default:
                        break;
                }
            }, //Execute
            (s) => { return true; } //!string.IsNullOrEmpty(_menuItem); } //CanExecute
            );
        }
    }
}

