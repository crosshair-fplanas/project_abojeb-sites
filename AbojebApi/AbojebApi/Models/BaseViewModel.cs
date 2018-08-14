using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbojebApi.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            this.ReturnMessage = new ReturnMessageViewModel();
        }

        public ReturnMessageViewModel ReturnMessage { get; set; }
    }
}