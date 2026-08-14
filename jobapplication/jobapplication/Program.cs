namespace jobapplication
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public Status Status { get; set; }

        public JobApplication(string company, string position)
        {
            Company = company;
            Position = position;
            Status = Status.Applied;
        }
        public string Jobs()
        {
            return $"Ansøgning hos {Company} as a {Position} and the status is : {Status}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            JobApplication Google = new JobApplication("Google","Backend");
            Console.WriteLine(Google.Jobs());
        }
    }

}

