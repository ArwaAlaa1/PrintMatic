namespace PrintMatic.Core.Entities
{
    public class CartItems
    {

        public int Id { get; set; }
        public string ItemName { get; set; }
        public string PhotoURL { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}