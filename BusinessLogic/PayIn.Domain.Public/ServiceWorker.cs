using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using System;

namespace PayIn.Domain.Public
{
	public class ServiceWorker : IEntity
	{
		                                      public int         Id    { get; set; }
		[Required(AllowEmptyStrings = false)] public string      Login { get; set; }
		[Required(AllowEmptyStrings = false)] public string      Name  { get; set; }
		                                      public WorkerState State { get; set; }

		#region Plannings
		[InverseProperty("Worker")]
		public ICollection<ControlPlanning> Plannings { get; set; }
		#endregion Plannings

		#region Presences

		#endregion Presences

		#region Supplier
		public int SupplierId { get; set; }
		public ServiceSupplier Supplier { get; set; }
		#endregion Supplier

		#region Tracks
		[InverseProperty("Worker")]
		public ICollection<ControlTrack> Tracks { get; set; }
		#endregion Tracks

		#region Constructors
		public ServiceWorker()
		{
			Plannings = new List<ControlPlanning>();
			Tracks = new List<ControlTrack>();
		}

        public static implicit operator string(ServiceWorker v)
        {
            throw new NotImplementedException();
        }
        #endregion Constructors
    }
}
