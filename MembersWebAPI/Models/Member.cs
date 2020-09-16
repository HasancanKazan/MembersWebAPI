using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembersWebAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }

        public Member(int id, string fullName, int age)
        {
            this.Id = id;
            this.FullName = fullName;
            this.Age = age;
        }
    }
}