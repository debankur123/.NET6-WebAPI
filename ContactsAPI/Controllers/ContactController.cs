using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactsAPIDbContext dbcontext;
        public ContactController(ContactsAPIDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await dbcontext.Contact.ToListAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbcontext.Contact.FindAsync(id);
            if (contact==null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest _addcontact)
        {
            var Contacts = new Contacts()
            {
                Id = Guid.NewGuid(),
                FullName = _addcontact.FullName,
                Email = _addcontact.Email,
                Phone = _addcontact.Phone,
                Address= _addcontact.Address
            };
            await dbcontext.Contact.AddAsync(Contacts);
            await dbcontext.SaveChangesAsync();
            return Ok(Contacts);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContact _UpdateContact)
        {
            var Contact = await dbcontext.Contact.FindAsync(id);
            if(Contact != null)
            {
                Contact.FullName = _UpdateContact.FullName;
                Contact.Email = _UpdateContact.Email;
                Contact.Phone = _UpdateContact.Phone;
                Contact.Address = _UpdateContact.Address;
                await dbcontext.SaveChangesAsync();
                return Ok(Contact);
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var Contact = await dbcontext.Contact.FindAsync(id);
            if (Contact!=null)
            {
                 dbcontext.Remove(Contact);
                await dbcontext.SaveChangesAsync();
                return Ok(Contact);
            }
            return NotFound();
        }
    }
}
