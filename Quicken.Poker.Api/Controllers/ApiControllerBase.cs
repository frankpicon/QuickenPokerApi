using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicken.Poker.Api.Utilities;

namespace Quicken.Poker.Api.Controllers
{


    public abstract class ApiControllerBase : ControllerBase
    {


        private string userLogin = null;
        public string UserLogin
        {
            get { return userLogin ?? (userLogin = TokenFunctions.GetUserLoginFromToken(HttpRequestMessageExtensions.GetHeader(Request, "GSK-Session"))); }
        }


        public T Fail<T>(Exception ex) where T : IActionResult, new()
        {

            var result = new T();
            result.ErrorMessage = (ex.InnerException != null)
                ? ex.InnerException.Message
                : ex.Message;

            result.IsSuccess = false;
            //Logger.Error(ex.Message, ex);

            return result;

        }
    }

    public interface IActionResult
    {
        bool IsSuccess { get; set; }
        string ErrorMessage { get; set; }
    }

    public class ActionResult : IActionResult
    {
        public ActionResult()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

}
