using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                    imageTemplateColumn.CellTemplate = (DataTemplate)Resources["convertedImage"];
                    // Replace the auto-generated column with the templateColumn.
                    e.Column = imageTemplateColumn;
                    e.Column.Width = 200;
                    break;
                default:
                    e.Column.Width = 100;
                    break;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var im = (System.Windows.Controls.Image)sender;
            FullScreenImage myFullScreenImage = new FullScreenImage();
            myFullScreenImage.fullImage.Source = im.Source;
            myFullScreenImage.Show();
        }
    }
}
