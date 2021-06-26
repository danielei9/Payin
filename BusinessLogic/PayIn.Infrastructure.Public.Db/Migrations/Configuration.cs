namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using PayIn.Domain.Public;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PayIn.Common;
    using PayIn.Domain.Payments;
    using Xp.Domain.Transport;
    using System.Data.Entity.Validation;
    using PayIn.Domain.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<PublicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PayIn.Infrastructure.Public.PublicContext";
        }

        protected override void Seed(PublicContext context)
        {
            try
            {
                #region ServiceSupplier

                if (!context.ServiceSupplier.Any(x => x.Login == "info@pay-in.es"))
                {
                    context.ServiceSupplier.AddOrUpdate(x => x.Name,
                        new ServiceSupplier
                        {
                            Login = "info@pay-in.es",
                            Name = "Pay[in]",
                            PaymentTest = false,
                            TaxName = "Payment Innovation Network, S.L.",
                            TaxNumber = "B-98.638.927",
                            TaxAddress = "C/ Pérez Galdós n.79 pta 16 46008 Valencia"
                        }
                    );
                    context.SaveChanges();
                }
                var payinSupplier = context.ServiceSupplier.First(x => x.Login == "info@pay-in.es");

                if (!context.ServiceConcession.Any(x => x.SupplierId == payinSupplier.Id && x.Type == ServiceType.Charge))
                {
                    context.ServiceConcession.AddOrUpdate(x => x.Name,
                        new ServiceConcession
                        {
                            Name = "Payment Pay[in]",
                            State = ConcessionState.Active,
                            SupplierId = payinSupplier.Id,
                            Type = ServiceType.Charge
                        }
                    );
                    context.SaveChanges();
                }
                var payinServiceConcession = context.ServiceConcession.First(x => x.SupplierId == payinSupplier.Id && x.Type == ServiceType.Charge);

                if (!context.PaymentConcession.Any(x => x.ConcessionId == payinServiceConcession.Id))
                {
                    context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
                        new PaymentConcession
                        {
                            BankAccountNumber = "BANK",
                            ConcessionId = payinServiceConcession.Id,
                            FormUrl = "",
                            Observations = "",
                            PayinCommision = 0,
                            Phone = "Phone",
                            LiquidationAmountMin = 100,
                            CreateConcessionDate = DateTime.Now,
                            Address = "Valor predefinido",
                            Vat = 0,
                            OnPaymentMediaCreatedUrl = "",
                            OnPaymentMediaErrorCreatedUrl = "",
                            //LiquidationConcession = payinConcession // Esta paymentConcession es Payin
                        }
                    );
                    context.SaveChanges();
                }
                var payinConcession = context.PaymentConcession.First(x => x.ConcessionId == payinServiceConcession.Id);
                payinConcession.LiquidationConcession = payinConcession;

                #endregion

                #region Control

                {
                    if (!context.ServiceSupplier.Any(x => x.Login == "commerce@pay-in.es"))
                    {
                        context.ServiceSupplier.AddOrUpdate(x => x.Name,
                            new PayIn.Domain.Public.ServiceSupplier
                            {
                                Name = "Logistica",
                                Login = "commerce@pay-in.es",
                                PaymentTest = true,
                                TaxName = "Logistica SL",
                                TaxNumber = "123456789X",
                                TaxAddress = "C/ Mayor,1 Vilamarxant"
                            }
                        );
                        context.SaveChanges();
                    }
                    var supplier = context.ServiceSupplier.First(x => x.Login == "commerce@pay-in.es");

                    if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Control Logistica"))
                    {
                        context.ServiceConcession.AddOrUpdate(x => x.Name,
                            new PayIn.Domain.Public.ServiceConcession
                            {
                                Name = "Control Logistica",
                                Type = ServiceType.Control,
                                SupplierId = supplier.Id
                            }
                        );
                        context.SaveChanges();
                    }
                    var concession = context.ServiceConcession.First(x => x.Name == "Control Logistica");

                    if (!context.ServiceWorker.Any(x => x.Login == "operator@pay-in.es"))
                    {
                        context.ServiceWorker.AddOrUpdate(x => x.Name,
                            new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Operador", Login = "operator@pay-in.es", SupplierId = supplier.Id }
                        );
                    }

                    if (!context.ServiceWorker.Any(x => x.Login == "user@pay-in.es"))
                    {
                        context.ServiceWorker.AddOrUpdate(x => x.Name,
                            new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Usuario", Login = "user@pay-in.es", SupplierId = supplier.Id }
                        );
                    }
#if TEST
				//if (!context.ServiceWorker.Any(x => x.Login == "info@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Xavi", Login = "info@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
				//if (!context.ServiceWorker.Any(x => x.Login == "info1@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Frank", Login = "info1@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
				//if (!context.ServiceWorker.Any(x => x.Login == "info2@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Toni", Login = "info2@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
				//if (!context.ServiceWorker.Any(x => x.Login == "info3@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Isra", Login = "info3@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
				//if (!context.ServiceWorker.Any(x => x.Login == "info4@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "info4", Login = "info4@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
				//if (!context.ServiceWorker.Any(x => x.Login == "info5@pay-in.es"))
				//{
				//	context.ServiceWorker.AddOrUpdate(x => x.Name,
				//		new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "Ruben", Login = "info5@pay-in.es", SupplierId = supplier.Id }
				//	);
				//}
#endif // TEST
                    if (!context.ServiceWorker.Any(x => x.Login == "tpv@pay-in.es"))
                    {
                        context.ServiceWorker.AddOrUpdate(x => x.Name,
                            new PayIn.Domain.Public.ServiceWorker { State = WorkerState.Active, Name = "TPV", Login = "tpv@pay-in.es", SupplierId = supplier.Id }
                        );
                    }
                }

                #endregion

                #region Payments

                {
                    CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Bar APTC",
                            Login = "payment@pay-in.es",
                            Phone = "600600600",
                            Address = "C/ San Blas, 1 Vilamarxant",
                            TaxName = "APTC SL",
                            TaxNumber = "123456789X",
                            TaxAddress = "C/ San Blas, 1 Vilamarxant",
                            LiquidationConcession = payinConcession
                        }
                    );
                }

                #endregion

                #region EntranceSystem

                context.EntranceSystem.AddOrUpdate(x => x.Name,
                    new EntranceSystem
                    {
                        IsDefault = true,
                        Name = "Pay[in]",
                        Type = EntranceSystemType.QR,
                        Template = "pay[in]/entrance:{{\"code\":{2}}}",
                        RegEx = "^pay\\[in\\]\\/entrance:{\"code\":(\\d*)}$",
                        RegExEventCode = null,
                        RegExEntranceCode = 1,
                        TemplateText = "{2}",
                        RegExText = "^(.*)$",
                        RegExEventCodeText = null,
                        RegExEntranceCodeText = 1
                    }
                );

                #endregion

                return;

