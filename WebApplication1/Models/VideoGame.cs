namespace WebApplication1.Models
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Platform System { get; set; }
    }

    public enum Platform
    {
        SegaGenesis, Pc
    }
}