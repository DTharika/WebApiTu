using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTu.Data;
using WebApiTu.Models;

namespace WebApiTu.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id: guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);

        }
        
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addcontactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addcontactRequest.Address,
                Email = addcontactRequest.Email,
                FullName = addcontactRequest.FullName,
                phone = addcontactRequest.phone,
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);

        }

        [HttpPost]
        [Route("{id: guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            
            if(contact == null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.phone = updateContactRequest.phone;

                await dbContext.SaveChangesAsync();

                return Ok(contact);

            }

            return NotFound();

        }

        [HttpDelete]
        [Route("{id: guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }

            return NotFound();
        }
    }
}
