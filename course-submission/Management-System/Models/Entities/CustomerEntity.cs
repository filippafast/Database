using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
internal class CustomerEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column(TypeName = "char(13)")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public int CustomerTypeId { get; set; }
    public int AddressId { get; set; }


    public CustomerTypeEntity CustomerType { get; set; } = null!;
    public AddressEntity Address { get; set; } = null!;

    public ICollection<CaseEntity> Cases { get; set; } = new HashSet<CaseEntity>();
    public ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();
}
