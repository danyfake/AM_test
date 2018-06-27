using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_test.objects
{
    public class loginPage
    {

        
        public List<callback> callbacks { get; set; }
        public string __authState { get; set; }
        public string header { get; set; }
        public List<object> errors { get; set; }
    }
    public class option
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class callback
    {
        public List<option> options { get; set; }
        public string prompt { get; set; }

        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }

    }

}
