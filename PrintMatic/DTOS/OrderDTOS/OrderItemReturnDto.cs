using PrintMatic.Core.Entities.Order;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class OrderItemReturnDto
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public string Photo { get; set; }
        //normal or busy
        public string ItemType { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public decimal? PriceAfterSale { get; set; }

        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }
        public List<string?> Photos { get; set; }
        public string? FilePdf { get; set; }

        public string OrderItemStatus { get; set; } 
        private decimal TotaPrice;

        public int Quantity { get; set; }
    }
}
