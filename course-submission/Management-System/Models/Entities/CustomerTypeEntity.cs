namespace Management_System.Models.Entities;

internal class CustomerTypeEntity
{

    public int Id { get; set; }
    public string TypeName { get; set; } = null!;

    public ICollection<CustomerEntity> Customers { get; set; } = new HashSet<CustomerEntity>();
}
