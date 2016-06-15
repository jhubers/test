using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            InitializeComponent();
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

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var im = (System.Windows.Controls.Image)sender;
            //if (e.ClickCount == 2)
            FullScreenImage myFullScreenImage = new FullScreenImage();
            myFullScreenImage.fullImage.Source = im.Source;
            myFullScreenImage.Show();
        }

        #region myImage_MouseEnter
        //private void myImage_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    //we have the next tree
        //    //Grid
        //    //DataGrid
        //    //DataGrid.Columns
        //    //DataGridTemplateColumn
        //    //DataGridTemplateColumn.CellTemplate
        //    //DataTemplate
        //    //ItemsControl
        //    //ItemsControl.ItemTemplate
        //    //DataTemplate
        //    //Border
        //    //Image

        //    var im = (System.Windows.Controls.Image)sender;
        //    double w = im.ActualWidth;
        //    double h = im.ActualHeight;
        //    double W = im.Source.Width;
        //    double H = im.Source.Height;
        //    double ratio = W / H;
        //    Border b = (Border)im.Parent;
        //    im.Height = 50;
        //    b.Height = 52;
        //    //var dgtc = (DataGridTemplateColumn)this.FindName("dgtc");
        //    //dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
        //    //ContentPresenter cP = (ContentPresenter)VisualTreeHelper.GetParent(b);
        //    //StackPanel sP = (StackPanel)VisualTreeHelper.GetParent(cP);
        //    //BringToFront(sP, cP);
        //    //foreach (ContentPresenter item in sP.Children)
        //    //{
        //    //    if (item == cP)
        //    //    {
        //    //        item.Height = 50;
        //    //        b.Height = 50;
        //    //        im.Height = 48;
        //    //    }
        //    //    else
        //    //    {
        //    //        item.Width = 22;
        //    //        item.Height = 16;
        //    //    }
        //    //}

        //} 
        #endregion
        private void imageBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            var b = (Border)sender;
            var im = (System.Windows.Controls.Image)b.Child;
            im.Height = 50;
            b.Height = 52;
        }

        #region myImage_MouseLeave
        //private void myImage_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    var im = (System.Windows.Controls.Image)sender;
        //    Border b = (Border)im.Parent;
        //    im.Height = 14;
        //    b.Height = 16;
        //    //ItemsControl ic = FindVisualParent<ItemsControl>(b);
        //    //ic.Height = 16;
        //    ////DataGridTemplateColumn dgtc = FindVisualParent<DataGridTemplateColumn>(im);
        //    var dgtc = (DataGridTemplateColumn)this.FindName("dgtc");
        //    var md = (Border)Mouse.DirectlyOver;
        //    if (md == null)
        //    {
        //        dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
        //    }
        //    else
        //    {
        //        string mdn = md.Name;
        //        if (mdn != "imageBorder")
        //        {
        //            dgtc.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
        //        }
        //    }
        //    #region MyRegion
        //    //dgtc.Width = 22;
        //    ////DataGridTemplateColumn dgtc = im
        //    ////Border b = (Border)im.Parent;
        //    ////b.Width = 22;
        //    ////ContentPresenter cP = (ContentPresenter)VisualTreeHelper.GetParent(b);
        //    ////StackPanel sP = (StackPanel)VisualTreeHelper.GetParent(cP);
        //    ////sP.Height = 16;
        //    ////sP.Width = 22;
        //    ////ItemsPresenter t = (ItemsPresenter)VisualTreeHelper.GetParent(sP);
        //    ////t.Width = 22;
        //    ////Border tt = (Border)VisualTreeHelper.GetParent(t);
        //    ////tt.Width = 22;
        //    ////ItemsControl ttt = (ItemsControl)VisualTreeHelper.GetParent(tt);
        //    ////ttt.Width = 22;
        //    ////ContentPresenter tttt = (ContentPresenter)VisualTreeHelper.GetParent(ttt);
        //    ////tttt.Width = 22;
        //    ////var ttttt = VisualTreeHelper.GetParent(tttt);
        //    ////var tttttt = VisualTreeHelper.GetParent(ttttt); 
        //    #endregion

        //} 
        #endregion

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

        static public void BringToFront(Panel pParent, ContentPresenter pToMove)
        {
            try
            {
                int currentIndex = Canvas.GetZIndex(pToMove);
                int zIndex = 0;
                int maxZ = 0;
                ContentPresenter child;
                for (int i = 0; i < pParent.Children.Count; i++)
                {
                    if (pParent.Children[i] is ContentPresenter &&
                        pParent.Children[i] != pToMove)
                    {
                        child = pParent.Children[i] as ContentPresenter;
                        zIndex = Canvas.GetZIndex(child);
                        maxZ = Math.Max(maxZ, zIndex);
                        if (zIndex > currentIndex)
                        {
                            Canvas.SetZIndex(child, zIndex - 1);
                        }
                    }
                }
                Canvas.SetZIndex(pToMove, maxZ);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't bring to front" + ex.Message);
            }
        }
        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }        
    }
}
