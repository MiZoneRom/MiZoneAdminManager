using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace MZcms.Web.Framework
{
	[SessionState(SessionStateBehavior.ReadOnly)]
	public class BaseAsyncController : AsyncController
	{
		public BaseAsyncController()
		{
		}
	}
}