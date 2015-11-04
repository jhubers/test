
namespace WpfApplication3
{
    public class Item
    {
        public string Svalue { get; set; }
        //public List<Image> ImageList { get; set; }
        //overiding the ToString method takes care of displaying the string value in the DataGrid
        public override string ToString()
        {
            return Svalue;
        }
        //constructor
        //public Item(string s, List<Image> il)
        public Item(string s)  
{
            Svalue = s;
            //ImageList = il;
        }
    }
}
