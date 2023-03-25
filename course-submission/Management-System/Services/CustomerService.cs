
using System.Linq.Expressions;
using Management_System.Contexts;
using Management_System.Models;
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Services;

internal class CustomerService : GenericService<CustomerEntity>
{
    private readonly DataContext _context = new DataContext();

    #region CustomerInfoAsync
    public async Task<CustomerModel> CustomerInfoAsync()
    {
        var _customer = new CustomerModel();

        var existCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == _customer.Email);

        if (existCustomer != null)
        {
            _customer.Id = existCustomer.Id;
            _customer.CustomerType = existCustomer.CustomerType.TypeName;
        }

        else
        {
            _customer = await CreateNewCustomerAsync(_customer);
        }

        return _customer;
    }
    #endregion

    #region CreateNewCustomerAsync
    private async Task<CustomerModel> CreateNewCustomerAsync(CustomerModel customer)
    {
        var newCustomer = new CustomerEntity
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            CustomerTypeId = (await _context.CustomerTypes.FirstOrDefaultAsync(x => x.TypeName == customer.CustomerType))!.Id,
            AddressId = (await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName == customer.StreetName))!.Id
        };

        _context.Add(newCustomer);
        await _context.SaveChangesAsync();

        return customer;
    }
    #endregion

    #region GetAllAsync
    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers
            .Include(x => x.Address)
            .Include(x => x.CustomerType).ToListAsync();  
    }
    #endregion

    #region GetAsync
    public override async Task<CustomerEntity> GetAsync(Expression<Func<CustomerEntity, bool>> predicate)
    {
        var item = await _context.Customers
            .Include(x => x.Address)
            .Include(x => x.CustomerType)
            .FirstOrDefaultAsync(predicate, CancellationToken.None);
        if (item != null) 
            return item;

        return null!;
    }

 #endregion

}