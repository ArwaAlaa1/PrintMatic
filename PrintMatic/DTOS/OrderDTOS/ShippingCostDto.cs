namespace PrintMatic.DTOS.OrderDTOS
{
    public class ShippingCostDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public decimal Cost { get; set; }
        public string ShippingTime { get; set; }
    }
}
