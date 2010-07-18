using System.Security.Principal;

namespace TaskTimer.Helpers
{
    public class User
    {
        internal static string Current
        {
            get
            {
                var user = WindowsIdentity.GetCurrent();
                return null == user ? "Unknown" : user.Name;
            }
        }
    }
}