﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Net8.Data;
using Minimal.Api.Net8.Helpers;
using Minimal.Api.Net8.Models;
using Minimal.Api.Net8.Models.DTO;
using Minimal.Api.Net8.Repository.IRepository;
using System.Net;
using System.Security.Claims;

namespace Minimal.Api.Net8.Endpoints
{
    public static class CouponEndpoints
    {

        public static void ConfigureCouponEndpoints(this WebApplication app)
        {
            app.MapGet("/api/coupon", GetAllCoupon)
                .WithName("GetCoupons")
                .Produces<APIResponse<IEnumerable<CouponDTO>>>(200)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization("AdminOnly"); //security policy

            app.MapGet("/api/coupon/{id}", GetCoupon)
                .WithName("GetCoupon")
                .AddEndpointFilter(async (context, next) =>
                {
                    APIResponse<object> response = new();
                    var id = context.GetArgument<string>(4);
                    int output;
                    if (!int.TryParse(id, out output))
                    {
                        response.Errors.Add("Invalid Id was received");
                        return Results.BadRequest(response);
                    }
                    if (output == 0)
                    {
                        response.Errors.Add("Id received cannot be zero");
                        return Results.BadRequest(response);
                    }
                    return await next(context);
                })
                .Produces<APIResponse<CouponDTO>>(200)
                .Produces(400)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization();

            app.MapPost("/api/coupon", CreateCoupon)
                .WithName("CreateCoupon")
                .Accepts<CouponRequestDTO>("application/json")
                .Produces<APIResponse<CouponDTO>>(201)
                .Produces(400)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization();

            app.MapPut("/api/coupon/{id}", UpdateCoupon)
                .WithName("UpdateCoupon")
                .Accepts<CouponRequestDTO>("application/json")
                .Produces<APIResponse<CouponDTO>>(200)
                .Produces(400)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization();

            app.MapDelete("/api/coupon/{id}", DeleteCoupon)
                .WithName("DeleteCoupon")
                .Produces<APIResponse<CouponDTO>>(200)
                .Produces(400)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization();

            app.MapGet("/api/coupon/search", SearchCoupons)
                .WithName("SearchCoupons")
                .Produces<APIResponse<ICollection<CouponDTO>>>(200)
                .Produces(400)
                .Produces(401)
                .Produces(403)
                .RequireAuthorization();
        }

        private async static Task<IResult> GetAllCoupon(HttpContext context, IConfiguration _configuration, ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger)
        {
            APIResponse<IEnumerable<CouponDTO>> response = new();

            var userName = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(userName))
            {
                response.Errors.Add("Invalid User Name was received");
                return Results.BadRequest(response);
            }

            _logger.Log(LogLevel.Information, "Getting all Coupons");
            response.IsSuccess = true;
            response.Result = (await _repository.GetAsync()).Select(_mapper.Map<CouponDTO>);
            response.StatusCode = HttpStatusCode.OK;
            response.Status = nameof(HttpStatusCode.OK);

            return Results.Ok(response);
        }

        [Authorize(Roles = "Admin,Manager")]
        private async static Task<IResult> GetCoupon(HttpContext context, ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger, string id)
        {
            APIResponse<CouponDTO> response = new();

            var userName = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(userName))
            {
                response.Errors.Add("Invalid User Name was received");
                return Results.BadRequest(response);
            }

            int output;
            if (!int.TryParse(id, out output))
            {
                response.Errors.Add("Invalid Id was received");
                return Results.BadRequest(response);
            }

            var couponDto = _mapper.Map<CouponDTO>(await _repository.GetAsync(output));
            if (couponDto == null)
            {
                response.Errors.Add($"Coupon with Id {id} not found");
                return Results.BadRequest(response);
            }

            response.IsSuccess = true;
            response.Result = couponDto;
            response.StatusCode = HttpStatusCode.OK;
            response.Status = nameof(HttpStatusCode.OK);

