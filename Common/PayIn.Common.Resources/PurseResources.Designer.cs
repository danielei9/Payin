﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayIn.Common.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PurseResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PurseResources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PayIn.Common.Resources.PurseResources", typeof(PurseResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You can not create a purse that has expired.
        /// </summary>
        public static string ExpiratedException {
            get {
                return ResourceManager.GetString("ExpiratedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a purse/foto{0}.jpeg.
        /// </summary>
        public static string FotoShortUrl {
            get {
                return ResourceManager.GetString("FotoShortUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a purse/foto{0}.jpeg.
        /// </summary>
        public static string FotoShortUrlTest {
            get {
                return ResourceManager.GetString("FotoShortUrlTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a https://payin.blob.core.windows.net/files/purse/foto{0}.jpeg.
        /// </summary>
        public static string FotoUrl {
            get {
                return ResourceManager.GetString("FotoUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a https://payintest.blob.core.windows.net/files/purse/foto{0}.jpeg.
        /// </summary>
        public static string FotoUrlTest {
            get {
                return ResourceManager.GetString("FotoUrlTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You cannot update a purse to a earlier expiration date than current or initial expiration date..
        /// </summary>
        public static string LowerExpirationException {
            get {
                return ResourceManager.GetString("LowerExpirationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You cannot update a purse to a earlier validity date than current or initial validity date..
        /// </summary>
        public static string LowerValidityException {
            get {
                return ResourceManager.GetString("LowerValidityException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a We could not find a company with the mail {0}.
        /// </summary>
        public static string NullCompanyException {
            get {
                return ResourceManager.GetString("NullCompanyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Your purse {0} will expire tomorrow. You can use it before being expired..
        /// </summary>
        public static string PaymentMediaLastDay {
            get {
                return ResourceManager.GetString("PaymentMediaLastDay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Your purse {0} is already expired..
        /// </summary>
        public static string PurseExpired {
            get {
                return ResourceManager.GetString("PurseExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You can not create a purse that is not going to be able to recharge..
        /// </summary>
        public static string ValidityException {
            get {
                return ResourceManager.GetString("ValidityException", resourceCulture);
            }
        }
    }
}
