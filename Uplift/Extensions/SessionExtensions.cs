using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Uplift.Extensions
{
    //This is a Extension class that uses the ISession of the AspNetCore.Http Library, not a heritage extension.
    //Please check: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
    //The AspNetCore.Http.ISession allow only storage an int or a string.
    //This extension will allow to store an object in a Json format as a string.
    public static class SessionExtensions
    {
        //The first parameter allow us to use the Session of AspNetCore.Http.
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
