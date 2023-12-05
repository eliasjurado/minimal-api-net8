namespace Minimal.Api.Net8.Models
{
    public class Coupon : BaseModel
    {
        public double Percent { get; set; }
        public char IsActive { get; set; }
    }
}
