﻿using System;
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
        //constructor
        public ViewModel()
        {
            PropDataTable = new DataTable();
            Image dum = new Image("test");
            List<Image> dumList = new List<Image>();
            dumList.Add(dum);
            Image B1I0 = new Image("D:\\Temp\\B1\\test0.jpg");
            Image B1I1 = new Image("D:\\Temp\\B1\\test1.jpg");
            List<Image> B1L = new List<Image>();
            B1L.Add(B1I0);
            B1L.Add(B1I1);
            Item A0 = new Item("A0", dumList);
            Item B0 = new Item("B0", dumList);
            Item A1 = new Item("A1", dumList);
            Item B1 = new Item("B1", B1L);
            //DataTable tempPropDataTable = new DataTable();
            PropDataTable.Columns.Add("A", typeof(Item));
            PropDataTable.Columns.Add("B", typeof(Item));
            DataRow row0 = PropDataTable.NewRow();
            DataRow row1 = PropDataTable.NewRow();
            row0[0] = A0;
            row0[1] = B0;
            row1[0] = A1;
            row1[1] = B1;
            PropDataTable.Rows.Add(row0);
            PropDataTable.Rows.Add(row1);
            //PropDataTable = tempPropDataTable;
        }
    }
}