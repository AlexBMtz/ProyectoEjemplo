using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }

    }
}
