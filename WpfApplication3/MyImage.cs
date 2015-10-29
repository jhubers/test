using System;
using System.ComponentModel;

namespace WpfApplication3
{
    public class MyImage// : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(String info)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //    }
        //}
        //private string myImagePath = "";
        public string MyImagePath { get; set; }
        //{
        //    get { return myImagePath; }
        //    set
        //    {
        //        myImagePath = value;
        //        NotifyPropertyChanged("MyImagePath");
        //    }
        //}
        //constructor
        public MyImage(string ip)
        {
            MyImagePath = ip;
        }
    }
}
