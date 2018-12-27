using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MZcms.AOPProxy
{
    public static class InterceptionInit
    {
        private static Type GetDelegateType(InterceptionType interceptionType)
        {
            Type type = null;
            switch (interceptionType)
            {
                case InterceptionType.OnEntry:
                    {
                        type = typeof(OnEntry);
                        break;
                    }
                case InterceptionType.OnExit:
                    {
                        type = typeof(OnExit);
                        break;
                    }
                case InterceptionType.OnSuccess:
                    {
                        type = typeof(OnSuccess);
                        break;
                    }
                case InterceptionType.OnException:
                    {
                        type = typeof(OnException);
                        break;
                    }
                case InterceptionType.OnLogException:
                    {
                        type = typeof(OnLogException);
                        break;
                    }
            }
            return type;
        }

        public static void Init()
        {
            IEnumerable<Assembly> assemblies =
                from item in AppDomain.CurrentDomain.GetAssemblies()
                where (item.FullName.StartsWith("System.") ? false : !item.FullName.StartsWith("Microsoft."))
                select item;
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types1 =
                    from item in assembly.GetTypes()
                    where IsSubClassOf(item, "IAOPInterception")
                    select item;
                types.AddRange(types1);
            }
            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i < methods.Length; i++)
                {
                    MethodInfo methodInfo = methods[i];
                    Attribute customAttribute = methodInfo.GetCustomAttribute(typeof(Interception));
                    if (customAttribute != null)
                    {
                        Interception interception = (Interception)customAttribute;
                        Type delegateType = GetDelegateType(interception.Type);
                        Delegate @delegate = Delegate.CreateDelegate(delegateType, methodInfo);
                        DelegateContainer.AddHandle(interception.TargetType.FullName, interception.TargetMethodName, interception.Type, @delegate);
                    }
                }
            }
        }

        private static bool IsSubClassOf(Type type, string interfaceName)
        {
            return type.GetInterface(interfaceName) != null;
        }
    }
}