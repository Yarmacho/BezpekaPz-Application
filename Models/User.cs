using System.Collections.Generic;

namespace Application.Models
{
    public class User
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public IEnumerable<File> Files { get; set; }
    }
}
