namespace Application.Models
{
    public class File
    {
        public int Id { get; set; }

        public string UserLogin { get; set; }
        public User User { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}
