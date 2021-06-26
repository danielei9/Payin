using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class EventUpdateMapPhotoArguments : IArgumentsBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "resources.event.updateImage")]
        public string MapUrl { get; set; }

        #region Constructor
        public EventUpdateMapPhotoArguments(string mapUrl)
        {
            MapUrl = mapUrl;
        }
        #endregion Constructor
    }
}
