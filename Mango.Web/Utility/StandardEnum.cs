namespace Mango.Web.Utility
{
    public class StandardUtility
    {
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
            PATCH
        }
    }
}
