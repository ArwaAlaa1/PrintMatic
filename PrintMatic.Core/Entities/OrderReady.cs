using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public enum OrderReady
    {
        [EnumMember(Value = "لم يتم التجهيز")]
        NotReady,
        [EnumMember(Value = "تم التجهيز")]
        Ready

    }
}
