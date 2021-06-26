angular
    .module('app')
    .controller('ticketCreateController',
                ['$scope', 'xpNavigation', '$state','xpInitialScope','xpGet',
                 function($scope, xpNavigation, $state, xpInitialScope, xpGet) {

                     setTimeout(function() {
                         $('#inputImporte').val('Importe');
                         $('#inputImporte').css('color', '#EEEEEE');
                         $('#inputImporte').css('margin-top','1.6em');
                         $('#inputImporte').css('text-align','center');                                     
                     },100);
                     $scope.focusStyles = function(){

                         $('#importIluminate').css('outline', 'none');
                         $('#importIluminate').css('borderColor', '#9ecaed');
                         $('#importIluminate').css('boxShadow','0 0 10px #9ecaed');
                         $('#inputImporte').css('color', '#000000');

                         if($('#inputImporte').val() === 'Importe'){
                             $('#inputImporte').val('');
                         }
                     }
                     $scope.blurStyles = function(){

                         $('#importIluminate').css('outline', 'none');
                         $('#importIluminate').css('border', '1px solid #DDDDDD');
                         $('#importIluminate').css('boxShadow','none');
                         $('#inputImporte').css('color', '#000000');

                     }

                     $scope.create = function(amount, title, reference) {
                         var that = this;
                         var scope = that.$new();
                         var date = scope.arguments.date;

                         angular.extend(scope, xpInitialScope, xpGet);
                         scope.apiUrl = scope.app.apiBaseUrl + "mobile/ticket/v2"
                             .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                             .replace("//", "/");
                         scope.arguments.reference = reference;
                         scope.arguments.lines =[];
                         scope.arguments.lines.push(JSON.stringify({
                             lines:{
                                 title:title,
                                 amount:amount,
                                 quantity:1
                             }
                         }));
                         if(scope.argumentSelect.concessionId.items.length===1)
                             scope.arguments.concessionId = $scope.argumentSelect.concessionId.items[0].id;
                         scope.arguments.date = date;
                         delete scope.arguments.amount;
                         delete scope.arguments.title;
                         scope.search()
                             .then(function(data) {
                             if (data) {
                                 scope.isPaymentWorker = true;
                                 data.isPaymentWorker = scope.isPaymentWorker;
                                 xpNavigation.goBackAndGo(1, "ticketget", {
                                     data: data,
                                     id: data.data[0].id});
                             }
                         });

                     };
                     $scope.createTicketMetro = function() {
                         $scope.accept()
                             .then(function(data) {
                             if (data) {
                                 xpNavigation.goBackAndGo(1, "ticketget", { id: data.id });
                             }
                         });

                     };
                 }]
               );