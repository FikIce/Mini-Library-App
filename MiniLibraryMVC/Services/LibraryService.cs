using MiniLibraryMVC.Models;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace MiniLibraryMVC.Services
{
    public class LibraryService
    {
        public List<Book> books { get; } = new();
        public List<Borrowing> borrowings { get; } = new();
        public List<Member> members { get; } = new();

        public List<ViewBorrowedBooks> borrowedBooks { get; } = new();

        public LibraryService()
        {
            // ✅ Seed dummy book
            books.Add(new Book
            {
                ISBN = 5543,
                Title = "Seeded Sample Book",
                Author = "John Doe",
                AvailableCopies = 4,
                TotalCopies = 4
            });

            books.Add(new Book
            {
                ISBN = 5466,
                Title = "The Jungle Book",
                Author = "Adam Sebastian",
                AvailableCopies = 3,
                TotalCopies = 3
            });

            books.Add(new Book
            {
                ISBN = 5643,
                Title = "Psychology of Money",
                Author = "Dorethy Syphany",
                AvailableCopies = 4,
                TotalCopies = 4
            });

            books.Add(new Book
            {
                ISBN = 8821,
                Title = "Whispers of Tomorrow",
                Author = "Emily Carter",
                AvailableCopies = 5,
                TotalCopies = 5
            });

            books.Add(new Book
            {
                ISBN = 3347,
                Title = "Echoes in the Fog",
                Author = "Liam Nguyen",
                AvailableCopies = 2,
                TotalCopies = 2
            });

            books.Add(new Book
            {
                ISBN = 4590,
                Title = "The Fractured Code",
                Author = "Sofia Martinez",
                AvailableCopies = 4,
                TotalCopies = 4
            });

            books.Add(new Book
            {
                ISBN = 7264,
                Title = "Beneath the Iron Sky",
                Author = "Marcus Lee",
                AvailableCopies = 1,
                TotalCopies = 1
            });

            books.Add(new Book
            {
                ISBN = 9132,
                Title = "Chronicles of Ember",
                Author = "Isabella Wright",
                AvailableCopies = 6,
                TotalCopies = 6
            });

            // ✅ Seed dummy member (for login testing)
            members.Add(new Member
            {
                ICNumber = "012345",
                Name = "Albert",
                Password = "pass123"
            });
        }

        public void AddBook(Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.ISBN == book.ISBN);
            if (existingBook == null)
            {
                books.Add(book);
            }
            else
            {
                Console.WriteLine("Book is already exist!");
            }
        }

        //public void AddBorrowing(int bookId, int memberIC)
        //{
        //    var book = books.FirstOrDefault(b => b.Id == bookId);
        //    if (book != null && book.Borrow())
        //    {
        //        borrowings.Add(new Borrowing
        //        {
        //            ISBN = bookId,
        //            MemberIC = memberIC
        //        });
        //    }
        //    else
        //    {
        //        Console.WriteLine("Book is not available for borrowing.");
        //    }
        //}

        public bool BorrowBook(int id, string memberic)
        {
            var bookId = books.FirstOrDefault(b => b.ISBN == id);

            if (bookId == null || !bookId.Borrow())
            {
                return false; // Not available
            }

            borrowings.Add(new Borrowing
            {
                ISBN = id,
                MemberIC = memberic,
                BorrowDate = DateTime.Now
            });

            return true;

            //how all of this works in more detailed steps

            //var member = members.FirstOrDefault(m => m.ICNumber == memberIC);
            //if (member == null)
            //{
            //    Console.WriteLine("Member not found.");
            //    return false;
            //}

            //// Step 2: Find book
            //var book = books.FirstOrDefault(b => b.Id == bookId);
            //if (book == null || !book.Borrow())
            //{
            //    Console.WriteLine("Book is not available.");
            //    return false;
            //}

            //// Step 3: Record borrowing
            //borrowings.Add(new Borrowing
            //{
            //    ISBN = bookId,
            //    MemberIC = member.Id // Assuming Member.Id is int
            //});
        }

        public List<Book> SearchAllBooks(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return books;
            }
            return books.Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void RegisterMember(Member member)
        {
            var existingMember = members.FirstOrDefault(m => m.ICNumber == member.ICNumber);
            if (existingMember == null)
            {
                members.Add(member);
            }
            else
            {
                Console.WriteLine("Member is already exist!");
            }
            //alternative validation before adding a member
            //if (!members.Any(m => m.ICNumber == member.ICNumber))
            //    members.Add(member);
        }

        public List<ViewBorrowedBooks> GetBorrowedByMemberId(string memberid)
        {
            var getbooks = borrowings.Where(b => b.MemberIC == memberid)

            .Join(books,
                borrowing => borrowing.ISBN,
                book => book.ISBN,
                (borrowings, books) => new ViewBorrowedBooks
                {
                    ISBN = books.ISBN,
                    Title = books.Title,
                    Author = books.Author,
                    BorrowDate = borrowings.BorrowDate,
                    DueDate = borrowings.BorrowDate.AddDays(14),
                }
            ).ToList();

            return getbooks;
        }

        public Member GetMemberByID(string icNumber)
        {
            return members.FirstOrDefault(m => m.ICNumber == icNumber);
        }

        public void ReturnBook(int bookId, string memberIC)
        {
            var borrowing = borrowings.FirstOrDefault(b => b.ISBN == bookId && b.MemberIC == memberIC);
            if (borrowing != null)
            {
                borrowings.Remove(borrowing);
                var book = books.FirstOrDefault(b => b.ISBN == bookId);
                if (book != null)
                {
                    book.AvailableCopies++;
                }
            }
        }
    }
}  
