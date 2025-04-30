using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public class Package
    {
        public string? Text { get; set; }
        public bool systemValue { get; set; }
        public ConsoleColor Color { get; set; }
        public string[] Data { get; set; }
        public Package(string? _text, bool _systemValue ,ConsoleColor consoleColor = ConsoleColor.Red, string[] data = null) 
        { 
            Text = _text;
            systemValue = _systemValue;
            Color = consoleColor;
            Data = data ?? new string[0];
            
        }

    }
}
