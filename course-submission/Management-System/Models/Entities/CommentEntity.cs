using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Models.Entities;

internal class CommentEntity
{
    //autogenerate
    public CommentEntity()
    {
        Id = Guid.NewGuid();
        Created = DateTime.Now;
    }
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string Comment { get; set; } = null!;


    [ForeignKey("CaseId")]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public CaseEntity Case { get; set; } = null!;
    public CustomerEntity Customer { get; set; } = null!;
}
