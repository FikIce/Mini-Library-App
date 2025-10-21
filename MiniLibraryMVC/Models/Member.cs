using System.ComponentModel.DataAnnotations;

namespace MiniLibraryMVC.Models
{
    public class Member : User
    {
        // ⚠️ Parameterless constructor is REQUIRED for model binding
        public Member() : base("", "", "") { }

        public Member(string name, string icnumber, string email) : base(name, icnumber, email) { }

        // Override and make these properties bindable
        [Required(ErrorMessage = "Name is required.")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "IC Number is required.")]
        public override string ICNumber { get; set; }

        public string Password { get; set; }
    }
}
