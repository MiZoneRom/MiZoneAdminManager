using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
	public abstract class AreaRegistrationOrder : AreaRegistration
	{
		protected static List<AreaRegistrationContext> areaContent;

		protected static List<AreaRegistrationOrder> areaRegistration;

		public abstract int Order
		{
			get;
		}

		static AreaRegistrationOrder()
		{
			AreaRegistrationOrder.areaContent = new List<AreaRegistrationContext>();
			AreaRegistrationOrder.areaRegistration = new List<AreaRegistrationOrder>();
		}

		protected AreaRegistrationOrder()
		{
		}

		private static void Register()
		{
			List<int[]> numArrays = new List<int[]>();
			for (int i = 0; i < AreaRegistrationOrder.areaRegistration.Count; i++)
			{
				int[] order = new int[] { AreaRegistrationOrder.areaRegistration[i].Order, i };
				numArrays.Add(order);
			}
			numArrays = (
				from o in numArrays
				orderby o[0]
				select o).ToList<int[]>();
			foreach (int[] numArray in numArrays)
			{
				AreaRegistrationOrder.areaRegistration[numArray[1]].RegisterAreaOrder(AreaRegistrationOrder.areaContent[numArray[1]]);
			}
		}

		public static void RegisterAllAreasOrder()
		{
			AreaRegistration.RegisterAllAreas();
			AreaRegistrationOrder.Register();
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			AreaRegistrationOrder.areaContent.Add(context);
			AreaRegistrationOrder.areaRegistration.Add(this);
		}

		public abstract void RegisterAreaOrder(AreaRegistrationContext context);
	}
}