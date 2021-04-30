using System;

namespace AmzCrawler.App.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class GoogleSheetHeaderAttribute : Attribute
    {
        public virtual string HeaderName { get; }

        public GoogleSheetHeaderAttribute(string headerName)
        {
            HeaderName = headerName;
        }
    }
}
