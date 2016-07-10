using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyImageLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string imagesPath { get; set; }
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void myXAMLtable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Images":
                    // Create a new template column.
                    DataGridTemplateColumn imageTemplateColumn = new DataGridTemplateColumn();
                    imageTemplateColumn.Header = "Images";
                    imageTemplateColumn.CellTemplate = (DataTemplate)Resources["convertedImagePath"];
                    // Replace the auto-generated column with the templateColumn.
                    e.Column = imageTemplateColumn;
                    //e.Column.Width = 200;
                    break;
                default:
                    //e.Column.Width = 100;
                    break;
            }
        }
        private void Image_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            var im = (System.Windows.Controls.Image)sender;
            //if (e.ClickCount == 2)
            string appFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BitmapImage bi = new BitmapImage();
            bi = (BitmapImage)im.Source;
            string imagePath = ((System.IO.FileStream)bi.StreamSource).Name;
            if (imagePath == appFolderPath + "\\pCOLADdummy.bmp")
            {
                //MessageBox.Show("Please first select a folder with images…");
                //show the context menu
                //var test = FindVisualParent<ItemsControl>(im);//this was when ContextMenu was on ItemsControl, now it is on Image
                ContextMenu contextMenu = im.ContextMenu;
                contextMenu.PlacementTarget = im;
                contextMenu.IsOpen = true;
                //OpenContextMenu(test);//this was when ContextMenu was on ItemsControl, now it is on Image
            }
            else
            {
                FullScreenImage myFullScreenImage = new FullScreenImage();
                myFullScreenImage.fullImage.Source = im.Source;
                myFullScreenImage.Show();
            }
        }
        //private void OpenContextMenu(FrameworkElement element)
        //{
        //    if (element.ContextMenu != null)
        //    {
        //        element.ContextMenu.PlacementTarget = element;
        //        element.ContextMenu.IsOpen = true;
        //    }
        //}

        private void imageBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            var b = (Border)sender;
            var im = (System.Windows.Controls.Image)b.Child;
            im.Height = 50;
            b.Height = 52;
        }

        private void imageBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            var b = (Border)sender;
            var im = (System.Windows.Controls.Image)b.Child;
            im.Height = 14;
            b.Height = 16;
            var dgtc = (DataGridTemplateColumn)this.FindName("dgtc");
            //apparently you have to first reset the DataGridTemplateColumn
            dgtc.Width = new DataGridLength();
            var md = Mouse.DirectlyOver;
            if (md == null)
            {
                dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                return;
            }
            if (md.GetType() != typeof(Border))
            {
                dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
            }
            else
            {
                Border mdb = (Border)md;
                string mdn = mdb.Name;
                if (mdn != "imageBorder")
                {
                    dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                }
            }
        }

        //static public void BringToFront(Panel pParent, ContentPresenter pToMove)
        //{
        //    try
        //    {
        //        int currentIndex = Canvas.GetZIndex(pToMove);
        //        int zIndex = 0;
        //        int maxZ = 0;
        //        ContentPresenter child;
        //        for (int i = 0; i < pParent.Children.Count; i++)
        //        {
        //            if (pParent.Children[i] is ContentPresenter &&
        //                pParent.Children[i] != pToMove)
        //            {
        //                child = pParent.Children[i] as ContentPresenter;
        //                zIndex = Canvas.GetZIndex(child);
        //                maxZ = Math.Max(maxZ, zIndex);
        //                if (zIndex > currentIndex)
        //                {
        //                    Canvas.SetZIndex(child, zIndex - 1);
        //                }
        //            }
        //        }
        //        Canvas.SetZIndex(pToMove, maxZ);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Can't bring to front" + ex.Message);
        //    }
        //}
        //public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        //{
        //    // get parent item
        //    DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        //    // we’ve reached the end of the tree
        //    if (parentObject == null) return null;

        //    // check if the parent matches the type we’re looking for
        //    T parent = parentObject as T;
        //    if (parent != null)
        //    {
        //        return parent;
        //    }
        //    else
        //    {
        //        // use recursion to proceed with next level
        //        return FindVisualParent<T>(parentObject);
        //    }
        //}

        private void myImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var im = (System.Windows.Controls.Image)sender;
            //if (e.ClickCount == 2)
            //string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase);//gave crash in Grasshopper
            BitmapImage bi = new BitmapImage();
            bi = (BitmapImage)im.Source;
            string imagePath = ((System.IO.FileStream)bi.StreamSource).Name;
            ViewModel.selectedImagePath = imagePath;
        }
    }
}
