using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_api.Components
{
    public interface IComponent
    {
        public void HandleKey(ConsoleKeyInfo info);
        public void Draw();
    }
}
