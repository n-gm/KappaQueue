using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KappaQueueClient.Interfaces
{
    public interface IRestSelector
    {
        List<object> GetEntity();
        object GetEntity(int id, params string[] filters);
    }
}
