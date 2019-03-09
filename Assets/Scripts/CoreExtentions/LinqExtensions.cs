using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class LinqExtensions
{
    /// <summary>
    /// IEnumerable 型のインスタンスからランダムに値を取得します
    /// </summary>
    public static T RandomAt<T>(this IEnumerable<T> self)
    {
        if (self.Any() == false) return default(T);
        return self.ElementAt(Random.Range(0, self.Count()));
    }

    /// <summary>
    /// Dictionary 型のインスタンスからランダムに値を取得します
    /// </summary>
    public static KeyValuePair<TKey, TValue> RandomAt<TKey, TValue>( 
        this Dictionary<TKey, TValue> self
    ) {
        if (self.Any() == false) return default(KeyValuePair<TKey, TValue>);
        return self.ElementAt(Random.Range(0, self.Count));
    }

    /// <summary>
    /// List 型のインスタンスから最後の一つを取り出します
    /// </summary>
    public static T Last<T>( 
        this List<T> self
    ) {
        if (self.Any() == false) return default(T);
        return self[self.Count - 1];
    }
}
