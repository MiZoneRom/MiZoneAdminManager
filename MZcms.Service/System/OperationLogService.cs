using MZcms.CommonModel;
using MZcms.DTO.QueryModel;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Linq;

namespace MZcms.Service
{
    public class OperationLogService : ServiceBase, IOperationLogService
    {
        public QueryPageModel<Model.Logs> GetPlatformOperationLogs(OperationLogQuery query)
        {
            int total = 0;
            IQueryable<Logs> logs = Context.Logs.FindAll();
            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                logs = logs.Where(item => query.UserName == item.UserName);
            }
            if (query.StartDate.HasValue)
            {
                logs = logs.Where(item => item.Date >= query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                var end = query.EndDate.Value.AddDays(1);
                logs = logs.Where(item => item.Date <= end);
            }
            logs = logs.GetPage(d => d.OrderByDescending(o => o.Id), out total, query.PageNo, query.PageSize);
            QueryPageModel<Logs> pageModel = new QueryPageModel<Logs>() { Models = logs.ToList(), Total = total };
            return pageModel;
        }

        public void AddPlatformOperationLog(Model.Logs model)
        {
            Context.Logs.Add(model);
            Context.SaveChanges();
        }


        public void DeletePlatformOperationLog(long id)
        {
            throw new NotImplementedException();
        }
    }
}
