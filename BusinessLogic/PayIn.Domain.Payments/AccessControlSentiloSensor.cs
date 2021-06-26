using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class AccessControlSentiloSensor : Entity
	{
		[Required(AllowEmptyStrings = true)]	public string Temperature { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Humidity { get; set; }
		[Required(AllowEmptyStrings = true)]	public string WindSpeed { get; set; }
		[Required(AllowEmptyStrings = true)]	public string WindDirection { get; set; }
		[Required(AllowEmptyStrings = true)]	public string BarometricPressure { get; set; }
		[Required(AllowEmptyStrings = true)]	public string UVIndex { get; set; }
		[Required(AllowEmptyStrings = true)]	public string SolarRadiation { get; set; }

		#region AccessControl

		public int AccessControlId { get; set; }
        [ForeignKey("AccessControlId")]
        public AccessControl AccessControl { get; set; }

        #endregion
	}
}
