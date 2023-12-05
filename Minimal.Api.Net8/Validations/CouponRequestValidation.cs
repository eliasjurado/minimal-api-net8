using FluentValidation;
using Minimal.Api.Net8.Helpers.Constant;
using Minimal.Api.Net8.Models.DTO;

namespace Minimal.Api.Net8.Validations
{
    public class CouponRequestValidation : AbstractValidator<CouponRequestDTO>
    {
        public CouponRequestValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
            RuleFor(model => model.IsActive).Must(x => x.Equals(BooleanByte.Yes.First()) || x.Equals(BooleanByte.No.First()));
        }
    }
}
