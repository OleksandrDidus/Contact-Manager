using Contact_Manager.Models;
using Contact_Manager;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Disable header validation and missing field detection
                    MissingFieldFound = null,
                    HeaderValidated = null
                }))
                {
                    var contacts = csv.GetRecords<Contact>().ToList();
                    _context.Contacts.AddRange(contacts);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> Update(int id, string field, string value)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            switch (field)
            {
                case "Name":
                    contact.Name = value;
                    break;
                case "DateOfBirth":
                    contact.DateOfBirth = DateTime.Parse(value);
                    break;
                case "Married":
                    contact.Married = bool.Parse(value);
                    break;
                case "Phone":
                    contact.Phone = value;
                    break;
                case "Salary":
                    contact.Salary = decimal.Parse(value);
                    break;
            }

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
