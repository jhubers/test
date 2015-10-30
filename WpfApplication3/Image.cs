using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApplication3
{
    public class Image 
    {
        public string ImagePath { get; set; }

        public Image(string ip)
        {
            ImagePath = ip;
        }
    }
}
