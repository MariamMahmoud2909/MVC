using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MVC_3PL.Helpers
{
	public static class DocumentSettings
	{
		public static async Task<string> UploadFile(IFormFile file, string folderName)
		{
			//1. Get Located Folder Path Dynamically
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName);

			if(!Directory.Exists(folderPath) )
				Directory.CreateDirectory(folderPath);

			//2. Get FileName and make it unique

			string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

			//3. Get FilePath [folderpath + filename]

			string filePath = Path.Combine(folderPath, fileName);

			//4. Save File as streams [Data Per Time]

			using var fileStream = new FileStream(filePath, FileMode.Create);
			await file.CopyToAsync(fileStream);
			//Dealing with files and streams is unmanaged by CLR (unmanaged resource) and therefore we used Using/TryFinally
			
			//5. Return FileName
			return fileName;//Stored in Db as folder path will always be repeated

		}

		public static void DeleteFile(string fileName, string folderName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
			
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
