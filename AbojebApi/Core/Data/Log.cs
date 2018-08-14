using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AbojebApi.Core.Data
{
    [DataContract]
    public class Log<T> where T : class
    {
        [DataMember]
        public virtual List<T> obj { get; set; }

        [DataMember]
        public int objCount { get; set; }

        [DataMember]
        [StringLength(200)]
        public string responseMessage { get; set; }

        [DataMember]
        public int responseCode { get; set; }

        [DataMember]
        public bool dataTruncated { get; set; }
    }
}