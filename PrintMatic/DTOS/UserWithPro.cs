namespace PrintMatic.DTOS
{
    public class UserWithPro
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? FilePath { get; set; }
        public int ProductsCount { get; set; }
        public int Orders {  get; set; }
        public decimal UserAvgRating { get; set; }
        public IEnumerable<ProductDto> products { get; set; }= new List<ProductDto>();
    }
}
