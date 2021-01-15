using People.Client.Helpers;

namespace People.Client.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public int Age { get; set; }
        public Enums.Gender Gender { get; set; }
    }
}