using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using FigureAbstract;


namespace ShapeIT
{
    
    public static class FiguresSerialize
    {
        
        public const int fCreate = 0;
        public const int fOpen = 1;
        public const int fSave = 2;

        public static JsonSerializerSettings Settings { get; set; }= new JsonSerializerSettings() {Formatting=Formatting.Indented, TypeNameHandling=TypeNameHandling.Auto };
        
        public static void FileSerialize(string fileName,List<Figure> figures)
        {
            
            using (FileStream fs = new FileStream(fileName, FileMode.Create,FileAccess.Write)) {
                byte[] textBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(figures, Settings));
                fs.Write(textBytes,0,textBytes.Length);
            }

        }
        public static void FileDeserialize(string fileName, ref List<Figure> figures)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] textBytes = new byte[fs.Length];
                fs.Read(textBytes,0,textBytes.Length);
                try
                {
                    figures = JsonConvert.DeserializeObject<List<Figure>>(Encoding.UTF8.GetString(textBytes, 0, textBytes.Length), Settings);
                }
                catch
                {
                    
                }
            }
        }

    }
}
