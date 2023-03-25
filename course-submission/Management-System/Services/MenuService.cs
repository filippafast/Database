using System.Reflection;
using Management_System.Models;
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Management_System.Services;

internal class MenuService
{
    private readonly CaseService _cases = new();

    #region CreateNewCaseAsync
    public async Task CreateNewCaseAsync()
    {
        Console.Write("##### DITT ÄRENDE #####\n");
        Console.Write("Titel på ärendet: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Beskriv ditt ärende: ");
        string description = Console.ReadLine() ?? "";
        Console.Write("Vilken mejl ärendet gäller för: ");
        string email = Console.ReadLine() ?? "";
        Console.Write("#### Adress ärendet gäller för: ####\n");
        Console.WriteLine("Gatuadress: ");
        string streetName = Console.ReadLine() ?? "";
        Console.WriteLine("Postnummer: ");
        string postalCode = Console.ReadLine() ?? "";
        Console.WriteLine("Stad: ");
        string city = Console.ReadLine() ?? "";

        Console.Write("##### DINA KONTAKTUPPGIFTER #####\n");
        Console.Write("Förnamn: ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("Efternamn: ");
        string lastName = Console.ReadLine() ?? "";
        Console.Write("Telefonnummer: ");
        string phoneNumber = Console.ReadLine() ?? "";
        Console.Write("Kundtyp (Landlord, Caretaker, Gardener, Tenant): ");
        string customerType = Console.ReadLine() ?? "";
        Console.WriteLine("Ärendet är nu skapat.");
        Console.WriteLine(DateTime.Now);


        var addCase = new AddCaseModel
        {
            Title = title,
            Description = description,

            Customer = new CustomerModel
            {

                Email = email,
                CustomerType = customerType,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                StreetName = streetName,
                PostalCode = postalCode,
                City = city,

            }

        };
        await CaseService.SaveAsync(addCase);
    }


    #endregion

    #region GetAllCasesAsync
    public async Task GetAllCases()
    {

        Console.WriteLine("#### ALLA ÄRENDEN ####");
        foreach (var caseModel in await _cases.GetAllAsync())
        {
            Console.WriteLine($"Ärendenummer: {caseModel.Id}");
            Console.WriteLine($"Skapad:{caseModel.Created}");
            Console.WriteLine($"Modifierad:{caseModel.Modified}");
            Console.WriteLine($"Titel: {caseModel.Title}");
            Console.WriteLine($"Beskrivning av ärendet: {caseModel.Description}");
            Console.WriteLine($"Status: {caseModel.StatusType.StatusName}");
            Console.WriteLine($"Kundtyp: {caseModel.Customer.CustomerType.TypeName}");
            Console.WriteLine("Kommentarer:");
            foreach (var comment in caseModel.Comments)
            {
                Console.WriteLine($"- {comment.Comment}");
            }
            Console.WriteLine();
        }
    }
    #endregion

    #region GetSpecificCaseAsync
    public async Task GetSpecificCaseAsync()
    {
        Console.WriteLine("#### SPECIFIKT ÄRENDE ####");

        Console.WriteLine("Vilken mejl gäller ärendet för?: ");

        var email = Console.ReadLine() ?? "";

        var specificCase = await CaseService.CaseInfoAsync(email);


        if (specificCase != null)
        {
            Console.WriteLine($"Hittade följande ärende för mejladressen {email}:");
            Console.WriteLine($"Ärendenummer: {specificCase.Id}");
            Console.WriteLine($"Titel på ärendet: {specificCase.Title}");
            Console.WriteLine($"Beskrivning på ärendet: {specificCase.Description}");
            Console.WriteLine($"Status på ärendet: {specificCase.Status}");
        }

        else
        {
            Console.WriteLine("Ärendet avslutas.");
        }


    }

    #endregion

    #region UpdateCaseAsync
    public async Task UpdateCaseAsync()
    {
        Console.WriteLine("Ange ärendenummer: ");
        Guid caseId = Guid.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("Ange den nya statusen: (Ej påbörjad, Pågående, Avslutad. ");
        string newStatus = Console.ReadLine() ?? "";

     

        await CaseService.UpdateCaseStatusAsync(caseId, newStatus);
    }

    #endregion

    #region CommentCaseAsync
    public async Task CommentCaseAsync()
    {
        Console.WriteLine("Ange ärende-id: ");
        Guid caseId = Guid.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("Ange kommentar:");
        string comment = Console.ReadLine() ?? "";

        Console.WriteLine("Ange kund-ID:");
        var customerId = Guid.Parse(Console.ReadLine() ?? "");

        var commentEntity = await CaseService.CreateCommentAsync(caseId, comment, customerId);

        if (commentEntity != null)
        {
            Console.WriteLine("Kommentar skapad.");
        }

        else
        {
            Console.WriteLine("Kunde inte lägga till kommentar. Kontrollera att ärende-ID stämmer och att du har behörighet att kommentera ärendet.");
        }
    }

    #endregion


}
