namespace ApiContracts;

public class CreateCommentDto
{
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int PostId { get; set; }
}