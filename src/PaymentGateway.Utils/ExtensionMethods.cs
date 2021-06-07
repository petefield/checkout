using System;

namespace PaymentGateway.Utils
{
    public static class ExtensionMethods
    {
        public static T ArgumentNullCheck<T>(this T value, string name) {
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
