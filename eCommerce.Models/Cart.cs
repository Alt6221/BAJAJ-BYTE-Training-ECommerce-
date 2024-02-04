using System.ComponentModel.DataAnnotations;
namespace eCommerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime CartDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }

        //Navigation property
        public virtual Customer Customer { get; set; } //connected to one customer'.

        public ICollection<CartDetail>? CartDetails { get; set; }
        public virtual Invoice InvoiceId { get; set; }
    }
}
