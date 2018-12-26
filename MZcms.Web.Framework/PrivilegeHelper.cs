using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MZcms.Web.Framework
{
	public class PrivilegeHelper
	{
		private static Privileges adminPrivileges;

		private static Privileges sellerAdminPrivileges;

		private static Privileges userPrivileges;

		public static Privileges AdminPrivileges
		{
			get
			{
				if (PrivilegeHelper.adminPrivileges == null)
				{
					PrivilegeHelper.adminPrivileges = PrivilegeHelper.GetPrivileges<AdminPrivilege>();
				}
				return PrivilegeHelper.adminPrivileges;
			}
			set
			{
				PrivilegeHelper.adminPrivileges = value;
			}
		}

		public static Privileges SellerAdminPrivileges
		{
			get
			{
				if (PrivilegeHelper.sellerAdminPrivileges == null)
				{
					PrivilegeHelper.sellerAdminPrivileges = PrivilegeHelper.GetPrivileges<SellerPrivilege>();
				}
				return PrivilegeHelper.sellerAdminPrivileges;
			}
			set
			{
				PrivilegeHelper.sellerAdminPrivileges = value;
			}
		}

		public static Privileges UserPrivileges
		{
			get
			{
				if (PrivilegeHelper.userPrivileges == null)
				{
					PrivilegeHelper.userPrivileges = PrivilegeHelper.GetPrivileges<UserPrivilege>();
				}
				return PrivilegeHelper.userPrivileges;
			}
			set
			{
				PrivilegeHelper.userPrivileges = value;
			}
		}

		public PrivilegeHelper()
		{
		}

		public static Privileges GetPrivileges<TEnum>()
		{
			FieldInfo[] fields = typeof(TEnum).GetFields();
			if (fields.Length == 1)
			{
				return null;
			}
			Privileges privilege = new Privileges();
			FieldInfo[] fieldInfoArray = fields;
			for (int i = 0; i < fieldInfoArray.Length; i++)
			{
				object[] customAttributes = fieldInfoArray[i].GetCustomAttributes(typeof(PrivilegeAttribute), true);
				if (customAttributes.Length != 0)
				{
					GroupActionItem groupActionItem = new GroupActionItem();
					ActionItem actionItem = new ActionItem();
					List<string> strs = new List<string>();
					List<PrivilegeAttribute> privilegeAttributes = new List<PrivilegeAttribute>();
					List<Controllers> controllers = new List<Controllers>();
					object[] objArray = customAttributes;
					for (int j = 0; j < objArray.Length; j++)
					{
						object obj = objArray[j];
						Controllers controller = new Controllers();
						PrivilegeAttribute privilegeAttribute = obj as PrivilegeAttribute;
						controller.ControllerName = privilegeAttribute.Controller;
						List<string> actionNames = controller.ActionNames;
						string action = privilegeAttribute.Action;
						char[] chrArray = new char[] { ',' };
						actionNames.AddRange(action.Split(chrArray));
						controllers.Add(controller);
						privilegeAttributes.Add(privilegeAttribute);
					}
					PrivilegeAttribute privilegeAttribute1 = privilegeAttributes.FirstOrDefault((PrivilegeAttribute a) => !string.IsNullOrEmpty(a.GroupName));
					groupActionItem.GroupName = privilegeAttribute1.GroupName;
					actionItem.PrivilegeId = privilegeAttribute1.Pid;
					actionItem.Name = privilegeAttribute1.Name;
					actionItem.Url = privilegeAttribute1.Url;
					actionItem.Controllers.AddRange(controllers);
					GroupActionItem groupActionItem1 = privilege.Privilege.FirstOrDefault((GroupActionItem a) => a.GroupName == groupActionItem.GroupName);
					if (groupActionItem1 != null)
					{
						groupActionItem1.Items.Add(actionItem);
					}
					else
					{
						groupActionItem.Items.Add(actionItem);
						privilege.Privilege.Add(groupActionItem);
					}
				}
			}
			return privilege;
		}
	}
}