namespace NoteMiniApi.Models
{
    public class User
    {
        public User(Guid id, string name, string passwordHash)
        {
            this.id = id;
            this.Name = name;
            this.PasswordHash = passwordHash;

        }
        public Guid id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}