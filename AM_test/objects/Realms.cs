using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_test.objects
{
    public class Realms
    {
        public string Id { get; set; }
        public string ItemType { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public List<Realm> Items { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }
    }

    public class Realm
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }
    }
}
