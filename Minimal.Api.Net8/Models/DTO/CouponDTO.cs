namespace Minimal.Api.Net8.Models.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Percent { get; set; }
        public char IsActive { get; set; }
    }
}
