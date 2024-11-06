using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrintMatic.Helper
{
    public class CustomeUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;

        public CustomeUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _imagePath = _webHostEnvironment.WebRootPath;
        }

        public async Task<List<string>?> Upload(List<IFormFile> files)
        {
            List<string> photos = new List<string>();
            string uploadDir = Path.Combine(_imagePath, "Custome", "Image");

            try
            {
                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    // Generate unique file name
                    var photoName = $"{Guid.NewGuid()}{Path.GetExtension(files[i].FileName)}";
                    var path = Path.Combine(uploadDir, photoName);

                    // Save the file
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await files[i].CopyToAsync(fileStream);
                    }

                    // Add photo name to the list
                    photos.Add(photoName);
                }

                return photos;
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"File upload failed: {ex.Message}");
                // Return null or empty list in case of failure
                return null;
            }
        }


		public async Task<string> Delete(List<string> files)
		{
			
            try
            {

                for (int i = 0; i < files.Count; i++)
                {
                    var existingPhotoPath = Path.Combine(_imagePath, "Custome", "Image", files[i]);
                    if (System.IO.File.Exists(existingPhotoPath))
                    {
                        try
                        {
                            System.IO.File.Delete(existingPhotoPath);
                        }
						catch (Exception ex)
                        {
							return $"An unexpected error occurred: {ex.Message}";
						}
                    }
                


                }
				return $"Deleted images done";
			}
            catch(Exception ex)
            {
				return $"An unexpected error occurred: {ex.Message}";
			}
		}

        public async Task<string?> UploadFile(IFormFile file)
        {
            string filepdf =" ";
            string uploadDir = Path.Combine(_imagePath, "Custome", "Pdfs");

            try
            {

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }


                var FileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(uploadDir, FileName);


                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Add photo name to the list
                filepdf=FileName;


                return filepdf;
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"File upload failed: {ex.Message}");
                // Return null or empty list in case of failure
                return null;
            }
}


        public async Task<string> DeleteFile(string file)
        {

            try
            { var existingPhotoPath = Path.Combine(_imagePath, "Custome", "Pdfs", file);
                    if (System.IO.File.Exists(existingPhotoPath))
                    {
                        try
                        {
                            System.IO.File.Delete(existingPhotoPath);
                        }
                        catch (Exception ex)
                        {
                            return $"An unexpected error occurred: {ex.Message}";
                        }
                    }

                return $"Deleted images done";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
    }
}