#if DEBUG

                #region T.C. Vilamarxant

                {
                    // PaymentConcession
                    var vilamarxant = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Vilamarxant",
                            Login = "vilamarxant@pay-in.es",
                            Phone = "600600600",
                            Address = "Av. 2 de Maig - Vilamarxant",
                            TaxName = "M.I. Ajuntament Vilamarxant",
                            TaxNumber = "123456789X",
                            TaxAddress = "Av. 2 de Maig - Vilamarxant",
                            AllowUnsafePayments = false,
                            LiquidationConcession = payinConcession
                        }
                    );

                    // Profile
                    var profile = CreateProfile(context,
                        new CreateProfileDto
                        {
                            Name = "Vilamarxant",
                            Icon = "http://www.aude-events.com/web/wp-content/uploads/fv-logo-color-vertical12-300x158.jpg",
                            Color = "",
                            ImageUrl = "https://pbs.twimg.com/profile_images/844105605895917568/DNSu3GcK.jpg",
                            Layout = "VILAMARXANT,VILA PUNTS DE INTERES,VILA TRANSPORT,CALENDARIO;VILA HISTORIA,VILA ESPORTS,VILA SERVEIS,VILA INFORMACIO;VALIDATE",
                            LayoutPro = ""
                        }
                    );

                    // SystemCard
                    var tmVilamarxant = CreateSystemCard(context,
                        new CreateSystemCardDto
                        {
                            Name = "T.C. Vilamarxant",
                            CardType = CardType.MIFAREClassic,
                            ConcessionOwner = vilamarxant.Concession,
                            NumberGenerationType = NumberGenerationType.Autogeneration,
                            Profile = profile,
                            ClientId = AccountClientId.AndroidVilamarxantNative
                        }
                    );

                    var loteVilamarxant = CreateServiceCardBatch(context,
                        new CreateServiceCardBatchDto
                        {
                            Name = "Lote1 Vilamarxant",
                            SystemCard = tmVilamarxant
                        }
                    );

                    // ServiceUser
                    CreateServiceUser(context,
                        new CreateServiceUserDto
                        {
                            Uid = 9876543210,
                            Name = "Ciudadano",
                            LastName = "Vilamarxanter",
                            VatNumber = "12345678A",
                            Address = "C/ Fallero",
                            Email = "ciudadano@pay-in.es",
                            Login = "ciudadano@pay-in.es",
                            Phone = 12345678,
                            BirthDate = new DateTime(2000, 8, 11),
                            Observations = "Este es el monedero del ciudadano de pruebas",
                            SystemCard = tmVilamarxant,
                            ServiceCardBatch = loteVilamarxant,
                            Concession = vilamarxant.Concession
                        }
                    );
                }

				#endregion

				#region T.C. Vinaros

				{
					// PaymentConcession
					var vinaros = CreatePaymentConcession(context,
						new CreatePaymentConcessionDto
						{
							Name = "Vinaros",
							Login = "vinaros@pay-in.es",
							Phone = "600600600",
							Address = "Plaça Parroquial, 12 - Vinaros",
							TaxName = "M.I. Ajuntament Vinaros",
							TaxNumber = "123456789X",
							TaxAddress = "Plaça Parroquial, 12 - Vinaros",
							AllowUnsafePayments = false,
							LiquidationConcession = payinConcession
						}
					);

					//// Profile
					//var profile = CreateProfile(context,
					//	new CreateProfileDto
					//	{
					//		Name = "Vinaros",
					//		Icon = "http://www.aude-events.com/web/wp-content/uploads/fv-logo-color-vertical12-300x158.jpg",
					//		Color = "",
					//		ImageUrl = "https://pbs.twimg.com/profile_images/844105605895917568/DNSu3GcK.jpg",
					//		Layout = "VINAROS,VILA PUNTS DE INTERES,VILA TRANSPORT,CALENDARIO;VILA HISTORIA,VILA ESPORTS,VILA SERVEIS,VILA INFORMACIO;VALIDATE",
					//		LayoutPro = ""
					//	}
					//);

					//// SystemCard
					//var tmVinaros = CreateSystemCard(context,
					//	new CreateSystemCardDto
					//	{
					//		Name = "T.C. Vinaros",
					//		CardType = CardType.MIFAREClassic,
					//		ConcessionOwner = vinaros.Concession,
					//		NumberGenerationType = NumberGenerationType.Autogeneration,
					//		Profile = profile,
					//		ClientId = AccountClientId.AndroidVilamarxantNative
					//	}
					//);

					//var loteVinaros = CreateServiceCardBatch(context,
					//	new CreateServiceCardBatchDto
					//	{
					//		Name = "Lote1 Vinaros",
					//		SystemCard = tmVinaros
					//	}
					//);

					//// ServiceUser
					//CreateServiceUser(context,
					//	new CreateServiceUserDto
					//	{
					//		Uid = 9876543210,
					//		Name = "Ciudadano",
					//		LastName = "Vilamarxanter",
					//		VatNumber = "12345678A",
					//		Address = "C/ Fallero",
					//		Email = "ciudadano@pay-in.es",
					//		Login = "ciudadano@pay-in.es",
					//		Phone = 12345678,
					//		BirthDate = new DateTime(2000, 8, 11),
					//		Observations = "Este es el monedero del ciudadano de pruebas",
					//		SystemCard = tmVinaros,
					//		ServiceCardBatch = loteVinaros,
					//		Concession = vinaros.Concession
					//	}
					//);
				}

				#endregion

				#region T.M. Falla El Colp

				{
					// PaymentConcession
					var falla = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Falla El Colp",
                            Login = "falla@pay-in.es",
                            Phone = "600600600",
                            Address = "C/ Sant Miquel, 40 - Vilamarxant",
                            TaxName = "Falla Sant Miquel - Gómez Arnés",
                            TaxNumber = "123456789X",
                            TaxAddress = "C/ Sant Miquel, 40 - Vilamarxant",
                            AllowUnsafePayments = false,
                            LiquidationConcession = payinConcession
                        }
                    );

                    // Profile
                    var profile = CreateProfile(context,
                        new CreateProfileDto
                        {
                            Name = "Falla El Colp",
                            Icon = "http://www.aude-events.com/web/wp-content/uploads/fv-logo-color-vertical12-300x158.jpg",
                            Color = "",
                            ImageUrl = "https://pbs.twimg.com/profile_images/844105605895917568/DNSu3GcK.jpg",
                            Layout = "TODAS LAS ENTRADAS,RECARGAR ONLINE USUARIO",
                            LayoutPro = "RECARGAR,TPV,TICKETS,PULSERAS,VALIDAR OFFLINE;EMITIR,DEVOLVER,COMPRAR ENTRADAS,CAJA;"
                        }
                    );

                    // SystemCard
                    var tmFalla = CreateSystemCard(context,
                        new CreateSystemCardDto
                        {
                            Name = "T.M. Falla El Colp",
                            CardType = CardType.MIFAREClassic,
                            ConcessionOwner = falla.Concession,
                            NumberGenerationType = NumberGenerationType.Autogeneration,
                            Profile = profile,
                            ClientId = AccountClientId.AndroidFallesNative
                        }
                    );

                    var loteFalla = CreateServiceCardBatch(context,
                        new CreateServiceCardBatchDto
                        {
                            Name = "Lote1 Falla El Colp",
                            SystemCard = tmFalla
                        }
                    );

                    // ServiceUser
                    CreateServiceUser(context,
                        new CreateServiceUserDto
                        {
                            Uid = 9876543210,
                            Name = "Fallero",
                            LastName = "Valenciano",
                            VatNumber = "12345678A",
                            Address = "C/ Fallero",
                            Email = "fallero@pay-in.es",
                            Login = "fallero@pay-in.es",
                            Phone = 12345678,
                            BirthDate = new DateTime(2000, 8, 11),
                            Observations = "Este es el monedero del fallero de pruebas",
                            SystemCard = tmFalla,
                            ServiceCardBatch = loteFalla,
                            Concession = falla.Concession
                        }
                    );
                }

                #endregion

                #region T.C. Mercado

                {
                    // PaymentConcession
                    var mercado = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Mercado",
                            Login = "mercado@pay-in.es",
                            Phone = "600600600",
                            Address = "Dirección comercial",
                            TaxName = "Mercado SL",
                            TaxNumber = "123456789X",
                            TaxAddress = "Dirección fiscal",
                            AllowUnsafePayments = false,
                            LiquidationConcession = null
                        }
                    );

                    // Profile
                    var profile = CreateProfile(context,
                        new CreateProfileDto
                        {
                            Name = "Mercado",
                            Icon = "https://www.ayuntamiento.es/imagenes/ayuntamientos/2261/d/finestrat.png",
                            Color = "",
                            ImageUrl = "https://static1.squarespace.com/static/58ec23b0f5e2312e1cc2ecad/t/59038e0ea5790a15f0dc12f2/1493405206144/ProcurementFairBackgroundBlue.png?format=1500w",
                            Layout = "",
                            LayoutPro = "RECARGAR,TPV,TICKETS,PULSERAS,VALIDAR OFFLINE;EMITIR,DEVOLVER,COMPRAR ENTRADAS,CAJA"
                        }
                    );

                    // SystemCard
                    var tmMercado = CreateSystemCard(context,
                        new CreateSystemCardDto
                        {
                            Name = "T. Mercat",
                            CardType = CardType.MIFAREClassic,
                            ConcessionOwner = mercado.Concession,
                            NumberGenerationType = NumberGenerationType.Autogeneration,
                            Profile = profile,
                            ClientId = ""
                        }
                    );

                    var loteMercado = CreateServiceCardBatch(context,
                        new CreateServiceCardBatchDto
                        {
                            Name = "Lote1 Mercado",
                            SystemCard = tmMercado
                        }
                    );


                    // PaymentConcession
                    var parada = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Parada mercado",
                            Login = "parada@pay-in.es",
                            Phone = "600600600",
                            Address = "Mercado comercial",
                            TaxName = "Parada SL",
                            TaxNumber = "123456789X",
                            TaxAddress = "Mercado fiscal",
                            AllowUnsafePayments = false,
                            LiquidationConcession = mercado
                        }
                    );

                    CreateServiceCardMember(context,
                        new CreateServiceCardMemberDto
                        {
                            Name = "Parada",
                            Login = parada.Concession.Supplier.Login,
                            CanEmit = false,
                            SystemCard = tmMercado
                        }
                    );
                }

                #endregion

                #region T.E. Manga

                // Profile
                {
                    var profile = CreateProfile(context,
                        new CreateProfileDto
                        {
                            Name = "Salón del Manga",
                            Icon = "http://www.rochesterareabuilders.com/homeshow/images/icon-exhibitors.png",
                            Color = "rgba(0, 0, 0, 0.7)",
                            ImageUrl = "https://static1.squarespace.com/static/58ec23b0f5e2312e1cc2ecad/t/59038e0ea5790a15f0dc12f2/1493405206144/ProcurementFairBackgroundBlue.png?format=1500w",
                            Layout = "MI EVENTO,MIS ENTRADAS;ESCANEAR EXPOSITOR,EXPOSITORES;MIS VISITANTES,VALIDAR,TPV,RECARGAR,PEDIDOS",
                            LayoutPro = "MI EVENTO,MIS ENTRADAS;ESCANEAR EXPOSITOR,EXPOSITORES;MIS VISITANTES,VALIDAR,TPV,RECARGAR,PEDIDOS"
                        }
                    );
                }

                #endregion

                #region T.F. Club

                {
                    // PaymentConcession
                    var club = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Club",
                            Login = "club@pay-in.es",
                            Phone = "600600600",
                            Address = "C/ Discozone Vilamarxant",
                            TaxName = "Club S.L.",
                            TaxNumber = "123456789X",
                            TaxAddress = "C/ Discozone Vilamarxant",
                            AllowUnsafePayments = true,
                            LiquidationConcession = payinConcession
                        }
                    );

                    // PaymentConcession
                    var clubManager = CreatePaymentConcession(context,
                        new CreatePaymentConcessionDto
                        {
                            Name = "Club manager",
                            Login = "clubmanager@pay-in.es",
                            Phone = "600600600",
                            Address = "C/ Discozone Vilamarxant",
                            TaxName = "Club S.L.",
                            TaxNumber = "123456789X",
                            TaxAddress = "C/ Discozone Vilamarxant",
                            AllowUnsafePayments = true,
                            LiquidationConcession = payinConcession
                        }
                    );

                    // PaymentWorker
                    CreatePaymentWorker(context,
                        new CreatePaymentWorkerDto
                        {
                            Name = "Club manager",
                            Login = "clubmanager@pay-in.es",
                            PaymentConcession = club
                        }
                    );

                    // Profile
                    //var profile = CreateProfile(context,
                    //	new CreateProfileDto
                    //	{
                    //		Name = "Falla El Colp",
                    //		Color = "rgba(23, 110, 200, 0.7)",
                    //		Icon = "http://www.aude-events.com/web/wp-content/uploads/fv-logo-color-vertical12-300x158.jpg",
                    //		ImageUrl = "https://pbs.twimg.com/profile_images/844105605895917568/DNSu3GcK.jpg",
                    //		Layout = "MI FALLA,MIS MONEDEROS;MIS ENTRADAS,MIS PEDIDOS;VALIDAR,TPV,RECARGAR,PEDIDOS"
                    //	}
                    //);

                    // SystemCard
                    var tfClub = CreateSystemCard(context,
                        new CreateSystemCardDto
                        {
                            Name = "T.F. Club",
                            CardType = CardType.MIFAREClassic,
                            ConcessionOwner = club.Concession,
                            NumberGenerationType = NumberGenerationType.Autogeneration,
                            Profile = null //profile
                        }
                    );

                    var loteClub = CreateServiceCardBatch(context,
                        new CreateServiceCardBatchDto
                        {
                            Name = "Lote1 Club",
                            SystemCard = tfClub
                        }
                    );

                    // Create member
                    CreateServiceCardMember(context, new CreateServiceCardMemberDto
                    {
                        CanEmit = true,
                        Name = "Club manager",
                        Login = "clubmanager@pay-in.es",
                        SystemCard = tfClub
                    });

                    // Create service user
                    CreateServiceUser(context, new CreateServiceUserDto
                    {
                        SystemCard = tfClub,
                        ServiceCardBatch = loteClub,
                        Concession = club.Concession,
                        Login = "festero@pay-in.es"
                    });
                }

                #endregion

