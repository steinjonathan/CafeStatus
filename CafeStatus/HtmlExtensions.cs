using System;
using System.Web.Mvc;

namespace CafeStatus
{
    public static class HtmlExtensions
    {
        private const string DEFAULT_FORMAT = null;

        public static MvcHtmlString ToDateTimeLocal(this HtmlHelper helper, DateTime? utctime, string datetimeformat = DEFAULT_FORMAT)
        {
            if (utctime.HasValue)
            {
                var fallbackValue = utctime.Value.ToString("dd/MM/yyyy, hh:mm:ss zzz");
                if (string.IsNullOrEmpty(datetimeformat))
                {
                    return new MvcHtmlString(string.Format("<datetimelocal date='{0}'>{1}</datetimelocal>", utctime.Value.ToString("s") + "Z", fallbackValue));
                }
                return new MvcHtmlString(string.Format("<datetimelocal date='{0}' format='{1}'>{2}</datetimelocal>", utctime.Value.ToString("s") + "Z", datetimeformat, fallbackValue));
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString ToDateTimeLocal<TModel>(this HtmlHelper<TModel> helper, DateTime? utctime, string datetimeformat = DEFAULT_FORMAT)
        {
            return HtmlExtensions.ToDateTimeLocal((HtmlHelper)helper, utctime, datetimeformat);
        }
    }
}