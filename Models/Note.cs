namespace NoteMiniApi.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreateAt { get; set; }
    //    public User User { get; set; } = null!;
   //     public Guid UserId { get; set; }
    }
}