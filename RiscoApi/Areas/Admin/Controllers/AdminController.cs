using BasketApi;
using BasketApi.Areas.SubAdmin.Models;
using BasketApi.CustomAuthorization;
using BasketApi.Models;
using BasketApi.ViewModels;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebApplication1.Areas.Admin.ViewModels;
using System.Data.Entity;
using BasketApi.BindingModels;
using static BasketApi.Utility;
using Newtonsoft.Json;
using static BasketApi.Global;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Web.Hosting;
using BasketApi.Components.Helpers;
using WebApplication1.ViewModels;
using WebApplication1.Models;
using WebApplication1.BindingModels;
using Z.EntityFramework.Plus;

namespace WebApplication1.Areas.Admin.Controllers
{
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        /// <summary>
        /// Add admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAdmin")]
        public async Task<IHttpActionResult> AddAdmin()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string newFullPath = string.Empty;
                string fileNameOnly = string.Empty;

                DAL.Admin model = new DAL.Admin();
                DAL.Admin existingAdmin = new DAL.Admin();

                if (httpRequest.Params["Id"] != null)
                    model.Id = Convert.ToInt32(httpRequest.Params["Id"]);

                if (httpRequest.Params["ImageDeletedOnEdit"] != null)
                    model.ImageDeletedOnEdit = Convert.ToBoolean(httpRequest.Params["ImageDeletedOnEdit"]);

                model.FirstName = httpRequest.Params["FirstName"];
                model.LastName = httpRequest.Params["LastName"];
                model.Email = httpRequest.Params["Email"];
                model.Phone = httpRequest.Params["Phone"];
                model.Role = Convert.ToInt16(httpRequest.Params["Role"]);
                model.Password = httpRequest.Params["Password"];
                model.ImageUrl = httpRequest.Params["ImageUrl"];

                model.Status = (int)Global.StatusCode.NotVerified;

                if (httpRequest.Params["Store_Id"] != null)
                    model.Store_Id = Convert.ToInt32(httpRequest.Params["Store_Id"]);

                Validate(model);

                #region Validations

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //if (!Request.Content.IsMimeMultipartContent())
                //{
                //    return Content(HttpStatusCode.OK, new CustomResponse<Error>
                //    {
                //        Message = "UnsupportedMediaType",
                //        StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                //        Result = new Error { ErrorMessage = "Multipart data is not included in request" }
                //    });
                //}
                //else if (httpRequest.Files.Count > 1)
                //{
                //    return Content(HttpStatusCode.OK, new CustomResponse<Error>
                //    {
                //        Message = "UnsupportedMediaType",
                //        StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                //        Result = new Error { ErrorMessage = "Multiple images are not supported, please upload one image" }
                //    });
                //}
                #endregion

