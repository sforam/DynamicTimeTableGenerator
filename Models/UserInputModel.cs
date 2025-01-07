using System.ComponentModel.DataAnnotations;

namespace DynamicTimeTableGenerator.Models
{
    public class UserInputModel
    {
        [Required]
        [Range(1,7,ErrorMessage ="Working days must be between 1 to 7.")]
        public int WorkingDays { get; set ;}
        
        [Required]
        [Range(1,8,ErrorMessage ="Subjects per day must be between 1 and 8.")]
        public int SubjectsPerDay { get; set;}

        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Total subjects must be a positive number.")]
        public int TotalSubjects { get; set;}

    }
}
