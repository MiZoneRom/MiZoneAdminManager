using System;
namespace MZcms.Core
{
    public class Instance
    {
        public static T Get<T>(string classFullName)
        {
            try
            {
                Type sourceType = Type.GetType(classFullName);
               return (T) Activator.CreateInstance(sourceType);
            }
            catch (Exception ex)
            {
                throw new InstanceCreateException("创建实例异常", ex);
            }
        }

    }
}
