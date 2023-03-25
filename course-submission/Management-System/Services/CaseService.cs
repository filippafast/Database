
using System.Linq.Expressions;
using System.Net;
using Management_System.Contexts;
using Management_System.Models;
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Services;
internal class CaseService : GenericService<CaseEntity>
{
    private static DataContext _context = new DataContext();

    #region CaseInfoAsync
    public static async Task<CaseModel> CaseInfoAsync(string email)
    {
        var _case = new CaseModel();


        var existCase = await _context.Cases
            .Include(x => x.StatusType)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Customer.Email == email);

        if (existCase != null)
        {
            _case.Id = existCase.Id;
            _case.Title = existCase.Title;
            _case.Description = existCase.Description;
            _case.Status = existCase.StatusType.StatusName;
            _case.Comments = existCase.Comments.Select(c => new CommentModel
            {
                Id = c.Id,
                Comment = c.Comment
            });
        }
        else
        {
            Console.WriteLine($"Kunde inte hitta något ärende för mejladressen {email}.");
            return null!;

        }
        return _case;

    }
    #endregion

    #region UpdateStatusAsync
    public static async Task UpdateCaseStatusAsync(Guid caseId, string newStatusName)
    {
        // Get StatusEntity from database
        var newStatus = await _context.StatusTypes.FirstOrDefaultAsync(s => s.StatusName == newStatusName);

        // Get CaseEntity from database
        var existingCase = await _context.Cases.FirstOrDefaultAsync(c => c.Id == caseId);

        if (newStatus != null && existingCase != null)
        {
            Console.WriteLine("Ärendet är nu uppdaterat.");
            // Update the status of the case
            existingCase.StatusType = newStatus;

            // Save the changes to database
            await _context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Kunde inte hitta ärendet eller statusen");
        }
    }

    #endregion

    #region CreateACommentAsync
    public static async Task<CommentEntity> CreateCommentAsync(Guid caseId, string comment, Guid customerId)
    {
        // Get CaseEntity from database
        var existingCase = await _context.Cases
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.Id == caseId && c.CustomerId == customerId);
        if (existingCase != null)
        {
            // Create a new comment & connect to the existing case
            var commentEntity = new CommentEntity()
            {
                Comment = comment,
                Case = existingCase,
                Customer = existingCase.Customer
            };

            _context.Add(commentEntity);
            await _context.SaveChangesAsync();

            return commentEntity;
        }
        else
        {
            Console.WriteLine("Kunde inte hitta ärendet");
            return null!;
        }
    }
    #endregion

    #region GetCaseAsync
    public static async Task<CaseEntity> GetCaseAsync(string email)
    {
        var item = await _context.Cases
            .Include(x => x.Customer).ThenInclude(x => x.CustomerType)
            .Include(x => x.StatusType)
            .FirstOrDefaultAsync(x => x.Customer.Email == email, CancellationToken.None);
        if (item != null)
            return item;

        return null!;
    }

    #endregion

    #region GetAllAsync
    public override async Task<IEnumerable<CaseEntity>> GetAllAsync()
    {
        return await _context.Cases
            .Include(x => x.Customer).ThenInclude(x => x.CustomerType)
            .Include(x => x.StatusType)
            .Include(x => x.Comments)
            .ToListAsync();
    }

    #endregion

    #region CreateACaseAsync
    // Save a case based on the address, else -> create a new address
    // and if the customer exist, else -> create a new customer based on the email
    public static async Task SaveAsync(AddCaseModel model)
    {
        var _caseEntity = new CaseEntity()
        {
            Title = model.Title,
            Description = model.Description,
            Created = DateTime.Now,
            Modified = DateTime.Now,
            StatusTypeId = 1,
        };

        var _customerType = await _context.CustomerTypes.FirstOrDefaultAsync(x => x.TypeName == model.Customer.CustomerType);
        if (_customerType == null)
        {
            throw new ArgumentException("Ogiltig kundtyp.");
        }
        var address = await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName == model.Customer.StreetName);
        if (address == null)
        {
            Console.WriteLine("Adressen finns ej med i registret. Lägger till ny address.\n");
            // Create a new address if it doesn't exist
            address = new AddressEntity
            {
                StreetName = model.Customer.StreetName,
                PostalCode = model.Customer.PostalCode,
                City = model.Customer.City
            };
            //Save address to database
            _context.Add(address);
            await _context.SaveChangesAsync();
        }


        {
            //check if customer exist by email
            var _customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Email == model.Customer.Email);
            if (_customerEntity != null)

            {
                Console.WriteLine("Användaren finns redan i registret. Uppdaterar användaren.");
                _caseEntity.CustomerId = _customerEntity.Id;
            }

            //if not, create a new customer
            else
            {
                _customerEntity = new CustomerEntity

                {
                    FirstName = model.Customer.FirstName,
                    LastName = model.Customer.LastName,
                    Email = model.Customer.Email,
                    PhoneNumber = model.Customer.PhoneNumber,
                    CustomerTypeId = (await _context.CustomerTypes.FirstOrDefaultAsync(x => x.TypeName == model.Customer.CustomerType))!.Id,
                    AddressId = (await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName == model.Customer.StreetName))!.Id
                };

                _context.Add(_customerEntity);
                await _context.SaveChangesAsync();
                //set customerId
                _caseEntity.CustomerId = _customerEntity.Id;
            }
        }
        if (_caseEntity != null)
        {
            //Saves in the database
            _context.Add(_caseEntity);
            await _context.SaveChangesAsync();

        }

    }
    #endregion
}
