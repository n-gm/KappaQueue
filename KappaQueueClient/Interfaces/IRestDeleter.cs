using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KappaQueueClient.Interfaces
{
    public interface IRestDeleter
    {
        List<object> Delete(int id);
    }
}
