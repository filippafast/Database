
namespace Management_System.Models;

internal class AddCaseModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public CustomerModel Customer { get; set; } = null!;



}
