namespace ZAnthe_BAckEnd.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool isDeleted { get; set; }
    }
}
