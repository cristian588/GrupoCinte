namespace GrupoCinte.Common.Dtos
{
    public class UserForAddDto
    {
        public int Id { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IdTypeID { get; set; }
    }
}
