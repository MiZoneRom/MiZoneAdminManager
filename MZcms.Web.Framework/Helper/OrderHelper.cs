using MZcms.Application;
using MZcms.Core;
using MZcms.DTO;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MZcms.Web.Framework
{
    public class OrderHelper
    {
        /// <summary>
        /// 在无法手动选择优惠券的场景下，自动选择合适的优惠券
        /// </summary>
        public static IBaseCoupon GetDefaultCoupon(long shopid, long userid, decimal totalPrice)
        {
            var shopBonus = ShopBonusApplication.GetDetailToUse(shopid, userid, totalPrice);
            var userCoupons = CouponApplication.GetUserCoupon(shopid, userid, totalPrice);

            if (shopBonus.Count() > 0 && userCoupons.Count() > 0)
            {
                IBaseCoupon sb = shopBonus.FirstOrDefault();      //商家红包
                IBaseCoupon uc = userCoupons.FirstOrDefault();  //优惠卷
                if (sb.BasePrice > uc.BasePrice)
                {
                    return sb;
                }
                else
                {
                    return uc;
                }
            }
            else if (shopBonus.Count() <= 0 && userCoupons.Count() <= 0)
            {
                return null;
            }
            else if (shopBonus.Count() <= 0 && userCoupons.Count() > 0)
            {
                IBaseCoupon uc = userCoupons.FirstOrDefault();
                return uc;
            }
            else if (shopBonus.Count() > 0 && userCoupons.Count() <= 0)
            {
                IBaseCoupon sb = shopBonus.FirstOrDefault();
                return sb;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将前端传入参数转换成适合操作的格式
        /// </summary>
        public static IEnumerable<string[]> ConvertUsedCoupon(string couponIds)
        {
            //couponIds格式  "id_type,id_type,id_type"
            IEnumerable<string> couponArr = null;
            if (!string.IsNullOrEmpty(couponIds))
            {
                couponArr = couponIds.Split(',');
            }

            //返回格式  string[0] = id , string[1] = type
            return couponArr == null ? null : couponArr.Select(p => p.Split('_'));
        }

        /// <summary>
        /// 获取用户所有可用的优惠券
        /// </summary>
        public static List<IBaseCoupon> GetBaseCoupon(long shopId, long userId, decimal totalPrice)
        {
            var userCoupons = CouponApplication.GetUserCoupon(shopId, userId, totalPrice);
            var userBonus = ShopBonusApplication.GetDetailToUse(shopId, userId, totalPrice);

            List<IBaseCoupon> coupons = new List<IBaseCoupon>();
            coupons.AddRange(userCoupons);
            coupons.AddRange(userBonus);
            return coupons;
        }

        /// <summary>
        /// 订单提交页面，需要展示的数据
        /// </summary>
        public static List<CartItemModel> GenerateCartItem(IEnumerable<string> skuIds, IEnumerable<int> counts)
        {
            int i = 0;
            var products = skuIds.Select(item =>
           {
               var sku = ProductManagerApplication.GetSKUInfo(item);
               var count = counts.ElementAt(i++);
               var ltmbuy = LimitTimeApplication.GetLimitTimeMarketItemByProductId(sku.ProductInfo.Id);
               if (ltmbuy != null)
               {
                   if (count > ltmbuy.LimitCountOfThePeople)
                       throw new MZcmsException("超过最大限购数量：" + ltmbuy.LimitCountOfThePeople.ToString() + "");
               }
               if (sku.Stock < count)
               {
                   //throw new MZcmsException("库存不足");
               }
               return new CartItemModel()
               {
                   skuId = item,
                   id = sku.ProductInfo.Id,
                   imgUrl = Core.MZcmsIO.GetRomoteProductSizeImage(sku.ProductInfo.RelativePath, 1, (int)MZcms.CommonModel.ImageSize.Size_100),
                   name = sku.ProductInfo.ProductName,
                   shopId = sku.ProductInfo.ShopId,
                   price = ltmbuy == null ? sku.SalePrice : (decimal)LimitTimeApplication.GetDetail(item).Price,
                   count = count,
                   productCode = sku.ProductInfo.ProductCode,
                   unit = sku.ProductInfo.MeasureUnit,
                   size = sku.Size,
                   color = sku.Color,
                   version = sku.Version,
                   IsSelf = sku.ProductInfo.MZcms_Shops.IsSelf
               };
           }).ToList();

            return products;
        }

        /// <summary>
        /// 订单提交页面，需要展示的数据
        /// </summary>
        public static List<CartItemModel> GenerateCartItem(IEnumerable<ShoppingCartItem> cartItems)
        {
            var products = cartItems.Select(item =>
           {
               var product = ProductManagerApplication.GetProduct(item.ProductId);
               var sku = ProductManagerApplication.GetSKU(item.SkuId);
               return new CartItemModel()
               {
                   skuId = item.SkuId,
                   id = product.Id,
                   imgUrl = Core.MZcmsIO.GetRomoteProductSizeImage(product.RelativePath, 1, (int)MZcms.CommonModel.ImageSize.Size_100),
                   name = product.ProductName,
                   price = sku.SalePrice,
                   shopId = product.ShopId,
                   count = item.Quantity,
                   productCode = product.ProductCode,
                   color = sku.Color,
                   size = sku.Size,
                   version = sku.Version,
                   IsSelf = product.MZcms_Shops.IsSelf
               };
           }).ToList();

            return products;
        }

        /// <summary>
        /// 计算PaidPrice
        /// </summary>
        public static decimal CalculatePaidPrice(ShopCartItemModel cart)
        {
            decimal ordTotalPrice = cart.CartItemModels.Sum(c => c.price * c.count);
            decimal ordDisPrice = cart.Coupon == null ? 0 : cart.Coupon.BasePrice;
            return ordTotalPrice - ordDisPrice;
        }

        /// <summary>
        /// 满额免
        /// </summary>
        public static void SetFullFree(decimal ordPaidPrice, decimal freeFreight, ShopCartItemModel item)
        {
            item.isFreeFreight = false;
            if (freeFreight > 0)
            {
                item.shopFreeFreight = freeFreight;
                if (ordPaidPrice >= freeFreight)
                {
                    item.Freight = 0;
                    item.isFreeFreight = true;
                }
            }
        }

        /// <summary>
        /// 计算积分
        /// </summary>
        public static OrderIntegralModel GetAvailableIntegral(decimal totalAmount, decimal totalUserCoupons, decimal memberIntegral)
        {
            var integralPerMoney = MemberIntegralApplication.GetIntegralChangeRule();

            OrderIntegralModel result = new OrderIntegralModel();
            if (integralPerMoney != null && integralPerMoney.IntegralPerMoney > 0)
            {
                if ((totalAmount - totalUserCoupons) - Math.Round(memberIntegral / (decimal)integralPerMoney.IntegralPerMoney, 2) > 0)
                {
                    result.IntegralPerMoney = Math.Round(memberIntegral / (decimal)integralPerMoney.IntegralPerMoney, 2);
                    result.UserIntegrals = memberIntegral;
                }
                else
                {
                    result.IntegralPerMoney = Math.Round(totalAmount - totalUserCoupons, 2);
                    result.UserIntegrals = Math.Round((totalAmount - totalUserCoupons) * integralPerMoney.IntegralPerMoney);
                }
                if (result.IntegralPerMoney <= 0)
                {
                    result.IntegralPerMoney = 0;
                    result.UserIntegrals = 0;
                }
            }
            else
            {
                result.IntegralPerMoney = 0;
                result.UserIntegrals = 0;
            }

            return result;
        }
    }
}