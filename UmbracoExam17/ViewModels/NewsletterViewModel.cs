using System.ComponentModel.DataAnnotations;

namespace UmbracoExam17.ViewModels
{
    public class NewsletterViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "You must agree to receive newsletters.")]
        [Display(Name = "I agree to receive newsletters.")]
        [Range(typeof(bool), "true", "true",
            ErrorMessage = "Please accept the newsletter subscription.")]
        public bool AcceptTerms { get; set; }
    }
}
