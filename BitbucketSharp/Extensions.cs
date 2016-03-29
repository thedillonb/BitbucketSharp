using System;
using System.Threading.Tasks;
using BitbucketSharp.Models.V2;
using System.Collections;
using System.Collections.Generic;

namespace BitbucketSharp
{
    public static class Extensions
    {
        internal static Uri With(this Uri uri, string part)
        {
            if (part == null)
                return uri;
            
            return new Uri(uri.AbsoluteUri.TrimEnd('/') + "/" + part);
        }

        internal static Uri WithQuery(this Uri uri, string data)
        {
            if (data == null)
                return uri;
            
            return new Uri(uri.AbsoluteUri.TrimEnd('/') + "/" + Uri.EscapeDataString(data));
        }

        public static async Task<IEnumerable<T>> AllItems<T>(this Client client, Func<Client, Task<Collection<T>>> operation)
        {
            var ret = await operation(client);
            var items = new List<T>(ret.Values);
            var next = ret.Next;

            while (!string.IsNullOrEmpty(next))
            {
                var t = await client.Get<Collection<T>>(new Uri(next));
                items.AddRange(t.Values);
                next = t.Next;
            }

            return items;
        }

        public static async Task ForAllItems<T>(this Client client, Func<Client, Task<Collection<T>>> operation, Action<IEnumerable<T>> addAction)
        {
            var ret = await operation(client);
            addAction(ret.Values);
            var next = ret.Next;

            while (!string.IsNullOrEmpty(next))
            {
                var t = await client.Get<Collection<T>>(new Uri(next));
                addAction(t.Values);
                next = t.Next;
            }
        }
    }
}

