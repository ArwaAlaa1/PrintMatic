using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core
{
	public interface IUnitOfWork<T> : IDisposable where T : BaseEntity
	{
		public IGenericRepository<T> generic { get; set; }

		int Complet();
	}
}
