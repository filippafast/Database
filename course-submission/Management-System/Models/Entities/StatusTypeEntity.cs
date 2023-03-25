namespace Management_System.Models.Entities;

internal class StatusTypeEntity
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;

    public ICollection<CaseEntity> Cases { get; set; } = new HashSet<CaseEntity>();
}
