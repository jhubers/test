

using System;
using System.Drawing.Imaging;

namespace MyImageLoader
{
    public class pImage 
    {
        public string ImagePath { get; set; }

        public pImage(string ip)
        {
            ImagePath = ip;
        }

        internal void Save(object p, ImageFormat png)
        {
            throw new NotImplementedException();
        }
    }
}
