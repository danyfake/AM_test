using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_test.objects
{
    public class Groups
    {
        public string Id { get; set; }
        public string ItemType { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public List<Group> Items { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }
    }

        public class Group
        {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Attribute> Attributes { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }

        public Group()
        {
            Attributes = new List<Attribute>();
        }

        public Group(string name, bool enable)
        {
            Name = name;
            Enabled = enable;
            Attributes = new List<Attribute>();
        }

        public Group groupGenerate()
        {
            Random rnd = new Random();
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group.LastModifiedOn = DateTime.Now;
            group.LastModifiedBy = "AM auto test at " + DateTime.Now;
            group.Id = Guid.NewGuid().ToString();
            group.Description = "Description" + rnd.Next(10000);
            var attrs = new List<Attribute>();
            group.Attributes = attrs;
            return group;
        }
    }
    
}
