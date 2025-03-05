namespace Api_ECommerce.DTO
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    }
}
