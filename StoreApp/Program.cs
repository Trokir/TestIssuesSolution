using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoreApp
{
    public interface IStore<T>
    {
        /// <summary>
        /// Adds T to the store and returns its Id
        /// </summary>
        int Add(T obj);

        /// <summary>
        /// Remove T by Id and returns T
        /// </summary>
        T Remove(int id);

        /// <summary>
        /// Returns Ts that are valid
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> ValidItems();
    }


    public abstract class Store<T> : IStore<T>
    {

        protected abstract bool IsValid(T x);
        private readonly Dictionary<int, T> _store;

        private ConcurrentDictionary<int, T> _storeDict = new ConcurrentDictionary<int, T>();
        /// <summary>
        /// Gets the store dictionary.
        /// </summary>
        /// <value>
        /// The store dictionary.
        /// </value>
        protected ConcurrentDictionary<int, T> StoreDict
        {
            get => _storeDict;
            private set
            {
                _storeDict = _store.GetConvertedDictionary();
                _storeDict = value;
            }
        }

        /// <summary>
        /// Adds T to the store and returns its Id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Add(T obj)
        {
            Debug.WriteLine(obj.GetType());
            if (StoreDict == null) return -1000;
            var newKey = StoreDict.GetUniqueKey<T>();
            StoreDict.AddOrUpdate(newKey, obj, (key, value) => value);
            return newKey;
        }
        /// <summary>
        /// Remove T by Id and returns T
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">StoreDict</exception>
        public T Remove(int id)
        {
            if (StoreDict == null) throw new ArgumentNullException(nameof(StoreDict));
            if (!StoreDict.TryGetValue(id, out var value)) return default;
            return StoreDict.TryRemove(id, out value) ? value : default;
        }

        /// <summary>
        /// Returns Ts that are valid
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ValidItems()
        {

            if (StoreDict == null) yield break;
            for (var i = 0; i < StoreDict.Count; i++)
            {
                if (!StoreDict.TryGetValue(StoreDict.ElementAtOrDefault(i).Key, out var value)) continue;
                if (IsValid(StoreDict.ElementAt(i).Value))
                    yield return value;
            }
        }


    }

    public class StoreInt : Store<int>
    {
        protected override bool IsValid(int x) => x > 0;
    }

    public class StoreString : Store<string>
    {
        protected override bool IsValid(string x) => x.StartsWith("a");
    }


    class Program
    {
        static void Main(string[] args)
        {
            StoreInt store1 = new StoreInt();
            StoreString store2 = new StoreString();

            List<int> ids1 = new List<int>();
            List<int> ids2 = new List<int>();

            void Show<T>(string title, IEnumerable<T> obs)
            {
                Console.Write($"{title}: ");
                foreach (var x in obs) Console.Write($"{x} ");
                Console.WriteLine();
            }

            Barrier b = new Barrier(3, _ => Show("Ids: ", ids1.Concat(ids2)));

            var t1 = Task.Run(FillStore1);
            var t2 = Task.Run(FillStore2);

            b.SignalAndWait();

            Show("Valid ints: ", store1.ValidItems());
            Show("Valid strings: ", store2.ValidItems());

            Console.ReadLine();

            void FillStore1()
            {
                ids1.Add(store1.Add(1));
                ids1.Add(store1.Add(-2));
                ids1.Add(store1.Add(-5));
                b.SignalAndWait();
            }

            void FillStore2()
            {
                ids2.Add(store2.Add("a1"));
                ids2.Add(store2.Add("a2"));
                ids2.Add(store2.Add("b3"));
                b.SignalAndWait();
            }
        }
    }
}
