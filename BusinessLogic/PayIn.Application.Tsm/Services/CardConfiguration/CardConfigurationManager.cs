using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace PayIn.Application.Tsm.Services.CardConfiguration
{
	public class CardConfigurationManager
	{
		public LinkedList<CardConfig> CardsList;
		public ConfigUrlList ConfList;

		public CardConfigurationManager()
		{
			CardsList = new LinkedList<CardConfig>();
			UpdateCardConfigs();
		}

		public bool UpdateCardConfigs()
		{
			var cardConfig = new CardConfig("Mobilis", "FFFFFFFFFFFF", "08778f00", "");
			cardConfig.checkSyntax();
			if (cardConfig.Valid)
				CardsList.AddLast(cardConfig);

			/*
			Url configFileUrl;

			try
			{
				configFileUrl = new URL(M4mServletMain.baseUrl + "Cards/cards.xml");

				XStream xstream = new XStream();

				xstream.alias("CardConfigurationUrls", ConfigUrlList.class);
				xstream.alias("ConfigurationUrl", String.class);
	
				confList = (ConfigUrlList) xstream.fromXML(configFileUrl);
			
				xstream.alias("CardConfig", CardConfig.class);

				for (String cardConfigUrl : confList.getUrlList()) {
					try {
						configFileUrl = new URL(M4mServletMain.baseUrl + cardConfigUrl + "ServiceObjectConfig.xml");
						CardConfig cardConfig = (CardConfig)xstream.fromXML(configFileUrl);
						cardConfig.checkSyntax();
						if (cardConfig.isValid()) {
							cardsList.add(cardConfig);
						}
					} catch (Exception e) {
						e.getMessage();
						continue;
					}
				}
			} catch (Exception e) {
				e.getMessage();
				return false;
			}*/
			return true;
		}

		public CardConfig GetCardConfig(string label)
		{
			return CardsList
				.Where(x => x.Label.ToLower() == label.ToLower())
				.FirstOrDefault();
		}
	}
}
