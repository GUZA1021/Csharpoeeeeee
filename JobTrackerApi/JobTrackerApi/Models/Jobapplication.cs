using System.ComponentModel.DataAnnotations;

namespace JobTrackerApi.Models
{
    public class JobApplication
    {
        public int Id { get; private set; } // Private set gør at det er kun selve klassen og ingen andre som kan redigere variablen
        public int? UserId { get;  set; } //foreign key til users skal tilføjes
        public User? User { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Position { get; set; }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public JobApplication(string company, string position)
        {
            Company = company;
            Position = position;
            Status = Status.Applied;
        }
    }
}
