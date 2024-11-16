namespace PrintMartic_DashBoard.Helper
{
	public class DocumentSetting
	{
        public static string UploadFile(IFormFile file, string foldername)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", foldername);

            string filename = file.FileName;

            string filePath = Path.Combine(folderPath, filename);

            using var filestream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(filestream);
            return filename;
        }
        public static string UploadFile(IFormFile file, string foldername , string webRootPath)
		{
            string folderPath = Path.Combine(webRootPath, "Uploads", foldername);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

           

			string filename = file.FileName;

			string filePath = Path.Combine(folderPath, filename);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                 file.CopyToAsync(fileStream);
            }

            return filename;
		}

		public static void DeleteFile(string foldername, string filename)
		{
			string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", foldername, filename);
			if (File.Exists(filepath))
				File.Delete(filepath);
		}
	}
}
