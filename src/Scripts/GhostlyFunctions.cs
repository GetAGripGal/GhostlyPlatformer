using System;
using Godot;

namespace GhostlyPlatformer.Scripts
{
    public static class GhostlyFunctions
    {
        public static dynamic ReadJson(string path)
        {
            var file = new File();
            try { file.Open(path, File.ModeFlags.Read); }
            catch { throw new Exception("Invalid JSON-file"); }

            if (!file.IsOpen()) return null;
            
            var jsonText = file.GetAsText();
            return JSON.Parse(jsonText).Result;
        }

        // public static bool JsonContainsKey(dynamic, string key)
        // {
        //     try
        //     {
        //         var i = 
        //     }
        // }
    }
    
}