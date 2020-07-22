namespace GrupoCinte.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public int IdTypeID { get; set; }
        public IdType IdType { get; set; }
    }
}
