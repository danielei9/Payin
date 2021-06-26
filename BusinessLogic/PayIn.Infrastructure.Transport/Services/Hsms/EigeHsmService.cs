using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Infrastructure.Transport.Services.Hsms
{
    public class EigeHsmService : IMifareClassicHsmService
    {
        private static string KeyNamePayin = "MobileKey_Eige";
        private static string KeyNameFgv = "MobileKey_Fgv";

        public string GetKeysName(CardSystem cardSystem)
        {
            return 
				cardSystem == CardSystem.FgvAlicante ? ConfigurationManager.AppSettings["Secret_FgvAlicante"] :
				cardSystem == CardSystem.Philips     ? ConfigurationManager.AppSettings["Secret_Philips"] :
				cardSystem == CardSystem.Vilamarxant ? ConfigurationManager.AppSettings["Secret_Vilamarxant"] :
										               ConfigurationManager.AppSettings["Secret_Eige"];
        }
        public string GetTransportKeyName()
        {
            return (
                (SessionData.ClientId == AccountClientId.FgvTsm) ||
                (SessionData.ClientId == AccountClientId.AlacantTsm) 
            ) ?
                KeyNameFgv :
                KeyNamePayin;
        }

        private const string METHOD_NAME = "EigeHsmService";

        private class EigeMifareKey
        {
            public EigeMifareKeyId Key { get; set; }
            public string Value { get; set; }
            public string Diversify { get; set; }
        }
        private class EigeMifareKeyId
        {
            public byte Sector { get; set; }
            public string Type { get; set; }
        }

        private static KeyVaultClient keyVaultClient;
        private static ClientCredential clientCredential;
        private readonly ISessionData SessionData;
        private static string vaultUrl;

        private static Dictionary<CardSystem, IEnumerable<EigeMifareKey>> KeysCache = new Dictionary<CardSystem, IEnumerable<EigeMifareKey>>();
        private async Task<IEnumerable<EigeMifareKey>> GetKeysAsync(CardSystem cardSystem)
        {
            if (!KeysCache.ContainsKey(cardSystem))
            {
                var secretUri = GetKeysName(cardSystem);
                if (secretUri == null)
                    throw new Exception("secretUri error");

                var secret = await keyVaultClient.GetSecretAsync(secretUri);
                KeysCache.Add(cardSystem, secret.Value.FromJson<IEnumerable<EigeMifareKey>>());
            }
            return KeysCache[cardSystem];
        }

        #region Constructors
        public EigeHsmService(ISessionData sessionData)
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            SessionData = sessionData;

            // The key specification and attributes
            vaultUrl = ConfigurationManager.AppSettings["HSMKeyVaultUrl"];
            var clientId = ConfigurationManager.AppSettings["HSMClientId"];
            var clientSecret = ConfigurationManager.AppSettings["HSMClientSecret"];
            clientCredential = new ClientCredential(clientId, clientSecret);

            keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetADTokenAsync));
        }
        #endregion Constructors

        #region EncryptAsync
        public async Task<byte[]> EncryptAsync(byte[] value, string keyName)
        {
            return await Task.Run(async () =>
            {
                var keyUrl = ConfigurationManager.AppSettings[keyName];
                if (keyUrl == null)
                    throw new Exception("Key Url don't exists in App.Config file");
                // http://web.townsendsecurity.com/bid/29195/How-Much-Data-Can-You-Encrypt-with-RSA-Keys
                var temp = value.Take(214).ToArray();
                var next = value.Skip(214);

                var result = new List<byte>();
                while (temp.Count() > 0)
                {
                    var operationResult = await keyVaultClient.EncryptAsync(keyUrl, JsonWebKeyEncryptionAlgorithm.RSAOAEP, temp);
                    result.AddRange(operationResult.Result);

                    temp = next.Take(214).ToArray();
                    next = next.Skip(214);
                }

                return result.ToArray();
            });
        }
        #endregion EncryptAsync

        //Método para proyecto de prueba Return keyOperationResult
        #region EncryptKeyOperationResultAsync
        private async Task<KeyOperationResult> EncryptKeyOperationResultAsync(string text, string keyName)
        {
            // Se debe devolver el texto cifrado con la clave pública keyName
            var keyUrl = ConfigurationManager.AppSettings[keyName];
            if (keyUrl == null)
                throw new Exception("Key Url don't exists in App.Config file");

            var operationResult = await keyVaultClient.EncryptAsync(keyUrl, JsonWebKeyEncryptionAlgorithm.RSAOAEP, text.FromUtf8());

            return operationResult;
        }
        #endregion EncryptKeyOperationResultAsync

        #region DecryptAsync
        public async Task<string> DecryptAsync(string text, string keyName)
        {
            // Se debe devolver el texto descifrado con la clave privada keyName
            var keyUrl = ConfigurationManager.AppSettings[keyName];
            if (keyUrl == null)
                throw new Exception("Key Url don't exists in App.Config file");
            var keyBundle = await keyVaultClient.GetKeyAsync(keyUrl);

            var textByte = Convert.FromBase64String(text);
            var operationResult = await keyVaultClient.DecryptDataAsync(keyBundle, JsonWebKeyEncryptionAlgorithm.RSAOAEP, textByte);

            return Convert.ToBase64String(operationResult.Result);
        }
        #endregion DecryptAsync

        //Decrypt return keyOperationResult
        #region DecryptKeyOperationReturnAsync
        private async Task<KeyOperationResult> DecryptKeyOperationReturnAsync(string text, string keyName)
        {
            // Se debe devolver el texto descifrado con la clave privada keyName
            var keyUrl = ConfigurationManager.AppSettings[keyName];
            if (keyUrl == null)
                throw new Exception("Key Url don't exists in App.Config file");
            var keyBundle = await keyVaultClient.GetKeyAsync(keyUrl);

            var textByte = Convert.FromBase64String(text);
            var operationResult = await keyVaultClient.DecryptDataAsync(keyBundle, JsonWebKeyEncryptionAlgorithm.RSAOAEP, textByte);

            return operationResult;
        }
        #endregion DecryptKeyOperationReturnAsync

        #region GetKeyEncryptedAsync
        public async Task<string> GetKeyEncryptedAsync(int sector, MifareKeyType keyType)
        {
            var sectorString = sector.ToString("D2"); // Formato decimal con 2 dígitos
            var eigekeyName = "EIGE" + sectorString + keyType;

            var secretUri = ConfigurationManager.AppSettings[eigekeyName];
            if (secretUri == null)
                throw new Exception("secretUri error");

            var secret = await keyVaultClient.GetSecretAsync(secretUri);
            var encrypted = await EncryptAsync(secret.Value.ToBytes(), GetTransportKeyName());
            var result = Convert.ToBase64String(encrypted);

            return result;
        }
        #endregion GetKeyEncryptedAsync

        //Secret contains keys in JSON format
        #region GetKeysEncryptedAsync
        public async Task<string> GetKeysEncryptedAsync(CardSystem cardSystem, IEnumerable<MifareKeyId> keys, long uid, int aux)
        {
#if DEBUG
            var watch = Stopwatch.StartNew();
#endif
            if (keys.Count() == 0)
            {
                var result = Convert.ToBase64String(await EncryptAsync("[]".FromUtf8(), GetTransportKeyName()));
#if DEBUG
                watch.Stop();
                Debug.WriteLine(METHOD_NAME + "GetKeysEncryptedAsync: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
                watch = Stopwatch.StartNew();
#endif

                return result;
            }

            var secretList = await GetKeysAsync(cardSystem);
#if DEBUG
            watch.Stop();
            Debug.WriteLine(METHOD_NAME + "GetKeysEncryptedAsync - Loading keys: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
            watch = Stopwatch.StartNew();
#endif

            var selectedList = keys
                .Select(x => new
                {
                    Key = x.Sector + x.Type.ToString(),
                    Value = secretList
                        .Where(y =>
                            y.Key.Sector == x.Sector &&
                            y.Key.Type == x.Type.ToString()
                        )
                        .Select(y =>
                            y.Diversify.IsNullOrEmpty() ?
                               y.Value :
                               Diversify(
                                  y,
                                  secretList
                                      .Where(z =>
                                         z.Key.Sector == y.Key.Sector &&
                                         z.Key.Type != y.Key.Type
                                      )
                                      .FirstOrDefault(),
                                  uid,
                                  aux)
                         )
                        .FirstOrDefault()
                })
                .ToList();
            var selected = selectedList.ToJson();
            var symbols = selected.FromUtf8();
#if DEBUG
            watch.Stop();
            Debug.WriteLine(METHOD_NAME + "GetKeysEncryptedAsync - Ordering and serializing keys: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
            watch = Stopwatch.StartNew();
#endif

            var encrypted = await EncryptAsync(symbols, GetTransportKeyName());
#if DEBUG
            watch.Stop();
            Debug.WriteLine(METHOD_NAME + "GetKeysEncryptedAsync - Encrypting keys: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
            watch = Stopwatch.StartNew();
#endif

            return Convert.ToBase64String(encrypted);
        }
        #endregion GetKeysEncryptedAsync

        //GetKeyEncrypt return KeyOperationResult
        #region GetKeyEncryptedKeyOperationResultAsync
        private async Task<KeyOperationResult> GetKeyEncryptedKeyOperationResultAsync(int sector, MifareKeyType keyType, string keyName)
        {
            //Obtencion de URI - Implementar
            var sectorString = sector.ToString("D2"); // Formato decimal con 2 dígitos
            var eigekeyName = "EIGE" + sectorString + keyType;

            var secretUri = ConfigurationManager.AppSettings[eigekeyName];
            if (secretUri == null)
                throw new Exception("secretUri error");

            var result = keyVaultClient.GetSecretAsync(secretUri).Result.Value;
            var operationResult = await EncryptKeyOperationResultAsync(result, keyName);

            return operationResult;
        }
        #endregion GetKeyEncryptedKeyOperationResultAsync

        #region SignAsync
        public async Task<string> SignAsync(string text, string keyName)
        {
            // Se debe devolver el hash firmado del texto con la clave privada keyName
            var keyUri = keyVaultClient.GetKeyAsync(keyName).Result.Key.Kid;
            var algorithm = JsonWebKeySignatureAlgorithm.RS256;
            var digest = GetSHA256Hash(text);

            var operationResult = await keyVaultClient.SignAsync(keyUri, algorithm, digest);

            return Convert.ToBase64String(operationResult.Result);
        }
        #endregion SignAsync

        #region SignKeyOperationResultAsync
        private async Task<KeyOperationResult> SignKeyOperationResultAsync(string text, string keyName)
        {
            // Se debe devolver el hash firmado del texto con la clave privada keyName
            var keyUri = keyVaultClient.GetKeyAsync(keyName).Result.Key.Kid;
            var algorithm = JsonWebKeySignatureAlgorithm.RS256;
            var digest = GetSHA256Hash(text);

            var operationResult = await keyVaultClient.SignAsync(keyUri, algorithm, digest);

            return operationResult;
        }
        #endregion SignKeyOperationResultAsync

        #region ValidateAsync
        public async Task<bool> ValidateAsync(string text, string sign, string keyName)
        {
            // Se debe devolver comprobar que el hash firmado del texto con la clave pública keyName coincide con la firma que se pasa
            var keyUri = keyVaultClient.GetKeyAsync(keyName).Result.Key.Kid;
            var algorithm = JsonWebKeySignatureAlgorithm.RS256;
            var digest = GetSHA256Hash(text);
            var signBytes = Convert.FromBase64String(sign);

            return await keyVaultClient.VerifyAsync(keyUri, algorithm, digest, signBytes);
        }
        #endregion ValidateAsync

        #region ValidateKeyOperationResultAsync
        private async Task<bool> ValidateKeyOperationResultAsync(string text, string sign, string keyName)
        {
            // Se debe devolver comprobar que el hash firmado del texto con la clave pública keyName coincide con la firma que se pasa
            var keyUri = keyVaultClient.GetKeyAsync(keyName).Result.Key.Kid;
            var algorithm = JsonWebKeySignatureAlgorithm.RS256;
            var digest = GetSHA256Hash(text);
            var signBytes = Convert.FromBase64String(sign);

            return await keyVaultClient.VerifyAsync(keyUri, algorithm, digest, signBytes);
        }
        #endregion ValidateKeyOperationResultAsync

        #region GetADTokenAsync
        private async Task<string> GetADTokenAsync(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            var clientCred = new ClientCredential(
                ConfigurationManager.AppSettings["HSMClientId"],
                ConfigurationManager.AppSettings["HSMClientSecret"]
            );
            var result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
        #endregion GetADTokenAsync

        #region GetSHA256Hash
        private byte[] GetSHA256Hash(string text)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
            var textBytes = Encoding.UTF8.GetBytes(text);
            var hashBytes = provider.ComputeHash(textBytes);

            return hashBytes;
        }
        #endregion GetSHA256Hash

        #region Diversify
        private string Diversify(EigeMifareKey key, EigeMifareKey otherKey, long uid, int aux)
        {
            if (key.Diversify == "Tuin")
            {
                //Inicializar variables.
                byte[] ED = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] KE = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] keyBd = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] RE = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                //TNS es el número de serie de la tarjeta. Este número es de la tarjeta de ejemplo. Lo sacamos a partir del uid
                //Lo hacemos para 4 bytes ya que es la situación actual de tarjetas Móbilis
                byte[] TNS = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                string TNSstr = "";
                string auxString = "";
                string finalDiversify = "";

                var numberhex = uid.ToHexadecimalBE();
                TNSstr = numberhex.ToString();
                TNS = StringToByteArray(TNSstr);


                //Calculo de ED según FGV. ED = Dato a encriptar
                ED = EDCalc(TNS);
                auxString = toHEX(ED);
                //Calculo de KE según FGV. Se genera según punto 7.1.1.
                KE = KECalc(otherKey.Value.FromHexadecimal(), key.Value.FromHexadecimal(), TNS, aux);
                auxString = toHEX(KE);

                //Llamada para recibir la nueva clave RE y de la clave diversificada KeyBd
                //RE = EncryptDES3_CBC(ED, KE);
                RE = XpSecurity.Cypher_DESede_CBC_NoPadding(KE, ED);

                for (int i = 0; i <= 5; i++)
                {
                    //La clave diversificada serán los primeros 6 bytes del resultado de cifrado.
                    keyBd[i] = RE[i];
                }

                finalDiversify = toHEX(keyBd);

                return finalDiversify;
            }

            return null;
        }

        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        private byte[] EDCalc(byte[] uidCard)
        {
            byte[] EDCalcul = new byte[] { uidCard[0], uidCard[1], uidCard[2], uidCard[3], 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            for (int i = 0; i <= 11; i++)
            {
                //Calculamos la XOR para la obtención del dato a Encriptar. Se extrae del UID de la tarjeta.
                EDCalcul[i + 4] = (byte)(EDCalcul[i] ^ EDCalcul[i + 1]);
            }

            return EDCalcul;
        }

        private byte[] KECalc(byte[] _KeyA, byte[] _KeyB, byte[] uidCard, int _MVC)
        {
            //Preasignamos los valores fijos que son los del UID y que siempre van a estar en la misma posición
            byte[] KECalcul = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, uidCard[0], uidCard[1], uidCard[2], uidCard[3] };

            _KeyA = rotar(_KeyA, _MVC);
            _KeyB = rotar(_KeyB, _MVC);

            for (int i = 0; i <= 5; i++)
            {
                //Introducimos la KeyA en la posición 6 a 11
                KECalcul[i + 6] = _KeyA[i];
                //Introducimos la KeyB en la posición 0 a 5.
                KECalcul[i] = _KeyB[i];
            }

            return KECalcul;
        }

        /// <summary>
        /// Encryption using DES3 algorithm in CBC mode 
        /// </summary>
        /// <param name="message">Input message bytes</param>
        /// <returns>Encrypted message bytes</returns>
        //public byte[] EncryptDES3_CBC(byte[] message, byte[] keyDES3)
        //{
        //DesEdeEngine desedeEngine = new DesEdeEngine();
        //BufferedBlockCipher bufferedCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(desedeEngine));
        //KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("DESEDE", keyDES3);
        //byte[] output = new byte[bufferedCipher.GetOutputSize(message.Length)];
        //bufferedCipher.Init(true, keyparam);
        //output = bufferedCipher.DoFinal(message);

        //return output;
        //}

        public static byte[] rotar(byte[] _Array, int _Saltos)
        {
            int x;
            int i;

            for (i = 0; i < _Saltos; i++)
            {
                byte ultimo = _Array[_Array.Length - 1];
                for (x = _Array.Length - 1; x > 0; x--)
                {
                    _Array[x] = _Array[x - 1];
                }
                _Array[x] = ultimo;
            }

            return _Array;
        }

        public string toHEX(byte[] convertArray)
        {

            string hex = BitConverter.ToString(convertArray);
            return hex.Replace("-", "");

        }

        #endregion Diversify
    }
}
