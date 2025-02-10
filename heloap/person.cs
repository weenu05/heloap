namespace heloap
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public List<string> Languages { get; set; } = new();
        public Company? Company { get; set; }
    }
    public class Company
    {
        public string Title { get; set; } = "";
        public string Country { get; set; } = "";
    }
}