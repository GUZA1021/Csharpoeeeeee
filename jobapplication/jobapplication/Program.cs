using System.Linq;
using System.Linq.Expressions;
namespace jobapplication
{
    public class JobApplication
    {
        public int Id { get; private set;} // Private set gør at det er kun selve klassen og ingen andre som kan redigere variablen
        public string Company { get; set; }
        public string Position { get; set; }
        public Status Status { get; set; }

        public JobApplication(string company, string position)
        {
            Company = company;
            Position = position;
            Status = Status.Applied;
            if (string.IsNullOrWhiteSpace(company))
            {
                throw new ArgumentException("Wela eri det forkert");
            }
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
            List<JobApplication> list = new List<JobApplication> {new JobApplication("Google","Backend"), new JobApplication(null, "Frontend"), new JobApplication("youtube", "backend") };
            list[1].Status = Status.Interview;
            try
            {
                int i = int.Parse(Console.ReadLine());
            }
            catch (Exception a)
            {
                throw new Exception("hej");
            }
            foreach (JobApplication app in list) {
                Console.WriteLine(app.Jobs());
            }
        }
    }

}

