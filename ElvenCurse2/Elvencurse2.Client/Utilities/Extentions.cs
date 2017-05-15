using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Elvencurse2.Client.Utilities
{
    public static class Extentions
    {
        public static Dictionary<string, T> LoadListContent<T>(this ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }
                
            var result = new Dictionary<String, T>();

            var files = dir.GetFiles("*.*");
            foreach (var file in files)
            {
                var key = Path.GetFileNameWithoutExtension(file.Name);
                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }
    }
}
