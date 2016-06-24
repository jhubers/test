
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyImageLoader
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string Svalue { get; set; }
        private List<pImage> iml;
        public List<pImage> Iml
        {
            get { return iml; }
            set
            {
                iml = value;
                NotifyPropertyChanged("Im");
            }
        }
        //overiding the ToString method takes care of displaying the string value in the DataGrid
        public override string ToString()
        {
            return Svalue;
        }
        //constructor
        public Item(string s)
        {
            Svalue = s;
        }
    }
}
