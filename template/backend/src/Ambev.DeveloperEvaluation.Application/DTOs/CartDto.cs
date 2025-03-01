namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public List<CartProductDto> Products { get; set; }
    }
}
