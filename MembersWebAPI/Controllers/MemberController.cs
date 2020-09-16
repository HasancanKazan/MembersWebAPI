using MembersWebAPI.Models;
using MembersWebAPI.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace MembersWebAPI.Controllers
{
    public class MemberController : IHttpController
    {
        public async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            HttpResponseMessage retVal = null;

            if (controllerContext.Request.Method == HttpMethod.Get)
            {
                retVal = HttpGet(controllerContext);
            }
            else if (controllerContext.Request.Method == HttpMethod.Post)
            {
                retVal = await HttpPost(controllerContext);
            }
            else if (controllerContext.Request.Method == HttpMethod.Put)
            {
                retVal = await HttpPut(controllerContext);
            }
            else if (controllerContext.Request.Method == HttpMethod.Delete)
            {
                retVal = HttpDelete(controllerContext);
            }
            return retVal;
        }
        private HttpResponseMessage HttpGet(HttpControllerContext controllerContext)
        {
            HttpResponseMessage retVal = null;

            var id = controllerContext.RouteData.Values["id"];

            if (id == null)
            {
                var content = MemberRepository.Get();
                retVal = controllerContext.Request.CreateResponse(HttpStatusCode.OK, content);
            }
            else
            {
                int idAsInteger;
                if (!int.TryParse(id.ToString(), out idAsInteger))
                {
                    retVal = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id must be a numeric value.");
                }
                else
                {
                    var content = MemberRepository.Get(idAsInteger);
                    retVal = controllerContext.Request.CreateResponse(HttpStatusCode.OK, content);
                }
            }
            return retVal;
        }
        private async Task<HttpResponseMessage> HttpPost(HttpControllerContext controllerContext)
        {
            HttpResponseMessage retval = null;

            //awaitable bir metodumuzun dönüşünü bekletmiş oluyoruz
            string contentAsString = await controllerContext.Request.Content.ReadAsStringAsync();

            Member postedMember = JsonConvert.DeserializeObject<Member>(contentAsString);

            if (MemberRepository.IsExist(postedMember.FullName))
            {
                retval = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Another member the same fullname is already exist.");
            }
            else
            {
                var newMember = MemberRepository.Add(postedMember.FullName, postedMember.Age);
                retval = controllerContext.Request.CreateResponse(HttpStatusCode.OK, newMember);
            }

            return retval;

        }
        private async Task<HttpResponseMessage> HttpPut(HttpControllerContext controllerContext)
        {
            HttpResponseMessage retval = null;

            string contentAsString = await controllerContext.Request.Content.ReadAsStringAsync();

            Member postedMember = JsonConvert.DeserializeObject<Member>(contentAsString);

            if (!MemberRepository.IsExist(postedMember.Id))
            {
                retval = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to find member with given Id.");
            }
            else
            {
                MemberRepository.Update(postedMember);
                retval = controllerContext.Request.CreateResponse(HttpStatusCode.OK, postedMember);
            }
            return retval;
        }

        private HttpResponseMessage HttpDelete(HttpControllerContext controllerContext)
        {
            HttpResponseMessage retVal = null;

            var id = controllerContext.RouteData.Values["id"];

            if (id == null)
            {
                retVal = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot call this metod withoud id.");
            }
            else
            {
                int idAsInteger;

                if (!int.TryParse(id.ToString(), out idAsInteger))
                {
                    retVal = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id must be a numeric value.");
                }
                else
                {
                    if (!MemberRepository.IsExist(idAsInteger))
                    {
                        retVal = controllerContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to find member with given id.");
                    }
                    else
                    {
                        var personToBeDeleted = MemberRepository.Get(idAsInteger);
                        MemberRepository.Remove(idAsInteger);
                        //silinen kullanıcının bilgilerini geri dönüyorum
                        retVal = controllerContext.Request.CreateResponse(HttpStatusCode.OK, personToBeDeleted);
                    }
                }
            }
            return retVal;
        }
    }
}