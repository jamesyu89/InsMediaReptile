using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder
{
    public class Table
    {       
        public Table(string name) {
            this.Name = name;
            this.Alias = this.Name.GetAlias();
        }
        public string Name { get; set; }
        public string Alias { get; set; }


    }
}
