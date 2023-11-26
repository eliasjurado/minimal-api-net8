namespace minimal_api_net8.Models
{
    public class Coupon : BaseModel
    {
        public double DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set;}
    }
}
