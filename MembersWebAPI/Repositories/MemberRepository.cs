using MembersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembersWebAPI.Repositories
{
    public class MemberRepository
    {

        //uygulama ayağa kalkarken değil bu nesneye ulaşmaya çalıştığımızda initialize olur.
        //2. Parametere true dediğimizde  concurrent yani eşzamanlı isteklerde korunmasını istediğimiz ifade ederiz.
        //Bunun sebebi Thread safety olması gerektiğinden kaynaklanır.
        private static Lazy<List<Member>> container = new Lazy<List<Member>>(() => Initialize(), true);

        private static List<Member> Initialize()
        {
            return new List<Member>
            {
                new Member(1,"Hasancan Kazan",24),
                new Member(2, "Mustafa Tuğkan Meral", 28)
            };
        }

        public static Member Add(string fullname, int age)
        {
            int lastId = container.Value.Max(x => x.Id);
            Member newMember = new Member(lastId + 1, fullname, age);
            container.Value.Add(newMember);
            return newMember;
        }


        public static List<Member> Get()
        {
            return container.Value.ToList();
        }

        public static Member Get(int id)
        {
            return container.Value.SingleOrDefault(x => x.Id == id);
        }

        public static void Remove(int id)
        {
            container.Value.Remove(Get(id));
        }

        public static void Update(Member member)
        {
            Remove(member.Id);
            container.Value.Add(member);
        }

        public static bool IsExist(int id)
        {
            return container.Value.Any(m => m.Id == id);
        }

        public static bool IsExist(string fullName)
        {
            return container.Value.Any(m => m.FullName == fullName);
        }

    }
}