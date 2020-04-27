using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // BindProperty makes Book available to the whole model
        [BindProperty]
        public Book Book { get; set; }

        // ID can be null for create so add '?' here
        public async Task<IActionResult> OnGet(int? Id)
        {
            Book = new Book();
            if (Id == null)
            {
                // create
                return Page();
            }

            // update
            Book = await _db.Book.FirstOrDefaultAsync(u => u.Id == Id);
            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    _db.Book.Add(Book);
                }
                else
                {
                    _db.Book.Update(Book);
                }
                // used for update few properties
                // var BookFromDb = await _db.Book.FindAsync(Book.Id);
                // BookFromDb.Name = Book.Name;
                // BookFromDb.Author = Book.Author;
                // BookFromDb.ISBN = Book.ISBN;

                await _db.SaveChangesAsync();

                return RedirectToPage("Index");

            }
            return RedirectToPage();

        }
    }
}
