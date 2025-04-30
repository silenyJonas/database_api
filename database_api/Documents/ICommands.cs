using database_api.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace database_api.Documents
{
    public interface ICommands
    {        
        public Package Proceed(string data);
    }
}
