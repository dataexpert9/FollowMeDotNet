using BasketApi;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Areas.Admin.Models;
using System.Data.Entity;
using static BasketApi.Global;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Friend")]
    public class FriendController : ApiController
    {

        [AllowAnonymous]
        [Route("SendFriendRequest")]
        [HttpPost]
        public async Task<IHttpActionResult> SendFriendRequest(FriendBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                using (RiscoContext ctx = new RiscoContext())
                {
                    if (ctx.Friends.Any(x => x.Requester_Id == model.User_Id && x.Addressee_Id == model.Friend_Id))
                        return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = "Request sent already." });
                    else
                    {
                        ctx.Friends.Add(new Friends
                        {
                            Requester_Id = model.User_Id,
                            Addressee_Id = model.Friend_Id,
                            FriendRequestStatus = Convert.ToInt32(FriendRequestStatus.Pending)
                        });
                        ctx.SaveChanges();
                        return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = "Friend request sent." });

                    }
                }


            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [AllowAnonymous]
        [Route("GetFriendRequests")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFriendRequests(int User_Id, FriendRequestStatus? status)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                using (RiscoContext ctx = new RiscoContext())
                {


                    if (status.HasValue)
                    {
                        var statuss = Convert.ToInt32(status.Value);
                        return Ok(new CustomResponse<List<Friends>>
                        {
                            Message = Global.ResponseMessages.Success,
                            StatusCode = (int)HttpStatusCode.OK,
                            Result = ctx.Friends.Include(x=>x.Requester).Include(x=>x.Addressee).Where(x => x.Requester_Id == User_Id && x.FriendRequestStatus == statuss).ToList()
                        });
                    }
                    else
                    {
                        var response = ctx.Friends.Include(x => x.Requester).Include(x => x.Addressee).Where(x => x.Requester_Id == User_Id).ToList();
                        return Ok(new CustomResponse<List<Friends>>
                        {
                            Message = Global.ResponseMessages.Success,
                            StatusCode = (int)HttpStatusCode.OK,
                            Result = response
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [AllowAnonymous]
        [Route("AcceptRejectFriendRequest")]
        [HttpPost]
        public async Task<IHttpActionResult> AcceptRejectFriendRequest(AcceptRejectFriendRequestBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                using (RiscoContext ctx = new RiscoContext())
                {

                    var friendRequest = ctx.Friends.FirstOrDefault(x => x.Requester_Id == model.User_Id && x.Addressee_Id == model.Friend_Id);
                    if (friendRequest != null)
                    {
                        if (model.Status == FriendRequestStatus.Accepted)
                            friendRequest.FriendRequestStatus = Convert.ToInt32(model.Status);
                        else
                            ctx.Friends.Remove(friendRequest);
                    }
                    ctx.SaveChanges();
                    return Ok(new CustomResponse<string>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = "Request has been" + model.Status.ToString() + " successfully."
                    });

                }


            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }



        [AllowAnonymous]
        [Route("GetMyFriendList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyFriendList(int User_Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                using (RiscoContext ctx = new RiscoContext())
                {
                    var statuss = Convert.ToInt32(FriendRequestStatus.Accepted);

                    var response = ctx.Friends.Include(x => x.Requester).Include(x => x.Addressee).Where(x => x.Requester_Id == User_Id || x.Addressee_Id== User_Id && x.FriendRequestStatus== statuss).ToList();
                    return Ok(new CustomResponse<List<Friends>>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = response
                    });


                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }
    }
}
