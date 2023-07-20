namespace ZAnthe_BAckEnd.Models
{
    public class Comment : BaseEntity
    {
        public string? CommentDetail { get; set; }
        public string? CommentHead { get; set; }
        public string? UserName { get; set; }
        public DateTime AddedDate { get; set; }


    }
}
