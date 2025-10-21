using MiniLibraryMVC.Models;

namespace MiniLibraryMVC.ViewModels
{
    // This class will hold ALL the data our dashboard needs.
    public class AdminDashboardViewModel
    {
        // For the stats cards
        public DashboardStats Stats { get; set; }

        // For the book management tab
        public List<Book> AllBooks { get; set; }

        // For the member activity tab
        public List<MemberBorrowingInfo> MemberBorrowings { get; set; }

        // To control which tab is active when the page loads
        public string ActiveTab { get; set; }
    }

    // A small helper class for the stats
    public class DashboardStats
    {
        public int TotalBookTitles { get; set; }
        public int TotalCopies { get; set; }
        public int MembersCount { get; set; }
        public int BorrowedCount { get; set; }
        public int AvailableCount { get; set; }
    }

    // A helper class to link a member to their borrowed books
    public class MemberBorrowingInfo
    {
        public string MemberName { get; set; }
        public List<Book> BorrowedBooks { get; set; }
    }
}