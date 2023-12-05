using Minimal.Api.Net8.Helpers.Constant;
using Minimal.Api.Net8.Models;

namespace Minimal.Api.Net8.Data
{
    public class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>
      {
         new Coupon{Id=1,Name="10+2OFF",Percent=10.2,IsActive=BooleanByte.Yes.First(),CreatedBy="JURAE008",CreatedAt=new DateTime(2023,11,10),UpdatedBy="JURAE008",UpdatedAt= DateTime.Now},
         new Coupon{Id=2,Name="20OFF",Percent=20,IsActive=BooleanByte.No.First(),CreatedBy="JURAE008",CreatedAt=new DateTime(2023,11,10),UpdatedBy="JURAE008",UpdatedAt= DateTime.Now},
      };
    }
}
