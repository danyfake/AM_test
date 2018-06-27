using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_test.objects
{
    public class Users
    {

        public List<User> Items { get; set; }
        public string Id { get; set; }
        public string ItemType { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }

        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }
    }
    public class User
    { 
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string TelephoneNumber { get; set; }
        public string ExternalId { get; set; }
        public List<Attribute> Attributes { get; set; }
        public Password Password { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Links Links { get; set; }

        public User() {
            Attributes = new List<Attribute>();
        }

        public User(string login, bool enable)
        {
            Login = login;
            Enabled = enable;
            Attributes = new List<Attribute>();
        }

        public User userGenerate()
        {
            Random rnd = new Random();
            User user = new User("user_test" + rnd.Next(10000), true);
            user.LastModifiedOn = DateTime.Now;
            user.LastModifiedBy = "AM auto test at " + DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Email = "Email" + rnd.Next(10000);
            user.FirstName = "FirstName" + rnd.Next(10000);
            user.LastName = "LastName" + rnd.Next(10000);
            user.Description = "Description" + rnd.Next(10000);
            user.DisplayName = "DisplayName" + rnd.Next(10000);
            user.TelephoneNumber = "TelephoneNumber" + rnd.Next(10000);
            user.ExternalId = "ExternalId" + rnd.Next(10000);
            user.TelephoneNumber = "TelephoneNumber" + rnd.Next(10000);
            Password pass = new Password();
            pass.Value = rnd.Next(1000000).ToString();
            pass.EffectiveDate = DateTime.Now;
            pass.ExpirationDate = DateTime.Now.AddMonths(3);
            user.Password = pass;
            var attrs = new List<Attribute>();
            attrs.Add(new AM_test.objects.Attribute("atr1_name", "atr1_Value"));
            attrs.Add(new AM_test.objects.Attribute("atr2_name", "atr2_Value"));
            user.Attributes = attrs;
            return user;
        }
        /*
        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public bool Equals(User other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id) &&
                (
                    object.ReferenceEquals(this.Login, other.Login) ||
                    this.Login != null &&
                    this.Login.Equals(other.Login)
                ) &&
                    this.Enabled.Equals(other.Enabled) &&
                (
                    object.ReferenceEquals(this.Email, other.Email) ||
                    this.Email != null &&
                    this.Email.Equals(other.Email)
                ) &&
                (
                    object.ReferenceEquals(this.FirstName, other.FirstName) ||
                    this.FirstName != null &&
                    this.FirstName.Equals(other.FirstName)
                ) &&
                (
                    object.ReferenceEquals(this.LastName, other.LastName) ||
                    this.LastName != null &&
                    this.LastName.Equals(other.LastName)
                ) ;
        }*/
    }
    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Attribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Password
    {
        public string Value { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class Links
    {
    }
}
