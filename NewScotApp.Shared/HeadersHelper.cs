namespace NewScotApp.Shared
{
    internal class HeadersHelper
    {
        //public static bool GetLanguageHeader(HttpRequest request)
        //{
        //    try
        //    {
        //        var lang = request.Headers[Strings.LangHeader];
        //        return lang.FirstOrDefault() != null ? lang.FirstOrDefault().ToLower() == Strings.DefaultLang : false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //internal static PageIntent? GetPageIntentHeader(HttpRequest request)
        //{
        //    try
        //    {

        //        var intentheader = request.Headers[Strings.PageIntentHeader];
        //        if (intentheader.Count == 0)
        //            return null;
        //        var intent = (PageIntent)Enum.Parse(typeof(PageIntent), intentheader.FirstOrDefault());
        //        return intent;

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //internal static DonorStepCode? GetStepCodeHeader(HttpRequest request)
        //{
        //    try
        //    {

        //        var stepCodeheader = request.Headers[Strings.DonorStepCodeHeader];
        //        if (stepCodeheader.Count == 0)
        //            return null;
        //        var intent = (DonorStepCode)Enum.Parse(typeof(DonorStepCode), stepCodeheader.FirstOrDefault());
        //        return intent;

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        //internal static void ReadCustomHeader(out string customHeader, HttpContext context)
        //{
        //    customHeader = string.Empty;
        //    if (context.Request.Headers.TryGetValue("Lang", out var traceValue))
        //    {
        //        customHeader = traceValue;
        //    }
        //}

        //public static string GetAuthToken(HttpRequest request)
        //{
        //    try
        //    {
        //        //I removed keyword bearer to connect with signalR hubConnectionBuilder
        //        request.Headers.TryGetValue("Authorization", out var token);
        //        return token.FirstOrDefault().Replace("Bearer", "").Replace("bearer", "");
        //        //return token;
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}

    }
}
