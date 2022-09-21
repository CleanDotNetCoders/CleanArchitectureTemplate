using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string CollectionName { get; set; }
    }
}
