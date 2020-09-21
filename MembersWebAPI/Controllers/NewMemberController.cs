using MembersWebAPI.Models;
using MembersWebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MembersWebAPI.Controllers
{
    public class NewMemberController : ApiController
    {
        //public List<Member> GetMember()
        //{
        //    return MemberRepository.Get(); 
        //}
        [HttpGet]
        public List<Member> List()
        {
            return MemberRepository.Get();
        }
        
        public Member Get(int id)
        {
            return MemberRepository.Get(id);
        }

        public List<Member> GetByAge(int age)
        {
            return MemberRepository.Get().Where(x => x.Age == age).ToList();
        }

        public Member Put([FromBody] Member member)
        {
            if (!MemberRepository.IsExist(member.Id))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to find member with given id."));
            }

            MemberRepository.Update(member);
            return member;
        }

        //public Member Post([FromBody] Member member)
        //{
        //    if (MemberRepository.IsExist(member.FullName))
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Another member with the same fullname is already exist."));
        //    }

        //    return MemberRepository.Add(member.FullName, member.Age);
        //}
        [HttpPost]
        public Member CreateNewMember([FromBody] Member member)
        {
            if (MemberRepository.IsExist(member.FullName))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Another member with the same fullname is already exist."));
            }

            return MemberRepository.Add(member.FullName, member.Age);
        }

        public HttpResponseMessage Delete(int id)
        {
            if (!MemberRepository.IsExist(id))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to find member with given id."));
            }
            MemberRepository.Remove(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}