using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace PayIn.Infrastructure.Sabadell
{
	public static class SabadellGatewayFunctions
	{
		// Payin
		public const string CommerceCodePayIn = "327624508";
		// FACCA
		public const string CommerceCodeFACCA = "344657044";

#if TEST || DEBUG || EMULATOR || HOMO
		public const string CommerceSecretKey256PayIn = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
		public const string CommerceSecretKey256FACCA = "sq7HjrUOBfKmC576ILgskD5srU870gJ7";
#else
		public const string CommerceSecretKey256PayIn = ""; //Cambiar también en SabadellGatewayFunctions
		public const string CommerceSecretKey256FACCA = ""; //Cambiar también en SabadellGatewayFunctions
#endif

		#region CreateWebPaymentRequest
		public static string CreateWebPaymentRequest(int orderId, PaymentMediaCreateType paymentMediaCreateType, string transactionType, string commerceUrl, string commerceCode, string commerceSecretKey, string commerceName, string urlOk, decimal amount, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, string currency = "EUR", string language = "es")
		{
			var orderString =
#if FALLAS
				"F" +
#elif VILAMARXANT
				"V" +
#elif TEST
				"T" +
#elif HOMO
				"H" +
#elif DEBUG
				"D" +
#else
				"_" +
#endif
				orderId.ToString("#0000");
			var amountString = (amount * 100).ToString("#0");
			var currencyString = GetCurrencyToSabadell(currency);
			var commerceNameFull = commerceName;
			var cardIdentifier = "REQUIRED";

			var data = new PaymentData
			{
				PaymentMediaId = paymentMediaId,
				PublicPaymentMediaId = publicPaymentMediaId,
				PublicTicketId = publicTicketId,
				PublicPaymentId = publicPaymentId,
				PaymentMediaCreateType = paymentMediaCreateType
			}.ToJson().ToBase64();

			var api = new RedsysAPI();

			api.SetParameter("DS_MERCHANT_AMOUNT", amountString);
			api.SetParameter("DS_MERCHANT_ORDER", orderString);
			api.SetParameter("DS_MERCHANT_MERCHANTCODE", commerceCode);
			api.SetParameter("DS_MERCHANT_TERMINAL", "1");
			api.SetParameter("DS_MERCHANT_CURRENCY", currencyString);
			api.SetParameter("DS_MERCHANT_TRANSACTIONTYPE", transactionType);
			api.SetParameter("DS_MERCHANT_MERCHANTURL", commerceUrl);
			api.SetParameter("DS_MERCHANT_URLOK", urlOk);
			api.SetParameter("DS_MERCHANT_URLKO", @"\/#\/");
			api.SetParameter("DS_MERCHANT_MERCHANTNAME", commerceNameFull);
			api.SetParameter("DS_MERCHANT_MERCHANTDATA", data);
			api.SetParameter("DS_MERCHANT_IDENTIFIER", cardIdentifier);

			var request = new List<string>();
			request.AddFormat("Ds_SignatureVersion:{0}", "HMAC_SHA256_V1");
			request.AddFormat("Ds_MerchantParameters:{0}", api.createMerchantParameters());
			request.AddFormat("Ds_Signature:{0}", api.createMerchantSignature(commerceSecretKey));

			var result = request.JoinString("&");
			return result;
		}
		#endregion CreateWebPaymentRequest

		#region GetWebPaymentResponse		
		public static PaymentMediaCreateWebCardSabadellArguments GetWebPaymentResponse(string response, string signatureVersion, string signature)
		{
			//Operaciones de conversión previas
			var responseDecoded = response.FromBase64().ToUtf8();
			var data = responseDecoded.FromJson();
			var merchantDataDynamic = Convert.ToString(data["Ds_MerchantData"]);
			string merchantDataString = (string)merchantDataDynamic;
			var paymentDataJson = merchantDataString.FromBase64().ToUtf8().FromJson<PaymentData>();
			var currency = Convert.ToString(data["Ds_Currency"]);
			var expiryDate = Convert.ToString(data["Ds_ExpiryDate"]);

			// Signature	verification process
			if (signature == null)
				throw new ApplicationException("Error obtaining signature");

			// OrderId
			string order = Convert.ToString(data["Ds_Order"]);
			var orderId = Convert.ToInt32(order.Substring(1));

			var arguments = new PaymentMediaCreateWebCardSabadellArguments(
				version: signatureVersion,
				amount: Convert.ToDecimal(data["Ds_Amount"]) / 100,
				currency: GetCurrencyFromSabadell(currency),
				orderId: orderId,
				commerceCode: Convert.ToString(data["Ds_MerchantCode"]),
				terminal: Convert.ToInt32(data["Ds_Terminal"]),
				response: Convert.ToString(data["Ds_Response"]),
				paymentMediaId: Convert.ToInt32(paymentDataJson.PaymentMediaId),
				publicPaymentMediaId: Convert.ToInt32(paymentDataJson.PublicPaymentMediaId),
				publicTicketId: Convert.ToInt32(paymentDataJson.PublicTicketId),
				publicPaymentId: Convert.ToInt32(paymentDataJson.PublicPaymentId),
				paymentMediaCreateType: (PaymentMediaCreateType)Convert.ToInt32(paymentDataJson.PaymentMediaCreateType),
				securePayment: Convert.ToBoolean(Convert.ToInt32(data["Ds_SecurePayment"])),
				expirationMonth: Convert.ToInt32(expiryDate.Substring(2, 2)),
				expirationYear: Convert.ToInt32(expiryDate.Substring(0, 2)), //probar
				cardIdentifier: Convert.ToString(data["Ds_Merchant_Identifier"]),
				transactionType: Convert.ToString(data["Ds_TransactionType"]),
				authorizationCode: Convert.ToString(data["Ds_AuthorisationCode"]),
				signature: Convert.ToString(signature),
				cardNumberHash: Convert.ToString(data["Ds_Card_Number"]),
				isError: false
			);

			string commerceSecretKey =
				arguments.CommerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
				CommerceSecretKey256PayIn;

			RedsysAPI Redsys = new RedsysAPI();
			var signatureGenerated = Redsys.createMerchantSignatureNotif(commerceSecretKey, response);

			if (signatureGenerated != signature)
			{
				arguments.ErrorCode = "PY_HMAC";
				arguments.ErrorText = "Signature verification failure";
				arguments.ErrorPublic = "Signature verification failure";
				arguments.IsError = true;
			}

			var error = CheckError(arguments.Response);
			var errorPublic = ErrorPublic(arguments.Response);
			if (error != null)
			{
				arguments.ErrorCode = "SB_" + error.Value.Key;
				arguments.ErrorText = error.Value.Value;
				arguments.ErrorPublic = errorPublic.Value.Value;
				arguments.IsError = true;
			}

			return arguments;
		}
		#endregion GetWebPaymentResponse

		#region CreatePaymentRequest
		public static string CreatePaymentRequest(string cardIdentifier, string transactionType, string commerceCode, string commerceSecretKey, string commerceName, decimal amount, int? paymentMediaId, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int orderId, PaymentMediaCreateType paymentMediaCreateType, string currency = "EUR", string language = "es")
		{
			var orderString =
#if FALLAS
				"F" +
#elif VILAMARXANT
				"V" +
#elif TEST
				"T" +
#elif HOMO
				"H" +
#elif DEBUG
				"D" +
#else
				"_" +
#endif
				orderId.ToString("#0000");
			var amountString = (amount * 100).ToString("#0");
			var currencyString = GetCurrencyToSabadell(currency);
			var commerceNameFull = commerceName;

			var data = new PaymentData
			{
				PaymentMediaId = paymentMediaId,
				PublicPaymentMediaId = publicPaymentMediaId,
				PublicTicketId = publicTicketId,
				PublicPaymentId = publicPaymentId,
				PaymentMediaCreateType = paymentMediaCreateType
			}.ToJson();

			var datosEntrada = new StringBuilder();
			datosEntrada.AppendFormat("<DATOSENTRADA>");
			//datosEntrada.AppendFormat("<DS_Version>{0}</DS_Version>", "0.1");
			datosEntrada.AppendFormat("<DS_MERCHANT_AMOUNT>{0}</DS_MERCHANT_AMOUNT>", amountString);
			datosEntrada.AppendFormat("<DS_MERCHANT_ORDER>{0}</DS_MERCHANT_ORDER>", orderString);
			datosEntrada.AppendFormat("<DS_MERCHANT_MERCHANTCODE>{0}</DS_MERCHANT_MERCHANTCODE>", commerceCode);
			datosEntrada.AppendFormat("<DS_MERCHANT_TERMINAL>{0}</DS_MERCHANT_TERMINAL>", 1);
			datosEntrada.AppendFormat("<DS_MERCHANT_CURRENCY>{0}</DS_MERCHANT_CURRENCY>", currencyString);
			datosEntrada.AppendFormat("<DS_MERCHANT_MERCHANTNAME>{0}</DS_MERCHANT_MERCHANTNAME>", commerceNameFull);
			datosEntrada.AppendFormat("<DS_MERCHANT_CONSUMERLANGUAGE>{0}</DS_MERCHANT_CONSUMERLANGUAGE>", GetLanguageToSabadell(language));
			datosEntrada.AppendFormat("<DS_MERCHANT_TRANSACTIONTYPE>{0}</DS_MERCHANT_TRANSACTIONTYPE>", transactionType);
			datosEntrada.AppendFormat("<DS_MERCHANT_DATA>{0}</DS_MERCHANT_DATA>", data.ToBase64());
			datosEntrada.AppendFormat("<DS_MERCHANT_IDENTIFIER>{0}</DS_MERCHANT_IDENTIFIER>", cardIdentifier);
			//datosEntrada.AppendFormat("<DS_MERCHANT_DIRECTPAYMENT>{0}</DS_MERCHANT_DIRECTPAYMENT>", "true");
			datosEntrada.AppendFormat("</DATOSENTRADA>");

			//Datos para cálculo de firma
			var apiWs = new RedsysAPIWs();
			string datosEntradaString = datosEntrada.ToString();

			var request = new StringBuilder();
			request.AppendFormat("<REQUEST>");
			request.AppendFormat("{0}", datosEntrada);
			request.AppendFormat("<DS_SIGNATUREVERSION>{0}</DS_SIGNATUREVERSION>", "HMAC_SHA256_V1");
			request.AppendFormat("<DS_SIGNATURE>{0}</DS_SIGNATURE>", HttpUtility.UrlEncode(apiWs.createMerchantSignatureHostToHost(commerceSecretKey, datosEntradaString)));
			request.AppendFormat("</REQUEST>");

			return request.ToString();
		}
		#endregion CreatePaymentRequest

		#region GetPaymentResponse
		public static PaymentMediaPayResult GetPaymentResponse(string data)
		{
			var doc = new XmlDocument();
			doc.LoadXml(data);

			var nodes = doc.DocumentElement;
			var commerceCode = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_MerchantCode");
			var commerceSecretKey =
				commerceCode == CommerceCodeFACCA ? CommerceSecretKey256FACCA :
				CommerceSecretKey256PayIn;
			var arguments = new PaymentMediaPayResult
			{
				Amount = GetXmlInt(nodes, "/RETORNOXML/OPERACION/Ds_Amount") / 100,
				Currency = GetCurrencyFromSabadell(GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Currency")),
				OrderId = GetXmlInt(nodes, "/RETORNOXML/OPERACION/Ds_Order", 1),
				Signature = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Signature"),
				CommerceCode = commerceCode,
				Terminal = GetXmlInt(nodes, "/RETORNOXML/OPERACION/Ds_Terminal"),
				Response = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Response"),
				Code = GetXmlString(nodes, "/RETORNOXML/CODIGO"),
				AuthorizationCode = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_AuthorisationCode"),
				TransactionType = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_TransactionType"),
				SecurePayment = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_SecurePayment") != "0",
				Language = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Language"),
				CardIdentifier = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Merchant_Identifier"),
				MerchantData = HttpUtility.UrlDecode(GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_MerchantData")).FromJson<PaymentMediaPayResult_Data>(),
				CardCountry = GetXmlString(nodes, "/RETORNOXML/OPERACION/Ds_Card_Country"),
				IsError = false
			};

			if (arguments.Code.IsNullOrEmpty() || arguments.Code == "0")
			{
				#region Signature verification
				RedsysAPIWs Redsys = new RedsysAPIWs();
				Redsys.XMLToDiccionary(data);
				var cadena = Redsys.GenerateCadena(data);
				var numOrder = Redsys.GetDictionary("Ds_Order");

				var signatureGenerated = Redsys.createSignatureResponseHostToHost(commerceSecretKey, cadena, numOrder);
				if (signatureGenerated != arguments.Signature)
				{
					arguments.ErrorCode = "PY_HMAC";
					arguments.ErrorText = "Signature verification failure";
					arguments.ErrorPublic = "Signature verification failure";
					arguments.IsError = true;
				}
				#endregion Signature verification
			}
			else
			{
				#region Check error
				var error = CheckError(arguments.Response, arguments.Code);
				var errorPublic = ErrorPublic(arguments.Response, arguments.Code);
				if (error != null)
				{
					arguments.ErrorCode = error.Value.Key;
					arguments.ErrorText = error.Value.Value;
					arguments.ErrorPublic = errorPublic.Value.Value;
					arguments.IsError = true;
				}
				#endregion Check error
			}

			return arguments;
		}
		#endregion GetPaymentResponse

		#region GetCurrencyToSabadell
		public static string GetCurrencyToSabadell(string currency)
		{
			switch (currency.ToLower())
			{
				case "eur": { return "978"; } // 978 – Euro
											  // 978 – Euro
											  // 840 – Dólar
											  // 826 – Libra Esterlina
											  // 392 – Yen
											  // 032 – Peso Argentino
											  // 124 – Dólar Canadiense
											  // 152 – Peso Chileno
											  // 170 – Peso Colombiano
											  // 356 – Rupia India
											  // 484 – Nuevo Peso Mejicano
											  // 604 – Nuevos Soles
											  // 756 – Franco Suizo
											  // 986 – Real Brasileño
											  // 937 – Bolívar Venezolano
											  // 949 – Lira Turca
			}
			return "";
		}
		#endregion GetCurrencyToSabadell

		#region GetCurrencyFromSabadell
		public static string GetCurrencyFromSabadell(string currency)
		{
			switch (currency)
			{
				case "978": { return "EUR"; } // 978 – Euro
											  // 978 – Euro
											  // 840 – Dólar
											  // 826 – Libra Esterlina
											  // 392 – Yen
											  // 032 – Peso Argentino
											  // 124 – Dólar Canadiense
											  // 152 – Peso Chileno
											  // 170 – Peso Colombiano
											  // 356 – Rupia India
											  // 484 – Nuevo Peso Mejicano
											  // 604 – Nuevos Soles
											  // 756 – Franco Suizo
											  // 986 – Real Brasileño
											  // 937 – Bolívar Venezolano
											  // 949 – Lira Turca
			}
			return "";
		}
		#endregion GetCurrencyFromSabadell

		#region GetLanguageToSabadell
		public static string GetLanguageToSabadell(string language)
		{
			switch (language.ToLower())
			{
				case "es": { return "1"; } // 1  – Castellano
										   // 2  – Inglés
										   // 3  – Catalán
										   // 4  – Francés
										   // 5  – Alemán
										   // 6  – Holandés
										   // 7  – Italiano
										   // 8  – Sueco
										   // 9  – Portugués
										   // 10 – Valenciano
										   // 11 – Polaco
										   // 12 – Gallego
										   // 13 – Euskera
				default: { return "0"; } // 0  – Cliente
			}
		}
		#endregion GetLanguageToSabadell

		#region Sha
		public static string Sha(string amountString, string orderString, string commerceCode, string currencyString, string transactionType, string commerceUrl, string cardIdentifier, string commerceSecretKey)
		{
			// Apartado 7.6
			var data = amountString + orderString + commerceCode + currencyString + transactionType + commerceUrl + cardIdentifier + commerceSecretKey;
			var result = Sha(data);
			return result;
		}
		public static string Sha(string data)
		{
			var sha = new SHA1CryptoServiceProvider();
			var result = sha.ComputeHash(Encoding.Default.GetBytes(data.ToCharArray()));

			var stringBuilder = new StringBuilder(40);
			for (int i = 0; i < result.Length; i++)
				stringBuilder.Append(result[i].ToString("x2"));

			return stringBuilder.ToString();
		}
		#endregion Sha

		#region Sha256
		public static string Sha256_(string data, string merchantOrder, string commerceSecretKey)
		{
			var key = TripleDes_IV_0(
				merchantOrder,
				commerceSecretKey.FromBase64()
			);

			using (var hmacSha256 = new HMACSHA256(key))
			{
				var result = hmacSha256
					.ComputeHash(new UTF8Encoding().GetBytes(data))
					.ToBase64();

				hmacSha256.Clear();
				return result;
			}
		}
		public static byte[] TripleDes_IV_0(string data, byte[] key)
		{
			using (var tdes = new TripleDESCryptoServiceProvider())
			{
				tdes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
				tdes.BlockSize = 64;
				tdes.KeySize = 192;
				tdes.Key = key;
				tdes.Mode = CipherMode.CBC;
				tdes.Padding = PaddingMode.Zeros;

				var dataArray = data.FromUtf8();
				var result = tdes.CreateEncryptor()
					.TransformFinalBlock(dataArray, 0, dataArray.Length);

				tdes.Clear();
				return result;
			}
		}
		public static string Sha256(string data, string order, string key)
		{
			// Decode key to byte[]
			byte[] k = Base64Decode(key);

			// Calculate Encoded base64 JSON string with Merchant Parameters 
			//string ent = createMerchantParameters();
			string ent = data;


			// Calculate derivated key by encrypting with 3DES the "DS_MERCHANT_ORDER" with decoded key 
			//byte[] kk = cryp.Encrypt3DES(getOrder(), k);
			byte[] kk = Encrypt3DES(order, k);


			/// Calculate HMAC SHA256 with Encoded base64 JSON string using derivated key calculated previously
			byte[] res = GetHMACSHA256(ent, kk);


			// Encode byte[] res to Base64 String
			string result = Base64Encode2(res);

			return result;

		}
		private static byte[] Base64Decode(string encodedData)
		{

			try
			{
				byte[] encodedDataAsBytes
			  = System.Convert.FromBase64String(encodedData);

				return encodedDataAsBytes;
			}
			catch (FormatException ex)
			{
				throw new FormatException(ex.Message);
			}
		}
		private static string Base64Encode2(byte[] toEncode)
		{
			try
			{
				string returnValue
					  = System.Convert.ToBase64String(toEncode);
				return returnValue;
			}
			catch (FormatException ex)
			{

				throw new FormatException(ex.Message);
			}
		}
		public static byte[] Encrypt3DES(string plainText, byte[] key)
		{
			if (String.IsNullOrEmpty(plainText))
			{
				throw new FormatException();
			}


			byte[] toEncryptArray = Encoding.UTF8.GetBytes(plainText);
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();


			try
			{

				/// <summary>
				/// SALT used in 3DES encryptation process.
				/// </summary>
				byte[] SALT = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

				// Block size 64 bit (8 bytes)
				tdes.BlockSize = 64;

				// Key Size 192 bit (24 bytes)
				tdes.KeySize = 192;
				tdes.Mode = CipherMode.CBC;
				tdes.Padding = PaddingMode.Zeros;


				tdes.IV = SALT;
				tdes.Key = key;

				var cTransform = tdes.CreateEncryptor();

				//transform the specified region of bytes array to resultArray
				byte[] resultArray =
				  cTransform.TransformFinalBlock(toEncryptArray, 0,
				  toEncryptArray.Length);

				//Release resources held by TripleDes Encryptor
				tdes.Clear();

				return resultArray;

			}
			// Error in Cryptographic method
			catch (CryptographicException ex)
			{
				throw new CryptographicException(ex.Message);
			}
		}
		public static byte[] GetHMACSHA256(string msg, byte[] key)
		{
			try
			{
				/// Obtain byte[] from input string 
				byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

				// Initialize the keyed hash object.
				using (HMACSHA256 hmac = new HMACSHA256(key))
				{

					//Compute the hash of the input file.
					byte[] hashValue = hmac.ComputeHash(msgBytes, 0, msgBytes.Length);
					return hashValue;
				}
			}
			// Error in crytographic process  
			catch (CryptographicException ex)
			{
				throw new CryptographicException(ex.Message);
			}
		}

		//public static string Sha256(string data)
		//{
		//	var sha = new SHA256CryptoServiceProvider();
		//	byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(data));

		//	var stringBuilder = new StringBuilder();
		//	for (int i = 0; i < result.Length; i++)
		//		stringBuilder.Append(result[i].ToString("x2").ToLower());

		//	return stringBuilder.ToString();
		//}
		#endregion Sha256

		#region GetXmlString
		private static string GetXmlString(XmlNode node, string path, int startIndex = 0)
		{
			var element = node.SelectSingleNode(path);
			if (element == null)
				return "";

			var text = element.InnerText;
			if (text.IsNullOrEmpty())
				return "";
			text = text.Substring(startIndex);

			return text;
		}
		#endregion GetXmlString

		#region GetXmlInt
		private static int GetXmlInt(XmlNode node, string path, int startIndex = 0)
		{
			var text = GetXmlString(node, path, startIndex);

			return text.IsNullOrEmpty() ? 0 : Convert.ToInt32(text);
		}
		#endregion GetXmlInt

		#region CheckError
		public static KeyValuePair<string, string>? CheckError(string response, string code = "0")
		{
			if (code != "0")
				return new KeyValuePair<string, string>(code, SabadellErrors.ResourceManager.GetString(code) ?? "");
			else
			{
				var resp = Convert.ToInt32(response);
				if ((resp > 99) && (resp != 900))
				{
					return new KeyValuePair<string, string>(response, SabadellErrors.ResourceManager.GetString("Response_" + response) ?? "");

				}
			}

			return null;
		}
		#endregion CheckError

		#region ErrorPublic
		public static KeyValuePair<string, string>? ErrorPublic(string response, string code = "0")
		{
			if (code != "0")
				return new KeyValuePair<string, string>(code, SabadellErrors.ResourceManager.GetString(code) ?? "");
			else
			{
				var resp = Convert.ToInt32(response);
				if ((resp > 99) && (resp != 900))
				{
					if ((resp == 101) || (resp == 201))
					{
						//Tarjeta caducada
						return new KeyValuePair<string, string>(response, SabadellErrors.ResourceManager.GetString("Response_" + response) ?? "");
					}
					else if ((resp == 116))
					{
						//Saldo insuficiente
						return new KeyValuePair<string, string>(response, "Saldo insuficiente");

					}
					else if ((resp == 129) || (resp == 280))
					{
						//CVV o CCV incorrecto
						return new KeyValuePair<string, string>(response, SabadellErrors.ResourceManager.GetString("Response_" + response) ?? "");

					}
					else
					{
						//Error general
						return new KeyValuePair<string, string>(response, "Error en la transacción");

					}

				}
			}

			return null;
		}
		#endregion ErrorPublic

		#region RebuildBase64
		//Añade 0 en cadena Base64 si hace falta
		public static string RebuildBase64(string data)
		{
			//calcular bits de ASCII a Base64
			var overflowBits = (data.Length * 4) % 3;

			if (overflowBits == 1)
				return data + '=';

			else if (overflowBits == 2)
				return data + "==";

			else return data;
		}
		#endregion RebuildBase64
	}
}
