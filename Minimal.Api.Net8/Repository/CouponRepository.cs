using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Minimal.Api.Net8.Data;
using Minimal.Api.Net8.Models;
using Minimal.Api.Net8.Repository.IRepository;
using System.Linq.Expressions;
using static Azure.Core.HttpHeader;

namespace Minimal.Api.Net8.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;

        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Coupon coupon)
        {
            _db.Add(coupon);
        }

        public async Task<Coupon> GetAsync(int id)
        {
            return await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Coupon> GetAsync(string name)
        {
            return await _db.Coupons.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<ICollection<Coupon>> GetAsync(Expression<Func<Coupon, bool>> expression)
        {
            return await _db.Coupons.Where(expression).ToListAsync();
        }

        public async Task<ICollection<Coupon>> GetAsync()
        {
            return await _db.Coupons.ToListAsync();
        }

        public async Task RemoveAsync(Coupon coupon)
        {
            _db.Remove(coupon);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coupon coupon)
        {
            _db.Coupons.Update(coupon);
        }
    }
}
