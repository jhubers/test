using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace MyImageLoader
{
    class ViewModel : INotifyPropertyChanged
    {
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
        //constructor
        public ViewModel()
        {
            PropDataTable = new DataTable();
            //MainWindow myMainWindow = new MainWindow();
            //myMainWindow.imagesPath = "D:\\Temp\\B1";
            //String searchFolder = "D:\\Temp\\B1";
            String searchFolder = MainWindow.imagesPath;
            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            //next function returns a list of strings with only the path names of files
            //with extension in filters
            var files = Functions.GetFilesFrom(searchFolder, filters, false);
            List<Image> B1L = new List<Image>();            
            foreach (var imagePath in files)
            {
                Image im = new Image(imagePath);
                B1L.Add(im);
            }
            Item A0 = new Item("A0");
            Item B0 = new Item("B0");
            Item A1 = new Item("A1");
            Item B1 = new Item("B1");
            Item Z0 = new Item("00");
            Z0.Im = B1L;
            //Item Z1 = new Item("01");
            PropDataTable.Columns.Add("Images", typeof(Item));
            PropDataTable.Columns.Add("A", typeof(Item));
            PropDataTable.Columns.Add("B", typeof(Item));
            DataRow row0 = PropDataTable.NewRow();
            DataRow row1 = PropDataTable.NewRow();
            row0[1] = A0;
            row0[2] = B0;
            row1[1] = A1;
            row1[2] = B1;
            row1[0] = Z0;
            PropDataTable.Rows.Add(row0);
            PropDataTable.Rows.Add(row1);
        }
    }
}
