using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spending.Enumerables;
namespace Spending.Helpers
{
    public static class EnumHelper
    {
        public static Array Categories => Enum.GetValues(typeof(Category));
    }
}
