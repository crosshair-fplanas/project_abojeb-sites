using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AbojebApi.Core.Data
{
    [DataContract]
    public class ApiVersion
    {
        [DataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        [DataMember]
        public string VersionDescription { get; set; }

        [DataMember]
        public virtual List<ApiFunction> Functions { get; set; }
    }
}
