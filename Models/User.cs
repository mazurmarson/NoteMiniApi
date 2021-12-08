namespace NoteMiniApi.Models
{
    public class User
    {
        public User(Guid id, string name, string passwordHash)
        {
            this.Id = id;
            this.Name = name;
            this.PasswordHash = passwordHash;

        }
        public User()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}