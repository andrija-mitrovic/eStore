using System.Runtime.CompilerServices;

namespace Application.Common.Helpers
{
    public class HelperFunction
    {
        public static string GetMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}
