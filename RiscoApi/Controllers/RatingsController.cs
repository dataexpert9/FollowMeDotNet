using BasketApi.CustomAuthorization;
using BasketApi.Models;
using BasketApi.ViewModels;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace BasketApi.Controllers
{
    [Authorize("User", "Guest", "Deliverer")]
    [RoutePrefix("api/Ratings")]
    public class RatingsController : ApiController
    {
        /// <summary>
        /// Mark product as favourite
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProductId"></param>
        /// <param name="Favourite"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("MarkProductAsFavourite")]
        public async Task<IHttpActionResult> MarkProductAsFavourite(int UserId, int ProductId, bool Favourite)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var favouriteModel = ctx.Favourites.FirstOrDefault(x => x.User_ID == UserId && x.Product_Id == ProductId);

                    if (Favourite)
                    {
                        if (favouriteModel == null)
                        {
                            ctx.Favourites.Add(new Favourite { Product_Id = ProductId, User_ID = UserId });
                            ctx.SaveChanges();
                        }
                        else
                        {
                            if (favouriteModel.IsDeleted)
                            {
                                favouriteModel.IsDeleted = false;
                                ctx.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (favouriteModel != null)
                            if (!favouriteModel.IsDeleted)
                            {
                                favouriteModel.IsDeleted = true;
                                ctx.SaveChanges();
                            }
                    }

                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        /// <summary>
        /// Rate user or deliverer.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RateUser")]
        public async Task<IHttpActionResult> RateUser(RateUserBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (RiscoContext ctx = new RiscoContext())
                {
                    var user = ctx.Users.FirstOrDefault(x => x.Id == model.UserId);
                    if (user == null)
                        return Ok(new CustomResponse<Error> { Message = Global.ResponseMessages.NotFound, StatusCode = (int)HttpStatusCode.NotFound, Result = new Error { ErrorMessage = "UserId is invalid" } });
                   
                    else
                        return Ok(new CustomResponse<Error> { Message = Global.ResponseMessages.NotFound, StatusCode = (int)HttpStatusCode.NotFound, Result = new Error { ErrorMessage = "Invalid SignInType" } });
                    ctx.SaveChanges();
                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        /// <summary>
        /// Rate App
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Rating"></param>
        /// <param name="Feedback"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RateApp")]
        public async Task<IHttpActionResult> RateApp(int UserId, short Rating, string Feedback)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var user = ctx.Users.FirstOrDefault(x => x.Id == UserId);

                    if (user != null)
                    {
                        user.AppRatings.Add(new AppRatings { User_ID = user.Id, Rating = Rating, Description = Feedback });
                        ctx.SaveChanges();
                        return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                    }
                    else
                        return Ok(new CustomResponse<Error> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = new Error { ErrorMessage = "Invalid UserId" } });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [Authorize("User")]
        [HttpGet]
        [Route("GetUserFavourites")]
        public async Task<IHttpActionResult> GetUserFavourites(int UserId)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var favourites = ctx.Favourites.Where(x => x.User_ID == UserId && x.IsDeleted == false).ToList();
                    return Ok(new CustomResponse<FavouritesViewModel> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = new FavouritesViewModel { Favourites = favourites } });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }
    }
}
