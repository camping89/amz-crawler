using AmzCrawler.App.Services.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmzCrawler.App.Services.Helpers
{
    public static class GoogleSheetHelper
    {
        public static string GetCellValue<TDto>(string propName, IList<string> headers, IList<object> rowValues)
        {
            var propertyInfo = typeof(TDto).GetProperties().FirstOrDefault(p => p.Name == propName);
            if (propertyInfo == null) return string.Empty;

            var att = propertyInfo.GetCustomAttributes(typeof(GoogleSheetHeaderAttribute), true).FirstOrDefault();
            headers = headers.Select(x => Regex.Replace(Regex.Replace(x, "\n", " ").ToLower().Trim(), @" [ ]+", " ")).ToList();

            if (att is GoogleSheetHeaderAttribute headerAtt)
            {
                var attHeaderName = headerAtt.HeaderName.ToLower().Trim();
                var index = headers.IndexOf(attHeaderName);

                return index >= 0 && index < rowValues.Count ? rowValues[index].ToString() : string.Empty;
            }

            return string.Empty;
        }

        public static object GetPropValue<T>(T src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static string GetColumnLetter<TDto>(List<string> headers, string propName)
        {
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            var propertyInfo = typeof(TDto).GetProperties().FirstOrDefault(p => p.Name == propName);
            if (propertyInfo == null) return string.Empty;

            var att = propertyInfo.GetCustomAttributes(typeof(GoogleSheetHeaderAttribute), true).FirstOrDefault();
            headers = headers.Select(x => x.Replace("\n", " ").ToLower().Trim()).ToList();
            if (att is GoogleSheetHeaderAttribute headerAtt)
            {
                var attHeaderName = headerAtt.HeaderName.ToLower().Trim();
                var index = headers.IndexOf(attHeaderName);

                return index >= 0 ? alpha[index].ToString() : string.Empty;
            }

            return string.Empty;
        }
    }
}
