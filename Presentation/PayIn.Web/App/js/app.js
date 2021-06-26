'use strict';
angular.module('app')
    .controller('AppCtrl', ['$rootScope', '$translate', '$localStorage', '$window', function ($rootScope, $translate, $localStorage, $window) {
        // add 'ie' classes to html
        var isIE = !!navigator.userAgent.match(/MSIE/i);
        isIE && angular.element($window.document.body).addClass('ie');
        isSmartDevice($window) && angular.element($window.document.body).addClass('smart');

        // config
        $rootScope.app = {
            nameFallas: "Pay[fallas]",
            nameVilamarxant: "Vilamarxant",
            nameFinestrat: "Finestrat",
            nameVinaros: "Vinaros",
            nameCarcaixent: "Carcaixent",
            nameFaura: "Faura",
            namePayIn: "Pay[in]",
            nameJustMoney: "JustMoney",
            name: function () { return this.nameFinestrat; }, // Esto es lo unico que se cambia

            test: "Test",
            production: "Production",
            configuration: function () { return this.test; }, // Test o Producción

            version: 'v5.15.07a',

            apiBaseUrl: '', // La baseURL es la raiz
            //tenant: '',
            pageSize: 12,
            columns: 3,
            // for chart colors
            color: {
                primary: '#E9AF30', //'#7266BA',
                info: '#23B7E5',
                success: '#27C24C',
                warning: '#FAD733',
                danger: '#F05050',
                light: '#E8EFF0',
                dark: '#3A3F51',
                black: '#1C2B36'
            },
            settings: {
                themeID: 1,
                navbarHeaderColor: 'bg-orange',
                navbarCollapseColor: 'bg-white-only',
                asideColor: 'bg-black',
                headerFixed: true,
                asideFixed: false,
                asideFolded: false,
                asideDock: false,
                container: false
            }
        }

        $rootScope.features = {
            logoUrl: function () {
                var logo = "/app/logo_orange.png";
                switch ($rootScope.app.name()) {
                    case $rootScope.app.nameFallas:
                        logo = "https://fallas.blob.core.windows.net:443/files/payfallas.jpg";
                        break;
                    case $rootScope.app.nameVinaros:
                        logo = "/app/logo_vinaros.png";
                        break;
                    case $rootScope.app.nameFinestrat:
                        logo = "/app/logo_finestrat.png";
                        break;
                    default:
                        logo = "/app/logo_orange.png";
                        break;
                }
                return logo;
            },
            logoMenuUrl: function () {
                var logo = "/app/logo.png";
                switch ($rootScope.app.name()) {
                    case $rootScope.app.nameVinaros:
                        logo = "/app/logo_vinaros.png";
                        break;
                    case $rootScope.app.nameFinestrat:
                        logo = "/app/logo_finestrat.png";
                        break;
                    default:
                        logo = "/app/logo.png";
                        break;
                }
                return logo;
            },
            logoSmallMenuUrl: function () {
                var logo = "/app/logo-small.png";
                switch ($rootScope.app.name()) {
                    case $rootScope.app.nameVinaros:
                        logo = "/app/logo_vinaros.png";
                        break;
                    case $rootScope.app.nameFinestrat:
                        logo = "/app/logo_finestrat.png";
                        break;
                    default:
                        logo = "/app/logo-small.png";
                        break;
                }
                return logo;
            },
            favicon: function () {
                var logo = "favicon.ico";
                switch ($rootScope.app.name()) {
                    case $rootScope.app.nameFallas:
                        logo = "fallas_16.png";
                        break;
                    case $rootScope.app.nameVinaros:
                        logo = "logo_vinaros.ico";
                        break;
                    case $rootScope.app.nameFinestrat:
                        logo = "logo_finestrat.ico";
                        break;
                    default:
                        logo = "favicon.ico";
                        break;
                }
                return logo;
            },
            faviconApple: function () {
                var logo = "favicon.ico";
                switch ($rootScope.app.name()) {
                    case $rootScope.app.nameFallas:
                        logo = "fallas_194.png";
                        break;
                    case $rootScope.app.nameVinaros:
                        logo = "logo_vinaros.ico";
                        break;
                    case $rootScope.app.nameFinestrat:
                        logo = "logo_vinaros.ico";
                        break;
                    default:
                        logo = "favicon.ico";
                        break;
                }
                return logo;
            },
            hasSmartCity: function () {
                return !(
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVilamarxant) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura)
                );
            },
            hasExhibitors: function () {
                return !(
                    ($rootScope.app.name() !== $rootScope.app.nameVilamarxant) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura)
                );
            },
            hasEdicts: function () {
                return (
                    ($rootScope.app.name() === $rootScope.app.nameVilamarxant) ||
                    ($rootScope.app.name() === $rootScope.app.nameFinestrat)
                );
            },
            hasAccessControl: function () {
                return (
                    ($rootScope.app.name() === $rootScope.app.nameVinaros)
                );
            },
            hasDocumentManagement: function () {
                return (
                    ($rootScope.app.name() === $rootScope.app.nameFallas) ||
                    ($rootScope.app.name() === $rootScope.app.namePayIn)
                );
            },
            hasCardSystem: function () {
                return (
                    ($rootScope.app.name() === $rootScope.app.nameFallas)
                );
            },
            sellProducts: function () {
                return (
                    ($rootScope.app.name() === $rootScope.app.nameFallas)
                );
            },
            isJustMoney: function () {
                return ($rootScope.app.name() === $rootScope.app.nameJustMoney);
            },
            hasMemberManagement: function () {
                return ($rootScope.app.name() === $rootScope.app.nameFinestrat);
            },
            hasConcessionManagement: function () {
                return false;
            },
            hasLoyaltySystem: function () {
                return (
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameJustMoney) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
            },
            hasForms: function () {
                return false;
            },
            hasCommunication: function () {
                return (
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameJustMoney) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
            },
            hasAccounting: function () {
                var toReturn = (
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVilamarxant) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFinestrat) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura) &&
                    ($rootScope.app.name() !== $rootScope.app.nameJustMoney) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
                return toReturn;
            },
            canRecoverPassword: function () {
                return (
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
            },
            canRegister: function () {
                return (
                    ($rootScope.app.name() !== $rootScope.app.nameFallas) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
            },
            emitWithEvents: function () {
                return (
                    ($rootScope.app.name() !== $rootScope.app.nameFallas)
                );
            },
            viewConcessionsInTicketList: function () {
                var result = (
                    ($rootScope.app.name() !== $rootScope.app.nameFallas)
                );
                return result;
            },
            viewUserInTicketList: function () {
                var result = !$rootScope.features.viewConcessionsInTicketList();
                return result;
            },
            manageBus: function () {
                var result = (
                    ($rootScope.app.name() === $rootScope.app.nameFinestrat)
                );
                return result;
            },
            canCharge: function () {
                var result = (
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFinestrat) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
                return result;
            },
            hasOrganizationStructure: function () {
                var result = (
                    ($rootScope.app.name() !== $rootScope.app.nameCarcaixent) &&
                    ($rootScope.app.name() !== $rootScope.app.nameFaura) &&
                    ($rootScope.app.name() !== $rootScope.app.nameVinaros)
                );
                return result;
            }
        }

        // save settings to local storage
        if (angular.isDefined($localStorage.settings)) {
            $rootScope.app.settings = $localStorage.settings;
        } else {
            $localStorage.settings = $rootScope.app.settings;
        }
        $rootScope.$watch('app.settings', function () {
            if ($rootScope.app.settings.asideDock && $rootScope.app.settings.asideFixed) {
                // aside dock and fixed must set the header fixed.
                $rootScope.app.settings.headerFixed = true;
            }
            // save to local storage
            $localStorage.settings = $rootScope.app.settings;
        }, true);

        // angular translate
        $rootScope.lang = { isopen: false };
        //$scope.langs = { en: 'English', de_DE: 'German', it_IT: 'Italian' };
        $rootScope.langs = { es_ES: 'Castellano', va_ES: 'Valencià', en: 'English' };
        var language = $localStorage.lang || navigator.userLanguage || navigator.language || navigator.languages[0];
        if ((language === 'es') || (language === 'es-ES') || (language === 'es_ES')) {
            $rootScope.selectLang = "Spanish"
        } else if ((language === 'va') || (language === 'va-ES') || (language === 'va_ES')) {
            $rootScope.selectLang = "Valencian";
        } else {
            $rootScope.selectLang = "English"
        }

        $rootScope.setLang = function (langKey, $event) {
            // set the current lang
            $rootScope.selectLang = $rootScope.langs[langKey];
            // You can change the language during runtime
            $translate.use(langKey);
            $rootScope.lang.isopen = !$rootScope.lang.isopen;
            $localStorage.lang = langKey;
        };


        function isSmartDevice($window) {
            // Adapted from http://www.detectmobilebrowsers.com
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        }
    }])
    .filter('xpJourneyStart', function () {
        return function (since) {
            if (since) {
                var hours = moment(since, ["HH:mm:ssZ", moment.ISO_8601]).hours() * 100 / 24;
                return hours;
            };
        }
    })
    .filter('xpJourneyStartCSS', function () {
        return function (since) {
            if (since) {
                var hours = (moment(since, ["HH:mm:ssZ", moment.ISO_8601]).hours() * 100 / 24) - 9;
                return hours;
            };
        }
    })
    .filter('xpJourneyLength', function () {
        return function (until, since) {
            if (until && since) {
                var hours = (
                    moment(until, ["HH:mm:ssZ", moment.ISO_8601]).hours() - moment(since, ["HH:mm:ssZ", moment.ISO_8601]).hours()
                ) * 100 / 24;
                return hours;
            };
        }
    })
    .filter('xpJourneyLengthCSS', function () {
        return function (until, since) {
            if (until && since) {
                var hours = (
                    (moment(until, ["HH:mm:ssZ", moment.ISO_8601]).hours() - moment(since, ["HH:mm:ssZ", moment.ISO_8601]).hours()
                    ) * 100 / 24) - 7;
                return hours;
            };
        }
    })
    .filter('eigeUid', function () {
        // function format (number, n, x, s, c) {
        //     var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')';
        //     var num = ("000" + number.toFixed(Math.max(0, ~~n))).slice(-12);

        //     return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
        // };
        return function (input) {
            if (!input)
                return '';
            var inputInteger =
                (
                    ((parseInt(input.substring(6, 8), 16) * 256 +
                        parseInt(input.substring(4, 6), 16)) * 256 +
                        parseInt(input.substring(2, 4), 16)) * 256 +
                    parseInt(input.substring(0, 2), 16)
                ) * 100 +
                (
                    parseInt(input.substring(0, 2), 16) ^
                    parseInt(input.substring(2, 4), 16) ^
                    parseInt(input.substring(4, 6), 16) ^
                    parseInt(input.substring(6, 8), 16)
                ) % 100;

            var result = ("000" + inputInteger.toString()).slice(-12)
            result =
                result.slice(0, 4) + " " +
                result.slice(4, 8) + " " +
                result.slice(8, 12);
            //format(inputInteger, 0, 4, ' ');
            return result;
        };
    })
    .filter('eigeZonaFilter', function () {
        return function (input) {
            var result =
                (input === 1 ? 'A' : '') +
                (input === 2 ? 'B' : '') +
                (input === 3 ? 'AB' : '') +
                (input === 4 ? 'C' : '') +
                (input === 6 ? 'BC' : '') +
                (input === 7 ? 'ABC' : '') +
                (input === 8 ? 'D' : '') +
                (input === 12 ? 'CD' : '') +
                (input === 15 ? 'ABCD' : '');

            return result;
        };
    })
    .filter('promoConditionTypeName', function () {
        return function (input) {
            var result =
                (input === 0 ? 'DNI' : '') +
                (input === 1 ? 'persona' : '') +
                (input === 2 ? 'dispositivo' : '') +
                (input === 3 ? 'tarjeta' : '') +
                (input === 4 ? 'recarga' : '');

            return result;
        };
    })
    .filter('eigeTipoOperacion', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Ninguno' : '') +
                (input === 1 ? 'Caducidad título' : '') +
                (input === 2 ? 'Título no personalizado' : '') +
                (input === 4 ? 'Impresión título personalizado' : '') +
                (input === 8 ? 'Fondo título' : '') +
                (input === 10 ? 'Caducidad tarjeta' : '') +
                (input === 20 ? 'Código identificación no viajero' : '') +
                (input === 40 ? 'Código usuario' : '') +
                (input === 80 ? 'Fondo empresa propietaria' : '') +
                (input === 100 ? 'Empresa propietaria' : '') +
                (input === 200 ? 'Nombre' : '') +
                (input === 400 ? 'Fotografías' : '') +
                (input === 800 ? 'Logotipos empresa propietaria' : '');

            return result;
        };
    })
    .filter('eigeAction', function () {
        return function (input) {
            var result =
                (input === 100 ? 'Incrementa saldo' : '') +
                (input === 101 ? 'Descuenta saldo' : '') +
                (input === 102 ? 'Modifica saldo' : '') +
                (input === 103 ? 'Inc. unidades temporales' : '') +
                (input === 104 ? 'Desc. unidades temporales' : '') +
                (input === 105 ? 'Modifica fecha inicio' : '') +
                (input === 106 ? 'Modifica bit ampliación' : '') +
                (input === 107 ? 'Modifica campo tarjeta' : '') +
                (input === 108 ? 'Modifica bloque' : '') +
                (input === 109 ? 'Emite tarjeta' : '') +
                (input === 999 ? 'Lectura completa' : '');
            return result;
        };
    })
    .filter('eigeChargeAction', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Carga' : '') +
                (input === 1 ? 'Recarga' : '') +
                (input === 2 ? 'Ampliación temporal' : '') +
                (input === 3 ? 'Ampliación cantidad' : '') +
                (input === 4 ? 'Cambio operador' : '') +
                (input === 5 ? 'Cambio zona' : '') +
                (input === 6 ? 'Anulación' : '') +
                (input === 7 ? 'Canje' : '');
            return result;
        };
    })
    .filter('eigeValidationZone', function () {
        return function (input) {
            var result =
                (input === 0 ? 'A' : '') +
                (input === 1 ? 'B' : '') +
                (input === 2 ? 'C' : '') +
                (input === 3 ? 'D' : '');
            return result;
        };
    })
    .filter('eigeUnidadesTemporal', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Años' : '') +
                (input === 1 ? 'Horas' : '') +
                (input === 2 ? 'Días' : '') +
                (input === 3 ? 'Meses' : '');
            return result;
        };
    })
    .filter('eigeValidityType', function () {
        return function (input) {
            var result =
                (input === 0 ? 'A\u00F1os' : '') +
                (input === 1 ? 'Horas' : '') +
                (input === 2 ? 'D\u00EDas' : '') +
                (input === 3 ? 'Meses' : '');
            return result;
        };
    })
    .filter('eigePaymentMedia', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Efectivo' : '') +
                (input === 1 ? 'Tarjeta d\u00E9bito' : '') +
                (input === 2 ? 'Tarjeta cr\u00E9dito' : '') +
                (input === 3 ? 'Cheque' : '') +
                (input === 4 ? 'Monedero externo' : '') +
                (input === 5 ? 'M\u00F3vil' : '') +
                (input === 6 ? 'Monedero' : '') +
                (input === 7 ? 'Bonus' : '') +
                (input === 8 ? 'Domiciliaci\u00F3n' : '') +
                (input === 9 ? 'Internet' : '') +
                (input === 10 ? 'Mixto' : '');
            return result;
        };
    })
    .filter('eigeRateType', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Normal' : '') +
                (input === 1 ? 'Joven' : '') +
                (input === 2 ? 'Mayores/Jubilados' : '') +
                (input === 3 ? 'Promocional' : '') +
                (input === 4 ? 'Estudiante' : '') +
                (input === 5 ? 'Discapacitado' : '') +
                (input === 6 ? 'Familia 1' : '') +
                (input === 7 ? 'Familia 2' : '') +
                (input === 8 ? 'Desempleado' : '');
            return result;
        };
    })
    .filter('eigeInformationType', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Origen-Destino' : '') +
                (input === 1 ? 'Adicionales 1 t\u00EDtulo' : '') +
                (input === 2 ? 'Adicionales 2 monedero' : '') +
                (input === 3 ? 'Adicionales 3 t\u00EDtulo 1' : '');
            return result;
        };
    })
    .filter('eigeManufacturingCompany', function () {
        return function (input) {
            var result =
                (input === 0 ? 'OTRO' : '') +
                (input === 1 ? 'Calmell' : '') +
                (input === 2 ? 'Saetic' : '') +
                (input === 3 ? 'Futurecard' : '') +
                (input === 4 ? 'ASK - Botella' : '') +
                (input === 5 ? 'Magnadata' : '') +
                (input === 6 ? 'ISRA' : '') +
                (input === 7 ? 'Card-Tiket' : '') +
                (input === 8 ? 'Bit4ID' : '') +
                (input === 9 ? 'Akrocard' : '') +
                (input === 10 ? 'GYD' : '') +
                (input === 11 ? 'Aplicard' : '') +
                (input === 12 ? 'ISB-Botella' : '') +
                (input === 17 ? 'MAREA' : '') +
                (input === 18 ? 'Orange' : '') +
                (input === 19 ? 'Vodafone' : '') +
                (input === 20 ? 'Movistar' : '');
            return result;
        };
    })
    .filter('eigeTypeCard', function () {
        return function (input) {
            var result =
                (input === 1 ? 'Viajero' : '') +
                (input === 2 ? 'Viajero personalizado' : '') +
                (input === 3 ? 'Inspector/Mantenimiento' : '') +
                (input === 4 ? 'Empleado' : '') +
                (input === 5 ? 'Pase' : '') +
                (input === 6 ? 'Pase personalizado' : '') +
                (input === 7 ? 'Expendedor' : '') +
                (input === 8 ? 'VLC Card no perso.' : '') +
                (input === 9 ? 'VLC Card personalizada' : '') +
                (input === 10 ? 'Tarifa especial' : '') +
                (input === 11 ? 'M\u00F3vil' : '') +
                (input === 12 ? 'Tarj. Ciudadana' : '');
            return result;
        };
    })
    .filter('eigeTypeRecharge', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Taquilla/Estanco' : '') +
                (input === 1 ? 'Val. ferroviar\u00EDo' : '') +
                (input === 2 ? 'Val. bus' : '') +
                (input === 3 ? 'Autom\u00E1tico' : '');
            return result;
        };
    })
    .filter('eigeDailyLife', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Sin restricci\u00F3n' : '') +
                (input === 1 ? 'Lunes a Viernes' : '') +
                (input === 2 ? 'Fines de semana' : '') +
                (input === 3 ? 'D\u00EDas lectivos' : '') +
                (input === 4 ? 'Viernes(15h) a Domingo(23:59h)' : '') +
                (input >= 5 ? 'Otros' : '');
            return result;
        };
    })
    .filter('eigeTimeValidity', function () {
        return function (input) {
            var result =
                (input === 0 ? 'Sin distinci\u00F3n' : '') +
                (input === 1 ? 'Hora punta' : '') +
                (input === 2 ? 'Hora valle' : '') +
                (input >= 3 ? 'Otros' : '');
            return result;
        };
    })
    .filter('eigeTitleUse', function () {
        return function (input) {
            var result =
                (input === 0 ? 'T\u00EDtulo 1' : '') +
                (input === 1 ? 'T\u00EDtulo 2' : '') +
                (input === 2 ? 'Monedero' : '') +
                (input >= 3 ? 'Bonus' : '');
            return result;
        };
    })
    .filter('eigeIssuingCompany', function () {
        return function (input) {
            var result =
                (input === 0 ? '\u00C1rea metropolitana Valencia' : '') +
                (input === 1 ? '\u00C1rea metropolitana Alicante' : '') +
                (input === 2 ? '\u00C1rea metropolitana Castell\u00F3n' : '') +
                (input === 3 ? 'Comunidad Valenciana' : '') +
                (input === 16 ? 'Vilamarxant' : '') +
                (input === 17 ? 'Moncada' : '') +
                (input === 18 ? 'Valle de Carcer' : '') +
                (input === 19 ? 'Ayto. Alicante' : '') +
                (input === 20 ? 'Los Serranos' : '') +
                (input === 21 ? 'Catarroja' : '') +
                (input === 22 ? 'Ayto. Godella' : '') +
                (input === 23 ? 'Man. Horta Sud' : '') +
                (input === 25 ? 'Ayto. Sagunto' : '') +
                (input === 26 ? 'Ayto. Aldaia' : '') +
                (input === 27 ? 'Ayto. Teulada' : '') +
                (input === 28 ? 'Ayto. Alboraya' : '') +
                (input === 29 ? 'Ayto. Paterna' : '') +
                (input === 30 ? 'Ayto. Castell\u00F3n' : '');
            return result;
        };
    })
    .filter('xpFormatTooltip', function () {
        return function (value) {
            var var1 = value.split(",");
            var result = "";
            angular.forEach(var1, function (key, value) {
                result += key + "\n";
            });
            return result;
        }
    })
    .filter('xpHourToTime', function () {
        return function (time) {
            var hours = Math.trunc(time);
            var minutes = Math.trunc(Math.abs(time - hours) * 60);
            var seconds = Math.trunc(Math.abs(time - hours) * 60 - minutes) * 60;
            var timeString =
                (hours || "0") +
                ((minutes < 10) ? ":0" + minutes : ":" + minutes) +
                ((seconds < 10) ? ":0" + seconds : ":" + seconds);
            return timeString;
        }
    })
    .filter('xpSum', function () {
        return function (data, key) {
            if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
                return 0;
            }

            var sum = 0;
            for (var i = data.length - 1; i >= 0; i--) {
                sum += Number(data[i][key]);
            }

            return sum;
        };
    })
    .filter('xpSumIf', function () {
        return function (data, key, condition) {
            if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
                return 0;
            }

            var sum = 0;
            for (var i = data.length - 1; i >= 0; i--) {
                if (data[i][condition])
                    sum += Number(data[i][key]);
            }

            return sum;
        };
    })
    .filter('xpOr', function () {
        return function (data, key) {
            if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
                return false;
            }

            var sum = 0;
            for (var i = data.length - 1; i >= 0; i--) {
                if (data[i][key])
                    return true;
            }

            return false;
        };
    })
    .factory('xpQueryMupi', ['xpCommunication', 'xpStack', '$interpolate', function (xpCommunication, xpStack, $interpolate) {
        return {
            'initializeQuery': function () {
                var that = this;
                this.$on('search', function (event) {
                    var params = {};
                    angular.extend(params, that.arguments);

                    that.count = 0;

                    that.connect();
                    if (that.onPrevious) {
                        var action = new Function('scope', that.onPrevious);
                        action(that);
                    }
                    xpCommunication
                        .get(that.apiUrl, that.id, params)
                        .success(that.success.bind(that), function (data) {
                            that.count = data.length;

                            that.data.length = 0;
                            angular.forEach(data.data, function (item, key) {
                                that.data[key] = item;
                            });
                            angular.forEach(data, function (item, key) {
                                if (key != 'data' && key != 'count')
                                    that.arguments[key] = item;
                            });

                            if (that.onSuccess) {
                                var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
                                action(that, xpStack, data);
                            }

                            setTimeout(function () {
                                that.$apply(function () {
                                    that.arguments.showList = true;
                                });
                                setTimeout(function () {
                                    xpStack.navigationAddPopup("accesscontrolpublicweather", "", "", that);
                                    that.open();
                                    setTimeout(function () {
                                        location.reload();
                                    }, 20000);
                                }, 20000);
                            }, 50000);
                        })
                        .error(that.error.bind(that));
                });
                this.$on('clean', function (event) {
                    that.arguments.filter = '';
                    that.search();
                });
            },
            'search': function () {
                this.$emit('search');
            },
            'cancel': function () {
                var that = this;

                // Refrescar
                if (xpStack.navigate(that))
                    that.clearInbound();
            },
            'clean': function () {
                this.$emit('clean');
            },
            'count': function () { return this.count; }
        };
    }])
    .directive('xpMupi', ['xpQueryMupi', 'xpQueryPaginable', 'xpStack', function (xpQueryMupi, xpQueryPaginable, xpStack) {
        return {
            restrict: 'E',
            priority: 100,
            scope: true,
            controller: ['$scope', '$attrs', '$stateParams', //'xpAnalitics',
                function ($scope, $attrs, $stateParams/*,   xpAnalitics*/) {
                    var isPaginable = $attrs.isPaginable;
                    var initialSearch = $attrs.$attr['initialSearch'];

                    // Initialize scope
                    angular.extend($scope, xpQueryMupi);
                    $scope.initializeQuery();
                    if (isPaginable)
                        angular.extend($scope, xpQueryPaginable);
                    $scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
                        .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                        .replace("//", "/");
                    $scope.onPrevious = $attrs.xpPrevious;
                    $scope.onSuccess = $attrs.xpSuccess;
                    $scope.onModal = $attrs.xpModal;

                    xpStack.push({
                        "key": "list",
                        "scope": $scope
                    });

                    var init = $attrs["xpInit"];
                    if (init) {
                        var action = new Function('scope', 'xpStack', 'params', init);
                        action($scope, xpStack, $stateParams);
                    }

                    if (initialSearch)
                        $scope.search();
                }
            ]
        };
    }])
    .controller('SearchCardCtrl', ['$scope', '$state', function ($scope, $state) {
        $scope.cardSearch = function (val) {
            $scope.app.cardSearchId = $scope.cardSearchId || val;
            $state.go('transportoperationdetails', { "id": $scope.app.cardSearchId });
        }
    }])
    .controller('ControlTrackGetItemController', ['$scope', 'xpCommunication', function ($scope, xpCommunication) {
        $scope.temp.tracks = [];

        $scope.$watch('data', function (data) {
            var tracks = [];
            var trackEnum = [];

            angular.forEach(data, function (item) {
                var positions = [];

                angular.forEach(item.items, function (pos) {
                    var position = {
                        latitude: pos.latitude,
                        longitude: pos.longitude,
                        title: moment(pos.date, moment.ISO_8601).format("HH:mm:ss"),
                        date: new Date(pos.date),
                        velocity: pos.velocity * 3600 / 1000
                    };

                    positions.push(position);
                });

                tracks.push({
                    name: item.since ? moment(item.since.date || item.until.date).format("HH:mm:ss") : "",
                    since: item.since,
                    until: item.until,
                    positions: positions
                });
            });

            $scope.temp.tracks = tracks;

        });
        $scope.$watch('arguments.date', function () {
            $scope.temp.tracks = [];

            if ($scope.arguments.trackId)
                delete $scope.arguments.trackId; // $scope.search() executed in watch
            else
                $scope.search();

            // Load tracks enum
            xpCommunication
                .get('Api/ControlTrack', "", { date: $scope.arguments.date, itemId: $scope.arguments.itemId })
                .success(function (data) {
                    $scope.temp.trackEnum = data.data;
                });
        });
        $scope.$watch('arguments.trackId', function () {
            $scope.search();
        });
    }])
    .controller('ControlTrackGetController', ['$scope', function ($scope) {
        $scope.temp.tracks = [];
        $scope.temp.tracksmobile = [];
        var start = {};
        var end = {};

        $scope.$watch('data', function (data) {
            if (!$scope.temp.tracks.length && data.length) {
                var tracks = [];
                var tracksmobile = [];

                angular.forEach(data, function (item) {
                    var positions = [];
                    var positionsmobile = [];

                    angular.forEach(item.items, function (pos) {
                        var position = {
                            latitude: pos.latitude,
                            longitude: pos.longitude,
                            title: moment(pos.date, moment.ISO_8601).format("HH:mm:ss"),
                            date: new Date(pos.date),
                            velocity: (pos.velocity * 3600 / 1000) || 0
                        };
                        positions.push(position);
                        var positionmobile = {
                            latitude: pos.latitude,
                            longitude: pos.longitude,
                            title: moment(pos.date, moment.ISO_8601).format("HH:mm:ss"),
                            date: new Date(pos.date),
                            velocity: pos.mobileVelocity * 3600 / 1000 || 0
                        };
                        positionsmobile.push(positionmobile);
                    });

                    tracks.push({
                        name: item.since ? moment(item.since.date || item.until.date).format("HH:mm:ss") : "",
                        since: item.since,
                        until: item.until,
                        positions: positions
                    });
                    tracksmobile.push({
                        name: item.since ? moment(item.since.date || item.until.date).format("HH:mm:ss") : "",
                        since: item.since,
                        until: item.until,
                        positions: positionsmobile
                    });
                });

                $scope.temp.tracks = tracks;
                $scope.temp.tracksmobile = tracksmobile;

            }
        });

        var refresh = function () {
            if (
                start === $scope.arguments.start &&
                end === $scope.arguments.end
            )
                return;

            start = $scope.arguments.start;
            end = $scope.arguments.end;

            $scope.search();
        }

        $scope.$watch('arguments.start', function () {
            refresh();
        });
        $scope.$watch('arguments.end', function () {
            refresh();
        });
    }])
    .controller('ControlTrackGetAllController', ['$scope', function ($scope) {
        var date = {};
        var workerId = {};
        var itemId = {};

        var refresh = function () {
            if (
                date === $scope.arguments.date &&
                workerId === $scope.arguments.workerId &&
                itemId === $scope.arguments.itemId
            )
                return;

            date = $scope.arguments.date;
            workerId = $scope.arguments.workerId;
            itemId = $scope.arguments.itemId;

            $scope.search();
        }

        $scope.$watch('arguments.date', function () {
            refresh();
        });
        $scope.$watch('arguments.workerId', function () {
            refresh();
        })
        $scope.$watch('arguments.itemId', function () {
            refresh();
        })
    }])
    .controller('ControlPresenceGraphController', ['$scope', function ($scope) {
        var since = {};
        var until = {};
        var workerId = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until &&
                workerId === $scope.arguments.workerId
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            workerId = $scope.arguments.workerId;

            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
        $scope.$watch('arguments.workerId', function () {
            refresh();
        })
    }])
    .controller('PaymentGraphController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('PaymentGetChargesController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        };

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('TicketPayController', ['$scope', function ($scope) {
        angular.extend($scope, {
            'iframeLoaded': function () {
                var iFrameID = document.getElementById('bankFrame');
                if (iFrameID) {
                    // here you can make the height, I delete it first, then I make it again
                    iFrameID.height = "";
                    iFrameID.height = iFrameID.contentWindow.document.body.scrollHeight + "px";
                }
            },
            'pay': function (data) {
                //$scope.show();
                var query = data.data[0].request;
                var url = data.data[0].url;
                var verb = data.data[0].verb;

                //Create Form
                var form = document.getElementById("formPost");
                if (!form.hasAttribute("action"))
                    form.setAttribute("action", url);
                if (!form.hasAttribute("method"))
                    form.setAttribute("method", verb);

                var split = query.split("&");
                split.forEach(function (element) {
                    var input = element.split(":");
                    var name = input[0];
                    var value = input[1];

                    var inp = document.createElement("input");
                    inp.setAttribute("type", "text");
                    inp.setAttribute("name", name);
                    inp.setAttribute("value", value);
                    form.appendChild(inp);
                }, this);

                //setTimeout(function () { $scope.hide(); }, 4000);

                var uiView = document.getElementById('uiView');
                uiView.style.display = 'none';

                form.submit();

                var bankFrame = document.getElementById('bankFrame');
                bankFrame.style.display = 'block';

                //uiView.style.display = 'block';
            }
        });
    }])
    .controller('TicketGraphController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('PaymentGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('ServiceNotificationGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('PaymentGetChargesController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('TransportCardSupportGetAllController', ['$scope', function ($scope) {
        var ownerId = {};

        var refresh = function () {
            if (
                ownerId === $scope.arguments.ownerId
            )
                return;

            ownerId = $scope.arguments.ownerId;
            $scope.search();
        }
        $scope.$watch('arguments.ownerId', function () {
            refresh();
        });
    }])
    .controller('TransportOperationGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('ShipmentReceiptGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('LiquidationGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('LiquidationGetAllController', ['$scope', function ($scope) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('UnliquidatedController', ['$scope', function ($scope) {
        var since = {};
        var until = {};
        var concessionId = null;
        var eventId = null;
        var filterByEvent = false;

        var sinceToggle = true;

        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until &&
                concessionId === $scope.arguments.concessionId &&
                eventId === $scope.arguments.eventId &&
                filterByEvent === $scope.arguments.filterByEvent
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            concessionId = $scope.arguments.concessionId;
            eventId = $scope.arguments.eventId;
            filterByEvent = $scope.arguments.filterByEvent;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
        $scope.$watch('arguments.concessionId', function () {
            refresh();
        });
        $scope.$watch('arguments.eventId', function () {
            refresh();
        });
        $scope.$watch('arguments.filterByDate', function () {
            refresh();
        });
        $scope.$watch('arguments.filterByEvent', function () {
            refresh();
        });
        $scope.addSelection = function (item) {
            if (item.selected || false)
                return;

            item.selected = true;
            $scope.temp.concessionIdSelected = item.concessionId;
        };
        $scope.removeSelection = function (item) {
            if (!(item.selected || false))
                return;

            item.selected = false;
            var sum = _.reduce(
                $scope.data,
                function (sum, item) {
                    return sum + (item.selected ? 1 : 0);
                },
                0
            );
            if (!sum)
                $scope.temp.concessionIdSelected = 0;
        };
        $scope.addAllSelections = function (concession) {
            $scope.temp.concessionIdSelected = concession.id;
            angular.forEach(
                $scope.data,
                function (item) {
                    if (concession.id === item.concessionId)
                        item.selected = true;
                }
            );
        };
        $scope.removeAllSelections = function (concession) {
            $scope.temp.concessionIdSelected = 0;
            angular.forEach(
                $scope.data,
                function (item) {
                    item.selected = false;
                }
            );
        };
    }])
    .controller('serviceUserUpdateCardSelectController', ['$scope', 'xpStack', function ($scope, xpStack) {
        $scope.accept = function () {
            xpStack.navigate($scope, "ServiceUser/UpdateCard/" + $scope.id + "?uid=" + $scope.arguments.uid);
        };
    }])
    .controller('LiquidationPayController', ['$scope', '$stateParams', function ($scope, $stateParams) {
        var since = {};
        var until = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
    }])
    .controller('TicketGetAllDayController', ['$scope', function ($scope) {
        var since = {};
        var until = {};

        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;

            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        })

    }])
    .controller('ControlPresenceGraphController', ['$scope', function ($scope) {
        var since = {};
        var until = {};
        var workerId = {};


        var refresh = function () {
            if (
                since === $scope.arguments.since &&
                until === $scope.arguments.until &&
                workerId === $scope.arguments.workerId
            )
                return;

            since = $scope.arguments.since;
            until = $scope.arguments.until;
            workerId = $scope.arguments.workerId;

            $scope.search();
        }

        $scope.$watch('arguments.since', function () {
            refresh();
        });
        $scope.$watch('arguments.until', function () {
            refresh();
        });
        $scope.$watch('arguments.workerId', function () {
            refresh();
        })
    }])
    .controller('getCode', ['$scope', '$timeout', function ($scope, $timeout) {

        //10 seconds delay
        $timeout(function () {
            var codeText = "pay[in]/entrance:{\"code\":";
            var code = $scope.arguments.entranceCode;
            var typeNumber = 4;
            var errorCorrectionLevel = 'H';
            var qr = qrcode(typeNumber, errorCorrectionLevel);
            var generatedQR = codeText + code + "}";
            console.log(generatedQR);
            qr.addData(generatedQR);
            qr.make();
            var generatedBarCode = generatedQR;
            console.log(generatedBarCode);
            document.getElementById('placeHolder').innerHTML = qr.createImgTag(4);
            document.getElementById('QR').innerHTML = qr.createImgTag();
            JsBarcode("#barcode", generatedBarCode, {
                width: 1,
                height: 40,
                displayValue: false
            });
        }, 2500);
    }])
    .controller('getCodeExhibitor', ['$scope', '$timeout', function ($scope, $timeout) {

        //10 seconds delay
        $timeout(function ($compileProvider) {
            var codeText = "pay[in]/exhibitor:{\"id\":";
            var code = $scope.arguments.id;
            var typeNumber = 4;
            var errorCorrectionLevel = 'H';
            var qr = qrcode(typeNumber, errorCorrectionLevel);
            var generatedQR = codeText + code + "}";
            console.log(generatedQR);
            qr.addData(generatedQR);
            qr.make();
            var image = qr.createImgTag(5);
            document.getElementById('placeHolder').innerHTML = image;
            $scope.arguments.image = document.querySelectorAll('#placeHolder img')[0].src;
            console.log($scope.arguments.image);
        }, 1500);
    }])
    .controller('ControlFormAssignUpdate_ValueController', ['$scope', '$element', '$compile', function ($scope, $element, $compile) {
        var isRequired = ($scope.arg.isRequired) ? "required" : "";
        var requiredMessage = "<div ng-show='form.value{{$index}}.$error.required'><span class='error control-label'>'{{arg.name}}' is required</span></div>";
        var observationMessage = "<div class='help-block m-b-none'>{{arg.observations}}</div>";
        var lineMessage = "<div class='line line-dashed b-b line-lg pull-in'></div>";

        //var value =
        //	($scope.arg.type === 6) ? "{{arg.valueDateTime|xpTime}}" : // Time
        //	($scope.arg.type === 8) ? "{{arg.valueDateTime|xpDuration}}" : // Duration
        //	($scope.arg.type === 5) ? "{{arg.valueDateTime|xpDate}}" : // Date
        //	($scope.arg.type === 7) ? "{{arg.valueDateTime|xpDateTime}}" : // Datetime
        //	($scope.arg.type === 3) ? "{{arg.valueNumeric}}" : // Double
        //	($scope.arg.type === 2) ? "{{arg.valueNumeric}}" : // Int
        //	($scope.arg.type === 4) ? "{{arg.valueBool}}" : // Bool
        //	"{{arg.valueString}}";

        //if ($scope.arg.target === 2) { // Check
        //	$element.html(
        //		"<div class='form-group col-md-12'>" +
        //			"<label>{{arg.name}}</label>" +
        //			"<div>" + value + "</div>" +
        //			observationMessage +
        //		"</div>" +
        //		lineMessage
        //	);
        //	// } else if ($scope.$arg.type === "enum") { }
        //} else
        if ($scope.arg.type === 6) { // Time
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<div class='input-group date' data-xp-time='arg.valueDateTime'>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='text' class='form-control' data-ng-model='value' " + isRequired + "/>" +
                "<span class='input-group-addon'><span class='glyphicon glyphicon-time'></span></span>" +
                ($scope.arg.isRequired ? "" : "<span class='input-group-addon'><span class='glyphicon glyphicon-remove'></span></span>") +
                "</div>" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else if ($scope.arg.type === 8) { // Duration
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<div class='input-group date' data-xp-duration='arg.valueDateTime'>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='text' class='form-control' data-ng-model='value' " + isRequired + "/>" +
                "<span class='input-group-addon'><span class='glyphicon glyphicon-time'></span></span>" +
                ($scope.arg.isRequired ? "" : "<span class='input-group-addon'><span class='glyphicon glyphicon-remove'></span></span>") +
                "</div>" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else if ($scope.arg.type === 5) { // Date
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<div class='input-group date' data-xp-date='arg.valueDateTime'>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='text' class='form-control' data-ng-model='value' " + isRequired + "/>" +
                "<span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span></span>" +
                ($scope.arg.isRequired ? "" : "<span class='input-group-addon'><span class='glyphicon glyphicon-remove'></span></span>") +
                "</div>" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else if ($scope.arg.type === 7) { // Datetime
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<div class='input-group date' data-xp-date-time='arg.valueDateTime'>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='text' class='form-control' data-ng-model='value' " + isRequired + "/>" +
                "<span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span></span>" +
                ($scope.arg.isRequired ? "" : "<span class='input-group-addon'><span class='glyphicon glyphicon-remove'></span></span>") +
                "</div>" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else if ($scope.arg.type === 3) { // Double
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='text' class='form-control' data-ng-model='arg.valueNumeric' " + isRequired + " ui-jq='TouchSpin' data-verticalbuttons='true' data-verticalupclass='fa fa-caret-up' data-verticaldownclass='fa fa-caret-down' data-step='0.000001' data-min='-180' data-decimals='6'>" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else if ($scope.arg.type === 2) { // Int
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='custom' class='form-control' data-ng-model='arg.valueNumeric' " + isRequired + ">" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
            // } else if ($scope.arg.type === "currency") {
            //	$element.html(
            //"<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
            //	"<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
            //		"<input id='value" + $scope.$index + "' name='value" + $scope.$index + "' type='text' class='form-control' data-ng-model='arg.valueDecimal' " + isRequired + " ui-jq='TouchSpin' data-verticalbuttons='true' data-verticalupclass='fa fa-caret-up' data-verticaldownclass='fa fa-caret-down' data-prefix='EUR'>" + 
            //    requiredMessage +
            //		observationMessage +
            //"</div>" +
            //lineMessage
            //	);
            //}
        } else if ($scope.arg.type === 4) { // Bool
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label class='i-checks'>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='checkbox' class='form-control' data-ng-model='arg.valueBool'/>" +
                "<i></i>" +
                $scope.arg.name +
                "</label>" +
                observationMessage +
                "</div>" +
                lineMessage
            );
        } else {
            $element.html(
                "<div data-ng-class=\"{'has-error':!form.value{{$index}}.$valid}\" class='form-group col-md-12'>" +
                "<label for='value{{$index}}' class='control-label'>{{arg.name}}</label>" +
                "<input id='value{{$index}}' name='value{{$index}}' type='custom' class='form-control' ng-model='arg.valueString' " + isRequired + ">" +
                requiredMessage +
                observationMessage +
                "</div>" +
                lineMessage
            );
        }
        $compile($element.contents())($scope);
    }])
    .controller('EnergyTariffGetAllController', ['$scope', function ($scope) {
        $scope.getColor = function (schedule, weekDay, hour) {
            //var schedule = _
            //	.find(schedules, function (schedule) {
            //		return schedule.weekDay % (2 * weekDay) >= weekDay;
            //	});
            var timeTable = _
                .find((schedule || {}).timeTables, function (item) {
                    return (
                        (item.since === item.until)
                    );
                });
            if (timeTable)
                return timeTable.periodColor;

            timeTable = _
                .find((schedule || {}).timeTables, function (item) {
                    return (
                        (
                            (item.since < item.until) &&
                            (moment(item.since, 'hh:mm:ssZ').utc().hour() <= hour) &&
                            (moment(item.until, 'hh:mm:ssZ').utc().hour() > hour)
                        )
                    );
                });
            if (timeTable)
                return timeTable.periodColor;

            timeTable = _
                .find((schedule || {}).timeTables, function (item) {
                    return (
                        (
                            (item.since > item.until) &&
                            (
                                (moment(item.since, 'hh:mm:ssZ').utc().hour() >= hour) ||
                                (moment(item.until, 'hh:mm:ssZ').utc().hour() < hour)
                            )
                        )
                    );
                });
            if (timeTable)
                return timeTable.periodColor;

            return "grey";
        };
    }]);