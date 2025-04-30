using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public class RowDataSingleton
    {
        private static RowDataSingleton instance = null;
        public int RowCounter = 0;
        public Row[] RowData { get; set; } = new Row[1];
        private RowDataSingleton() { }     
        public static RowDataSingleton GetInstance()
        {
            if (instance == null)
                instance = new RowDataSingleton();
            return instance;
        }
        
    }
}
