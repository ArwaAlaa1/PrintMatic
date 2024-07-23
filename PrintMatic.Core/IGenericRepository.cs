using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
	}
}
