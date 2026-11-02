namespace ApiContracts;

public class CommentDto
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int PostId { get; set; }
}