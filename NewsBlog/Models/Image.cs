namespace NewsBlog.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; } // название картинки
        public byte[] Picture { get; set; }
    }
}