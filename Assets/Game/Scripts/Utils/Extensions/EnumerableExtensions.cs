using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> pEnumerable, Action<T> pAction)
        {
            foreach (var item in pEnumerable)
                pAction(item);
        }
    }
}
