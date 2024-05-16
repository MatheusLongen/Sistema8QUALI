using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema8QUALY.Data;
using Sistema8QUALY.Models;

namespace Sistema8QUALY.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var contacts = await _context.Contacts.ToListAsync();

            ViewData["CurrentFilter"] = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.ToLower().Split(' ').ToList();

                contacts = contacts.Where(c =>
                    searchTerms.Any(term =>
                        (c.Name != null && c.Name.ToLower().Contains(term)) ||
                        (c.Company != null && c.Company.ToLower().Contains(term)) ||
                        (c.Emails != null && c.Emails.Any(e => e.ToLower().Contains(term))) ||
                        (c.PersonalPhone != null && c.PersonalPhone.ToLower().Contains(term)) ||
                        (c.BusinessPhone != null && c.BusinessPhone.ToLower().Contains(term))
                    )
                ).ToList();
            }

            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Name,Company,Emails,PersonalPhone,BusinessPhone")] Contacts contact)
        {
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Contacts.Any(e => e.ContactId == contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
