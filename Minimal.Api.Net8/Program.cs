using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Net8;
using Minimal.Api.Net8.Data;
using Minimal.Api.Net8.Models;
using Minimal.Api.Net8.Models.DTO;
using FluentValidation;
using System.Net;
using Infrastructure.Minimal.Api.Net8;
using Microsoft.EntityFrameworkCore;
using Minimal.Api.Net8.Repository.IRepository;
using Minimal.Api.Net8.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", async (ICouponRepository _repository, IMapper _mapper, ILogger<Program> _logger, [FromHeader(Name = "x-user-id")] string userId) =>
{
    APIResponse<IEnumerable<CouponDTO>> response = new();

    if (string.IsNullOrWhiteSpace(userId))
    {
        response.Errors.Add("Invalid User Id was received");
        return Results.BadRequest(response);
    }

    _logger.Log(LogLevel.Information, "Getting all Coupons");
    response.IsSuccess = true;
    response.Result = (await _repository.GetAsync()).Select(coupon => _mapper.Map<CouponDTO>(coupon));
    response.StatusCode = HttpStatusCode.OK;
    response.Status = nameof(HttpStatusCode.OK);

    return Results.Ok(response);

}).WithName("GetCoupons").Produces<APIResponse<IEnumerable<CouponDTO>>>(200);

app.MapGet("/api/coupon/{id}", async (ICouponRepository _repository, IMapper _mapper, [FromHeader(Name = "x-user-id")] string userId, string id) =>
{
    var output = 0;
    APIResponse<CouponDTO> response = new();

    if (string.IsNullOrWhiteSpace(userId))
    {
        response.Errors.Add("Invalid User Id was received");
        return Results.BadRequest(response);
    }

    if (int.TryParse(id, out output))
    {
        var couponDto = _mapper.Map<CouponDTO>(await _repository.GetAsync(output));
        response.IsSuccess = true;
        response.Result = couponDto;
        response.StatusCode = HttpStatusCode.OK;
        response.Status = nameof(HttpStatusCode.OK);
        return Results.Ok(couponDto);
    }

    return Results.BadRequest("Invalid Coupon Id");

}).WithName("GetCoupon").Produces<APIResponse<CouponDTO>>(200).Produces(400);

app.MapPost("/api/coupon", async (ICouponRepository _repository, IMapper _mapper, IValidator<CouponRequestDTO> _validator, [FromHeader(Name = "x-user-id")] string userId, [FromBody] CouponRequestDTO couponRequestDto) =>
{
    var date = DateTime.Now;
    APIResponse<CouponDTO> response = new();

    if (string.IsNullOrWhiteSpace(userId))
    {
        response.Errors.Add("Invalid User Id was received");
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
    coupon.CreatedBy = userId;
    coupon.UpdatedBy = userId;

    await _repository.CreateAsync(coupon);
    await _repository.SaveAsync();

    response.IsSuccess = true;
    response.Result = _mapper.Map<CouponDTO>(coupon);
    response.StatusCode = HttpStatusCode.Created;
    response.Status = Format.GetName(nameof(HttpStatusCode.Created));

    return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, response);

}).WithName("CreateCoupon").Accepts<CouponRequestDTO>("application/json").Produces<APIResponse<CouponDTO>>(201).Produces(400);

app.MapPut("/api/coupon/{id}", async (ICouponRepository _repository, IMapper _mapper, IValidator<CouponRequestDTO> _validator, [FromHeader(Name = "x-user-id")] string userId, [FromBody] CouponRequestDTO couponRequestDto, string id) =>
{
    var output = 0;
    APIResponse<CouponDTO> response = new();

    if (string.IsNullOrWhiteSpace(userId))
    {
        response.Errors.Add("Invalid User Id was received");
        return Results.BadRequest(response);
    }

    var validationResult = await _validator.ValidateAsync(couponRequestDto);

    if (!validationResult.IsValid)
    {
        validationResult.Errors.ForEach(x => response.Errors.Add(x.ErrorMessage));
        return Results.BadRequest(response);
    }

    if (!int.TryParse(id, out output))
    {
        response.Errors.Add("Invalid Id was received");
        return Results.BadRequest(response);
    }

    var existingCoupon = await _repository.GetAsync(couponRequestDto.Name);
    if (existingCoupon!=null && existingCoupon.Id != output)
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
    coupon.UpdatedBy = userId;
    coupon.UpdatedAt = DateTime.Now;

    await _repository.UpdateAsync(coupon);
    await _repository.SaveAsync();

    response.IsSuccess = true;
    response.Result = _mapper.Map<CouponDTO>(coupon);
    response.StatusCode = HttpStatusCode.OK;
    response.Status = nameof(HttpStatusCode.OK);

    return Results.Ok(response);

}).WithName("UpdateCoupon").Accepts<CouponRequestDTO>("application/json").Produces<APIResponse<CouponDTO>>(200).Produces(400);

app.MapDelete("/api/coupon/{id}", async (ICouponRepository _repository, IMapper _mapper, [FromHeader(Name = "x-user-id")] string userId, string id) =>
{
    var output = 0;
    APIResponse<CouponDTO> response = new();

    if (string.IsNullOrWhiteSpace(userId))
    {
        response.Errors.Add("Invalid User Id was received");
        return Results.BadRequest(response);
    }

    var parsed = int.TryParse(id, out output);
    if (!parsed)
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

}).WithName("DeleteCoupon").Accepts<string>("application/json").Produces<APIResponse<CouponDTO>>(200).Produces(400);

app.UseHttpsRedirection();

app.Run();