            return Results.Ok(response);
        }

        private async static Task<IResult> CreateCoupon(HttpContext context, ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger, IValidator<CouponRequestDTO> _validator, [FromBody] CouponRequestDTO couponRequestDto)
        {
            var date = DateTime.Now;
            APIResponse<CouponDTO> response = new();

            var userName = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(userName))
            {
                response.Errors.Add("Invalid User Name was received");
                return Results.BadRequest(response);
            }

            var validationResult = await _validator.ValidateAsync(couponRequestDto);

            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => response.Errors.Add(x.ErrorMessage));
                return Results.BadRequest(response);
            }

            if (await _repository.GetAsync(couponRequestDto.Name) != null)
            {
                response.Errors.Add("Coupon Name already exists");
                return Results.BadRequest(response);
            }

            var coupon = _mapper.Map<Coupon>(couponRequestDto);
            coupon.Name = couponRequestDto.Name;
            coupon.Percent = couponRequestDto.Percent;
            coupon.IsActive = couponRequestDto.IsActive;
            coupon.CreatedAt = date;
            coupon.UpdatedAt = date;
            coupon.CreatedBy = userName;
            coupon.UpdatedBy = userName;

            await _repository.CreateAsync(coupon);
            await _repository.SaveAsync();

            response.IsSuccess = true;
            response.Result = _mapper.Map<CouponDTO>(coupon);
            response.StatusCode = HttpStatusCode.Created;
            response.Status = Format.GetName(nameof(HttpStatusCode.Created));

            return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, response);
        }

        private async static Task<IResult> UpdateCoupon(HttpContext context, ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger, IValidator<CouponRequestDTO> _validator, [FromBody] CouponRequestDTO couponRequestDto, string id)
        {
            APIResponse<CouponDTO> response = new();

            var userName = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).SingleOrDefault();
            if (string.IsNullOrWhiteSpace(userName))
            {
                response.Errors.Add("Invalid User Name was received");
                return Results.BadRequest(response);
            }

            var validationResult = await _validator.ValidateAsync(couponRequestDto);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => response.Errors.Add(x.ErrorMessage));
                return Results.BadRequest(response);
            }

            int output;
            if (!int.TryParse(id, out output))
            {
                response.Errors.Add("Invalid Id was received");
                return Results.BadRequest(response);
            }

            var existingCoupon = await _repository.GetAsync(couponRequestDto.Name);
            if (existingCoupon != null && existingCoupon.Id != output)
            {
                response.Errors.Add("Coupon Name already exists");
                return Results.BadRequest(response);
            }

            var coupon = await _repository.GetAsync(output);

            if (coupon == null)
            {
                response.Errors.Add($"Coupon with Id {id} not found");
                return Results.BadRequest(response);
            }

            coupon.IsActive = couponRequestDto.IsActive;
            coupon.Name = couponRequestDto.Name;
            coupon.Percent = couponRequestDto.Percent;
            coupon.UpdatedBy = userName;
            coupon.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(coupon);
            await _repository.SaveAsync();

            response.IsSuccess = true;
            response.Result = _mapper.Map<CouponDTO>(coupon);
            response.StatusCode = HttpStatusCode.OK;
            response.Status = nameof(HttpStatusCode.OK);

            return Results.Ok(response);
        }

        private async static Task<IResult> DeleteCoupon(HttpContext context, ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger, string id)
        {
            APIResponse<CouponDTO> response = new();

            var userName = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).SingleOrDefault();
            if (string.IsNullOrWhiteSpace(userName))
            {
                response.Errors.Add("Invalid User Name was received");
                return Results.BadRequest(response);
            }

            int output;
            if (!int.TryParse(id, out output))
            {
                response.Errors.Add("Invalid Id was received");
                return Results.BadRequest(response);
            }

            var coupon = await _repository.GetAsync(output);

            if (coupon == null)
            {
                response.Errors.Add($"Coupon with Id {id} not found");
                return Results.BadRequest(response);
            }

            response.Result = _mapper.Map<CouponDTO>(coupon);
            await _repository.RemoveAsync(coupon);
            await _repository.SaveAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Status = nameof(HttpStatusCode.OK);

            return Results.Ok(response);
        }

        private async static Task<IResult> SearchCoupons(IMapper _mapper, [AsParameters] CouponSearchRequestDTO request, ApplicationDbContext _db)
        {
            APIResponse<ICollection<CouponDTO>> response = new();

            await Task.Run(() =>
            {
                if (request.CouponName != null)
                {
                    response.Result = _db.Coupons.Where(u => u.Name.Contains(request.CouponName)).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(x => _mapper.Map<CouponDTO>(x)).ToList();
                }
                response.Result = _db.Coupons.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(x => _mapper.Map<CouponDTO>(x)).ToList();
            });

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Status = nameof(HttpStatusCode.OK);

            return Results.Ok(response);
        }

    }
}
