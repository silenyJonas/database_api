using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Documents
{
    public class ResponseList
    {
        public string SystemPrefix { get; } = "~";
        public string IncorrectLogin { get; } = "Incorrect Name or Password";
        public string IncorrectSyntax { get; } = "Incorrect syntax";
        public string Success { get; } = "Success";
        public string DbConError { get; } = "Database connecting Error";
        public string DBQuerryError { get; } = "Database Querry Error";
        
    }
}
