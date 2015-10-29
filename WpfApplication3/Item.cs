using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    public class Item
    {
        public string Value { get; set; }
        public MyImageFolder ItemImageFolder { get; set; }

        //constructor
        public Item(string value)
        {
            Value = value;
        }
    }
}
