using System.ComponentModel.DataAnnotations;

namespace Contact_Manager.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Range(0, 1000000)]
        public decimal Salary { get; set; }
    }

}
