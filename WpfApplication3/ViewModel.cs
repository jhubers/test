using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace WpfApplication3
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
        private List<MyImageFolder> myImageFolderList;
        public List<MyImageFolder> MyImageFolderList
        {
            get { return myImageFolderList; }
            set
            {
                myImageFolderList = value;
                NotifyPropertyChanged("MyImageFolderList");
            }
        }
        public ViewModel()
        {
            DataTable tempPropDataTable = new DataTable();
            tempPropDataTable.Columns.Add("A", typeof(string));
            tempPropDataTable.Columns.Add("B", typeof(string));
            DataRow row0 = tempPropDataTable.NewRow();
            DataRow row1 = tempPropDataTable.NewRow();
            row0[0] = "A0";
            row0[1] = "B0";
            row1[0] = "A1";
            row1[1] = "B1";
            tempPropDataTable.Rows.Add(row0);
            tempPropDataTable.Rows.Add(row1);
            PropDataTable = tempPropDataTable;

            MyImageFolderList = new List<MyImageFolder>();
            //in D:\Temp\B1 there are two filesP test0.jpg and test1.jpg
            string B0 = "D:\\Temp\\B1\\test0.jpg";
            string B1 = "D:\\Temp\\B1\\test1.jpg";
            MyImageFolder mif = new MyImageFolder("B1");
            MyImage mi0 = new MyImage(B0);
            MyImage mi1 = new MyImage(B1);
            mif.MyImageList = new List<MyImage>();//did you forget this???
            mif.MyImageList.Add(mi0);
            mif.MyImageList.Add(mi1);
            MyImageFolderList.Add(mif);
        }
    }
}
