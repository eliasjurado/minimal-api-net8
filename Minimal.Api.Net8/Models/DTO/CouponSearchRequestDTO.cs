using Microsoft.AspNetCore.Mvc;

namespace Minimal.Api.Net8.Models.DTO
{
    public class CouponSearchRequestDTO
    {
        public string CouponName { get; set; }
        [FromHeader(Name = "PageSize")]
        public int PageSize { get; set; }
        [FromHeader(Name = "Page")]
        public int Page { get; set; }
        public ILogger<CouponSearchRequestDTO> Logger { get; set; }
    }
}
