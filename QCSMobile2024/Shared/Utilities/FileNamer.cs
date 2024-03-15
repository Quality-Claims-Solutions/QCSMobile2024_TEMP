using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCSMobile2024.Shared.Utilities
{
    public class FileNamer
    {
        /// <summary>
         /// technically this introduces a race condition, but a collision would only happen if two threads
         /// would generate the same Path.GetRandomFileName() at the same time. Pretty unlikely. 
         /// </summary>
         /// <param name="server"></param>
         /// <param name="rootFolder"></param>
         /// <param name="fileName"></param>
         /// <returns></returns> 
        public static string GetUniqueFileName(string rootFolder, string fileName, string claimNumber = "")
        {
            var now = DateTime.Now;
            var dateFolder = $"{now.Year}\\{now.Month}\\{now.Day}";
            if (Directory.Exists(Path.Combine(rootFolder, dateFolder)) == false)
                Directory.CreateDirectory(Path.Combine(rootFolder, dateFolder));

            fileName = RemoveIllegalPathChars(fileName);

            var noExt = Path.GetFileNameWithoutExtension(fileName);
            if (!string.IsNullOrEmpty(claimNumber) && fileName.ToLower().Contains(claimNumber.ToLower()) == false)
                noExt = claimNumber.Trim() + "_" + noExt.TrimStart('_');

            var ext = Path.GetExtension(fileName);

            var rand = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            //var path = Path.Combine(dateFolder, noExt + "." + rand + ext);
            var path = string.Concat(dateFolder, "\\", noExt, ".", rand, ext);

            // make sure path isn't too long
            var totalLength = rootFolder.Length + path.Length + 1;
            if (totalLength > 259) // path must be less than 260 characters
            {
                var excess = totalLength - 259;
                noExt = noExt.Substring(0, noExt.Length - excess);
                path = Path.Combine(dateFolder, noExt + "." + rand + ext);
            }

            while (File.Exists(Path.Combine(rootFolder, path)))
            {
                rand = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                path = Path.Combine(dateFolder, noExt + "." + rand + ext);
            }
            return path;
        }

        public static bool IsImageFile(string ext)
        {
            ext = Path.GetExtension(ext);
            ext = ext.ToLower();
            return ext == ".png" || ext == ".gif" || ext == ".jpg"
               || ext == ".jpeg" || ext == ".tif" || ext == ".tiff"
               || ext == ".bmp";
        }

        public static string RemoveIllegalPathChars(string @string)
        {
            foreach (var illegal in Path.GetInvalidFileNameChars())
            {
                @string = @string.Replace(illegal.ToString(), "");
            }
            @string = @string.Replace("\t", "");
            @string = @string.Replace("$", ""); // these may cause problems with Hudson FTP upload
            return @string;
        }
    }
}
