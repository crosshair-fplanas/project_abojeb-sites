using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbojebApi.Core
{
    public abstract class BaseEntity
    {
        public Int64 Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}