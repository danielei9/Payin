using PayIn.Domain.Payments;
using PayIn.Domain.Promotions;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using Xp.Infrastructure.Db;

namespace PayIn.Infrastructure.Public.Db
{
	public class PublicContext : SchemaContext<PublicContext>
	{
		// Public
		public DbSet<AccessControl> AccessControls { get; set; }
		public DbSet<AccessControlEntrance> AccessControlEntrances { get; set; }
		public DbSet<AccessControlEntry> AccessControlEntries { get; set; }
		public DbSet<AccessControlSentiloSensor> AccessControlSentiloSensors { get; set; }
		public DbSet<ControlIncident> ControlIncident { get; set; }
		public DbSet<ControlForm> ControlForm { get; set; }
		public DbSet<ControlFormArgument> ControlFormArgument { get; set; }
		public DbSet<ControlFormValue> ControlFormValue { get; set; }
		public DbSet<ControlFormAssign> ControlFormAssign { get; set; }
		public DbSet<ControlItem> ControlItem { get; set; }
		public DbSet<ControlPlanning> ControlPlanning { get; set; }
		public DbSet<ControlPlanningCheck> ControlPlanningCheck { get; set; }
		public DbSet<ControlPlanningItem> ControlPlanningItem { get; set; }
		public DbSet<ControlPresence> ControlPresence { get; set; }
		public DbSet<ControlTemplate> ControlTemplate { get; set; }
		public DbSet<ControlTemplateItem> ControlTemplateItem { get; set; }
		public DbSet<ControlTrack> ControlTrack { get; set; }
		public DbSet<Log> Log { get; set; }
		public DbSet<LogArgument> LogArgument { get; set; }
		public DbSet<ServiceAddress> ServiceAddress { get; set; }
		public DbSet<ServiceAddressName> ServiceAddressName { get; set; }
		public DbSet<ServiceCheckPoint> ServiceCheckPoint { get; set; }
		public DbSet<ServiceCity> ServiceCity { get; set; }
		public DbSet<ServiceConcession> ServiceConcession { get; set; }
		public DbSet<ServiceConfigurationData> ServiceConfigurationData { get; set; }
		public DbSet<ServiceCountry> ServiceCountry { get; set; }
		public DbSet<ServiceFreeDays> ServiceFreeDays { get; set; }
		public DbSet<ServiceNotification> ServiceNotification { get; set; }
        public DbSet<ServiceIncidence> ServiceIncidence { get; set; }
        public DbSet<ServiceNumberPlate> ServiceNumberPlate { get; set; }
		public DbSet<ServiceOption> ServiceOption { get; set; }
		public DbSet<ServicePrice> ServicePrice { get; set; }
		public DbSet<ServiceProvince> ServiceProvince { get; set; }
		public DbSet<ServiceSupplier> ServiceSupplier { get; set; }
		public DbSet<ServiceTag> ServiceTag { get; set; }
		public DbSet<ServiceTimeTable> ServiceTimeTable { get; set; }
		public DbSet<ServiceWorker> ServiceWorker { get; set; }
		public DbSet<ServiceZone> ServiceZone { get; set; }
		public DbSet<ServiceOperation> ServiceOperation { get; set; }
		// Payments
		public DbSet<Payment> Payment { get; set; }
		public DbSet<PaymentConcession> PaymentConcession { get; set; }
		public DbSet<PaymentMedia> PaymentMedia { get; set; }
		public DbSet<PaymentWorker> PaymentWorker { get; set; }
		public DbSet<Ticket> Ticket { get; set; }
		public DbSet<Purse> Purse { get; set; }
		public DbSet<PurseValue> PurseValue { get; set; }
		public DbSet<TransportConcession> TransportConcession { get; set; }
		//public DbSet<PaymentGateway>							PaymentGateway            { get; set; }
		//public DbSet<TicketDetail>							TicketDetail              { get; set; } 
		public DbSet<Platform> Platform { get; set; }
		public DbSet<Device> Device { get; set; }
		// Transport
		public DbSet<BlackList> BlackList { get; set; }
		public DbSet<TransportOperation> TransportOperation { get; set; }
		public DbSet<WhiteList> WhiteList { get; set; }
		public DbSet<GreyList> GreyList { get; set; }
		public DbSet<TransportTitle> TransportTitle { get; set; }
		public DbSet<TransportSystem> TransportSystem { get; set; }
		public DbSet<TransportPrice> TransportCardDamaged { get; set; }
		public DbSet<TransportCardSupport> TransportCardSupport { get; set; }
		public DbSet<TransportCardSupportTitleCompatibility> TransportCardSupportTitleCompatibility { get; set; }
		public DbSet<TransportSimultaneousTitleCompatibility> TransportSimultaneousTitleCompatibility { get; set; }
		public DbSet<TransportValidation> TransportValidation { get; set; }
		// Products / Eventos
		public DbSet<ProductFamily> ProductFamily { get; set; }
		public DbSet<Product> Product { get; set; }
		public DbSet<EntranceSystem> EntranceSystem { get; set; }
		public DbSet<Event> Event { get; set; }
		public DbSet<EntranceType> EntranceType { get; set; }
		public DbSet<EntranceTypeForm> EntranceTypeForm { get; set; }
        public DbSet<EntranceFormValue> EntranceFormValue { get; set; }
        // Promotions
        public DbSet<Promotion> Promotion { get; set; }
		public DbSet<PromoCondition> PromoCondition { get; set; }
		public DbSet<PromoAction> PromoAction { get; set; }
		public DbSet<PromoLauncher> PromoLauncher { get; set; }
		public DbSet<PromoExecution> PromoExecution { get; set; }
		public DbSet<PromoUser> PromoUser { get; set; }
		public DbSet<Profile> Profile { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<CampaignLine> CampaignLine { get; set; }
        public DbSet<CampaignCode> CampaignCode { get; set; }

        // T.C.
        public DbSet<SystemCard> SystemCard { get; set; }
		public DbSet<SystemCardMember> SystemCardMember { get; set; }
		public DbSet<ServiceUser> ServiceUser { get; set; }
		public DbSet<ServiceGroup> ServiceGroup { get; set; }
		public DbSet<ServiceCategory> ServiceCategory { get; set; }
		public DbSet<ServiceGroupProduct> ServiceGroupProduct { get; set; }
        public DbSet<ServiceGroupEntranceType> ServiceGroupEntranceType { get; set; }
        public DbSet<ServiceCard> ServiceCard { get; set; }
		public DbSet<ServiceCardBatch> ServiceCardBatch { get; set; }
		public DbSet<ServiceDocument> ServiceDocument { get; set; }

		#region Constructors
		public PublicContext()
			: base("PayInDb", "")
		{
		}
		#endregion Constructors

		#region OnModelCreating
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<ServiceAddressName>()
				.HasRequired(x => x.Address)
				.WithMany(x => x.Names)
				.HasForeignKey(x => x.AddressId)
				.WillCascadeOnDelete();
			//modelBuilder
			//	.Entity<TicketDetail>()
			//	.HasRequired(x => x.Ticket)
			//	.WithMany(x => x.Details)
			//	.HasForeignKey(x => x.TicketId)
			//	.WillCascadeOnDelete();
			modelBuilder
				.Entity<ControlPlanningItem>()
				.HasRequired(x => x.Planning)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.PlanningId)
				.WillCascadeOnDelete();
			modelBuilder
				.Entity<ControlPlanningCheck>()
				.HasRequired(x => x.Planning)
				.WithMany(x => x.Checks)
				.HasForeignKey(x => x.PlanningId)
				.WillCascadeOnDelete();
			modelBuilder
				.Entity<ControlFormAssign>()
				.HasRequired(x => x.Check)
				.WithMany(x => x.FormAssigns)
				.HasForeignKey(x => x.CheckId)
				.WillCascadeOnDelete();
			modelBuilder
				.Entity<ControlTrack>()
				.HasOptional(x => x.PresenceSince)
				.WithOptionalDependent(x => x.TrackSince)
				.Map(x => x.MapKey("PresenceSinceId"));
			modelBuilder
				.Entity<ControlTrack>()
				.HasOptional(x => x.PresenceUntil)
				.WithOptionalDependent(x => x.TrackUntil)
				.Map(x => x.MapKey("PresenceUntilId"));
			modelBuilder
				.Entity<ControlTemplateItem>()
				.HasRequired(x => x.Template)
				.WithMany(x => x.TemplateItems)
				.HasForeignKey(x => x.TemplateId)
				.WillCascadeOnDelete();
			modelBuilder
				.Entity<ControlTemplateCheck>()
				.HasRequired(x => x.Template)
				.WithMany(x => x.Checks)
				.HasForeignKey(x => x.TemplateId)
				.WillCascadeOnDelete();			
			modelBuilder
				.Entity<ControlFormAssignTemplate>()
				.HasRequired(x => x.Check)
				.WithMany(x => x.FormAssignTemplates)
				.HasForeignKey(x => x.CheckId)
				.WillCascadeOnDelete();
			modelBuilder
				.Entity<GreyList>()
				.Property(x => x.Uid)
				.HasColumnAnnotation(
					IndexAnnotation.AnnotationName,
					new IndexAnnotation(new IndexAttribute()));
			base.OnModelCreating(modelBuilder);
		}
        #endregion OnModelCreating
        
        #region BulkInsert
        public static void BulkInsert<T>(List<T> items)
            where T: class
        {
            PublicContext context = null;

            try
            {
                context = new PublicContext();
                context.Configuration.AutoDetectChangesEnabled = false;

                int count = 0;
                foreach (var entityToInsert in items)
                {
                    ++count;
                    context = AddToContext(context, entityToInsert, count, 1000, true);
                }

                context.SaveChanges();
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }
        }
        #endregion BulkInsert

        #region AddToContext
        private static PublicContext AddToContext<T>(PublicContext context, T entity, int count, int commitCount, bool recreateContext)
            where T: class
        {
            context.Set<T>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();

                if (recreateContext)
                {
                    context.Dispose();

                    context = new PublicContext();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }
        #endregion AddToContext
    }
}
