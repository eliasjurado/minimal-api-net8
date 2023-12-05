namespace Minimal.Api.Net8.Models.DTO
{
    public class CouponRequestDTO
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public char IsActive { get; set; }
    }
}
