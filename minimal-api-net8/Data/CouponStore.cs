using minimal_api_net8.Models;

namespace minimal_api_net8.Data
{
   public class CouponStore
   {
      public static List<Coupon> couponList = new List<Coupon>
      {
         new Coupon{Id=1,Name="10+2OFF",DiscountPercent=10.2,IsActive=true,EffectiveStartDate=new DateTime(2023,11,10),EffectiveEndDate=new DateTime(2023,11,30),CreatedBy="JURAE008",CreatedAt=new DateTime(2023,11,10),UpdatedBy="JURAE008",UpdatedAt= DateTime.Now},
         new Coupon{Id=2,Name="20OFF",DiscountPercent=20,IsActive=false,EffectiveStartDate=new DateTime(2023,12,10),EffectiveEndDate=new DateTime(2023,12,30),CreatedBy="JURAE008",CreatedAt=new DateTime(2023,11,10),UpdatedBy="JURAE008",UpdatedAt= DateTime.Now},
      };
   }
}
