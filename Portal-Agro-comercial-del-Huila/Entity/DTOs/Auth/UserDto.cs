namespace Entity.DTOs.Auth
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public PersonDto Person { get; set; }
        public List<string> Roles { get; set; }
    }
}
