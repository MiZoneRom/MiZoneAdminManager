using MZcms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MZcms.Web.Framework
{
	public static class AdminPermission
	{
		private readonly static Dictionary<AdminPrivilege, IEnumerable<ActionPermission>> privileges;

		private readonly static IEnumerable<ActionPermission> ActionPermissions;

		public static Dictionary<AdminPrivilege, IEnumerable<ActionPermission>> Privileges
		{
			get
			{
				return AdminPermission.privileges;
			}
		}

		static AdminPermission()
		{
			AdminPermission.ActionPermissions = AdminPermission.GetAllActionByAssembly();
			AdminPermission.privileges = new Dictionary<AdminPrivilege, IEnumerable<ActionPermission>>();
			IEnumerable<List<ActionItem>> privilege = 
				from a in PrivilegeHelper.GetPrivileges<AdminPrivilege>().Privilege
				select a.Items;
			foreach (List<ActionItem> actionItems in privilege)
			{
				foreach (ActionItem actionItem in actionItems)
				{
					List<ActionPermission> actionPermissions = new List<ActionPermission>();
					foreach (Controllers controller in actionItem.Controllers)
					{
						foreach (string actionName in controller.ActionNames)
						{
							actionPermissions.AddRange(AdminPermission.GetActionByControllerName(controller.ControllerName, actionName));
						}
					}
					AdminPermission.privileges.Add((AdminPrivilege)actionItem.PrivilegeId, actionPermissions);
				}
			}
		}

		public static bool CheckPermissions(List<AdminPrivilege> userprivileages, string controllerName, string actionName)
		{
			if (userprivileages.Contains(0))
			{
				return true;
			}
			return (
				from a in AdminPermission.privileges
				where userprivileages.Contains(a.Key)
				select a).Any<KeyValuePair<AdminPrivilege, IEnumerable<ActionPermission>>>((KeyValuePair<AdminPrivilege, IEnumerable<ActionPermission>> b) => b.Value.Any((ActionPermission c) => {
				if (c.ControllerName.ToLower() != controllerName.ToLower())
				{
					return false;
				}
				return c.ActionName.ToLower() == actionName.ToLower();
			}));
		}

		private static IEnumerable<ActionPermission> GetActionByControllerName(string controllername, string actionname = "")
		{
			return AdminPermission.ActionPermissions.Where((ActionPermission item) => {
				if (item.ControllerName.ToLower() != controllername.ToLower())
				{
					return false;
				}
				if (actionname == "")
				{
					return true;
				}
				return item.ActionName.ToLower() == actionname.ToLower();
			});
		}

		private static IList<ActionPermission> GetAllActionByAssembly()
		{
			List<ActionPermission> actionPermissions = new List<ActionPermission>();
			IEnumerable<Type> types = ((IEnumerable<Type>)Assembly.Load("Himall.Web").GetTypes()).Where((Type a) => {
				if (a.BaseType == null)
				{
					return false;
				}
				return a.BaseType.Name == "BaseAdminController";
			});
			foreach (Type type in types)
			{
				MethodInfo[] methods = type.GetMethods();
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo methodInfo = methods[i];
					if (methodInfo.ReturnType.Name == "ActionResult" || methodInfo.ReturnType.Name == "JsonResult")
					{
						ActionPermission actionPermission = new ActionPermission()
						{
							ActionName = methodInfo.Name,
							ControllerName = methodInfo.DeclaringType.Name.Substring(0, methodInfo.DeclaringType.Name.Length - 10)
						};
						object[] customAttributes = methodInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
						if (customAttributes.Length > 0)
						{
							actionPermission.Description = (customAttributes[0] as DescriptionAttribute).Description;
						}
						actionPermissions.Add(actionPermission);
					}
				}
			}
			return actionPermissions;
		}
	}
}