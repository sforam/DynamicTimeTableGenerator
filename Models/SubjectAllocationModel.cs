using System.ComponentModel.DataAnnotations;

namespace DynamicTimeTableGenerator.Models
{
    public class SubjectAllocationModel
    {
        [Required(ErrorMessage = "Subject name is required.")]
        public string SubjectName { get; set; }


        [Required(ErrorMessage = "Hours are required.")]
        [Range(1, 24, ErrorMessage = "Hours must be between 1 and 24.")]
        public int Hours {  get; set; }

    }
}
