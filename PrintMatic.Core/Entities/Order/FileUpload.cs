using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class FileUpload
	{
		public string ContentType { get; set; }
		public string ContentDisposition { get; set; }
		public Dictionary<string, string[]> Headers { get; set; }
		public long Length { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public byte[] FileContent { get; set; }  // Store the file content as byte[]
	}
}
