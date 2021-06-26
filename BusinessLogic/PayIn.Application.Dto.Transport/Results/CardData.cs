using PayIn.Application.Dto.Results;
using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results
{
	public class CardData
    {
        public class Log
        {
            public DateTime? Date { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
            public string TypeName { get; set; }
            public string TitleName { get; set; }
            public string TitleOwnerName { get; set; }
            public EigeZonaEnum? TitleZone { get; set; }
            public int? Code { get; set; }
            public EigeZonaHistoricoEnum? Zone { get; set; }
            public decimal? Quantity { get; set; }
            public string QuantityUnits { get; set; }
            public bool HasBalance { get; set; }
            public string Place { get; set; }
            public string Operator { get; set; }

            public Log() { }
            public Log(ServiceCardReadInfoResult_Log item)
            {
                Date = item.Date;
                TypeName = item.TypeName;
                TitleName = item.TitleName;
                TitleOwnerName = item.TitleOwnerName;
                TitleZone = item.TitleZone;
                Code = item.Code;
                Zone = item.Zone;
                Quantity = item.Quantity;
                QuantityUnits = item.QuantityUnits;
                HasBalance = item.HasBalance;
                Place = item.Place;
                Operator = item.Operator;
            }
        }
        public class Charge
        {
            public DateTime? Date { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
            public string TypeName { get; set; }
            public string TitleOwnerName { get; set; }
            public string TitleName { get; set; }
            public EigeZonaEnum? TitleZone { get; set; }
            public decimal? Quantity { get; set; }
            public MeanTransportEnum? MeanTransport { get; set; }

            public Charge() { }
            public Charge(ServiceCardReadInfoResult_Charge item)
            {
                Date = item.Date;
                TypeName = item.TypeName;
                TitleOwnerName = item.TitleOwnerName;
                TitleName = item.TitleName;
                TitleZone = item.TitleZone;
                Quantity = item.Quantity;
                MeanTransport = item.MeanTransport;
            }
        }
        public class RechargeTitle
        {
            public int Id { get; set; }
            public int Code { get; set; }
            public string Name { get; set; }
            public int PaymentConcessionId { get; set; }
            public int TransportConcession { get; set; }
            public string OwnerName { get; set; }
            public string OwnerCity { get; set; }
            public IEnumerable<RechargePrice> Prices { get; set; }
			public decimal? RechargeMax { get; set; }
			public decimal? MaxQuantity { get; set; }
            public decimal? RechargeMin { get; set; }
            public decimal? RechargeStep { get; set; }
            public decimal? Quantity { get; set; }
            public decimal? QuantityInverse { get; set; }
            public bool AskQuantity { get; set; }
			public MeanTransportEnum? MeanTransport { get; set; }

            public RechargeTitle() { }
            public RechargeTitle(ServiceCardReadInfoResult_RechargeTitle item)
            {
                Id = item.Id;
                Code = item.Code;
                Name = item.Name;
                PaymentConcessionId = item.PaymentConcessionId;
                TransportConcession = item.TransportConcession;
                OwnerName = item.OwnerName;
                OwnerCity = item.OwnerCity;
                Prices = item.Prices
                    .Select(y => new CardData.RechargePrice(y));
                MaxQuantity = item.MaxQuantity;
				RechargeMax = item.RechargeMax;
                RechargeMin = item.RechargeMin;
                RechargeStep = item.RechargeStep;
				Quantity = item.Quantity;
                QuantityInverse = item.QuantityInverse;
                AskQuantity = item.AskQuantity;
				MeanTransport = item.MeanTransport;
            }
        }
        public class RechargePrice
        {
            public int Id { get; set; }
            public EigeZonaEnum? Zone { get; set; }
            public string ZoneName { get; set; }
            public decimal Price { get; set; }
            public decimal ChangePrice { get; set; }
            public RechargeType RechargeType { get; set; }
            public EigeTituloEnUsoEnum? Slot { get; set; }

            public RechargePrice() { }
            public RechargePrice(ServiceCardReadInfoResult_RechargePrice item)
            {
                Id = item.Id;
                Zone = item.Zone;
                ZoneName = item.ZoneName;
                Price = item.Price;
                ChangePrice = item.ChangePrice;
                RechargeType = item.RechargeType;
                Slot = item.Slot;
            }
        }
        public class PromotionPrice
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Code { get; set; }
            public EigeZonaEnum? Zone { get; set; }
            public bool HasZone { get; set; }

            public PromotionPrice() { }
            public PromotionPrice(ServiceCardReadInfoResult_PromotionPrice item)
            {
                Id = item.Id;
                Name = item.Name;
                Code = item.Code;
                Zone = item.Zone;
                HasZone = item.HasZone;
            }
        }
        public class PromotionAction
        {
            public PromoActionType Type { get; set; }
            public int Quantity { get; set; }

            public PromotionAction() { }
            public PromotionAction(ServiceCardReadInfoResult_PromotionAction item)
            {
                Type = item.Type;
                Quantity = item.Quantity;
            }
        }
        public class Promotion
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? EndDate { get; set; }
            public string Concession { get; set; }
            public string Image { get; set; }
            public IEnumerable<RechargeTitle> Titles { get; set; }
            public IEnumerable<PromotionAction> Actions { get; set; }

            public Promotion() { }
            public Promotion(ServiceCardReadInfoResult_Promotion item)
            {
                Id = item.Id;
                Name = item.Name;
                EndDate = item.EndDate;
                Concession = item.Concession;
                Image = item.Image;
                Titles = item.Titles
                    .Select(x => new RechargeTitle(x));
                Actions = item.Actions
                    .Select(x => new PromotionAction(x));
            }
        }
        public class Concession
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string VatNumber { get; set; }
            public string Email { get; set; }

            public Concession() { }
            public Concession(ServiceCardReadInfoResult_Concession item)
            {
                Id = item.Id;
                Name = item.Name;
                Address = item.Address;
                VatNumber = item.VatNumber;
                Email = item.Email;
            }
        }
        public class Title
        {
            public int? Code { get; set; }
            public SlotEnum Slot { get; set; }
            public string Name { get; set; }
            public string OwnerName { get; set; }
            public EigeZonaEnum? Zone { get; set; }
            public DateTime? Caducity { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
            public bool IsRechargable { get; set; }
            public bool HasTariff { get; set; }
            public bool IsExhausted { get; set; }
            public bool IsExpired { get; set; }
            // Balance
            public bool HasBalance { get; set; }
            public decimal? Balance { get; set; }
            public decimal? BalanceAcumulated { get; set; }
            public string BalanceUnits { get; set; }
            // Temporal
            public bool IsTemporal { get; set; }
            public DateTime? ExhaustedDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
            public DateTime? ActivatedDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
            public int? Ampliation { get; set; }
            public int? AmpliationQuantity { get; set; }
            public string AmpliationUnits { get; set; }
            // Recharges
            public RechargeTitle RechargeTitle { get; set; }
            public List<string> RechargeImpediments { get; set; }
            // Others
            public bool ReadAll { get; set; }
            public int? LastRechargeOperationId { get; set; }
            public dynamic Operation { get; set; }
            public MeanTransportEnum? MeanTransport { get; set; }

            public Title() { }
            public Title(ServiceCardReadInfoResult item)
            {
                Code = item.Code;
                Slot = item.Slot;
                Name = item.Name;
                OwnerName = item.OwnerName;
                Zone = item.Zone;
                Caducity = item.Caducity;
                IsRechargable = item.IsRechargable;
                HasTariff = item.HasTariff;
                IsExhausted = item.IsExhausted;
                IsExpired = item.IsExpired;
                // Balance
                HasBalance = item.HasBalance;
                Balance = item.Balance;
                BalanceAcumulated = item.BalanceAcumulated;
                BalanceUnits = item.BalanceUnits;
                // Temporal
                IsTemporal = item.IsTemporal;
                ExhaustedDate = item.ExhaustedDate;
                ActivatedDate = item.ActivatedDate;
                Ampliation = item.Ampliation;
                AmpliationQuantity = item.AmpliationQuantity;
                AmpliationUnits = item.AmpliationUnits;
                // Others
                ReadAll = item.ReadAll;
                LastRechargeOperationId = item.LastRechargeOperationId;
                Operation = item.Operation;
                MeanTransport = item.MeanTransport;
                // Recharges
                RechargeTitle = item.RechargeTitle == null ? null : new CardData.RechargeTitle(item.RechargeTitle);
                RechargeImpediments = item.RechargeImpediments;
            }
        }

        public string CardId { get; set; }
        public int? Owner { get; set; }
        public string OwnerName { get; set; }
        public string TypeName { get; set; }
        public XpDate ExpiredDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
        public bool InBlackList { get; set; }
        public bool IsExpired { get; set; }
        public bool IsRechargable { get; set; }
        public bool IsRevokable { get; set; }
        public decimal? RevokablePrice { get; set; }
        public bool HasHourValidity { get; set; }
        public bool HasDayValidity { get; set; }
        public bool ApproximateValues { get; set; }
        public bool IsPersonalized { get; set; }
        public bool IsDamaged { get; set; }
        public IEnumerable<Log> Logs { get; set; }
        public IEnumerable<Charge> Charges { get; set; }
        public IEnumerable<RechargeTitle> RechargeTitles { get; set; }
        public IEnumerable<RechargeTitle> ChargeTitles { get; set; }
        public IEnumerable<Title> Titles { get; set; }
        public long? UserCode { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserDni { get; set; }
        public string UserPhoto { get; set; }
        public DateTime? LastValidationDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
        public string LastValidationTypeName { get; set; }
        public string LastValidationPlace { get; set; }
        public string LastValidationOperator { get; set; }
        public EigeZonaHistoricoEnum? LastValidationZone { get; set; }
        public string LastValidationTitleName { get; set; }
        public string LastValidationTitleOwnerName { get; set; }
        public EigeZonaEnum? LastValidationTitleZone { get; set; }
        public int? PeopleTraveling { get; set; }
        public int? PeopleInTransfer { get; set; }
        public int? MaxPeopleInTransfer { get; set; }
        public int? InternalTransfers { get; set; }
        public int? ExternalTransfers { get; set; }
        public int? MaxInternalTransfers { get; set; }
        public int? MaxExternalTransfers { get; set; }
        public int? Mode { get; set; }
        public EigeFechaPersonalizacion_DispositivoEnum? DeviceType { get; set; }
        public int? LastRechargeOperationId { get; set; }
        public IEnumerable<Promotion> Promotions { get; set; }
        public IEnumerable<Concession> Concessions { get; set; }
        public MeanTransportEnum? MeanTransport { get; set; }

        public CardData() { }
        public CardData(ServiceCardReadInfoResultBase resultReadInfo)
        {
            // General
            CardId = resultReadInfo.CardId;
            Owner = resultReadInfo.Owner;
            OwnerName = resultReadInfo.OwnerName;
            TypeName = resultReadInfo.TypeName;
            ExpiredDate = resultReadInfo.ExpiredDate;
            InBlackList = resultReadInfo.InBlackList;
            IsExpired = resultReadInfo.IsExpired;
            IsRechargable = resultReadInfo.IsRechargable;
            IsRevokable = resultReadInfo.IsRevokable;
            RevokablePrice = resultReadInfo.RevokablePrice;
            HasHourValidity = resultReadInfo.HasHourValidity;
            HasDayValidity = resultReadInfo.HasDayValidity;
            ApproximateValues = resultReadInfo.ApproximateValues;
            IsPersonalized = resultReadInfo.IsPersonalized;
            IsDamaged = resultReadInfo.IsDamaged;
            UserCode = resultReadInfo.UserCode;
            UserName = resultReadInfo.UserName;
            UserSurname = resultReadInfo.UserSurname;
            UserDni = resultReadInfo.UserDni;
            UserPhoto = resultReadInfo.UserPhoto;
            LastValidationDate = resultReadInfo.LastValidationDate;
            LastValidationTypeName = resultReadInfo.LastValidationTypeName;
            LastValidationPlace = resultReadInfo.LastValidationPlace;
            LastValidationOperator = resultReadInfo.LastValidationOperator;
            LastValidationZone = resultReadInfo.LastValidationZone;
            LastValidationTitleName = resultReadInfo.LastValidationTitleName;
            LastValidationTitleOwnerName = resultReadInfo.LastValidationTitleOwnerName;
            LastValidationTitleZone = resultReadInfo.LastValidationTitleZone;
            PeopleTraveling = resultReadInfo.PeopleTraveling;
            PeopleInTransfer = resultReadInfo.PeopleInTransfer;
            MaxPeopleInTransfer = resultReadInfo.MaxPeopleInTransfer;
            InternalTransfers = resultReadInfo.InternalTransfers;
            ExternalTransfers = resultReadInfo.ExternalTransfers;
            MaxInternalTransfers = resultReadInfo.MaxInternalTransfers;
            MaxExternalTransfers = resultReadInfo.MaxExternalTransfers;
            Mode = resultReadInfo.Mode;
            DeviceType = resultReadInfo.DeviceType;
            LastRechargeOperationId = resultReadInfo.LastRechargeOperationId;
            MeanTransport = resultReadInfo.MeanTransport;
            Logs = resultReadInfo.Logs == null ? null : resultReadInfo.Logs
                .Select(x => new CardData.Log(x));
            Charges = resultReadInfo.Charges == null ? null : resultReadInfo.Charges
                .Select(x => new CardData.Charge(x));
            RechargeTitles = resultReadInfo.RechargeTitles == null ? null : resultReadInfo.RechargeTitles
                .Select(x => new CardData.RechargeTitle(x));
            ChargeTitles = resultReadInfo.ChargeTitles == null ? null : resultReadInfo.ChargeTitles
                .Select(x => new CardData.RechargeTitle(x));
            Promotions = resultReadInfo.Promotions == null ? null : resultReadInfo.Promotions
                .Select(x => new CardData.Promotion(x));
            Concessions = resultReadInfo.Concessions == null ? null : resultReadInfo.Concessions
                .Select(x => new CardData.Concession(x));
            Titles = resultReadInfo.Data == null ? null : resultReadInfo.Data
                .Select(x => new CardData.Title(x));
        }
    }
}
