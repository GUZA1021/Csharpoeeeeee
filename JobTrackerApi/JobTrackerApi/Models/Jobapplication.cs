using System.Linq;
using System.Linq.Expressions;

namespace JobTrackerApi.Models
{
    public class JobApplication
    {
        public int Id { get; private set; } // Private set gør at det er kun selve klassen og ingen andre som kan redigere variablen
        public string Company { get; set; }
        public string Position { get; set; }
        public Status Status { get; set; }

        public JobApplication(string company, string position)
        {
            if (string.IsNullOrWhiteSpace(company))
            {
                throw new ArgumentException("Wela eri det forkert");
            }
            Company = company;
            Position = position;
            Status = Status.Applied;
        }
        public string Jobs()
        {
            return $"Ansøgning hos {Company} as a {Position} and the status is : {Status}";
        }
    }

}
