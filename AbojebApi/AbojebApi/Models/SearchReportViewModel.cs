using AbojebApi.Core.DataTransferObjects;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AbojebApi.Models
{
    public class SearchReportViewModel
    {
        public SearchReportViewModel()
        {
            Vessels = new List<VesselDto>();
        }

        [DisplayName("IMO - Vessel Name")]
        public int[] SelectedIMOs { get; set; }

        public List<VesselDto> Vessels { get; set; }

        [DisplayName("Timestamp")]
        [Required]
        public string DateRange { get; set; }
    }
}