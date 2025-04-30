using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public class Row
    {
        public string Value { get; set; }
        public bool SystemValue { get; set; }
        public ConsoleColor Color { get; set; }
        public Row(string value, bool systemValue, ConsoleColor color)
        {
            this.Value = value;
            this.SystemValue = systemValue;
            this.Color = color;
            
        }
    }
}
