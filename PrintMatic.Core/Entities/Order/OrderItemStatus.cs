using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public enum OrderItemStatus
	{
		[EnumMember(Value = "قيد التنفيذ")]
		Pending,
		[EnumMember(Value = "تم التجهيز")]
		Ready
	}
}
