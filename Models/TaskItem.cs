namespace TodoApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }
}
