namespace OakbrookFinanceTechTest.Model
{
    using System.ComponentModel.DataAnnotations;

    public class DetailsModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        public string DateOfBirth { get; set; }

        public string Result { get; set; }
    }
}
