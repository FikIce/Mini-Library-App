using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniLibraryMVC.Models;
using MiniLibraryMVC.Services;
using MiniLibraryMVC.ViewModels;

namespace MiniLibraryMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly LibraryService library;
        public AdminController(LibraryService libraryService)
        {
            library = libraryService;
        }

        // The action now accepts an optional 'tab' parameter to remember which tab was active.
        public IActionResult Index(string tab = "stats")
        {
            // --- 1. Populate the Stats ---
            var totalCopies = library.books.Sum(b => b.TotalCopies);
            var borrowedCount = library.borrowings.Count;
            var stats = new DashboardStats
            {
                TotalBookTitles = library.books.Count,
                TotalCopies = library.books.Sum(b => b.TotalCopies),
                MembersCount = library.members.Count,
                BorrowedCount = library.borrowings.Count,
                AvailableCount = totalCopies - borrowedCount
            };

            // --- 2. Populate the Member Borrowings ---
            var memberBorrowings = new List<MemberBorrowingInfo>();
            foreach (var member in library.members)
            {
                var borrowedIsbns = library.borrowings
                    .Where(b => b.MemberIC == member.ICNumber)
                    .Select(b => b.ISBN)
                    .ToList();

                var borrowedBooks = library.books
                    .Where(book => borrowedIsbns.Contains(book.ISBN))
                    .ToList();

                memberBorrowings.Add(new MemberBorrowingInfo
                {
                    MemberName = member.Name,
                    BorrowedBooks = borrowedBooks
                });
            }

            // --- 3. Build the final ViewModel ---
            var viewModel = new AdminDashboardViewModel
            {
                Stats = stats,
                AllBooks = library.books.ToList(), // Get a copy of all books
                MemberBorrowings = memberBorrowings,
                ActiveTab = tab
            };

            return View(viewModel);
        }

        public IActionResult Search(String keyword)
        {
            // Note: The search will now need to be updated to work with the new ViewModel
            // For now, let's focus on the main dashboard view.
            // A simple fix would be to return a separate view for search results.
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index");
            }
            else
            {
                // This will need adjustment later, as "Index" now expects the full ViewModel
                var searchResults = library.SearchAllBooks(keyword);
                // We'll create a simple search results view later if needed.
                return View("SearchResults", searchResults);
            }
        }

        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                book.TotalCopies = book.AvailableCopies; // Set initial total copies
                library.AddBook(book);
                return RedirectToAction("Index", new { tab = "books" }); // Redirect back to the books tab
            }
            else
            {
                return View(book);
            }
        }
    }
}
