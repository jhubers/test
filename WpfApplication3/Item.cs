using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    public class Item
    {
        public string Svalue { get; set; }
        public List<Image> ImageList { get; set; }
        //overiding the ToString method takes care of displaying the string value in the DataGrid
        //public override string ToString()
        //{
        //    return Svalue;
        //}
        //constructor
        public Item(string s, List<Image> il)  
        {
            Svalue = s;
            ImageList = il;
        }
    }
}
