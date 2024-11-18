namespace PrintMatic.DTOS.IdentityDTOS
{
    public class AddressUseIdDto
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string Country { get; set; }
        public string? AddressDetails { get; set; }
    }
}
