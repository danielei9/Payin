﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EntranceCheckResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EntranceCheckResources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PayIn.Common.Resources.EntranceCheckResources", typeof(EntranceCheckResources).GetTypeInfo().Assembly);
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
        ///   Busca una cadena traducida similar a Entrance is already inside.
        /// </summary>
        public static string EntranceIsAlreadyInException {
            get {
                return ResourceManager.GetString("EntranceIsAlreadyInException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Entrance is outside.
        /// </summary>
        public static string EntranceIsOutException {
            get {
                return ResourceManager.GetString("EntranceIsOutException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Entrance can&apos;t access now to event.
        /// </summary>
        public static string EntranceTypeCheckPeriodOutException {
            get {
                return ResourceManager.GetString("EntranceTypeCheckPeriodOutException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Entrance only valid form {0} days and was first used in {1}.
        /// </summary>
        public static string EntranceTypeNumDaysExceededException {
            get {
                return ResourceManager.GetString("EntranceTypeNumDaysExceededException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a This entrance type only can access since {0} until {1}.
        /// </summary>
        public static string EntranceTypeOutDaylyTimePeriodException {
            get {
                return ResourceManager.GetString("EntranceTypeOutDaylyTimePeriodException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Event isn&apos;t accesible.
        /// </summary>
        public static string EventCheckPeriodOutException {
            get {
                return ResourceManager.GetString("EventCheckPeriodOutException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Event is full!!!.
        /// </summary>
        public static string EventFullException {
            get {
                return ResourceManager.GetString("EventFullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Code not found.
        /// </summary>
        public static string NoCodeException {
            get {
                return ResourceManager.GetString("NoCodeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a There is no entrance for the event.
        /// </summary>
        public static string NoEntranceException {
            get {
                return ResourceManager.GetString("NoEntranceException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Event not found.
        /// </summary>
        public static string NoEventException {
            get {
                return ResourceManager.GetString("NoEventException", resourceCulture);
            }
        }
    }
}