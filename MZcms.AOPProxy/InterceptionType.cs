using System;

namespace MZcms.AOPProxy
{
	public enum InterceptionType
	{
		OnEntry,
		OnExit,
		OnSuccess,
		OnException,
		OnLogException
	}
}