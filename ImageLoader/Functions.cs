using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyImageLoader
{
    class Functions
    {
        public static List<string> GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            if (searchFolder == null)
            {
                var answer = MessageBox.Show("You did'nt provide a folder parth to search... I'll take C:\\Temp and make it if it doesn't exist.", "Error", MessageBoxButton.OKCancel);
                if (answer == MessageBoxResult.Cancel)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    if (!Directory.Exists("C:\\Temp"))
                    {
                        Directory.CreateDirectory("C:\\Temp");
                    }
                    searchFolder = "C:\\Temp";
                }
            }
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound;
        }
    }
}
