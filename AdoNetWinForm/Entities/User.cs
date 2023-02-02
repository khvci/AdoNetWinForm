namespace AdoNetWinForm.Entities
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public UserType Type { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum UserType
    {
        Personal,
        Business
    }
}