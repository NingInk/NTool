using System;

namespace NTool.Singleton
{
    [AttributeUsage(AttributeTargets.Class)] //这个特性只能标记在Class上
    public class MonoSingletonPathAttribute : Attribute
    {
        public MonoSingletonPathAttribute(string pathInHierarchy)
        {
            PathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy { get; private set; }
    }
}
