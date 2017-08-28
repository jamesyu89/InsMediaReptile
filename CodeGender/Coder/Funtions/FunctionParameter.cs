namespace InstagramPhotos.CodeGender.Coder.Funtions
{
    public class FunctionParameter
    {
        public FunctionParameter(string name, string type)
        {
            this.Name = name;
            this.CSType = type;
        }

        public string Name { get; set; }

        public string CSType { get; set; }

        public string Description { get; set; }
    }
}
