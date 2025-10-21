namespace MiniLibraryMVC.Models
{
    public abstract class User
    {
        public virtual string Name { get; set; }
        public virtual string ICNumber { get; set; }
        public virtual string Email { get; set; }

        protected User(string name, string icNumber, string email)
        {
            Name = name;
            ICNumber = icNumber;
            Email = email;
        }
    }
}
