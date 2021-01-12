namespace NewsBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string CommentText { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}