using MZcms.CommonModel;
using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.DTO.QueryModel;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace MZcms.Service
{
    public class ManagerService : ServiceBase, IManagerService
    {
        public QueryPageModel<Managers> GetPlatformManagers(ManagerQuery query)
        {
            int total = 0;
            IQueryable<Managers> users = Context.Managers.FindBy(item => item.Id > 0, query.PageNo, query.PageSize, out total, item => item.RoleId, true);
            QueryPageModel<Managers> pageModel = new QueryPageModel<Managers>()
            {
                Models = users.ToList(),
                Total = total
            };
            return pageModel;
        }
        public QueryPageModel<Managers> GetSellerManagers(ManagerQuery query)
        {
            int total = 0;
            IQueryable<Managers> users = Context.Managers.FindBy(item => item.RoleId != 0 && item.Id != query.userID, item => item.Id, query.PageNo, query.PageSize, out total);
            QueryPageModel<Managers> pageModel = new QueryPageModel<Managers>()
            {
                Models = users.ToList(),
                Total = total
            };
            return pageModel;
        }

        public IQueryable<Managers> GetPlatformManagerByRoleId(long roleId)
        {
            return Context.Managers.FindBy(item => item.RoleId == roleId);
        }

        public Managers GetPlatformManager(long userId)
        {
            Managers manager = null;
            string CACHE_MANAGER_KEY = CacheKeyCollection.Manager(userId);

            if (Cache.Exists(CACHE_MANAGER_KEY))
            {
                manager = Core.Cache.Get<Managers>(CACHE_MANAGER_KEY);
            }
            else
            {
                manager = Context.Managers.FirstOrDefault(item => item.Id == userId);
                if (manager == null)
                    return null;
                if (manager.RoleId == 0)
                {
                    List<AdminPrivilege> AdminPrivileges = new List<AdminPrivilege>();
                    AdminPrivileges.Add((AdminPrivilege)0);
                    manager.RoleName = "系统管理员";
                    manager.AdminPrivileges = AdminPrivileges;
                    manager.Description = "系统管理员";
                }
                else
                {
                    var model = Context.RoleInfo.FirstOrDefault(p => p.Id == manager.RoleId);
                    if (model != null)
                    {
                        List<AdminPrivilege> AdminPrivileges = new List<AdminPrivilege>();
                        model.RolePrivilegeInfo.ToList().ForEach(a => AdminPrivileges.Add((AdminPrivilege)a.Privilege));
                        manager.RoleName = model.RoleName;
                        manager.AdminPrivileges = AdminPrivileges;
                        manager.Description = model.Description;
                    }
                }
                Cache.Insert(CACHE_MANAGER_KEY, manager);
            }
            return manager;
        }

        public IQueryable<Managers> GetSellerManagerByRoleId(long roleId, long shopId)
        {
            return Context.Managers.FindBy(item => item.RoleId == roleId);
        }

        /// <summary>
        /// 根据ShopId获取对应系统管理信息
        /// <para>仅获取首页店铺系统管理员</para>
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public Managers GetSellerManagerByShopId(long shopId)
        {
            return Context.Managers.SingleOrDefault(item => item.RoleId == 0);
        }

        public Managers GetSellerManager(long userId)
        {
            Managers manager = null;

            string CACHE_MANAGER_KEY = CacheKeyCollection.Seller(userId);

            if (Cache.Exists(CACHE_MANAGER_KEY))
            {
                manager = Core.Cache.Get<Managers>(CACHE_MANAGER_KEY);
            }
            else
            {
                manager = Context.Managers.Where(item => item.Id == userId ).FirstOrDefault();
                if (manager == null)
                    return null;
                if (manager.RoleId == 0)
                {
                    List<SellerPrivilege> SellerPrivileges = new List<SellerPrivilege>();
                    SellerPrivileges.Add(0);
                    manager.RoleName = "系统管理员";
                    manager.SellerPrivileges = SellerPrivileges;
                    manager.Description = "系统管理员";
                }
                else
                {
                    var model = Context.RoleInfo.FirstOrDefault(p => p.Id == manager.RoleId);
                    if (model != null)
                    {
                        List<SellerPrivilege> SellerPrivileges = new List<SellerPrivilege>();
                        model.RolePrivilegeInfo.ToList().ForEach(a => SellerPrivileges.Add((SellerPrivilege)a.Privilege));
                        manager.RoleName = model.RoleName;
                        manager.SellerPrivileges = SellerPrivileges;
                        manager.Description = model.Description;
                    }
                }
                Cache.Insert(CACHE_MANAGER_KEY, manager);
            }
            if (manager != null)
            {
                var vshop = Context.VShopInfo.FirstOrDefault(item => item.ShopId == manager.ShopId);
                manager.VShopId = -1;
                if (vshop != null)
                {
                    manager.VShopId = vshop.Id;
                }
            }
            return manager;
        }

        public void AddPlatformManager(Managers model)
        {
            if (model.RoleId == 0)
                throw new MZcmsException("权限组选择不正确!");
            if (CheckUserNameExist(model.UserName, true))
            {
                throw new MZcmsException("该用户名已存在！");
            }
            model.ShopId = 0;
            model.PasswordSalt = Guid.NewGuid().ToString();
            model.CreateDate = DateTime.Now;
            var pwd = SecureHelper.MD5(model.Password);
            model.Password = SecureHelper.MD5(pwd + model.PasswordSalt);
            Context.Managers.Add(model);
            Context.SaveChanges();
        }

        public void AddSellerManager(Managers model, string currentSellerName)
        {
            if (model.RoleId == 0)
                throw new MZcmsException("权限组选择不正确!");
            if (CheckUserNameExist(model.UserName))
            {
                throw new MZcmsException("该用户名已存在！");
            }
            if (model.ShopId == 0)
            {
                throw new MZcmsException("没有权限进行该操作！");
            }
            model.PasswordSalt = Guid.NewGuid().ToString();
            model.CreateDate = DateTime.Now;
            var pwd = SecureHelper.MD5(model.Password);
            model.Password = SecureHelper.MD5(pwd + model.PasswordSalt);
            Context.Managers.Add(model);
            Context.SaveChanges();
        }


        public void ChangePlatformManagerPassword(long id, string password, long roleId)
        {
            var model = Context.Managers.FindBy(item => item.Id == id && item.ShopId == 0).FirstOrDefault();
            if (model == null)
                throw new MZcmsException("该管理员不存在，或者已被删除!");
            if (roleId != 0 && model.RoleId != 0)
                model.RoleId = roleId;
            if (!string.IsNullOrWhiteSpace(password))
            {
                var pwd = SecureHelper.MD5(password);
                model.Password = SecureHelper.MD5(pwd + model.PasswordSalt);
            }

            Context.SaveChanges();
            string CACHE_MANAGER_KEY = CacheKeyCollection.Manager(id);
            Core.Cache.Remove(CACHE_MANAGER_KEY);
        }


        public void ChangeSellerManager(Managers info)
        {
            var model = Context.Managers.FindBy(item => item.Id == info.Id && item.ShopId == info.ShopId).FirstOrDefault();
            if (model == null)
                throw new MZcmsException("该管理员不存在，或者已被删除!");
            if (info.RoleId != 0 && model.RoleId != 0)
                model.RoleId = info.RoleId;
            if (!string.IsNullOrWhiteSpace(info.Password))
            {
                var pwd = SecureHelper.MD5(info.Password);
                model.Password = SecureHelper.MD5(pwd + model.PasswordSalt);
            }
            model.RealName = info.RealName;
            model.Remark = info.Remark;
            Context.SaveChanges();
            string CACHE_MANAGER_KEY = CacheKeyCollection.Seller(info.Id);
            Core.Cache.Remove(CACHE_MANAGER_KEY);
        }

        public void ChangeSellerManagerPassword(long id, long shopId, string password, long roleId)
        {
            var model = Context.Managers.FindBy(item => item.Id == id && item.ShopId == shopId).FirstOrDefault();
            if (model == null)
                throw new MZcmsException("该管理员不存在，或者已被删除!");
            if (roleId != 0 && model.RoleId != 0)
                model.RoleId = roleId;
            if (!string.IsNullOrWhiteSpace(password))
            {
                var pwd = SecureHelper.MD5(password);
                model.Password = SecureHelper.MD5(pwd + model.PasswordSalt);
            }
            Context.SaveChanges();
            string CACHE_MANAGER_KEY = CacheKeyCollection.Seller(id);
            Core.Cache.Remove(CACHE_MANAGER_KEY);
        }


        public void DeletePlatformManager(long id)
        {
            var model = Context.Managers.FindBy(item => item.Id == id && item.ShopId == 0 && item.RoleId != 0).FirstOrDefault();
            Context.Managers.Remove(model);
            Context.SaveChanges();
            string CACHE_MANAGER_KEY = CacheKeyCollection.Manager(id);
            Core.Cache.Remove(CACHE_MANAGER_KEY);
        }


        public void BatchDeletePlatformManager(long[] ids)
        {
            var model = Context.Managers.FindBy(item => item.ShopId == 0 && item.RoleId != 0 && ids.Contains(item.Id));
            Context.Managers.RemoveRange(model);
            Context.SaveChanges();
            foreach (var id in ids)
            {
                string CACHE_MANAGER_KEY = CacheKeyCollection.Manager(id);
                Core.Cache.Remove(CACHE_MANAGER_KEY);
            }
        }


        public void DeleteSellerManager(long id, long shopId)
        {
            var model = Context.Managers.FindBy(item => item.Id == id && item.ShopId == shopId && item.RoleId != 0).FirstOrDefault();
            //日龙修改
            //var user = context.UserMemberInfo.FirstOrDefault( a => a.UserName == model.UserName );
            //context.Managers.Remove( user );
            Context.Managers.Remove(model);
            Context.SaveChanges();
            string CACHE_MANAGER_KEY = CacheKeyCollection.Seller(id);
            Core.Cache.Remove(CACHE_MANAGER_KEY);
        }

        public void BatchDeleteSellerManager(long[] ids, long shopId)
        {
            var model = Context.Managers.FindBy(item => item.ShopId == shopId && item.RoleId != 0 && ids.Contains(item.Id));
            //    var username = model.Select( a => a.UserName ).ToList();
            //日龙修改
            //var user = context.UserMemberInfo.FindBy( item => username.Contains( item.UserName ) );
            //context.UserMemberInfo.Remove( user );
            Context.Managers.RemoveRange(model);
            Context.SaveChanges();
            foreach (var id in ids)
            {
                string CACHE_MANAGER_KEY = CacheKeyCollection.Seller(id);
                Core.Cache.Remove(CACHE_MANAGER_KEY);
            }
        }


        public IQueryable<Managers> GetManagers(string keyWords)
        {
            IQueryable<Managers> managers = Context.Managers.FindBy(item =>
                         (keyWords == null || keyWords == "" || item.UserName.Contains(keyWords)));
            return managers;
        }



        public Managers Login(string username, string password, bool isPlatFormManager = false)
        {
            var _iSiteSettingService = ObjectContainer.Current.Resolve<ISiteSettingService>();
            Managers manager;
            if (isPlatFormManager)
                manager = Context.Managers.FindBy(item => item.UserName == username && item.ShopId == 0).FirstOrDefault();
            else
                manager = Context.Managers.FindBy(item => item.UserName == username && item.ShopId != 0).FirstOrDefault();
            if (manager != null)
            {
#if !DEBUG
                string msg = "", host = System.Web.HttpContext.Current.Request.Url.Host;
                bool isOpenStore;
                bool isOpenShopApp;
                bool isOpenPC, isOpenH5, isOpenApp, isOpenMallSmallProg, isOpenMultiStoreSmallProg;
                //if (isPlatFormManager)
                //{
                //    if (!LicenseChecker.Check(out msg, out isOpenStore, out isOpenShopApp, out isOpenPC, out isOpenH5, out isOpenApp, out isOpenMallSmallProg, out isOpenMultiStoreSmallProg, host))
                //    {
                //        Log.Info("抛出异常代码323行");
                //        throw new MZcms.Core.MZcmsException(msg);
                //    }
                //    else
                //    {
                      isOpenStore = true;
                        isOpenShopApp = true;
                        isOpenPC = true;
                        isOpenH5 = true;
                        isOpenApp = true;
                        isOpenMallSmallProg = true;
                        isOpenMultiStoreSmallProg = true;
                        _iSiteSettingService.SaveSetting("IsOpenStore", isOpenStore);
                        _iSiteSettingService.SaveSetting("IsOpenShopApp", isOpenShopApp);
                        _iSiteSettingService.SaveSetting("IsOpenApp", isOpenApp);
                        _iSiteSettingService.SaveSetting("IsOpenH5", isOpenH5);
                        _iSiteSettingService.SaveSetting("IsOpenMallSmallProg", isOpenMallSmallProg);
                        _iSiteSettingService.SaveSetting("IsOpenMultiStoreSmallProg", isOpenMultiStoreSmallProg);
                        _iSiteSettingService.SaveSetting("IsOpenPC", isOpenPC);
                        Log.Info("IsOpenStore="+isOpenStore+ " IsOpenShopApp="+ isOpenShopApp + "  IsOpenApp="+ isOpenApp+ " IsOpenH5="+ isOpenH5+ " IsOpenMallSmallProg="+ isOpenMallSmallProg+ " IsOpenMultiStoreSmallProg="+ isOpenMultiStoreSmallProg+ " IsOpenPC="+ isOpenPC);
                        Core.Cache.Remove(CacheKeyCollection.SiteSettings);
                   // }
                //}
#endif
                string encryptedWithSaltPassword = GetPasswrodWithTwiceEncode(password, manager.PasswordSalt);
                if (encryptedWithSaltPassword.ToLower() != manager.Password)//比较密码是否一致
                    manager = null;//不一致，则置空，表示未找到指定的管理员
                else//一致，则表示登录成功，更新登录时间
                {
                    if (manager.ShopId > 0)//不处理平台
                    {
                        var shop = ServiceProvider.Instance<IShopService>.Create.GetShop(manager.ShopId);
                        if (shop == null)
                            throw new MZcmsException("未找到帐户对应的店铺");

                        if (!shop.IsSelf)//只处理非官方店铺
                        {
                            if (shop.ShopStatus == ShopInfo.ShopAuditStatus.Freeze)//冻结店铺
                            {
                                //throw new MZcmsException("帐户所在店铺已被冻结");
                            }
                        }
                    }
                }
            }
            return manager;
        }




        string GetPasswrodWithTwiceEncode(string password, string salt)
        {
            string encryptedPassword = Core.Helper.SecureHelper.MD5(password);//一次MD5加密
            string encryptedWithSaltPassword = Core.Helper.SecureHelper.MD5(encryptedPassword + salt);//一次结果加盐后二次加密
            return encryptedWithSaltPassword;
        }

        public Managers AddSellerManager(string username, string password, string salt = "")
        {
            var model = Context.Managers.FirstOrDefault(p => p.UserName == username && p.ShopId != 0);
            if (model != null)
            {
                return new Managers()
                {
                    Id = model.Id
                };
            }
            if (string.IsNullOrEmpty(salt))
            {
                salt = Core.Helper.SecureHelper.MD5(Guid.NewGuid().ToString("N"));
            }
            Managers manager;
            using (TransactionScope scope = new TransactionScope())
            {
                ShopInfo shopInfo = ServiceProvider.Instance<IShopService>.Create.CreateEmptyShop();
                manager = new Managers()
                {
                    CreateDate = DateTime.Now,
                    UserName = username,
                    Password = password,
                    PasswordSalt = salt,
                    ShopId = shopInfo.Id,
                    SellerPrivileges = new List<SellerPrivilege>() { (SellerPrivilege)0 },
                    AdminPrivileges = new List<AdminPrivilege>()
                    {
                    },
                    RoleId = 0,
                };
                Context.Managers.Add(manager);
                Context.SaveChanges();
                scope.Complete();
            }
            return manager;
        }

        public bool CheckUserNameExist(string username, bool isPlatFormManager = false)
        {
            if (isPlatFormManager)
            {
                return Context.Managers.Any(item => item.UserName.ToLower() == username.ToLower() && item.ShopId == 0);
            }
            var sellerManager = Context.Managers.Any(item => item.UserName.ToLower() == username.ToLower() && item.ShopId != 0);
            return Context.UserMemberInfo.Any(item => item.UserName.ToLower() == username.ToLower()) || sellerManager;
        }

        public Managers GetSellerManager(string userName)
        {
            var manager = Context.Managers.Where(item => item.UserName == userName && item.ShopId != 0).FirstOrDefault();
            return manager;
        }

        public void UpdateShopStatus()
        {
            List<ShopInfo> models = Context.ShopInfo.Where(s => s.EndDate < DateTime.Now).ToList();
            foreach (var m in models)
            {
                if (m.IsSelf)
                {
                    //TODO:DZY[150729] 官方自营店到期自动延期
                    /* zjt  
                     * TODO可移除，保留注释即可
                     */
                    m.EndDate = DateTime.Now.AddYears(10);
                    m.ShopStatus = ShopInfo.ShopAuditStatus.Open;
                }
                else
                {
                    m.ShopStatus = ShopInfo.ShopAuditStatus.Unusable;
                }
            }

            Context.SaveChanges();

        }
    }
}

