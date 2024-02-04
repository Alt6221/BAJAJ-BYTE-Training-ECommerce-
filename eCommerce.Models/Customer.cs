using System.ComponentModel.DataAnnotations;
namespace eCommerce.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage="Customer Name is a required field!")]
        [MaxLength(100,ErrorMessage ="Customer name cannot exceed 100 characters!")]
        public string ContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer Address is a required field!")]
        [MaxLength(500, ErrorMessage = "Customer Address cannot exceed 500 characters!")]
        public string Address { get; set; }= string.Empty;

        [Required(ErrorMessage = "City is a required field!")]
        [MaxLength(100, ErrorMessage = "City name cannot exceed 100 characters!")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is a required field!")]
        [MaxLength(100, ErrorMessage = "Email Id cannot exceed 100 characters!")]
        [EmailAddress(ErrorMessage ="Please enter valid email address")]
        public string Email { get; set; }= string.Empty;
        public virtual ICollection<Cart>? Carts { get; set; } //connected to many carts

        [Required(ErrorMessage = "Phone number is a required field!")]
        [MaxLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters!")]
        public string Phone { get; set; } = string.Empty;
    }
}
