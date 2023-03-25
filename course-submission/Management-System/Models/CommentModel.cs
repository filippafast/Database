namespace Management_System.Models;

internal class CommentModel
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string Comment { get; set; } = null!;
}
