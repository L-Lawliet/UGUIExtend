using System.Collections;
using System.Collections.Generic;

/// <summary>
///
/// name:ListPool
/// author:Administrator
/// date:2017/2/21 16:55:18
/// versions:
/// introduce:
/// note:
/// 
/// </summary>
namespace Waiting.UGUI.Collections
{
    internal static class ListPool<T>
    {
        private static readonly Pool<List<T>> _listPool = new Pool<List<T>>(null, l => l.Clear());

        public static List<T> Get()
        {
            return _listPool.Get();
        }

        public static void Recycle(List<T> element)
        {
            _listPool.Recycle(element);
        }
    }
}
