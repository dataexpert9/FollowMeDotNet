using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Areas.Admin.ViewModels;
using System.Data.Entity;
using WebApplication1.BindingModels;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Configuration;
using static BasketApi.Global;
using System.Text.RegularExpressions;

namespace BasketApi.Areas.SubAdmin.Controllers
{
    [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin", "User", "Guest")]
    [RoutePrefix("api/FollowFollower")]
    public class FollowFollowerController : ApiController
    { 
        [HttpGet]
        [Route("Follow")]
        public async Task<IHttpActionResult> Follow(int FollowUser_Id)
        {
            try
            {
                var userId = Convert.ToInt32(User.GetClaimValue("userid"));

                using (RiscoContext ctx = new RiscoContext())
                {
                    FollowFollower followFollower = new FollowFollower
                    {
                        FirstUser_Id = userId,
                        SecondUser_Id = FollowUser_Id,
                        CreatedDate = DateTime.UtcNow,
                    };

                    ctx.FollowFollowers.Add(followFollower);
                    ctx.SaveChanges();

                    CustomResponse<FollowFollower> response = new CustomResponse<FollowFollower>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = followFollower
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [HttpGet]
        [Route("UnFollow")]
        public async Task<IHttpActionResult> UnFollow(int UnFollowUser_Id)
        {
            try
            {
                var userId = Convert.ToInt32(User.GetClaimValue("userid"));

                using (RiscoContext ctx = new RiscoContext())
                {
                    FollowFollower followFollower = ctx.FollowFollowers.FirstOrDefault(x => x.FirstUser_Id == userId && x.SecondUser_Id == UnFollowUser_Id && x.IsDeleted == false);
                    followFollower.IsDeleted = true;
                    ctx.SaveChanges();

                    CustomResponse<FollowFollower> response = new CustomResponse<FollowFollower>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = followFollower
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [HttpGet]
        [Route("GetFollowers")]
        public async Task<IHttpActionResult> GetFollowers()
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var userId = Convert.ToInt32(User.GetClaimValue("userid"));

                    List<FollowFollower> followFollowers = new List<FollowFollower>();

                    followFollowers = ctx.FollowFollowers
                        .Include(x => x.FirstUser)
                        .Where(x => x.IsDeleted == false && x.SecondUser_Id == userId)
                        .ToList();

                    foreach (FollowFollower followFoller in followFollowers)
                    {
                        followFoller.IsFollowing = ctx.FollowFollowers.Any(x => x.FirstUser_Id == userId && x.SecondUser_Id == followFoller.FirstUser_Id);
                    }

                    CustomResponse<List<FollowFollower>> response = new CustomResponse<List<FollowFollower>>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = followFollowers
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [HttpGet]
        [Route("GetFollowings")]
        public async Task<IHttpActionResult> GetFollowings()
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var userId = Convert.ToInt32(User.GetClaimValue("userid"));

                    List<FollowFollower> followFollowers = new List<FollowFollower>();

                    followFollowers = ctx.FollowFollowers
                        .Include(x => x.SecondUser)
                        .Where(x => x.IsDeleted == false && x.FirstUser_Id == userId)
                        .ToList();

                    CustomResponse<List<FollowFollower>> response = new CustomResponse<List<FollowFollower>>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = followFollowers
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [HttpGet]
        [Route("GetTopFollowers")]
        public async Task<IHttpActionResult> GetTopFollowers()
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var userId = Convert.ToInt32(User.GetClaimValue("userid"));

                    string query = @"select top 5 FirstUser_Id, count(Id) as [Count] from TopFollowerLogs where  SecondUser_Id = " + userId +
@" and FirstUser_Id in (select FirstUser_Id from FollowFollowers f where f.IsDeleted = 0 and f.SecondUser_Id = " + userId + 
@") group by FirstUser_Id  
order by Count desc";
                    List<TopFollowersBindingModel> topFollowers = ctx.Database.SqlQuery<TopFollowersBindingModel>(query).ToList();

                    List<FollowFollower> followFollowers = new List<FollowFollower>();

                    followFollowers = ctx.FollowFollowers
                        .Include(x => x.FirstUser)
                        .Where(x => x.IsDeleted == false && x.SecondUser_Id == userId && topFollowers.Select(y => y.FirstUser_Id).ToList().Contains(x.FirstUser_Id))
                        .ToList();

                    CustomResponse<List<FollowFollower>> response = new CustomResponse<List<FollowFollower>>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = followFollowers
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        #region Private Regions
        #endregion
    }
}
