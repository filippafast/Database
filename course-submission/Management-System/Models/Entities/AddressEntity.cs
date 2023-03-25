
using System.ComponentModel.DataAnnotations.Schema;


namespace Management_System.Models.Entities;

internal class AddressEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string StreetName { get; set; } = null!;

    [Column(TypeName = "char(6)")]
    public string PostalCode { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string City { get; set; } = null!;

    //för att undvika dubletter i listan
    public ICollection<CustomerEntity> Customers = new HashSet<CustomerEntity>();
}