                using (RiscoContext ctx = new RiscoContext())
                {
                    if (model.Id == 0)
                    {

                        if (ctx.Admins.Any(x => x.Email == model.Email && x.IsDeleted == false))
                        {
                            return Content(HttpStatusCode.OK, new CustomResponse<Error>
                            {
                                Message = "Conflict",
                                StatusCode = (int)HttpStatusCode.Conflict,
                                Result = new Error { ErrorMessage = "Admin with same email already exists" }
                            });
                        }
                    }
                    else
                    {
                        existingAdmin = ctx.Admins.FirstOrDefault(x => x.Id == model.Id);
                        model.Password = existingAdmin.Password;
                        if (existingAdmin.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase) == false || existingAdmin.Store_Id != model.Store_Id)
                        {
                            if (ctx.Admins.Any(x => x.IsDeleted == false && x.Store_Id == model.Store_Id && x.Email.Equals(model.Email.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                            {
                                return Content(HttpStatusCode.OK, new CustomResponse<Error>
                                {
                                    Message = "Conflict",
                                    StatusCode = (int)HttpStatusCode.Conflict,
                                    Result = new Error { ErrorMessage = "Admin with same email already exists" }
                                });
                            }
                        }
                    }

                    string fileExtension = string.Empty;
                    HttpPostedFile postedFile = null;
                    //#region ImageSaving
                    //if (httpRequest.Files.Count > 0)
                    //{
                    //    postedFile = httpRequest.Files[0];
                    //    if (postedFile != null && postedFile.ContentLength > 0)
                    //    {
                    //        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    //        var ext = Path.GetExtension(postedFile.FileName);
                    //        fileExtension = ext.ToLower();
                    //        if (!AllowedFileExtensions.Contains(fileExtension))
                    //        {
                    //            return Content(HttpStatusCode.OK, new CustomResponse<Error>
                    //            {
                    //                Message = "UnsupportedMediaType",
                    //                StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                    //                Result = new Error { ErrorMessage = "Please Upload image of type .jpg,.gif,.png" }
                    //            });
                    //        }
                    //        else if (postedFile.ContentLength > Global.MaximumImageSize)
                    //        {
                    //            return Content(HttpStatusCode.OK, new CustomResponse<Error>
                    //            {
                    //                Message = "UnsupportedMediaType",
                    //                StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                    //                Result = new Error { ErrorMessage = "Please Upload a file upto " + Global.ImageSize }
                    //            });
                    //        }
                    //        else
                    //        {
                    //            //int count = 1;
                    //            //fileNameOnly = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    //            //newFullPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["AdminImageFolderPath"] + postedFile.FileName);

                    //            //while (File.Exists(newFullPath))
                    //            //{
                    //            //    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    //            //    newFullPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["AdminImageFolderPath"] + tempFileName + extension);
                    //            //}
                    //            //postedFile.SaveAs(newFullPath);
                    //        }
                    //    }
                    //    //model.ImageUrl = ConfigurationManager.AppSettings["AdminImageFolderPath"] + Path.GetFileName(newFullPath);
                    //}
                    //#endregion

                    if (model.Id == 0)
                    {
                        ctx.Admins.Add(model);
                        ctx.SaveChanges();
                     //   var guid = Guid.NewGuid();
                      //  newFullPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["AdminImageFolderPath"] + model.Id + "_" + guid + fileExtension);
                     ////   postedFile.SaveAs(newFullPath);
                      //  model.ImageUrl = ConfigurationManager.AppSettings["AdminImageFolderPath"] + model.Id + "_" + guid + fileExtension;
                        ctx.SaveChanges();

                    }
                    else
                    {
                        //existingProduct = ctx.Products.FirstOrDefault(x => x.Id == model.Id);
                        //if (httpRequest.Files.Count == 0)
                        //{
                            // Check if image deleted
                            //if (model.ImageDeletedOnEdit == false)
                            //{
                            //    model.ImageUrl = existingAdmin.ImageUrl;
                            //}
                        //}
                        //else
                        //{
                        //    Utility.DeleteFileIfExists(existingAdmin.ImageUrl);
                        //    var guid = Guid.NewGuid();
                        //    newFullPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["AdminImageFolderPath"] + model.Id + "_" + guid + fileExtension);
                        //    postedFile.SaveAs(newFullPath);
                        //    model.ImageUrl = ConfigurationManager.AppSettings["AdminImageFolderPath"] + model.Id + "_" + guid + fileExtension;
                        //}

                        ctx.Entry(existingAdmin).CurrentValues.SetValues(model);
                        ctx.SaveChanges();
                    }

                    await model.GenerateToken(Request);

                    CustomResponse<DAL.Admin> response = new CustomResponse<DAL.Admin>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = model
                    };

                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


      
        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpGet]
        [Route("SearchAdmins")]
        public async Task<IHttpActionResult> SearchAdmins(string FirstName, string LastName, string Email, string Phone, int? StoreId)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    string conditions = string.Empty;

                    if (!String.IsNullOrEmpty(FirstName))
                        conditions += " And Admins.FirstName Like '%" + FirstName.Trim() + "%'";

                    if (!String.IsNullOrEmpty(LastName))
                        conditions += " And Admins.LastName Like '%" + LastName.Trim() + "%'";

                    if (!String.IsNullOrEmpty(Email))
                        conditions += " And Admins.Email Like '%" + Email.Trim() + "%'";

                    if (!String.IsNullOrEmpty(Phone))
                        conditions += " And Admins.Phone Like '%" + Phone.Trim() + "%'";

                    if (StoreId.HasValue && StoreId.Value != 0)
                        conditions += " And Admins.Store_Id = " + StoreId;

                    #region query
                    var query = @"SELECT
  Admins.Id,
  Admins.FirstName,
  Admins.LastName,
  Admins.Email,
  Admins.Phone,
  Admins.Role,
  Admins.ImageUrl,
  Stores.Name AS StoreName
FROM Admins
LEFT OUTER JOIN Stores
  ON Stores.Id = Admins.Store_Id
WHERE Admins.IsDeleted = 0
AND Stores.IsDeleted = 0 " + conditions + @" UNION
SELECT
  Admins.Id,
  Admins.FirstName,
  Admins.LastName,
  Admins.Email,
  Admins.Phone,
  Admins.Role,
  Admins.ImageUrl,
  '' AS StoreName
FROM Admins
WHERE Admins.IsDeleted = 0
AND ISNULL(Admins.Store_Id, 0) = 0 " + conditions;

                    #endregion


                    var admins = ctx.Database.SqlQuery<SearchAdminViewModel>(query).ToList();

                    return Ok(new CustomResponse<SearchAdminListViewModel> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = new SearchAdminListViewModel { Admins = admins } });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

       

        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpGet]
        [Route("DeleteEntity")]
        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        public async Task<IHttpActionResult> DeleteEntity(int EntityType, int Id)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    switch (EntityType)
                    {
                      
                     
                        case (int)BasketEntityTypes.Admin:
                            ctx.Admins.FirstOrDefault(x => x.Id == Id).IsDeleted = true;
                            break;
                        default:
                            break;
                    }
                    ctx.SaveChanges();
                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }



        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(AdminSetPasswordBindingModel model)
        {
            try
            {
                var userEmail = User.Identity.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    throw new Exception("User Email is empty in user.identity.name.");
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (RiscoContext ctx = new RiscoContext())
                {
                    var hashedPassword = CryptoHelper.Hash(model.OldPassword);
                    var user = ctx.Admins.FirstOrDefault(x => x.Email == userEmail && x.Password == hashedPassword);
                    if (user != null)
                    {
                        user.Password = CryptoHelper.Hash(model.NewPassword);
                        ctx.SaveChanges();
                        return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                    }
                    else
                        return Ok(new CustomResponse<Error> { Message = "Forbidden", StatusCode = (int)HttpStatusCode.Forbidden, Result = new Error { ErrorMessage = "Invalid old password." } });


                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [Route("AddNotification")]
        public async Task<IHttpActionResult> AddNotification(NotificationBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                TimeZoneInfo UAETimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"); DateTime utc = DateTime.UtcNow;
                DateTime UAE = TimeZoneInfo.ConvertTimeFromUtc(utc, UAETimeZone);

                using (RiscoContext ctx = new RiscoContext())
                {
                    AdminNotifications adminNotification = new AdminNotifications { CreatedDate = UAE, Title = model.Title, TargetAudienceType = model.TargetAudience, Description = model.Description };

                    ctx.AdminNotifications.Add(adminNotification);
                    ctx.SaveChanges();
                    if (model.TargetAudience == (int)NotificationTargetAudienceTypes.User || model.TargetAudience == (int)NotificationTargetAudienceTypes.UserAndDeliverer)
                    {
                        var users = ctx.Users.Where(x => x.IsDeleted == false).Include(x => x.UserDevices).Where(x => x.IsDeleted == false);

                        await users.ForEachAsync(a => a.Notifications.Add(new Notification { Title = model.Title, Text = model.Description, Status = 0, AdminNotification_Id = adminNotification.Id }));

                        await ctx.SaveChangesAsync();

                        var usersToPushAndroid = users.Where(x => x.IsNotificationsOn).SelectMany(x => x.UserDevices.Where(x1 => x1.Platform == true)).ToList();
                        var usersToPushIOS = users.Where(x => x.IsNotificationsOn).SelectMany(x => x.UserDevices.Where(x1 => x1.Platform == false)).ToList();

                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            Global.objPushNotifications.SendAndroidPushNotification(usersToPushAndroid, adminNotification);
                            Global.objPushNotifications.SendIOSPushNotification(usersToPushIOS, adminNotification);

                        });

                    }

                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpGet]
        [Route("SearchNotifications")]
        public async Task<IHttpActionResult> SearchNotifications()
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    return Ok(new CustomResponse<SearchAdminNotificationsViewModel>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = new SearchAdminNotificationsViewModel
                        {
                            Notifications = ctx.AdminNotifications.ToList()
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpPost]
        [Route("ChangeUserStatuses")]
        public async Task<IHttpActionResult> ChangeUserStatuses(ChangeUserStatusListBindingModel model)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    foreach (var user in model.Users)
                        ctx.Users.FirstOrDefault(x => x.Id == user.UserId).IsDeleted = user.Status;

                    ctx.SaveChanges();
                }

                return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

       


        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers()
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    return Ok(new CustomResponse<SearchUsersViewModel>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = new SearchUsersViewModel
                        {
                            Users = ctx.Users.ToList()
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        //[BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]

        [HttpGet]
        [Route("GetUser")]
        public async Task<IHttpActionResult> GetUser(int UserId, int SignInType)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    BasketSettings.LoadSettings();

                    if (SignInType == (int)RoleTypes.User)
                    {
                        var user = ctx.Users.Include(x => x.Notifications.Select(x1 => x1.AdminNotification)).Include(x => x.UserAddresses).Include(x => x.PaymentCards).FirstOrDefault(x => x.Id == UserId);
                        return Ok(new CustomResponse<User> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = user });
                    }
                    else
                    {
                        return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK, Result = "" });

                    }


                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [HttpGet]
        [Route("GetSalesGraphData")]
        public async Task<IHttpActionResult> GetSalesGraphData()
        {
            try
            {
                var query = "select Sum(Total) as Total, CAST(OrderDateTime AS DATE) as OrderDateTime from Orders where IsDeleted = 0 group by CAST(OrderDateTime AS DATE) order by CAST(OrderDateTime AS DATE) desc";

                using (RiscoContext ctx = new RiscoContext())
                {
                    return Ok(new CustomResponse<ListOrderSalesGraph>
                    {
                        Message = Global.ResponseMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = new ListOrderSalesGraph
                        {
                            Orders = ctx.Database.SqlQuery<OrdersSalesGraph>(query).ToList()
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpPost]
        [Route("SendNotificationToUser")]
        public async Task<IHttpActionResult> SendNotificationToUser(SendNotificationToUserBindingModel model)
        {
            try
            {
                TimeZoneInfo UAETimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"); DateTime utc = DateTime.UtcNow;
                DateTime UAE = TimeZoneInfo.ConvertTimeFromUtc(utc, UAETimeZone);

                AdminNotifications adminNotification = new AdminNotifications
                {
                    CreatedDate = UAE,
                    TargetAudienceType = (int)NotificationTargetAudienceTypes.IndividualUser,
                    Title = model.Title,
                    Description = model.Text
                };

                using (RiscoContext ctx = new RiscoContext())
                {
                    ctx.AdminNotifications.Add(adminNotification);
                    ctx.SaveChanges();

                    Notification Notification = new Notification
                    {
                        AdminNotification_Id = adminNotification.Id,
                        Title = model.Title,
                        Text = model.Text,
                        User_ID = model.User_Id
                    };

                    ctx.Notifications.Add(Notification);
                    ctx.SaveChanges();

                    var users = ctx.Users.Include(x => x.UserDevices).Where(x => x.IsNotificationsOn && x.IsDeleted == false && x.Id == model.User_Id).ToList();

                    var androidDevices = users.Where(x => x.IsNotificationsOn).SelectMany(x => x.UserDevices.Where(x1 => x1.Platform == true)).ToList();
                    var iosDevices = users.Where(x => x.IsNotificationsOn).SelectMany(x => x.UserDevices.Where(x1 => x1.Platform == false)).ToList();
                    if (androidDevices.Count > 0 || iosDevices.Count > 0)
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            Global.objPushNotifications.SendAndroidPushNotification(androidDevices, adminNotification);
                            Global.objPushNotifications.SendIOSPushNotification(iosDevices, adminNotification);
                        });
                    }
                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


        [BasketApi.Authorize("SubAdmin", "SuperAdmin", "ApplicationAdmin")]
        [HttpPost]
        [Route("AddUpdateFCMToken")]
        public async Task<IHttpActionResult> AddUpdateFCMToken(FCMTokenModel model)
        {
            try
            {
                var userId = Convert.ToInt32(User.GetClaimValue("userid"));
                using (RiscoContext ctx = new RiscoContext())
                {
                    var adminToken = ctx.AdminTokens.FirstOrDefault(x => x.Admin_Id == userId && x.Token == model.Token);

                    if (adminToken != null)
                        adminToken.IsActive = true;
                    else
                        ctx.AdminTokens.Add(new AdminTokens { Admin_Id = userId, Token = model.Token, IsActive = true });

                    ctx.SaveChanges();

                    return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("TestNotificationToAdmin")]
        public async Task<IHttpActionResult> TestNotificationToAdmin(string Text)
        {
            try
            {
                using (RiscoContext ctx = new RiscoContext())
                {
                    var adminsToPush = ctx.AdminTokens.ToList();
                    var orderUrl = ConfigurationManager.AppSettings["WebsiteBaseUrl"] + "Dashboard/Orders?OrderId=2204";

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        Global.objPushNotifications.SendWebGCMPushNotification(adminsToPush, "New Order Received! #2204", "Brush Type A, WaterColor, BrushType D", orderUrl);
                    });
                }

                return Ok(new CustomResponse<string> { Message = Global.ResponseMessages.Success, StatusCode = (int)HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(Utility.LogError(ex));
            }
        }


    }
}
