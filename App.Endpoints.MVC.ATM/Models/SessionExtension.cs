using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace App.Endpoints.MVC.ATM.Models;

public static class SessionExtension
{
    public static void SetObject<T>(this ISession session,string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
    public static T? GetObject<T>(this ISession session,string key)
    {
        var txt = session.GetString(key);
        if (string.IsNullOrEmpty(txt))
            return default;
        return JsonConvert.DeserializeObject<T>(txt);
    }
}
