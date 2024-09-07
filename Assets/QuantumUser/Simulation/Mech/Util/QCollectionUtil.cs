using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quantum.Collections;

namespace Quantum
{
    static unsafe class QCollectionUtil
    {
        public static void Resolve<K,V>(this QDictionaryPtr<K,V> d_ptr, Frame f, out QDictionary<K, V> dict)
            where K : unmanaged, IEquatable<K>
            where V : unmanaged
        {
            if (!f.TryResolveDictionary(d_ptr, out dict)) Log.Error("Unable to resolve dictionary. Ensure that it has been allocated.");
        }

        public static void Resolve<V>(this QListPtr<V> d_ptr, Frame f, out QList<V> list)
            where V : unmanaged
        {
            if (!f.TryResolveList(d_ptr, out list)) Log.Error("Unable to resolve list. Ensure that it has been allocated.");
        }
    }
}