using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
  
        public class SerializableTuple<T1, T2, T3,T4>
        {
            public T1 Item1 { get; set; }
            public T2 Item2 { get; set; }
            public T3 Item3 { get; set; }
            public T4 Item4 { get; set; }


            public static implicit operator Tuple<T1, T2, T3, T4>(SerializableTuple<T1, T2, T3,T4> st)
            {
                return Tuple.Create(st.Item1, st.Item2, st.Item3, st.Item4);
            }

            public static implicit operator SerializableTuple<T1, T2, T3,T4>(Tuple<T1, T2, T3, T4> t)
            {
                return new SerializableTuple<T1, T2, T3,T4>()
                {
                    Item1 = t.Item1,
                    Item2 = t.Item2,
                    Item3 = t.Item3,
                    Item4 = t.Item4
                };
            }

            public SerializableTuple()
            {
            }
        }
    
}