#endif // DEBUG ESCANEAR EXPOSITOR

                #region Transport

                /*{
				#region ServiceSupplier
								if (!context.ServiceSupplier.Any(x => x.Login == "transport@pay-in.es"))
								{
									context.ServiceSupplier.AddOrUpdate(x => x.Name,
											new ServiceSupplier { Name = "Transportes Valencia", Login = "transport@pay-in.es", PaymentTest = true, TaxName = "TRANSPORTES SL", TaxNumber = "123456789X", TaxAddress = "C/ San Blas, 1 Vilamarxant" }
									);
									context.SaveChanges();
								}
								var supplier = context.ServiceSupplier.First(x => x.Login == "transport@pay-in.es");
				#endregion ServiceSupplier

				#region Transport Systems
								if (!context.TransportSystem.Any(x => x.Name == "Valencia Classic"))
								{
									context.TransportSystem.AddOrUpdate(x => x.Name,
										new TransportSystem { Name = "Valencia Classic", cardType = CardType.MIFAREClassic }
									);
								}
								if (!context.TransportSystem.Any(x => x.Name == "Vigo Desfire"))
								{
									context.TransportSystem.AddOrUpdate(x => x.Name,
										new TransportSystem { Name = "Vigo Desfire", cardType = CardType.MIFAREDesfire }
									);
									context.SaveChanges();
								}
								var transportSystemClassic = context.TransportSystem.First(x => x.Name == "Valencia Classic");
								var transportSystemDesfire = context.TransportSystem.First(x => x.Name == "Vigo Desfire");
								context.SaveChanges();
				#endregion Transport Systems

				#region aVM concessions

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "aVM Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "aVM Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionP = context.ServiceConcession.First(x => x.Name == "aVM Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcession = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "aVM"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "aVM", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionT = context.ServiceConcession.First(x => x.Name == "aVM");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcession.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();

								}

								var transportConcessionAVM = context.TransportConcession.First(x => x.ConcessionId == paymentConcession.Id && x.TransportType == TransportType.aVM);
				#endregion aVM concessions

				#region aVM TransportSimultaneousTitleCompatibility

				#endregion aVM TransportSimultaneousTitleCompatibility

				#region TransportCardSupport

								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Personalizada Todos"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Personalizada Todos",
											OwnerCode = 1,
											OwnerName = "CITMA",
											Type = 2,
											SubType = null //Todos
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Personalizada Jove 5"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Personalizada Jove 5",
											OwnerCode = 1,
											OwnerName = "CITMA",
											Type = 2,
											SubType = 5
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Personalizada Jove 6"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Personalizada Jove 6",
											OwnerCode = 1,
											OwnerName = "CITMA",
											Type = 2,
											SubType = 6
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima",
											OwnerCode = 1,
											OwnerName = "CITMA",
											Type = 1,
											SubType = 1
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima/Reloj Mobilis"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima/Reloj Mobilis",
											OwnerCode = 1,
											OwnerName = "CITMA",
											Type = 1,
											SubType = 3
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima Inicial"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima Inicial",
											OwnerCode = 2,
											OwnerName = "FGV",
											Type = 1,
											SubType = 1
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima FGV"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima FGV",
											OwnerCode = 2,
											OwnerName = "FGV",
											Type = 1,
											SubType = 3
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Cartón"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Cartón",
											OwnerCode = 2,
											OwnerName = "FGV",
											Type = 1,
											SubType = null //Todos
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima EMT"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima EMT",
											OwnerCode = 3,
											OwnerName = "EMT",
											Type = 1,
											SubType = 1
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima Normalizada"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima Normalizada",
											OwnerCode = 3,
											OwnerName = "EMT",
											Type = 1,
											SubType = 3
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Personalizada YUM"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Personalizada YUM",
											OwnerCode = 23,
											OwnerName = "YUM",
											Type = 2,
											SubType = null //Todos
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Anónima Normalizada Bankia"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Anónima Normalizada Bankia",
											OwnerCode = 24,
											OwnerName = "Bankia",
											Type = 1,
											SubType = 3
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Valencia Card"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Valencia Card",
											OwnerCode = 31,
											OwnerName = "Turismo Valencia",
											Type = 8,
											SubType = 1
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 0"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 0",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 0
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 1"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 1",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 1
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 4"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 4",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 4
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 5"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 5",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 5
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 6"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 6",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 6
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 7"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 7",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 7
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 9"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 9",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 9
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 10"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 10",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 10
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 11"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 11",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 11
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 12"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 12",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 12
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Personalizada 13"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Personalizada 13",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 13
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Anónima 2"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Anónima 2",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 2
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Anónima 3"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Anónima 3",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 3
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Anónima 8"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Anónima 8",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 8
										}
									);
								}
								if (!context.TransportCardSupport.Any(x => x.Name == "Tarjeta Ciudadana Anónima 14"))
								{
									context.TransportCardSupport.AddOrUpdate(x => x.Name,
										new TransportCardSupport
										{
											Name = "Tarjeta Ciudadana Anónima 14",
											OwnerCode = 28,
											OwnerName = "",
											Type = 15,
											SubType = 14
										}
									);
								}
								context.SaveChanges();
								var TransportCardSupportTarjetaPersonalizadaTodos = context.TransportCardSupport.First(x => x.Name == "Tarjeta Personalizada Todos");
								var TransportCardSupportTarjetaPersonalizadaJove5 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Personalizada Jove 5");
								var TransportCardSupportTarjetaPersonalizadaJove6 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Personalizada Jove 6");
								var TransportCardSupportTarjetaAnónima = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima");
								var TransportCardSupportTarjetaAnónimaRelojMobilis = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima/Reloj Mobilis");
								var TransportCardSupportTarjetaAnónimaInicial = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima Inicial");
								var TransportCardSupportTarjetaAnónimaFGV = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima FGV");
								var TransportCardSupportTarjetaCartón = context.TransportCardSupport.First(x => x.Name == "Tarjeta Cartón");
								var TransportCardSupportTarjetaAnónimaEMT = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima EMT");
								var TransportCardSupportTarjetaAnónimaNormalizada = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima Normalizada");
								var TransportCardSupportTarjetaPersonalizadaYUM = context.TransportCardSupport.First(x => x.Name == "Tarjeta Personalizada YUM");
								var TransportCardSupportTarjetaAnónimaNormalizadaBankia = context.TransportCardSupport.First(x => x.Name == "Tarjeta Anónima Normalizada Bankia");
								var TransportCardSupportTarjetaValenciaCard = context.TransportCardSupport.First(x => x.Name == "Tarjeta Valencia Card");
								var TransportCardSupportTarjetaCiudadanaPersonalizada0 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 0");
								var TransportCardSupportTarjetaCiudadanaPersonalizada1 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 1");
								var TransportCardSupportTarjetaCiudadanaPersonalizada4 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 4");
								var TransportCardSupportTarjetaCiudadanaPersonalizada5 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 5");
								var TransportCardSupportTarjetaCiudadanaPersonalizada6 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 6");
								var TransportCardSupportTarjetaCiudadanaPersonalizada7 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 7");
								var TransportCardSupportTarjetaCiudadanaPersonalizada9 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 9");
								var TransportCardSupportTarjetaCiudadanaPersonalizada10 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 10");
								var TransportCardSupportTarjetaCiudadanaPersonalizada11 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 11");
								var TransportCardSupportTarjetaCiudadanaPersonalizada12 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 12");
								var TransportCardSupportTarjetaCiudadanaPersonalizada13 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Personalizada 13");
								var TransportCardSupportTarjetaCiudadanaAnónima2 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Anónima 2");
								var TransportCardSupportTarjetaCiudadanaAnónima3 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Anónima 3");
								var TransportCardSupportTarjetaCiudadanaAnónima8 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Anónima 8");
								var TransportCardSupportTarjetaCiudadanaAnónima14 = context.TransportCardSupport.First(x => x.Name == "Tarjeta Ciudadana Anónima 14");
								context.SaveChanges();
				#endregion TransportCardSupport

				#region aVM TransportCardTitleCompatibility
								////Abono Transporte Zona A 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteA.Id }
								//	);
								//}
								////Abono Transporte Zona B
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteB.Id }
								//	);
								//}
								////Abono transporte Zona C
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteC.Id }
								//	);
								//}
								////Abono transporte zona D
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteD.Id }
								//	);
								//}
								////Abono transporte Zona AB
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteAB.Id }
								//	);
								//}
								////Abono transporte BC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteBC.Id }
								//	);
								//}
								////Abono transporte CD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteCD.Id }
								//	);
								//}
								////Abono transporte Zona ABC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteABC.Id }
								//	);
								//}
								////Abono transporte Zona BCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteBCD.Id }
								//	);
								//}
								////Abono transporte Zona ABCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporteABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporteABCD.Id }
								//	);
								//}
								////Bono Transbordo 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
								//	);
								//}
								////T1,T2 y T3
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT1.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT1.Id }
								//	);
								//}
								////T2
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT2.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT2.Id }
								//	);
								//}
								////T3
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT3.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT3.Id }
								//	);
								//}
								////Tarjeta Valencia
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportTitleValenciaTourist24.Id && x.TransportTitleId == transportCardTarjetaValenciaCard.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportTitleValenciaTourist24.Id, TransportTitleId = transportCardTarjetaValenciaCard.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportTitleValenciaTourist72.Id && x.TransportTitleId == transportCardTarjetaValenciaCard.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportTitleValenciaTourist72.Id, TransportTitleId = transportCardTarjetaValenciaCard.Id }
								//	);
								//}
								////Bono 10 Metrorbital
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
								//	);
								//}
								////Valencia MasCamarena Bono10 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBono10.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaMasCamarenaBono10.Id }
								//	);
								//}
								////Valencia MasCamarena BonoTransbordo 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaMasCamarenaBonoTransbordo.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaMasCamarenaBonoTransbordo.Id }
								//	);
								//}
								////ValenciaaRibaroja Bono 10viatges B 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesB.Id }
								//	);
								//}
								////Valencia a Ribaroja Bono 10viatges BC 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10viatgesBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10viatgesBC.Id }
								//	);
								//}
								//// Alcasser Silla Bono10 viatges 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
								//	);
								//}
								//// Bono Mislata 
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBonoMislata.Id }
								//	);
								//}
								//// Bono Marxant xpres
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoMarxantxpres.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoMarxantxpres.Id }
								//	);
								//}
				#endregion aVM TransportCardTitleCompatibility

				#region FGV concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "FGV Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "FGV Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionFGVP = context.ServiceConcession.First(x => x.Name == "FGV Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionFGVP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionFGVP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionFGV = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionFGVP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "FGV"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "FGV", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionFGVT = context.ServiceConcession.First(x => x.Name == "FGV");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionFGVT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionFGV.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.FGV, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionFGV = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionFGV.Id);
								context.SaveChanges();

				#endregion FGV concessions

				#region FGV TransportCardTitleCompatibility
								////Bono 60x60 A
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60A.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60A.Id }
								//	);
								//}
								////Bono 60x60 B
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60B.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60B.Id }
								//	);
								//}
								////Bono 60x60 C
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60C.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60C.Id }
								//	);
								//}
								////Bono 60x60 D
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60D.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60D.Id }
								//	);
								//}
								////Bono 60x60 AB
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60AB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60AB.Id }
								//	);
								//}
								////Bono 60x60 BC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60BC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60BC.Id }
								//	);
								//}
								////Bono 60x60 CD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60CD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60CD.Id }
								//	);
								//}
								////Bono 60x60 ABC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60ABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60ABC.Id }
								//	);
								//}
								////Bono 60x60 BCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60BCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60BCD.Id }
								//	);
								//}
								////Bono 60x60 ABCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono60x60ABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono60x60ABCD.Id }
								//	);
								//}
								////BonoMetro A
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroA.Id }
								//	);
								//}
								////BonoMetro B
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroB.Id }
								//	);
								//}
								////BonoMetro C
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroC.Id }
								//	);
								//}
								////BonoMetro D
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroD.Id }
								//	);
								//}
								////BonoMetro AB
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroAB.Id }
								//	);
								//}
								////BonoMetro BC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroBC.Id }
								//	);
								//}
								////BonoMetro CD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroCD.Id }
								//	);
								//}
								////BonoMetro ABC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroABC.Id }
								//	);
								//}
								////BonoMetro BCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroBCD.Id }
								//	);
								//}
								////BonoMetro ABCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMetroABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMetroABCD.Id }
								//	);
								//}
								////Sencillo A
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloA.Id }
								//	);
								//}
								////Sencillo B
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloB.Id }
								//	);
								//}
								////Sencillo C
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloC.Id }
								//	);
								//}
								////Sencillo D
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloD.Id }
								//	);
								//}
								////Sencillo AB
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloAB.Id }
								//	);
								//}
								////Sencillo BC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloBC.Id }
								//	);
								//}
								////Sencillo CD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloCD.Id }
								//	);
								//}
								////Sencillo ABC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloABC.Id }
								//	);
								//}
								////Sencillo BCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloBCD.Id }
								//	);
								//}
								////Sencillo ABCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleSencilloABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleSencilloABCD.Id }
								//	);
								//}
								////IdayVuelta A
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaA.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaA.Id }
								//	);
								//}
								////IdayVuelta B
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaB.Id }
								//	);
								//}
								////IdayVuelta C
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaC.Id }
								//	);
								//}
								////IdayVuelta D
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaD.Id }
								//	);
								//}
								////IdayVuelta AB
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaAB.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaAB.Id }
								//	);
								//}
								////IdayVuelta BC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaBC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaBC.Id }
								//	);
								//}
								////IdayVuelta CD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaCD.Id }
								//	);
								//}
								////IdayVuelta ABC
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaABC.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaABC.Id }
								//	);
								//}
								////IdayVuelta BCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaBCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaBCD.Id }
								//	);
								//}
								////IdayVuelta ABCD
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónima.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónima.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaInicial.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaInicial.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaCartón.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaCartón.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
								//if (!context.TransportCardTitleCompatibility.Any(x => x.TransportCardId == transportCardTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleIdayVueltaABCD.Id))
								//{
								//	context.TransportCardTitleCompatibility.AddOrUpdate(x => x.TransportCard,
								//		new TransportCardTitleCompatibility { TransportCardId = transportCardTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleIdayVueltaABCD.Id }
								//	);
								//}
				#endregion FGV TransportCardTitleCompatibility

				#region Alboraya concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Alboraya Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Alboraya Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoAlborayaP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Alboraya Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoAlborayaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoAlborayaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoAlboraya = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoAlborayaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Alboraya"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Alboraya", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoAlborayaT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Alboraya");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoAlborayaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoAlboraya.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoAlboraya = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoAlboraya.Id);
				#endregion Alboraya concessions

				#region Paterna concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Paterna Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Paterna Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoPaternaP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Paterna Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoPaternaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoPaternaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoPaterna = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoPaternaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Paterna"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Paterna", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoPaternaT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Paterna");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoPaternaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoPaterna.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoPaterna = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoPaterna.Id);
				#endregion Paterna concessions

				#region Castellon concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Castellon Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Castellon Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoCastellonP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Castellon Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoCastellonP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoCastellonP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoCastellon = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoCastellonP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Castellon"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Castellon", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoCastellonT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Castellon");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoCastellonT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoCastellon.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoCastellon = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoCastellon.Id);
				#endregion Castellon concessions

				#region AVSA concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "AVSA Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "AVSA Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAVSAP = context.ServiceConcession.First(x => x.Name == "AVSA Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAVSAP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAVSAP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAVSA = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAVSAP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "AVSA"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "AVSA", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAVSAT = context.ServiceConcession.First(x => x.Name == "AVSA");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAVSAT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAVSA.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAVSA = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAVSA.Id);
				#endregion AVSA concessions

				#region TUASA concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TUASA Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "TUASA Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTUASAP = context.ServiceConcession.First(x => x.Name == "TUASA Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionTUASAP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionTUASAP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionTUASA = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionTUASAP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TUASA"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "TUASA", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTUASAT = context.ServiceConcession.First(x => x.Name == "TUASA");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionTUASAT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionTUASA.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionTUASA = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionTUASA.Id);
				#endregion TUASA concessions

				#region TAM concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TAM Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "TAM Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTAMP = context.ServiceConcession.First(x => x.Name == "TAM Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionTAMP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionTAMP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionTAM = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionTAMP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TAM"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "TAM", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTAMT = context.ServiceConcession.First(x => x.Name == "TAM");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionTAMT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionTAM.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionTAM = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionTAM.Id);
				#endregion TAM concessions

				#region TRAM concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TRAM Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "TRAM Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTRAMP = context.ServiceConcession.First(x => x.Name == "TRAM Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionTRAMP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionTRAMP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionTRAM = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionTRAMP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "TRAM"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "TRAM", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTRAMT = context.ServiceConcession.First(x => x.Name == "TRAM");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionTRAMT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionTRAM.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionTRAM = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionTRAM.Id);
				#endregion TRAM concessions

				#region Masatusa concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Masatusa Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Masatusa Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMasatusaP = context.ServiceConcession.First(x => x.Name == "Masatusa Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionMasatusaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionMasatusaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionMasatusa = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionMasatusaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Masatusa"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Masatusa", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMasatusaT = context.ServiceConcession.First(x => x.Name == "Masatusa");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionMasatusaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionMasatusa.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionMasatusa = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionMasatusa.Id);
				#endregion Masatusa concessions

				#region Los Serranos concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Los Serranos Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Los Serranos Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionLosSerranosP = context.ServiceConcession.First(x => x.Name == "Los Serranos Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionLosSerranosP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionLosSerranosP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionLosSerranos = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionLosSerranosP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Los Serranos"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Los Serranos", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionLosSerranosT = context.ServiceConcession.First(x => x.Name == "Los Serranos");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionLosSerranosT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionLosSerranos.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionLosSerranos = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionLosSerranos.Id);
				#endregion Los Serranos concessions

				#region Teulada concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Teulada Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Teulada Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoTeuladaP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Teulada Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoTeuladaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoTeuladaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoTeulada = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoTeuladaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Teulada"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Teulada", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoTeuladaT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Teulada");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoTeuladaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoTeulada.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoTeulada = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoTeulada.Id);
				#endregion Teulada concessions

				#region LlorenteBus concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "LlorenteBus Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "LlorenteBus Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionLlorenteBusP = context.ServiceConcession.First(x => x.Name == "LlorenteBus Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionLlorenteBusP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionLlorenteBusP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionLlorenteBus = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionLlorenteBusP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "LlorenteBus"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "LlorenteBus", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionLlorenteBusT = context.ServiceConcession.First(x => x.Name == "LlorenteBus");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionLlorenteBusT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionLlorenteBus.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionLlorenteBus = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionLlorenteBus.Id);
				#endregion LlorenteBus concessions

				#region Mancomunitat L’Horta Sud concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Mancomunitat L’Horta Sud Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Mancomunitat L’Horta Sud Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMancomunitatLHortaSudP = context.ServiceConcession.First(x => x.Name == "Mancomunitat L’Horta Sud Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionMancomunitatLHortaSudP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionMancomunitatLHortaSudP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionMancomunitatLHortaSud = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionMancomunitatLHortaSudP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Mancomunitat L’Horta Sud"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Mancomunitat L’Horta Sud", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMancomunitatLHortaSudT = context.ServiceConcession.First(x => x.Name == "Mancomunitat L’Horta Sud");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionMancomunitatLHortaSudT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionMancomunitatLHortaSud.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionMancomunitatLHortaSud = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionMancomunitatLHortaSud.Id);
				#endregion Mancomunitat L’Horta Sud concessions

				#region Valle de Cárcer concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Valle de Cárcer Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Valle de Cárcer Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionValledeCárcerP = context.ServiceConcession.First(x => x.Name == "Valle de Cárcer Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionValledeCárcerP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionValledeCárcerP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionValledeCárcer = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionValledeCárcerP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Valle de Cárcer"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Valle de Cárcer", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionValledeCárcerT = context.ServiceConcession.First(x => x.Name == "Valle de Cárcer");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionValledeCárcerT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionValledeCárcer.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionValledeCárcer = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionValledeCárcer.Id);
				#endregion Valle de Cárcer concessions

				#region Godella concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Godella Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Godella Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionGodellaP = context.ServiceConcession.First(x => x.Name == "Godella Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionGodellaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionGodellaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionGodella = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionGodellaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Godella"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Godella", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionGodellaT = context.ServiceConcession.First(x => x.Name == "Godella");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionGodellaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionGodella.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionGodella = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionGodella.Id);
				#endregion Godella concessions

				#region RENFE concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "RENFE Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "RENFE Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionRENFEP = context.ServiceConcession.First(x => x.Name == "RENFE Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionRENFEP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionRENFEP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionRENFE = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionRENFEP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "RENFE"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "RENFE", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionRENFET = context.ServiceConcession.First(x => x.Name == "RENFE");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionRENFET.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionRENFE.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionRENFE = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionRENFE.Id);
				#endregion RENFE concessions

				#region Metrobus concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Metrobus Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Metrobus Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMetrobusP = context.ServiceConcession.First(x => x.Name == "Metrobus Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionMetrobusP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionMetrobusP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionMetrobus = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionMetrobusP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Metrobus"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Metrobus", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionMetrobusT = context.ServiceConcession.First(x => x.Name == "Metrobus");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionMetrobusT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionMetrobus.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionMetrobus = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionMetrobus.Id);
				#endregion Metrobus concessions

				#region Torrent concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Torrent Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Torrent Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTorrentP = context.ServiceConcession.First(x => x.Name == "Torrent Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionTorrentP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionTorrentP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionTorrent = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionTorrentP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Torrent"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Torrent", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionTorrentT = context.ServiceConcession.First(x => x.Name == "Torrent");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionTorrentT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionTorrent.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionTorrent = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionTorrent.Id);
				#endregion Torrent concessions

				#region Autocares Baile concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Autocares Baile Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Autocares Baile Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAutocaresBaileP = context.ServiceConcession.First(x => x.Name == "Autocares Baile Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAutocaresBaileP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAutocaresBaileP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAutocaresBaile = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAutocaresBaileP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Autocares Baile"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Autocares Baile", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAutocaresBaileT = context.ServiceConcession.First(x => x.Name == "Autocares Baile");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAutocaresBaileT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAutocaresBaile.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAutocaresBaile = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAutocaresBaile.Id);
				#endregion Autocares Baile concessions

				#region Buñol concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Buñol Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Buñol Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoBuñolP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Buñol Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoBuñolP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoBuñolP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoBuñol = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoBuñolP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Buñol"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Buñol", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoBuñolT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Buñol");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoBuñolT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoBuñol.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoBuñol = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoBuñol.Id);
				#endregion Buñol concessions

				#region Aldaia concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Aldaia Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
									new ServiceConcession { Name = "Ayuntamiento Aldaia Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoAldaiaP = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Aldaia Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoAldaiaP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionAyuntamientoAldaiaP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionAyuntamientoAldaia = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionAyuntamientoAldaiaP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "Ayuntamiento Aldaia"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "Ayuntamiento Aldaia", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionAyuntamientoAldaiaT = context.ServiceConcession.First(x => x.Name == "Ayuntamiento Aldaia");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionAyuntamientoAldaiaT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionAyuntamientoAldaia.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.aVM, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionAyuntamientoAldaia = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionAyuntamientoAldaia.Id);
				#endregion Aldaia concessions

				#region EMT concessions
								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "EMT Tesoreria"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "EMT Tesoreria", Type = ServiceType.Charge, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionEMTP = context.ServiceConcession.First(x => x.Name == "EMT Tesoreria");

								if (!context.PaymentConcession.Any(x => x.ConcessionId == serviceConcessionEMTP.Id))
								{
									context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
										new PaymentConcession { ConcessionId = serviceConcessionEMTP.Id, Phone = "123456789", FormUrl = "", LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), LiquidationAmountMin = 100m, Address = "C/Lanzadera", CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local), BankAccountNumber = "ES0000000000000000000000", }
									);
									context.SaveChanges();
								}
								var paymentConcessionEMT = context.PaymentConcession.First(x => x.ConcessionId == serviceConcessionEMTP.Id);

								if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == "EMT"))
								{
									context.ServiceConcession.AddOrUpdate(x => x.Name,
										new ServiceConcession { Name = "EMT", Type = ServiceType.Transport, SupplierId = supplier.Id, PhotoUrl="" }
									);
									context.SaveChanges();
								}
								var serviceConcessionEMTT = context.ServiceConcession.First(x => x.Name == "EMT");

								if (!context.TransportConcession.Any(x => x.ConcessionId == serviceConcessionEMTT.Id))
								{
									context.TransportConcession.AddOrUpdate(x => x.ConcessionId,
										new TransportConcession { ConcessionId = paymentConcessionEMT.Id, CardType = TransportCardType.Mobilis, TransportType = TransportType.EMT, UrlServer = "www.google.es", TransportSystemId = transportSystemClassic.Id }
								   );
									context.SaveChanges();
								}
								var transportConcessionEMT = context.TransportConcession.First(x => x.ConcessionId == paymentConcessionEMT.Id);
				#endregion EMT concessions

				#region Titles
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 1).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1, Name = "BONO ANUAL JUBILADOS", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 2).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2, Name = "BONO ANUAL JOVENES", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 3).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 3, Name = "BONO ANUAL NORMAL", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 4).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4, Name = "BONO SEMESTRAL JOVENES", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 5).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 5, Name = "BONO SEMESTRAL NORMAL", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 6).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 6, Name = "BONO TRIMESTRAL JOVENES", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 7).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 7, Name = "BONO TRIMESTRAL NORMAL", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 8).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 8, Name = "BONO DE 10 VIAJES", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 9).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 9, Name = "BILLETE SENCILLO", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAlboraya.Id && x.Code == 10).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 10, Name = "Reservado Futuros Usos", OwnerName = "Ayto. Alboraya", Image = "", TransportConcessionId = transportConcessionAyuntamientoAlboraya.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 11).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 11, Name = "Bono 10 Paterna", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 12).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 12, Name = "Bono 10 Metropolitano Paterna", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 13).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 13, Name = "Bono 10 Metropolitano Transbordo Paterna", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 14).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 14, Name = "Abono Transporte Urbano de Paterna", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 15).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 15, Name = "Bono Personalizado Estudiante", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 16).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 16, Name = "Bono Personalizado Jubilado", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 17).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 17, Name = "Bono Personalizado Municipal", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 18).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 18, Name = "Bono 10 Estudiante", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 19).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 19, Name = "Bono 10 Jubilado", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoPaterna.Id && x.Code == 20).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 20, Name = "Bono 10Municipal", OwnerName = "Ayto. Paterna", Image = "", TransportConcessionId = transportConcessionAyuntamientoPaterna.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 21).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 21, Name = "Bono 10 Normal", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 22).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 22, Name = "Bono 10 Joven", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 23).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 23, Name = "Bono 30 Días", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 24).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 24, Name = "Bono Ilimitado Jubilado", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 25).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 25, Name = "Bono Ilimitado Subvención Social", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 26).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 26, Name = "Reservado Título Castellón", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 27).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 27, Name = "Reservado Título Castellón", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 28).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 28, Name = "Reservado Título Castellón", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 29).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 29, Name = "Reservado Título Castellón", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoCastellon.Id && x.Code == 30).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 30, Name = "Reservado Título Castellón", OwnerName = "Ayto. de Castellón", Image = "", TransportConcessionId = transportConcessionAyuntamientoCastellon.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 41).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 41, Name = "Bono 10 AVSA, TRANSPORT", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 42).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 42, Name = "Bono 10 AVSA, JUBILAT", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 43).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 43, Name = "Bono 10 AVSA, ESTUDIANT", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 44).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 44, Name = "Bono 10 AVSA, MUNICIPAL", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 45).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 45, Name = "Abono AVSA otorgado por el Ayuntamiento", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 46).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 46, Name = "ESTUDIANT TRANSBORD", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 47).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 47, Name = "Reservado Título AVSA", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 48).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 48, Name = "Reservado Título AVSA", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 49).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 49, Name = "Reservado Título AVSA", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVSA.Id && x.Code == 50).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 50, Name = "Reservado Título AVSA", OwnerName = "AVSA", Image = "", TransportConcessionId = transportConcessionAVSA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 96).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 96, Name = "Bonobús", OwnerName = "EMT ", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 112).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 112, Name = "T-1", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 128).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 128, Name = "Bono ORO", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 368).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 368, Name = "Valencia Tourist Card 24 Hs", OwnerName = "Turismo Valencia", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 624).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 624, Name = "Valencia Tourist Card 48 Hs", OwnerName = "Turismo Valencia", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 800).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 800, Name = "ESCOLAR", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 801).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 801, Name = "Reservado TítuloTUASA", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 802).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 802, Name = "ESTUDIANTE", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 803).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 803, Name = "Bono Ordinario", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 804).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 804, Name = "Bono Jubilado (ANTIGUO)", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 805).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 805, Name = "Bono Estudiante (ANTIGUO)", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 806).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 806, Name = "Bono Concesión CVA018 combinado urbano Alcoi", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 807).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 807, Name = "Bono Concesión CVA018", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 808).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 808, Name = "ET PORTEM PENS. CLASSE 1", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 809).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 809, Name = "ET PORTEM PENS. CLASSE 2", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTUASA.Id && x.Code == 810).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 810, Name = "ET PORTEM DESEMPLEADOS", OwnerName = "TUASA (Alcoi)", Image = "", TransportConcessionId = transportConcessionTUASA.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 811).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 811, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 812).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 812, Name = "Título Turístico de 24h", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 813).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 813, Name = "Título TuriBús", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 814).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 814, Name = "Título Congresista", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 815).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 815, Name = "Título Turístico de 48h", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 816).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 816, Name = "Título Turístico de 72h", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 817).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 817, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 818).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 818, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 819).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 819, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 820).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 820, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 821).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 821, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 822).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 822, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 823).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 823, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 824).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 824, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 825).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 825, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 826).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 826, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 827).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 827, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 828).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 828, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 829).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 829, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 830).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 830, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 831).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 831, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 832).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 832, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 833).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 833, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 834).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 834, Name = "Bono Oro Mutxamel", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 835).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 835, Name = "Bono Oro San Vicente", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 836).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 836, Name = "Bono Móbilis unipersonal Bonificado", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 837).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 837, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 838).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 838, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 839).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 839, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 840).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 840, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 841).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 841, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 842).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 842, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 843).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 843, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 844).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 844, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 845).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 845, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 846).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 846, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 847).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 847, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 848).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 848, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 849).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 849, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 850).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 850, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 851).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 851, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 852).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 852, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 853).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 853, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 854).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 854, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 855).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 855, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 856).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 856, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 857).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 857, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 858).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 858, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 859).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 859, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 860).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 860, Name = "Bono Estudiante Campello", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 861).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 861, Name = "Bono Bus 10", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 862).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 862, Name = "Bono Móbilis Unipersonal no Bonificado", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 863).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 863, Name = "Bono Jove 30", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 864).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 864, Name = "Bono Escolar 30", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 865).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 865, Name = "Bono Oro Alicante", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 866).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 866, Name = "Bono Oro San Juan", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 867).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 867, Name = "Bono Oro Campello", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 868).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 868, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 869).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 869, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 870).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 870, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 871).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 871, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 872).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 872, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMasatusa.Id && x.Code == 873).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 873, Name = "Bono Jubilado Masatusa", OwnerName = "Masatusa", Image = "", TransportConcessionId = transportConcessionMasatusa.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 874).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 874, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 875).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 875, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 876).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 876, Name = "Bono Empleado Masatusa", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 877).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 877, Name = "Bono Empleado Alcoyana", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 878).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 878, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 879).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 879, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 880).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 880, Name = "Valencia Tourist Card 72 Hs", OwnerName = "Turismo Valencia", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 881).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 881, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 882).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 882, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 883).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 883, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 884).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 884, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 885).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 885, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 886).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 886, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 887).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 887, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 888).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 888, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 889).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 889, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 890).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 890, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 891).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 891, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 892).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 892, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 893).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 893, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 894).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 894, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 895).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 895, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 896).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 896, Name = "Nuevo Bono oro Alicante", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 897).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 897, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 898).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 898, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 899).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 899, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTAM.Id && x.Code == 900).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 900, Name = "Reservado Título TAM", OwnerName = "TAM (Alicante)", Image = "", TransportConcessionId = transportConcessionTAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 901).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 901, Name = "Bono 10 TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 902).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 902, Name = "Bono 30 TRAM (macrobono)", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 903).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 903, Name = "Bono 30 Jove TRAM (macrobono)", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 904).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 904, Name = "TAT Mensual TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 905).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 905, Name = "TAT Mensual JoveTRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 906).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 906, Name = "TAT Mensual Gent Major TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 907).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 907, Name = "TAT Anual Gent Major Major TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 908).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 908, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 909).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 909, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 910).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 910, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 911).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 911, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 912).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 912, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 913).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 913, Name = "PENSIONISTA IDA", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 914).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 914, Name = "PENSIONISTA IDA/VUELTA", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 915).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 915, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 916).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 916, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 917).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 917, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 918).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 918, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 919).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 919, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTRAM.Id && x.Code == 920).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 920, Name = "Reservado Título TRAM", OwnerName = "TRAM (FGV Alicante)", Image = "", TransportConcessionId = transportConcessionTRAM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1001).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1001, Name = "Sencillo (TSC)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1002).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1002, Name = "Ida y Vuelta (TSC)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1003).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1003, Name = "Bonometro", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1004).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1004, Name = "TAT", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1005).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1005, Name = "TAT Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1006).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1006, Name = "TAT Gent Major Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1007).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1007, Name = "TAT Gent Major Anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1008).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1008, Name = "TAT Familiar fin de semana", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1009).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1009, Name = "SOV (TSC)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1010).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1010, Name = "Sencillo 20% (TSC)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1011).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1011, Name = "Sencillo 50% (TSC)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1012).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1012, Name = "Familiares de agentes de FGV en activo", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1013).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1013, Name = "Pensionistas de FGV y FEVE (transferidos a FGV)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1014).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1014, Name = "Familiares de pensionistas de FGV y FEVE (transferidos a FGV)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1015).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1015, Name = "Pase de validez Limitada (Personalizado)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1016).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1016, Name = "Pase de validez Limitada (No personalizado)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1017).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1017, Name = "GTP", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1018).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1018, Name = "Bono Promoción", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1019).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1019, Name = "Bono EMT", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1020).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1020, Name = "TAT Anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1021).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1021, Name = "TAT Anual / Trimestre 1", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1022).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1022, Name = "TAT Anual / Trimestre 2", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1023).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1023, Name = "TAT Anual / Trimestre 3", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1024).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1024, Name = "TAT Anual / Trimestre 4", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1025).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1025, Name = "Bonometro 10%", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1026).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1026, Name = "Pase inspección/mantenimiento de peaje", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1027).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1027, Name = "TAT Mobilitat mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1028).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1028, Name = "TAT Mobilitat anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1029).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1029, Name = "Bono 1", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1030).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1030, Name = "Billete de salida", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1031).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1031, Name = "TAT Trimestral Colegio Mayor La Coma", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1032).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1032, Name = "TAT Trimestral La Coma residente", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1033).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1033, Name = "CEU Trimestral", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1034).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1034, Name = "UCV - 9 M", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1035).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1035, Name = "UCV - 10 M", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1036).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1036, Name = "Convenio IALE Trimestral escolar", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1037).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1037, Name = "Convenio IALE Anual escolar", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1038).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1038, Name = "Escola La Masia", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1039).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1039, Name = "EPLA", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1040).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1040, Name = "Colegio del Vedat Trimestral", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1041).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1041, Name = "Colegio El Vedat", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1042).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1042, Name = "UVEG Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1043).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1043, Name = "UVEG Mensual Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1044).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1044, Name = "UVEG Anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1045).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1045, Name = "Hospital General mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1046).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1046, Name = "Convenio con AENA", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1047).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1047, Name = "UCV - Edetania Trimestral", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1048).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1048, Name = "E.P. La Salle Trimestral", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1049).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1049, Name = "E.P. La Salle Anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1050).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1050, Name = "Estudiantes Ayto. Burjassot", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1051).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1051, Name = "Estudiantes Ayto. Moncada", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1052).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1052, Name = "Estudiantes Ayto. Rocafort", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1053).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1053, Name = "Estudiantes Ayto. La Pobla de Farnals", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1054).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1054, Name = "Estudiantes Ayto. Bétera", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1055).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1055, Name = "Estudiantes Ayto. Massamagrell", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1056).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1056, Name = "Estudiantes Ayto. Godella", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1057).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1057, Name = "Bono 60x60", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1058).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1058, Name = "Pensionistas Ayto. Godella", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1059).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1059, Name = "Pensionistas Ayto. Massamagrell", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1060).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1060, Name = "Pensionistas Ayto. Museros", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1061).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1061, Name = "Pensionistas Ayto. Foios", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1062).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1062, Name = "Pensionistas Ayto. Rafelbunyol", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1063).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1063, Name = "Pensionistas Ayto. Moncada", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1064).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1064, Name = "Pensionistas Ayto. Meliana", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1065).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1065, Name = "Pensionistas Ayto. Emperador", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1066).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1066, Name = "Pensionistas Ayto. Albalat dels Sorells", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1067).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1067, Name = "Pensionistas Ayto. La Pobla de Farnals", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1068).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1068, Name = "Pensionistas Ayto. Almàssera", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1069).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1069, Name = "Pensionistas Ayto. Rocafort", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1070).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1070, Name = "Pensionistas Ayto. Alboraya", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1071).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1071, Name = "Pensionistas Ayto. Mislata", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1072).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1072, Name = "Pensionistas Ayto. Alfara del Patriarca", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1073).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1073, Name = "Pensionistas Ayto. Manises", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1074).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1074, Name = "UPV Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1075).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1075, Name = "UPV Mensual Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1076).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1076, Name = "Estudiantes Ayto. Albalat dels Sorells", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1077).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1077, Name = "CEU Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1078).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1078, Name = "CEU Mensual Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1079).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1079, Name = "Pensionistas Ayto. Paterna", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1080).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1080, Name = "Estudiantes Ayto. Paterna 9 meses", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1081).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1081, Name = "Estudiantes Ayto. Paterna Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1082).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1082, Name = "Estudiantes Ayto. Paterna Mensual Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1083).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1083, Name = "Estudiantes Ayto. Paterna Bonometro 10%", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1084).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1084, Name = "Fundación La Fé Bonometro 10%", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1085).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1085, Name = "Fundación La Fé Mensual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1086).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1086, Name = "Fundación La Fé Mensual Jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1087).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1087, Name = "Pens. Ayto. Rafelbunyol anual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1088).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1088, Name = "Hospital General mensual jove", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1089).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1089, Name = "Hospital General bonometro 10%", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1090).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1090, Name = "Billete zonal de expecdición manual", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1091).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1091, Name = "Billete modelo I-4", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1092).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1092, Name = "Sencillo (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1093).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1093, Name = "Sencillo 20% (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1094).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1094, Name = "Sencillo 50% (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1095).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1095, Name = "SOV (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1096).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1096, Name = "Billete colectivo (Grupo) (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1097).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1097, Name = "Billete de sustitución (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1098).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1098, Name = "Suplemento valor doble sencillo (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1099).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1099, Name = "Suplemento valor 10€ (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1100).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1100, Name = "Suplemento valor 50€ (Papel térmico)", OwnerName = "FGV (Valencia)", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1101).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1101, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1102).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1102, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1103).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1103, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1104).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1104, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1105).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1105, Name = "Pensionistas EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1106).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1106, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1107).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1107, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1108).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1108, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1109).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1109, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1110).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1110, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1111).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1111, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1112).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1112, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1113).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1113, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1114).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1114, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1115).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1115, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1116).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1116, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1117).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1117, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1118).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1118, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1119).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1119, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1120).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1120, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1121).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1121, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1122).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1122, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1123).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1123, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1124).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1124, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1125).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1125, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1126).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1126, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1127).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1127, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1128).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1128, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1129).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1129, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1130).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1130, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1131).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1131, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1132).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1132, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1133).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1133, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1134).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1134, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1135).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1135, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1136).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1136, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1137).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1137, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1138).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1138, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1139).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1139, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1140).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1140, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1141).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1141, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1142).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1142, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1143).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1143, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1144).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1144, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1145).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1145, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1146).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1146, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1147).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1147, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1148).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1148, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1149).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1149, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1150).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1150, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1151).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1151, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1152).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1152, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1153).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1153, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1154).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1154, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1155).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1155, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1156).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1156, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1157).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1157, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1158).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1158, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1159).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1159, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1160).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1160, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1161).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1161, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1162).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1162, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1163).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1163, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1164).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1164, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1165).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1165, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1166).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1166, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1167).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1167, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1168).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1168, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1169).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1169, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1170).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1170, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1171).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1171, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1172).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1172, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1173).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1173, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1174).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1174, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1175).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1175, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1176).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1176, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1177).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1177, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1178).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1178, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1179).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1179, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1180).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1180, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1181).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1181, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1182).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1182, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1183).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1183, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1184).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1184, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1185).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1185, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1186).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1186, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1187).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1187, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1188).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1188, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1189).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1189, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1190).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1190, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1191).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1191, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1192).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1192, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1193).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1193, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1194).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1194, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1195).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1195, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1196).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1196, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1197).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1197, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1198).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1198, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1199).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1199, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 1200).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1200, Name = "Reservado Título EMT", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1201).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1201, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1202).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1202, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1203).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1203, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1204).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1204, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1205).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1205, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1206).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1206, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1207).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1207, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1208).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1208, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1209).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1209, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1210).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1210, Name = "Pase empleado RENFE", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1211).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1211, Name = "Pase empleado ADIF", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1212).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1212, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1213).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1213, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1214).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1214, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1215).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1215, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1216).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1216, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1217).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1217, Name = "FUNDAR", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1218).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1218, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1219).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1219, Name = "Sencillo FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1220).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1220, Name = "Ida y Vuelta FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1221).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1221, Name = "Bonometro FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1222).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1222, Name = "TAT FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1223).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1223, Name = "TAT Anual FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1224).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1224, Name = "Bono 60x60 FN 50%", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1225).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1225, Name = "AENA/Aeroport", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1226).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1226, Name = "TAT Tr. Aeroport (Mensual tarifa especial trabajadores empresas Aeroport-no AENA)", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1227).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1227, Name = "Bono AENA (Bonometro trabajadores AENA)", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1228).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1228, Name = "Bono Tr. Aeroport (Bonometro tarifa especial trabajadores empresas Aeroport-no AENA)", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1229).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1229, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1230).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1230, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1231).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1231, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1232).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1232, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1233).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1233, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1234).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1234, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1235).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1235, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1236).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1236, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1237).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1237, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1238).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1238, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1239).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1239, Name = "Estudiantess Ayto. Mislata", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1240).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1240, Name = "Estudiantes Ayto. Museos (Temporal Ayto Mislata)", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1241).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1241, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1242).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1242, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1243).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1243, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1244).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1244, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1245).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1245, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1246).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1246, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1247).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1247, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1248).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1248, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1249).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1249, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1250).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1250, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1251).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1251, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1252).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1252, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1253).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1253, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1254).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1254, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1255).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1255, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1256).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1256, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1257).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1257, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1258).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1258, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1259).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1259, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1260).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1260, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1261).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1261, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1262).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1262, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1263).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1263, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1264).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1264, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1265).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1265, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1266).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1266, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1267).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1267, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1268).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1268, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1269).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1269, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1270).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1270, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1271).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1271, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1272).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1272, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1273).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1273, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1274).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1274, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1275).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1275, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1276).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1276, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1277).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1277, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1278).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1278, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1279).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1279, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1280).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1280, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1281).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1281, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1282).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1282, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1283).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1283, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1284).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1284, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1285).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1285, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1286).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1286, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1287).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1287, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1288).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1288, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1289).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1289, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1290).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1290, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1291).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1291, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1292).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1292, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1293).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1293, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1294).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1294, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1295).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1295, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1296).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1296, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1297).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1297, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1298).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1298, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1299).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1299, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 1300).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1300, Name = "Reservado FGV", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 1552).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1552, Name = "Bono Transbordo (10 viajes)", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 1568).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1568, Name = "Abono Transporte", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 1648).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1648, Name = "T-2", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 1824).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1824, Name = "Abono Transporte Jove", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 1904).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 1904, Name = "T-3", OwnerName = "Coordinación aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2000).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2000, Name = "Reserva para título monedero", OwnerName = "Ayto. TEULADA", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2001).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2001, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2002).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2002, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2003).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2003, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2004).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2004, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2005).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2005, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2006).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2006, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2007).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2007, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2008).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2008, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2009).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2009, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoTeulada.Id && x.Code == 2010).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2010, Name = "Reservado Ayto. Teulada", OwnerName = "Ayto. Teulada", Image = "", TransportConcessionId = transportConcessionAyuntamientoTeulada.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2011).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2011, Name = "Título: Bono Temporal 30 dias", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2012).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2012, Name = "TBus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2013).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2013, Name = "Bono Dis Altea", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2014).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2014, Name = "Bono Oro Villajoyosa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2015).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2015, Name = "Bono Oro L'Alfas", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2016).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2016, Name = "Bono Oro Benidorm", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2017).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2017, Name = "Bono Oro Finestrat", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2018).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2018, Name = "Bono Oro Altea", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2019).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2019, Name = "Bono Oro General", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2020).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2020, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2021).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2021, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2022).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2022, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2023).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2023, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2024).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2024, Name = "Bono Joven/ escolar Altea", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2025).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2025, Name = "Bono Joven/escolar L’Alfàs", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2026).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2026, Name = "Bono Escolar Benidorm", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2027).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2027, Name = "Bono<14 Villajoyosa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2028).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2028, Name = "Pase Escuelas Deportivas Villajoyosa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2029).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2029, Name = "Bono Escolar Finestrat", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2030).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2030, Name = "Pase Escolar C.Educación", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2031).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2031, Name = "Bono Joven General", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2032).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2032, Name = "Pass 1day", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2033).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2033, Name = "Pass 4days", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2034).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2034, Name = "Pass 7days", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2035).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2035, Name = "Pass 2 days", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2036).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2036, Name = "Pase empleado", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2037).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2037, Name = "Pase favor empresa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2038).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2038, Name = "Familia Numerosa General", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2039).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2039, Name = "Familia numerosa especial", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2040).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2040, Name = "Familia numerosa general Villajoyosa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2041).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2041, Name = "Familia NumerosaEspecial Villajoyosa", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2042).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2042, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2043).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2043, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2044).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2044, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2045).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2045, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2046).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2046, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2047).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2047, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2048).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2048, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2049).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2049, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLlorenteBus.Id && x.Code == 2050).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2050, Name = "Reservado Llorente Bus", OwnerName = "Llorente Bus", Image = "", TransportConcessionId = transportConcessionLlorenteBus.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2081).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2081, Name = "Bono 10 Tarifa Estudiante", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2082).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2082, Name = "Bono 10 Tarifa Jubilado", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2083).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2083, Name = "Bono 10 Tarifa Normal", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2084).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2084, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2085).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2085, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2086).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2086, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2087).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2087, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2088).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2088, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2089).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2089, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoBuñol.Id && x.Code == 2090).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2090, Name = "Reservado Ayto. Buñol", OwnerName = "Ayto. Buñol", Image = "", TransportConcessionId = transportConcessionAyuntamientoBuñol.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2091).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2091, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2092).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2092, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2093).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2093, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2094).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2094, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2095).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2095, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2096).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2096, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2097).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2097, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2098).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2098, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2099).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2099, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMancomunitatLHortaSud.Id && x.Code == 2100).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2100, Name = "Reservado Mancomunitat L’Horta Sud", OwnerName = "Mancomunitat L’Horta Sud", Image = "", TransportConcessionId = transportConcessionMancomunitatLHortaSud.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2101).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2101, Name = "Título Monedero", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2102).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2102, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2103).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2103, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2104).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2104, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2105).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2105, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2106).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2106, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2107).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2107, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2108).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2108, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2109).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2109, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionLosSerranos.Id && x.Code == 2110).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2110, Name = "Reservado Los Serranos", OwnerName = "Los Serranos", Image = "", TransportConcessionId = transportConcessionLosSerranos.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2111).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2111, Name = "Bono 10 METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2112).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2112, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2113).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2113, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2114).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2114, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2115).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2115, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2116).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2116, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2117).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2117, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2118).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2118, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2119).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2119, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 2120).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2120, Name = "Reservado METRORBITAL", OwnerName = "METRORBITAL", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2121).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2121, Name = "Título Monedero", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2122).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2122, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2123).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2123, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2124).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2124, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2125).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2125, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2126).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2126, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2127).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2127, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2128).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2128, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2129).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2129, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionValledeCárcer.Id && x.Code == 2130).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2130, Name = "Reservado Valle de Carcer", OwnerName = "Valle de Cárcer", Image = "", TransportConcessionId = transportConcessionValledeCárcer.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2131).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2131, Name = "Godella Residente Semestral", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2132).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2132, Name = "Godella Residente Anual", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2133).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2133, Name = "Godella Residente Semestral FN (familia numerosa)", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2134).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2134, Name = "Godella Residente Anual FN", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2135).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2135, Name = "Godella No Residente Semestral", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2136).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2136, Name = "Godella No Residente Anual", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2137).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2137, Name = "Godella No Residente Semestral FN", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2138).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2138, Name = "Godella No Residente Anual FN", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2139).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2139, Name = "Godella Parados", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2140).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2140, Name = "Godella Jubilados Rentas Bajas", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2141).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2141, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2142).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2142, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2143).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2143, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2144).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2144, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2145).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2145, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2146).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2146, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2147).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2147, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2148).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2148, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2149).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2149, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionGodella.Id && x.Code == 2150).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2150, Name = "Reservado Godella", OwnerName = "Godella", Image = "", TransportConcessionId = transportConcessionGodella.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2501).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2501, Name = "Sencillo", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2502).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2502, Name = "Ida y vuelta", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2503).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2503, Name = "Bonotren", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2504).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2504, Name = "Mensual limitado", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2505).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2505, Name = "Mensual ilimitado", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2506).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2506, Name = "Reservado RENFE", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2507).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2507, Name = "Reservado RENFE", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2508).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2508, Name = "Reservado RENFE", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2509).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2509, Name = "Reservado RENFE", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionRENFE.Id && x.Code == 2510).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 2510, Name = "Reservado RENFE", OwnerName = "RENFE", Image = "", TransportConcessionId = transportConcessionRENFE.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 3120).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 3120, Name = "Pase Empleado", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Code == 3376).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 3376, Name = "Agentes de FGV en activo", OwnerName = "FGV", Image = "", TransportConcessionId = transportConcessionFGV.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Code == 3632).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 3632, Name = "Pase Empleado", OwnerName = "EMT", Image = "", TransportConcessionId = transportConcessionEMT.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4000).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4000, Name = "Bono trasbordo (otras zonas)", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4001).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4001, Name = "(Vinalesa y P. Tecnológico)", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4002).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4002, Name = "Bono 10 Viajes - S. Transversal Horta Nord", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4003).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4003, Name = "Bono 10 Viajes - València - Burjassot- P. Tecnològic B", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4004).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4004, Name = "Bono Transbordo - Bono 10 Viajes - València - Burjassot- P. Tecnològic AB", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4005).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4005, Name = "A>65 Corredor Valencia – Perellonet", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4006).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4006, Name = "Bono 10 Viajes Riba-Roja - Manises – Valencia", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4007).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4007, Name = "Bono 10 Viajes Servicio Alcàsser – Silla", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMetrobus.Id && x.Code == 4008).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4008, Name = "Sencillo Jubilado Metrobus", OwnerName = "Metrobus", Image = "", TransportConcessionId = transportConcessionMetrobus.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionMetrobus.Id && x.Code == 4009).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4009, Name = "Otros Sencillos Metrobus", OwnerName = "Metrobus", Image = "", TransportConcessionId = transportConcessionMetrobus.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4010).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4010, Name = "Bono 10 Viajes Metrobus Nord", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4011).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4011, Name = "Bono 10 viajes Marxant-bus", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4012).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4012, Name = "Bono Transbordo MISLATA", OwnerName = "AVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4013).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4013, Name = "Bono Transbordo ALBORAYA", OwnerName = "AVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4014).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4014, Name = "Bono 10 viajes Estudios superiores Marxant-bus", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4015).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4015, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4016).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4016, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4017).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4017, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4018).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4018, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4019).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4019, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4020).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4020, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4021).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4021, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4022).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4022, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4023).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4023, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4024).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4024, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4025).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4025, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4026).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4026, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4027).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4027, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4028).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4028, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4029).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4029, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4030).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4030, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4031).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4031, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4032).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4032, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4033).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4033, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4034).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4034, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4035).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4035, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4036).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4036, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4037).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4037, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4038).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4038, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4039).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4039, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4040).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4040, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4041).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4041, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4042).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4042, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4043).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4043, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4044).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4044, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4045).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4045, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4046).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4046, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4047).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4047, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4048).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4048, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4049).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4049, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4050).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4050, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4050).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4050, Name = "Reservado Título aVM", OwnerName = "aVM", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = true }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4051).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4051, Name = "Título Torrent Daurada", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4052).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4052, Name = "Título Torrent Estudiant", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4053).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4053, Name = "Abonamente 10", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4054).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4054, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4055).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4055, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4056).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4056, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4057).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4057, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4058).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4058, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4059).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4059, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionTorrent.Id && x.Code == 4060).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4060, Name = "Reservado Ayto. Torrent", OwnerName = "Torrent", Image = "", TransportConcessionId = transportConcessionTorrent.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4061).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4061, Name = "Bono 10 Tamarit- Santa Pola- Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4062).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4062, Name = "Bono 10 Tamarit- Santa Pola", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4063).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4063, Name = "Bono 10 Gran Alacant- Arenales-Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4064).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4064, Name = "Bono 10 Santa Pola-Gran Alacant-Arenales-El Altet", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4065).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4065, Name = "Bono 10 Alicante-El Altet- Playa del Altet", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4066).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4066, Name = "Bono 10 San Vicente- Arenales del Sol", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4067).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4067, Name = "Bono 10 San Vicente- Santa Pola", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4068).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4068, Name = "Bono 10 Gran Alacant- Universidad de Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4069).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4069, Name = "Bono 10 Agua Amarga- Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4070).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4070, Name = "Bono 20 Viajes Agua- Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4071).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4071, Name = "Bono 10 Gran Alacant- Gran Alacant", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4072).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4072, Name = "Bono Santa Pola- Universidad Alicante", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4073).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4073, Name = "Bono Gran Alacant- Universidad", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4074).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4074, Name = "Bono Arenales del Sol- Universidad", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4075).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4075, Name = "Bono El Altel-Universidad", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4076).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4076, Name = "Reservado Autocares Baile", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4077).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4077, Name = "Reservado Autocares Baile", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4078).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4078, Name = "Reservado Autocares Baile", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4079).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4079, Name = "Reservado Autocares Baile", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAutocaresBaile.Id && x.Code == 4080).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4080, Name = "Reservado Autocares Baile", OwnerName = "Autocares Baile", Image = "", TransportConcessionId = transportConcessionAutocaresBaile.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4080).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4080, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4081).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4081, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4082).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4082, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4083).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4083, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4084).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4084, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4085).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4085, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4086).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4086, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4087).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4087, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4088).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4088, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4089).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4089, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAyuntamientoAldaia.Id && x.Code == 4090).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4090, Name = "Reservado Ayto. Aldaia", OwnerName = "Ayto. Aldaia", Image = "", TransportConcessionId = transportConcessionAyuntamientoAldaia.Id, HasZone = false }); }
								if (context.TransportTitle.Where(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Code == 4094).FirstOrDefault() == null) { context.TransportTitle.AddOrUpdate(x => x.Code, new TransportTitle { Code = 4094, Name = "Tarjeta vinculada", OwnerName = "", Image = "", TransportConcessionId = transportConcessionAVM.Id, HasZone = false }); }
								context.SaveChanges();
								var transportTitleAbonoTransporte = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Abono Transporte");
								var transportTitleT1 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "T-1");
								var transportTitleT2 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "T-2");
								var transportTitleT3 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "T-3");
								var transportTitleBonoTransbordo = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono Transbordo (10 viajes)");
								var transportTitleBono10Metrorbital = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono 10 METRORBITAL");
								var transportTitleValenciaTourist24 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Valencia Tourist Card 24 Hs");
								var transportTitleValenciaTourist48 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Valencia Tourist Card 48 Hs");
								var transportTitleValenciaTourist72 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Valencia Tourist Card 72 Hs");
								var transportTitleValenciaBurjassotPTecnologicB = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono 10 Viajes - València - Burjassot- P. Tecnològic B");
								var transportTitleValenciaBurjassotPTecnologicAB = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono Transbordo - Bono 10 Viajes - València - Burjassot- P. Tecnològic AB");
								var transportTitleValenciaaRibarojaBono10 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono 10 Viajes Riba-Roja - Manises – Valencia");
								var transportTitleAlcasserSillaBono10viatges = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono 10 Viajes Servicio Alcàsser – Silla");
								var transportTitleBonoMislata = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono Transbordo MISLATA");
								var transportTitleBonoMarxantBus = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Bono 10 viajes Marxant-bus");
								var transportTitleBono60x60 = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Name == "Bono 60x60");
								var transportTitleBonoMetro = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Name == "Bonometro");
								var transportTitleSencillo = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Name == "Sencillo (TSC)");
								var transportTitleIdayVuelta = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionFGV.Id && x.Name == "Ida y Vuelta (TSC)");
								var transportTitleBonoBus = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionEMT.Id && x.Name == "Bonobús");
								var transportTitleTarjetavinculada = context.TransportTitle.First(x => x.TransportConcessionId == transportConcessionAVM.Id && x.Name == "Tarjeta vinculada");				
								context.SaveChanges();
				#endregion Titles

				#region Prices
								var S = new DateTime(2016, 01, 01);
								var D = new DateTime(2016, 12, 31);
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 45, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 45, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 45, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 34, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 58.30m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.AB, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 58.30m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 58.30m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 68.70m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 68.70m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 79.10m, Start = S, End = D, TransportTitleId = transportTitleAbonoTransporte.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Dias, Unities = 30 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 5, Price = 4, Start = S, End = D, TransportTitleId = transportTitleT1.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 24 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 5, Price = 6.70m, Start = S, End = D, TransportTitleId = transportTitleT2.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 48 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 5, Price = 9.70m, Start = S, End = D, TransportTitleId = transportTitleT3.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 72 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 5, Price = 9, Start = S, End = D, TransportTitleId = transportTitleBonoTransbordo.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 7.55m, Start = S, End = D, TransportTitleId = transportTitleBono10Metrorbital.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaTourist24.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 15, Start = S, End = D, TransportTitleId = transportTitleValenciaTourist24.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 24 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaTourist48.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 20, Start = S, End = D, TransportTitleId = transportTitleValenciaTourist48.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 48 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaTourist72.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 25, Start = S, End = D, TransportTitleId = transportTitleValenciaTourist72.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Horas, Unities = 72 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 7.75m, Start = S, End = D, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 15.45m, Start = S, End = D, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id, Zone = ZoneType.AB, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 7.75m, Start = S, End = D, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 7.75m, Start = S, End = D, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 6, Price = 10.30m, Start = S, End = D, TransportTitleId = transportTitleBonoMislata.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 5, Price = 31.25m, Start = S, End = D, TransportTitleId = transportTitleBonoMarxantBus.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 8m, Start = S, End = D, TransportTitleId = transportTitleBonoBus.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								//Bono 60x60
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 77.70m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 77.70m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 85.50m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
									);
								}
								//bono metro
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 14m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 14m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 20m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
									);
								}
								//Sencillo
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.8m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.8m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 3.9m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
									);
								}
								////// Ida y vuelta
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 5.3m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 5.3m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
								if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								{
									context.TransportPrice.AddOrUpdate(x => x.Version,
										new TransportPrice { Version = 0, Price = 3.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
									);
								}
				#endregion Prices

				#region FGV Prices
								////version 0 mientras sepa la version
								////Bono 60x60
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 40.1m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 57.55m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 77.70m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 77.70m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBono60x60.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 85.50m, Start = S, End = D, TransportTitleId = transportTitleBono60x60.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 60 }
								//	);
								//}
								////bono metro
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 7.2m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 10.4m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 14m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 14m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 20m, Start = S, End = D, TransportTitleId = transportTitleBonoMetro.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
								////Sencillo
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 1.5m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.1m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.8m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.8m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleSencillo.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 3.9m, Start = S, End = D, TransportTitleId = transportTitleSencillo.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 1 }
								//	);
								//}
								//////// Ida y vuelta
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.A, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.B, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.C, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 2.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.D, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.BC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 4m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.CD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 5.3m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.ABC, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 5.3m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.BCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleIdayVuelta.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 3.9m, Start = S, End = D, TransportTitleId = transportTitleIdayVuelta.Id, Zone = ZoneType.ABCD, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Hybrid, Unities = 2 }
								//	);
								//}
				#endregion FGV Prices

				#region EMT Prices
								////version 0 por ahora..
								//if (!context.TransportPrice.Any(x => x.TransportTitleId == transportTitleBonoBus.Id))
								//{
								//	context.TransportPrice.AddOrUpdate(x => x.Version,
								//		new TransportPrice { Version = 0, Price = 8m, Start = S, End = D, TransportTitleId = transportTitleBonoBus.Id, TemporalUnities = EigeTipoUnidadesValidezTemporalEnum.Viajes, Unities = 10 }
								//	);
								//}
				#endregion EMT Prices

				#region aVM TransportCardSupportTitleCompatibility
								//Abono Transporte
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAbonoTransporte.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAbonoTransporte.Id }
									);
								}
								//Bono Transbordo 
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBonoTransbordo.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBonoTransbordo.Id }
									);
								}
								//T1,T2 y T3
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT1.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT1.Id }
									);
								}
								//T2
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT2.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT2.Id }
									);
								}
								//T3
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleT3.Id }
									);
								}







								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleT3.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleT3.Id }
									);
								}
								//Tarjeta Valencia
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportTitleId == transportTitleValenciaTourist24.Id && x.TransportCardSupportId == TransportCardSupportTarjetaValenciaCard.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportTitleId = transportTitleValenciaTourist24.Id, TransportCardSupportId = TransportCardSupportTarjetaValenciaCard.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportTitleId == transportTitleValenciaTourist72.Id && x.TransportCardSupportId == TransportCardSupportTarjetaValenciaCard.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportTitleId = transportTitleValenciaTourist72.Id, TransportCardSupportId = TransportCardSupportTarjetaValenciaCard.Id }
									);
								}
								//Bono 10 Metrorbital
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBono10Metrorbital.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBono10Metrorbital.Id }
									);
								}
								//Valencia MasCamarena Bono10 
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicB.Id }
									);
								}
								//Valencia MasCamarena BonoTransbordo 
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaBurjassotPTecnologicAB.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaBurjassotPTecnologicAB.Id }
									);
								}
								//Valencia a Ribaroja Bono 10
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleValenciaaRibarojaBono10.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleValenciaaRibarojaBono10.Id }
									);
								}

								// Alcasser Silla Bono10 viatges 
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleAlcasserSillaBono10viatges.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleAlcasserSillaBono10viatges.Id }
									);
								}
								// Bono Mislata 
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada0.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada0.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada1.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada1.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada4.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada4.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada5.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada5.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada6.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada6.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada7.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada7.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada9.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada9.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada10.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada10.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada11.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada11.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada12.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada12.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaPersonalizada13.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaPersonalizada13.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima2.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima2.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima3.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima3.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima8.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima8.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaCiudadanaAnónima14.Id && x.TransportTitleId == transportTitleBonoMislata.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaCiudadanaAnónima14.Id, TransportTitleId = transportTitleBonoMislata.Id }
									);
								}
								// Bono Marxant xpres
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id && x.TransportTitleId == transportTitleBonoMarxantBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizadaBankia.Id, TransportTitleId = transportTitleBonoMarxantBus.Id }
									);
								}
				#endregion aVM TransportCardSupportTitleCompatibility

				#region EMT TransportCardSupportTitleCompatibility
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaTodos.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaTodos.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove5.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove5.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaJove6.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaJove6.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónima.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónima.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaRelojMobilis.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaRelojMobilis.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaPersonalizadaYUM.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaPersonalizadaYUM.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaFGV.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaFGV.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaEMT.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaEMT.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
								if (!context.TransportCardSupportTitleCompatibility.Any(x => x.TransportCardSupportId == TransportCardSupportTarjetaAnónimaNormalizada.Id && x.TransportTitleId == transportTitleBonoBus.Id))
								{
									context.TransportCardSupportTitleCompatibility.AddOrUpdate(x => x.TransportCardSupport,
										new TransportCardSupportTitleCompatibility { TransportCardSupportId = TransportCardSupportTarjetaAnónimaNormalizada.Id, TransportTitleId = transportTitleBonoBus.Id }
									);
								}
				#endregion EMT TransportCardSupportTitleCompatibility

				#region FGV TransportSimultaneousTitleCompatibility
								//Bono 60x60 con Bono Bus
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBono60x60.Id && x.TransportTitleId2 == transportTitleBonoBus.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBono60x60.Id, TransportTitleId2 = transportTitleBonoBus.Id }
									);
								}
								//BonoBus con Bono 60x60
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBonoBus.Id && x.TransportTitleId2 == transportTitleBono60x60.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBonoBus.Id, TransportTitleId2 = transportTitleBono60x60.Id }
									);
								}
								//BonoMEtro con BonoBus
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id && x.TransportTitleId2 == transportTitleBonoBus.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBonoMetro.Id, TransportTitleId2 = transportTitleBonoBus.Id }
									);
								}
								//Bonobus con Bonometro
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBonoBus.Id && x.TransportTitleId2 == transportTitleBonoMetro.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBonoBus.Id, TransportTitleId2 = transportTitleBonoMetro.Id }
									);
								}
								//Bono 60x60 con Metrorbital
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBono60x60.Id && x.TransportTitleId2 == transportTitleBono10Metrorbital.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBono60x60.Id, TransportTitleId2 = transportTitleBono10Metrorbital.Id }
									);
								}
								//Bonometro con Metrorbital
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBonoMetro.Id && x.TransportTitleId2 == transportTitleBono10Metrorbital.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBonoMetro.Id, TransportTitleId2 = transportTitleBono10Metrorbital.Id }
									);
								}
								//Metrorbital con Bono 60x60
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBono10Metrorbital.Id && x.TransportTitleId2 == transportTitleBono60x60.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBono10Metrorbital.Id, TransportTitleId2 = transportTitleBono60x60.Id }
									);
								}
								//Metrorbital con BonoMetro
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBono10Metrorbital.Id && x.TransportTitleId2 == transportTitleBonoMetro.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBono10Metrorbital.Id, TransportTitleId2 = transportTitleBonoMetro.Id }
									);
								}
				#endregion FGV TransportSimultaneousTitleCompatibility

				#region EMT TransportSimultaneousTitleCompatibility
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBonoBus.Id && x.TransportTitleId2 == transportTitleBono10Metrorbital.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBonoBus.Id, TransportTitleId2 = transportTitleBono10Metrorbital.Id }
									);
								}
								if (!context.TransportSimultaneousTitleCompatibility.Any(x => x.TransportTitleId == transportTitleBono10Metrorbital.Id && x.TransportTitleId2 == transportTitleBonoBus.Id))
								{
									context.TransportSimultaneousTitleCompatibility.AddOrUpdate(x => x.TransportTitle,
										new TransportSimultaneousTitleCompatibility { TransportTitleId = transportTitleBono10Metrorbital.Id, TransportTitleId2 = transportTitleBonoBus.Id }
									);
								}
				#endregion EMT TransportSimultaneousTitleCompatibility
				}*/

                #endregion

                #region Version

                context.ServiceOption.AddOrUpdate(x => x.Id, new ServiceOption
                {
                    Id = 1,
                    Name = "AndroidVersionCode",
                    ValueType = "int",
                    Value = "19"
                });
                context.ServiceOption.AddOrUpdate(x => x.Id, new ServiceOption
                {
                    Id = 2,
                    Name = "AndroidVersionName",
                    ValueType = "String",
                    Value = "2.0.1"
                });
                if (!context.ServiceOption.Any(x =>
                    x.Id == 3 &&
                    x.Name == "LastOrderId"
                ))
                    context.ServiceOption.AddOrUpdate(x => x.Id, new ServiceOption
                    {
                        Id = 3,
                        Name = "LastOrderId",
                        ValueType = "int",
                        Value = "0"
                    });
                context.ServiceOption.AddOrUpdate(x => x.Id, new ServiceOption
                {
                    Id = 4,
                    Name = "ServerVersionName",
                    ValueType = "string",
                    Value = "v5.5.3e"
                });

                context.Platform.AddOrUpdate(x => x.Type, new Platform
                {
                    Id = 1,
                    PushCertificate = "AIzaSyABrK580cO5WVbs6J7d3Zh1qYjSb_nz6S0",
                    PushId = "849435164237",
                    Type = DeviceType.Android
                });
                context.Platform.AddOrUpdate(x => x.Type, new Platform
                {
                    Id = 2,
                    PushCertificate = "",
                    PushId = "",
                    Type = DeviceType.Ios
                });
                context.Platform.AddOrUpdate(x => x.Type, new Platform
                {
                    Id = 3,
                    PushCertificate = "",
                    PushId = "",
                    Type = DeviceType.SignalR
                });

                #endregion
            }
            catch (DbEntityValidationException ex)
            {
                throw new Exception(
                    ex.EntityValidationErrors
                        .SelectMany(x =>
                            x.ValidationErrors
                                .Select(y => y.ErrorMessage)
                        )
                        .JoinString("\n")
                    , ex);
            }
        }

        #region CreatePaymentWorker
        public class CreatePaymentWorkerDto
        {
            public string Login { get; set; }
            public string Name { get; set; }
            public PaymentConcession PaymentConcession { get; set; }
        }
        public PaymentWorker CreatePaymentWorker(PublicContext context, CreatePaymentWorkerDto arguments)
        {
            if (!context.PaymentWorker.Any(x => x.Login == arguments.Login))
            {
                context.PaymentWorker.AddOrUpdate(x => x.Name,
                    new PaymentWorker
                    {
                        Login = arguments.Login,
                        Name = arguments.Name,
                        Concession = arguments.PaymentConcession,
                        State = WorkerState.Active
                    }
                );
                context.SaveChanges();
            }
            var paymentWorker = context.PaymentWorker.First(x => x.Login == arguments.Login);

            return paymentWorker;
        }
        #endregion CreatePaymentConcession

        #region CreatePaymentConcession
        public class CreatePaymentConcessionDto
        {
            public string Login { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string TaxName { get; set; }
            public string TaxNumber { get; set; }
            public string TaxAddress { get; set; }
            public bool AllowUnsafePayments { get; set; }
            public PaymentConcession LiquidationConcession { get; set; }
        }
        public PaymentConcession CreatePaymentConcession(PublicContext context, CreatePaymentConcessionDto arguments)
        {
            if (!context.ServiceSupplier.Any(x => x.Login == arguments.Login))
            {
                context.ServiceSupplier.AddOrUpdate(x => x.Name,
                    new ServiceSupplier
                    {
                        Login = arguments.Login,
                        Name = arguments.Name,
                        PaymentTest = true,
                        TaxName = arguments.TaxName,
                        TaxNumber = arguments.TaxNumber,
                        TaxAddress = arguments.TaxAddress
                    }
                );
                context.SaveChanges();
            }
            var supplier = context.ServiceSupplier.First(x => x.Login == arguments.Login);

            if (!context.ServiceConcession.Any(x => x.SupplierId == supplier.Id && x.Name == arguments.Name))
            {
                context.ServiceConcession.AddOrUpdate(x => x.Name,
                    new ServiceConcession
                    {
                        Name = arguments.Name,
                        Type = ServiceType.Charge,
                        SupplierId = supplier.Id
                    }
                );
                context.SaveChanges();
            }
            var concession = context.ServiceConcession.First(x => x.SupplierId == supplier.Id && x.Name == arguments.Name);

            if (!context.PaymentConcession.Any(x => x.ConcessionId == concession.Id))
            {
                context.PaymentConcession.AddOrUpdate(x => x.ConcessionId,
                    new PaymentConcession
                    {
                        ConcessionId = concession.Id,
                        Phone = arguments.Phone,
                        FormUrl = "",
                        LiquidationRequestDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        LiquidationAmountMin = 100m,
                        Address = arguments.Address,
                        CreateConcessionDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        BankAccountNumber = "ES0000000000000000000000",
                        AllowUnsafePayments = arguments.AllowUnsafePayments,
                        OnPayedUrl = "",
                        OnPayedEmail = "",
                        Key = "",
                        OnPaymentMediaCreatedUrl = "",
                        OnPaymentMediaErrorCreatedUrl = "",
                        LiquidationConcession = arguments.LiquidationConcession
                    }
                );
                context.SaveChanges();
            }
            return context.PaymentConcession.First(x => x.ConcessionId == concession.Id);
        }
        #endregion CreatePaymentConcession

        #region CreateProfile
        public class CreateProfileDto
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }
            public string Layout { get; set; }
            public string LayoutPro { get; set; }
            public string ImageUrl { get; set; }
        }
        public Profile CreateProfile(PublicContext context, CreateProfileDto arguments)
        {
            context.Profile.AddOrUpdate(x => x.Name,
                new Profile
                {
                    Name = arguments.Name,
                    Color = arguments.Color,
                    Icon = arguments.Icon,
                    Layout = arguments.Layout,
                    LayoutPro = arguments.LayoutPro,
                    ImageUrl = arguments.ImageUrl
                }
            );
            context.SaveChanges();
            return context.Profile.First(x => x.Name == arguments.Name);
        }
        #endregion CreateProfile

        #region CreateSystemCard
        public class CreateSystemCardDto
        {
            public string Name { get; set; }
            public ServiceConcession ConcessionOwner { get; set; }
            public CardType CardType { get; set; }
            public NumberGenerationType NumberGenerationType { get; set; }
            public Profile Profile { get; set; }
            public string ClientId { get; set; } = "";
        }
        public SystemCard CreateSystemCard(PublicContext context, CreateSystemCardDto arguments)
        {
            if (!context.SystemCard.Any(x => x.Name == arguments.Name))
            {
                context.SystemCard.AddOrUpdate(x => x.Name,
                    new SystemCard
                    {
                        Name = arguments.Name,
                        ConcessionOwner = arguments.ConcessionOwner,
                        CardType = arguments.CardType,
                        NumberGenerationType = arguments.NumberGenerationType,
                        Profile = arguments.Profile,
                        PhotoUrl = "https://pbs.twimg.com/media/C9wzMI-XgAA6bGO.jpg",
                        ClientId = arguments.ClientId
                    }
                );
                context.SaveChanges();
            }
            return context.SystemCard.First(x => x.Name == arguments.Name);
        }
        #endregion CreateSystemCard

        #region CreateServiceCardBatch
        public class CreateServiceCardBatchDto
        {
            public string Name { get; set; }
            public SystemCard SystemCard { get; set; }
            public ServiceConcession ServiceConcession { get; set; }
        }
        public ServiceCardBatch CreateServiceCardBatch(PublicContext context, CreateServiceCardBatchDto arguments)
        {
            if (!context.ServiceCardBatch.Any(x => x.Name == arguments.Name))
            {
                context.ServiceCardBatch.AddOrUpdate(x => x.Name,
                    new ServiceCardBatch
                    {
                        Name = arguments.Name,
                        State = ServiceCardBatchState.Active,
                        SystemCard = arguments.SystemCard,
                        Concession = arguments.SystemCard.ConcessionOwner ?? arguments.ServiceConcession
                    }
                );
                context.SaveChanges();
            }
            return context.ServiceCardBatch.First(x => x.Name == arguments.Name);
        }
        #endregion CreateSystemCard

        #region CreateServiceCardMember
        public class CreateServiceCardMemberDto
        {
            public bool CanEmit { get; set; }
            public SystemCard SystemCard { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
        }
        public SystemCardMember CreateServiceCardMember(PublicContext context, CreateServiceCardMemberDto arguments)
        {
            // ServiceCard
            if (!context.SystemCardMember.Any(x =>
                x.SystemCardId == arguments.SystemCard.Id &&
                x.Login == arguments.Login
            ))
            {
                context.SystemCardMember.AddOrUpdate(x => new { x.SystemCardId, x.Login },
                    new SystemCardMember
                    {
                        CanEmit = arguments.CanEmit,
                        SystemCard = arguments.SystemCard,
                        Name = arguments.Name,
                        Login = arguments.Login
                    }
                );
                context.SaveChanges();
            }
            var cardMember = context.SystemCardMember.First(x => x.SystemCardId == arguments.SystemCard.Id && x.Login == arguments.Login);

            return cardMember;
        }
        #endregion CreateServiceUser

        #region CreateServiceUser
        public class CreateServiceUserDto
        {
            public long Uid { get; set; }
            public string Name { get; set; } = "";
            public string LastName { get; set; } = "";
            public string VatNumber { get; set; } = "";
            public string Login { get; set; } = "";
            public string Email { get; set; } = "";
            public long? Phone { get; set; }
            public string Address { get; set; } = "";
            public DateTime? BirthDate { get; set; }
            public string Observations { get; set; } = "";
            public ServiceConcession Concession { get; set; }
            public SystemCard SystemCard { get; set; }
            public ServiceCardBatch ServiceCardBatch { get; set; }
        }
        public ServiceUser CreateServiceUser(PublicContext context, CreateServiceUserDto arguments)
        {
            // ServiceCard
            if (!context.ServiceCard.Any(x => x.Uid == arguments.Uid))
            {
                context.ServiceCard.AddOrUpdate(x => x.Uid,
                    new ServiceCard
                    {
                        Uid = arguments.Uid,
                        Concession = arguments.Concession,
                        SystemCard = arguments.SystemCard,
                        ServiceCardBatch = arguments.ServiceCardBatch,
                        State = ServiceCardState.Active
                    }
                );
                context.SaveChanges();
            }
            var serviceCard = context.ServiceCard.First(x => x.Uid == arguments.Uid);

            // ServiceUser
            if (!context.ServiceUser.Any(x => x.Name == arguments.Name))
            {
                context.ServiceUser.AddOrUpdate(x => x.Name,
                    new ServiceUser
                    {
                        Name = arguments.Name,
                        LastName = arguments.LastName,
                        VatNumber = arguments.VatNumber,
                        Login = arguments.Login,
                        Email = arguments.Email,
                        Phone = arguments.Phone,
                        Address = arguments.Address,
                        BirthDate = arguments.BirthDate,
                        Observations = arguments.Observations,
                        Card = serviceCard,
                        Concession = arguments.Concession,
                        State = ServiceUserState.Active,
                        Photo = "",
                        Code = "",
                        AssertDoc = ""
                    }
                );
                context.SaveChanges();
            }

            return context.ServiceUser.First(x => x.Name == arguments.Name);
        }
        #endregion CreateServiceUser
    }
}
