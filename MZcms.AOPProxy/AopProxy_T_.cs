using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace MZcms.AOPProxy
{
	public class AopProxy<T> : RealProxy
	{
		private T _realObject;

		public AopProxy(T realObject) : base(typeof(T))
		{
            _realObject = realObject;
		}

		private IMessage GetReturnMessage(object returnValue, IMethodCallMessage callMessage)
		{
			IMessage returnMessage = new ReturnMessage(returnValue, callMessage.Args, callMessage.ArgCount - callMessage.InArgCount, callMessage.LogicalCallContext, callMessage);
			return returnMessage;
		}

		public override IMessage Invoke(IMessage msg)
		{
			IMessage returnMessage;
			IMessage message;
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			MethodParameters methodParameter = new MethodParameters(methodCallMessage.Args);
			object handle = DelegateContainer.GetHandle(typeof(T).FullName, methodCallMessage.MethodName, InterceptionType.OnEntry);
			if (handle != null)
			{
				((OnEntry)handle)(methodParameter);
			}
			if (methodParameter.MethodFlow != FlowBehavior.Return)
			{
				try
				{
					try
					{
						object obj = methodCallMessage.MethodBase.Invoke(_realObject, methodParameter.Argugemts);
						object obj1 = obj;
						methodParameter.ReturnValue = obj;
						object handle1 = DelegateContainer.GetHandle(typeof(T).FullName, methodCallMessage.MethodName, InterceptionType.OnSuccess);
						if (handle1 != null)
						{
							((OnSuccess)handle1)(methodParameter);
						}
						message = GetReturnMessage(methodParameter.ReturnValue, methodCallMessage);
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						methodParameter.Exception = exception;
						methodParameter.MethodFlow = FlowBehavior.ThrowException;
						object handle2 = DelegateContainer.GetHandle(typeof(T).FullName, methodCallMessage.MethodName, InterceptionType.OnException);
						if (handle2 != null)
						{
							((OnException)handle2)(methodParameter);
						}
						object obj2 = DelegateContainer.GetHandle(typeof(T).GetInterfaces()[0].FullName, "LogException", InterceptionType.OnLogException);
						if (obj2 != null)
						{
							Dictionary<string, object> strs = new Dictionary<string, object>();
							ParameterInfo[] parameters = ((MethodInfo)methodCallMessage.MethodBase).GetParameters();
							for (int i = 0; i < methodCallMessage.ArgCount; i++)
							{
								string name = parameters[i].Name;
								strs.Add(name, methodCallMessage.Args[i]);
							}
							((OnLogException)obj2)(methodCallMessage.MethodName, strs, (exception.InnerException != null ? exception.InnerException : exception));
						}
						switch (methodParameter.MethodFlow)
						{
							case FlowBehavior.Continue:
							case FlowBehavior.Return:
							{
								returnMessage = GetReturnMessage(methodParameter.ReturnValue, methodCallMessage);
								break;
							}
							case FlowBehavior.ThrowException:
							{
								returnMessage = new ReturnMessage(methodParameter.Exception, methodCallMessage);
								break;
							}
							default:
							{
								goto case FlowBehavior.Return;
							}
						}
						message = returnMessage;
					}
				}
				finally
				{
					object handle3 = DelegateContainer.GetHandle(typeof(T).FullName, methodCallMessage.MethodName, InterceptionType.OnExit);
					if (handle3 != null)
					{
						((OnExit)handle3)(methodParameter);
					}
				}
			}
			else if (methodParameter.MethodFlow != FlowBehavior.Return)
			{
				message = new ReturnMessage(methodParameter.Exception, methodCallMessage);
			}
			else
			{
				message = GetReturnMessage(methodParameter.ReturnValue, methodCallMessage);
			}
			return message;
		}
	}
}