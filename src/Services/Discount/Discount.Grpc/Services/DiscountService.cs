using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName) 
                     ?? new Coupon{ProductName = "No Discount", Description = "No Discount Description", Amount = 0};

        logger.LogInformation("Discount found for ProductName: {ProductName}, Amount: {amount}", request.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            logger.LogError("Coupon request is null {request}", request.Coupon);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        }
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            logger.LogError("Coupon request is null {request}", request.Coupon);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        }
        
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount updated for ProductName: {ProductName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        if (coupon == null)
        {
            logger.LogError("Coupon not found for ProductName: {ProductName}", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found with ProductName: {request.ProductName}"));
        }
        
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount deleted for ProductName: {ProductName}", request.ProductName);

        var response = new DeleteDiscountResponse
        {
            Success = true
        };
        return response;
    }
}