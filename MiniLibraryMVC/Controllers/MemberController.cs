using Microsoft.AspNetCore.Mvc;
using MiniLibraryMVC.Models;
using MiniLibraryMVC.Services;

namespace MiniLibraryMVC.Controllers
{
    public class MemberController : Controller
    {
        private readonly LibraryService library;

        public MemberController(LibraryService libraryService)
        {
            library = libraryService;
        }

        public IActionResult Index(int page = 1)
        {
            // 1. Get the current user's ID. This part is correct.
            var memberIC = HttpContext.Session.GetString("MemberIC");
            if (string.IsNullOrEmpty(memberIC))
            {
                return RedirectToAction("Index", "Login");
            }

            // 2. Get ONLY the books for the logged-in member.
            // I'm assuming you have a method like this. If not, you need to create it.
            var memberBooks = library.GetBorrowedByMemberId(memberIC);

            // 3. All calculations MUST be based on THIS user's books.
            int totalBooks = memberBooks.Count();
            int booksPerPage = 5;

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / booksPerPage);
            ViewBag.CurrentPage = page;
            ViewBag.MemberName = HttpContext.Session.GetString("MemberName");

            // 4. Now, paginate the CORRECT list.
            var paginatedBooks = memberBooks
                .Skip((page - 1) * booksPerPage)
                .Take(booksPerPage)
                .ToList();

            return View(paginatedBooks);
        }

        //var book = library.borrowings;
        //return View(book);
        //or I can write
        //return View(library.books);

        public IActionResult Search(string keyword)
        {
            var memberIC = HttpContext.Session.GetString("MemberIC");
            if (string.IsNullOrEmpty(memberIC))
            {
                return RedirectToAction("Index", "Login");
            }

            // Get all borrowed books for this member
            var memberBooks = library.GetBorrowedByMemberId(memberIC);

            // Apply keyword filter
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                memberBooks = memberBooks
                    .Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View("Index", memberBooks);
        }


        [HttpGet]
        public IActionResult MemberBorrowing()
        {
            var list = library.books.ToList();
            return View("Borrowing", list);
        }

        //[HttpPost]
        //public IActionResult BorrowBook(Book books)
        //{
        //    var borrowing = library.BorrowBooks(books);

        //    if (borrowing == null)
        //    {
        //        TempData["SuccessMessage"] = "❌ Book is not available for borrowing.";
        //        return RedirectToAction("MemberBorrowing");
        //    }

        //    TempData["SuccessMessage"] = $"✅ Successfully borrowed book";
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult BorrowBook(int bookId)
        {
            var memberIC = HttpContext.Session.GetString("MemberIC");
            if (string.IsNullOrEmpty(memberIC))
                return RedirectToAction("Index", "Login");

            bool success = library.BorrowBook(bookId, memberIC);

            if (!success)
            {
                TempData["SuccessMessage"] = "❌ Book is not available for borrowing.";
                return RedirectToAction("MemberBorrowing");
            }

            TempData["SuccessMessage"] = "✅ Successfully borrowed book";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ReturnBook(int ISBN)
        {
            var memberIC = HttpContext.Session.GetString("MemberIC");
            library.ReturnBook(ISBN, memberIC);
            return RedirectToAction("Index");
        }
    }
}
