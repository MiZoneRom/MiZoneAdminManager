
using System.ComponentModel.DataAnnotations.Schema;
namespace MZcms.Model
{
    public partial class SiteSettings
    {

        /// <summary>
        /// 站点名称
        /// </summary>
        [NotMapped]
        public string SiteName { get; set; }

        /// <summary>
        /// ICP编号
        /// </summary>
        [NotMapped]
        public string ICPNubmer { get; set; }

        /// <summary>
        /// 客服联系电话
        /// </summary>
        [NotMapped]
        public string CustomerTel { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        [NotMapped]
        public bool SiteIsClose { get; set; }

        /// <summary>
        /// 注册方式(枚举)
        /// </summary>
        public enum RegisterTypes
        {
            /// <summary>
            /// 普通账号
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 手机账号
            /// </summary>
            Mobile = 1
        }

        /// <summary>
        /// 注册方式
        /// </summary>
        [NotMapped]
        public int RegisterType { get; set; }
        /// <summary>
        /// 手机是否需验证
        /// </summary>
        [NotMapped]
        public bool MobileVerifOpen { set; get; }
        /// <summary>
        /// 邮箱是否必填
        /// </summary>
        [NotMapped]
        public bool RegisterEmailRequired { get; set; }
        /// <summary>
        /// 邮箱是否需要验证
        /// </summary>
        [NotMapped]
        public bool EmailVerifOpen { set; get; }

        /// <summary>
        /// lOGO图片
        /// </summary>
        [NotMapped]
        public string Logo { set; get; }
        /// <summary>
        /// 微信Logo
        /// <para>用于微信卡券，100*100，小于1M</para>
        /// </summary>
        [NotMapped]
        public string WXLogo
        {
            get;
            set;
        }
        /// <summary>
        /// PC登录页左侧图片
        /// </summary>
        [NotMapped]
        public string PCLoginPic
        {
            get;
            set;
        }
        /// <summary>
        /// PC底部服务图片
        /// </summary>
        [NotMapped]
        public string PCBottomPic
        {
            get;
            set;
        }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [NotMapped]
        public string Keyword { set; get; }
        /// <summary>
        /// 热门关键字
        /// </summary>
        [NotMapped]
        public string Hotkeywords { set; get; }

        /// <summary>
        /// 首页页脚
        /// </summary>
        [NotMapped]
        public string PageFoot { get; set; }

        /// <summary>
        /// 微信AppId
        /// </summary>
        [NotMapped]
        public string WeixinAppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        [NotMapped]
        public string WeixinAppSecret { get; set; }

        /// <summary>
        /// 微信AppId
        /// </summary>
        [NotMapped]
        public string WeixinAppletId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        [NotMapped]
        public string WeixinAppletSecret { get; set; }

        /// <summary>
        /// 门店小程序AppId
        /// </summary>
        [NotMapped]
        public string WeixinO2OAppletId { get; set; }

        /// <summary>
        /// 门店小程序AppSecret
        /// </summary>
        [NotMapped]
        public string WeixinO2OAppletSecret { get; set; }

        /// <summary>
        /// 微信WeixinToken
        /// </summary>
        [NotMapped]
        public string WeixinToken { get; set; }


        /// <summary>
        /// 微信合作伙伴ID
        /// </summary>
        [NotMapped]
        public string WeixinPartnerID { get; set; }

        /// <summary>
        /// 微信合作伙伴key
        /// </summary>
        [NotMapped]
        public string WeixinPartnerKey { get; set; }

        /// <summary>
        /// 微信URL配置
        /// </summary>
        [NotMapped]
        public string WeixinLoginUrl { get; set; }

        /// <summary>
        /// 微信是否为服务号
        /// </summary>
        [NotMapped]
        public bool WeixinIsValidationService { get; set; }

        /// <summary>
        /// 用户Cookie密钥
        /// </summary>
        [NotMapped]
        public string UserCookieKey { get; set; }

        /// <summary>
        /// 商家入住协议
        /// </summary>
        public string SellerAdminAgreement { get; set; }

        /// <summary>
        /// 预付款百分比
        /// </summary>
        [NotMapped]
        public decimal AdvancePaymentPercent { get; set; }

        /// <summary>
        /// 预付款上限
        /// </summary>
        [NotMapped]
        public decimal AdvancePaymentLimit { get; set; }

        /// <summary>
        /// 结算周期
        /// </summary>
        [NotMapped]
        public int WeekSettlement { get; set; }

        [NotMapped]
        public string MemberLogo
        {
            get;
            set;
        }

        [NotMapped]
        public string QRCode
        {
            get;
            set;
        }

        [NotMapped]
        public string FlowScript
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEOTitle
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEOKeywords
        {
            get;
            set;
        }

        [NotMapped]
        public string Site_SEODescription
        {
            get;
            set;
        }

        /// <summary>
        /// 未付款超时(小时)
        /// </summary>        
        public int UnpaidTimeout { get; set; }

        /// <summary>
        /// 确认收货超时(天数)
        /// </summary>        
        public int NoReceivingTimeout { get; set; }
        /// <summary>
        /// 关闭评价通道时限(天数)
        /// </summary>
        public int OrderCommentTimeout { get; set; }

        /// <summary>
        /// 确认收货后可退货周期(天数)
        /// </summary>
        public int SalesReturnTimeout { get; set; }
        /// <summary>
        /// 售后-商家自动确认售后时限(天数)
        /// <para>到期未审核，自动认为同意售后</para>
        /// </summary>
        public int AS_ShopConfirmTimeout { get; set; }
        /// <summary>
        /// 售后-用户发货限时(天数)
        /// <para>到期未发货自动关闭售后</para>
        /// </summary>
        public int AS_SendGoodsCloseTimeout { get; set; }
        /// <summary>
        /// 售后-商家确认到货时限(天数)
        /// <para>到期未收货，自动收货</para>
        /// </summary>
        public int AS_ShopNoReceivingTimeout { get; set; }


        /// <summary>
        /// 是否开启商品免审核上架
        /// </summary>
        public int ProdutAuditOnOff { get; set; }

        /// <summary>
        /// 微信获取红包模板消息编号
        /// </summary>
        [NotMapped]
        public string WX_MSGGetCouponTemplateId
        {
            get;
            set;
        }
        [NotMapped]
        /// <summary>
        /// APP更新说明
        /// </summary>
        public string AppUpdateDescription { set; get; }
        [NotMapped]
        /// <summary>
        /// app版本号
        /// </summary>
        public string AppVersion { set; get; }
        [NotMapped]
        /// <summary>
        /// 安卓下载地址
        /// </summary>
        public string AndriodDownLoad { set; get; }


        [NotMapped]
        /// <summary>
        /// IOS下载地址
        /// </summary>
        public string IOSDownLoad { set; get; }

        [NotMapped]
        /// <summary>
        /// 是否提供下载
        /// </summary>
        public bool CanDownload { set; get; }

        [NotMapped]
        /// <summary>
        /// 门店、商家app版本号
        /// </summary>
        public string ShopAppVersion { set; get; }
        [NotMapped]
        /// <summary>
        /// 门店、商家安卓下载地址
        /// </summary>
        public string ShopAndriodDownLoad { set; get; }


        [NotMapped]
        /// <summary>
        /// 门店、商家IOS下载地址
        /// </summary>
        public string ShopIOSDownLoad { set; get; }

        [NotMapped]
        /// <summary>
        /// 0、快递100；1、快递鸟
        /// </summary>
        public int KuaidiType { get; set; }

        [NotMapped]
        /// <summary>
        /// 快递100KEY
        /// </summary>
        public string Kuaidi100Key { set; get; }

        /// <summary>
        /// 快递鸟物流appkey
        /// </summary>
        public string KuaidiApp_key { get; set; }

        /// <summary>
        /// 快递鸟物流AppSecret
        /// </summary>
        public string KuaidiAppSecret { get; set; }

        /// <summary>
        /// 提现最低金额
        /// </summary>
        [NotMapped]
        public int WithDrawMinimum { get; set; }

        /// <summary>
        /// 提现最高金额
        /// </summary>
        [NotMapped]
        public int WithDrawMaximum { get; set; }

        /// <summary>
        /// 商家提现最低金额
        /// </summary>
        [NotMapped]
        public int ShopWithDrawMinimum { get; set; }

        /// <summary>
        /// 商家提现最高金额
        /// </summary>
        [NotMapped]
        public int ShopWithDrawMaximum { get; set; }

        /// <summary>
        /// 首页是否显示限时购
        /// </summary>
        public bool Limittime { get; set; }

        /// <summary>
        /// 广告图片地址
        /// </summary>
        public string AdvertisementImagePath { get; set; }

        /// <summary>
        /// 广告链接地址
        /// </summary>
        public string AdvertisementUrl { get; set; }

        /// <summary>
        /// 广告状态
        /// </summary>
        public bool AdvertisementState { get; set; }

        [NotMapped]
        /// <summary>
        /// 是否开启订单自动匹配到门店
        /// </summary>
        public bool AutoAllotOrder { set; get; }

        /// <summary>
        /// 是否授权门店
        /// </summary>
        public bool IsOpenStore { get; set; }

        /// <summary>
        /// 是否授权商家APP
        /// </summary>
        public bool IsOpenShopApp { get; set; }

        /// <summary>
        /// 客服电话
        /// </summary>
        [NotMapped]
        public string SitePhone { get; set; }

        /// <summary>
        /// 小程序版本号（编辑首页模版时每次改变）
        /// </summary>
        public long XcxHomeVersionCode { get; set; }

        /// <summary>
        /// 是否开启支付宝提现
        /// </summary>
        [NotMapped]
        public bool Withdraw_AlipayEnable { set; get; }

        /// <summary>
        /// 多门店小程序是否使用顶部轮播图
        /// </summary>
        [NotMapped]
        public bool O2OApplet_IsUseTopSlide { set; get; }
        /// <summary>
        /// 多门店小程序是否使用图标区
        /// </summary>
        [NotMapped]
        public bool O2OApplet_IsUseIconArea { set; get; }
        /// <summary>
        /// 多门店小程序是否使用广告区
        /// </summary>
        [NotMapped]
        public bool O2OApplet_IsUseAdArea { set; get; }
        /// <summary>
        /// 多门店小程序是否使用中间轮播图
        /// </summary>
        [NotMapped]
        public bool O2OApplet_IsUseMiddleSlide { set; get; }
        /// <summary>
        /// 积分最高可扣抵比例
        /// </summary>
        [NotMapped]
        public int IntegralDeductibleRate { get; set; }

        /// <summary>
        /// 京东地址库APPKEY
        /// </summary>
        [NotMapped]
        public string JDRegionAppKey { get; set; }
        /// <summary>
        /// 是否授权PC端
        /// </summary>
        public bool IsOpenPC { get; set; }
        /// <summary>
        /// 是否授权H5
        /// </summary>
        public bool IsOpenH5 { get; set; }
        /// <summary>
        /// 是否授权APP[IOS和安卓的商城APP]
        /// </summary>
        public bool IsOpenApp { get; set; }
        /// <summary>
        /// 是否授权商城小程序
        /// </summary>
        public bool IsOpenMallSmallProg { get; set; }
        /// <summary>
        /// 是否授权多门店小程序
        /// </summary>
        public bool IsOpenMultiStoreSmallProg { get; set; }
        /// <summary>
        /// 是否所有会员都显示分销佣金提示
        /// </summary>
        public bool IsAllUserShowDistributorTips { get; set; }
        /// <summary>
        /// 限时购活动是否需要审核
        /// </summary>
        public bool LimitTimeBuyNeedAuditing { get; set; }
        /// <summary>
        /// 限时购活动是否需要审核
        /// </summary>
        public bool BargainNeedAuditing { get; set; }

        public bool IsOpenHistoryOrder { get; set; }

        [NotMapped]
        /// <summary>
        /// 门店、商家app版本号
        /// </summary>
        public string MZcmsJDVersion { set; get; }
    }
}
