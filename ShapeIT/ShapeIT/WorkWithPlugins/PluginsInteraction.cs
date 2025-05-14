using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeIT
{
    internal class PluginsInteraction
    {
        public static async Task<bool> AddPluginAsync(string fileName)
        {
            string createdFileName = MainWindow.WorkDir + "\\Plugins" + "\\" + Path.GetFileName(fileName);
            if (File.Exists(createdFileName))
            {
                return false;
            }
            byte[] bytes;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fs.Length];
                await fs.ReadAsync(bytes, 0, bytes.Length);
            }
            using (FileStream fs = new FileStream(createdFileName, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
            return true;
        }
    }
}
