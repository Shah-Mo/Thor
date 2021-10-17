using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thor.Helpers
{
    public static class Constants
    {
        public static class Messages
        {
            public const string Success = "Success";
            public const string UnauthorizedPassword = "Unauthorized: Bad password.";
            public const string UnauthorizedLogin = "Unauthorized: Must login first.";

            public const string CModelError = "Value for {0} must be between {1} and {2}.";
        }
    }
}
