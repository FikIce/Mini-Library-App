namespace MiniLibraryMVC.Models
{
    public class Borrowing
    {
        public string MemberIC { get; set; }
        public int ISBN { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
