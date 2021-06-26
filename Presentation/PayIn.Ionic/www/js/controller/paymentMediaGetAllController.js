angular
    .module('app')
    .controller('paymentMediaGetAllController', 
                ['$scope','$state', '$ionicActionSheet', '$timeout', '$ionicPopup', 'xpDelete', 'xpPost','xpInitialScope','xpGet','$window',
                 function($scope, $state, $ionicActionSheet, $timeout, $ionicPopup, xpDelete, xpPost,xpInitialScope, xpGet, $window) {
                     $scope.activeButton = true;                   
                     $scope.buttonActived = function(value){
                         if(value === 0){
                             document.getElementById("Tarjeta").style.background = "#CCCCCC";
                             document.getElementById("Monedero").style.background = "";
                             $scope.activeButton = true;
                             return $scope.activeButton;
                         }else{
                             document.getElementById("Tarjeta").style.background = "";
                             document.getElementById("Monedero").style.background = "#CCCCCC";
                             $scope.activeButton = false;
                             return $scope.activeButton;
                         }
                     };

                     $scope.executeWizard = function() {               
                         var cards = _.filter($scope.data, function(item) { return item.type === 4; });

                         if ($scope.arguments.userHasPayment === true && cards.length === 0 && $scope.data.firsttime !== 1){
                             $scope.data.firsttime = 1;
                             $state.go('wizardUser');
                         }else if(($scope.arguments.userHasPayment !== true) && cards.length === 0 && $scope.data.firsttime !== 1){
                             $scope.data.firsttime = 1;
                             $state.go('wizard');
                             }
                     };                

                     $scope.delete = function(item) {

                         var scope = $scope.$new();
                         angular.extend(scope, xpDelete);
                         scope.apiUrl = scope.app.apiBaseUrl + item.api
                             .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                             .replace("//", "/");
                         scope.id = item.id;
                         scope.goBack = 0;

                         var hideSheet = $ionicActionSheet.show({
                             destructiveText: 'Borrar',
                             titleText: 'Tarjeta',
                             destructiveButtonClicked: function() {
                                 $ionicPopup.confirm({
                                     title: 'Borrar',
                                     template: '<div>¿Desea borrar la tarjeta ' + item.text + '?</div>',
                                     okType: 'button-payin'
                                 })
                                     .then(function(buttonOK) {
                                     // no button = 0, 'CANCELAR' = 1, 'ACEPTAR' = 2
                                     if(buttonOK){
                                         scope.accept();
                                         setTimeout(function(){ console.log("esperando..."); }, 1000);
                                        }
                                     $scope.data.splice(item.index, 1);
                                 });
                                 return true;
                             }
                         });

                         $timeout(function() {
                             hideSheet();
                         }, 10000);
                     };

                       $scope.codigoDescuento = function(){
                         var scope = $scope.$new();
                         angular.extend(scope, xpInitialScope, xpGet);
                         scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/Promotion/Check"
                             .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                             .replace("//", "/");
                         $ionicPopup.prompt({
                             title: 'Código de Descuento',
                             template: 'Introduce tu código',
                             inputType: 'text',
                             inputPlaceholder: 'Código de Descuento',
                             okType: 'button-payin'
                         }).then(function(res) {
                             // $('#descuentoCheck').css('opacity', 1);
                             if(res != undefined){
                                 $scope.arguments.promotionCode = res;
                                 $scope.arguments.promotionCodeType = 0;
                                 scope.arguments = {
                                     "code": $scope.arguments.promotionCode,
                                     "promotionCodeType": $scope.arguments.promotionCodeType
                                 };
                                 $scope.search()
                                     .then(function(data) {
                                     if(data){
                                      $scope.arguments.promotions = data.data;                                                                     
                                     }else{
                                         
                                     }
                                 })
                             }
                         });
                     };
                     $scope.activate = function(item) {

                         var scope = $scope.$new();
                         angular.extend(scope,xpPost);
                         var scope2 = $scope.$new();
                         angular.extend(scope2, xpDelete);
                         $scope.arguments.id = item.id;

                         var hideSheet = $ionicActionSheet.show({
                             destructiveText: 'Borrar',
                             titleText: 'Tarjeta',
                             buttons: [{ text: 'Activar' }],
                             destructiveButtonClicked: function() {
                                 $ionicPopup.confirm({
                                     title: 'Borrar',
                                     template: '<div>¿Desea borrar la tarjeta ' + item.title + '?</div>',
                                     okType: 'button-payin'
                                 })
                                     .then(function(buttonOK) {
                                     // no button = 0, 'CANCELAR' = 1, 'ACEPTAR' = 2        
                                     if(buttonOK){
                                         scope2.apiUrl = scope2.app.apiBaseUrl + 'mobile/paymentmedia/v1'
                                             .replace("{0}", (scope2.app.tenant ? "/" + scope2.app.tenant : ""))
                                             .replace("//", "/");
                                         scope2.id = item.id;
                                         scope2.goBack = 0;   
                                         scope2.accept();
                                        
                                        $state.reload();
                                        //$window.location.reload();
                                     }
                                 });
                                 return true;
                             },
                             buttonClicked: function(index) {
                                 if(index === 0){            
                                     var myPopup = $ionicPopup.prompt({
                                         title:"Activar tarjeta",
                                         template: '¿Desea activar la tarjeta ' + item.title + '? Para ello debe introducir su código Pay[In].',
                                         inputType: 'password',
                                         inputPlaceholder: 'Introduce tu código PIN',
                                         cancelType:'button',
                                         cssClass: 'popupBlackText',
                                         okType: 'button-payin'
                                     })
                                     .then(function(buttonOK) {
                                         scope.apiUrl = scope.app.apiBaseUrl + 'mobile/paymentmedia/v1/WebCardActivate'
                                             .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                             .replace("//", "/");
                                         $scope.arguments.pin = buttonOK;
                                         scope.goBack = 0;
                                         
                                         scope.accept()
                                             .then(function(data) {
                                             if (data && data.request) {
                                                 var query = data.request;
                                                 //Create Form
                                                 var form = document.getElementById("formPost");
                                                 var split = query.split("&");		
                                                 split.forEach(function(element) {
                                                     var input = element.split(":");
                                                     var name = input[0];
                                                     var value = input[1];
                                                     var inp = document.createElement("input");
                                                     inp.setAttribute("type","text");
                                                     inp.setAttribute("name",name);
                                                     inp.setAttribute("value",value);
                                                     form.appendChild(inp);
                                                 }, this);	
                                                 document.getElementById('slide').style.display='none';
                                                 form.submit();
                                                 document.getElementById('bankFrame').style.display='block';
                                             }
                                         });
                                     })
                                     };
                                 return true;
                             }
                         })      
                         };
                 }]
               );