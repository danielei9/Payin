
angular.module('app')
    .controller('datepickerServiceController',
                ['$scope',
                 function($scope){
                     $scope.datepickerObject = {
                         titleLabel: 'Selecciona una fecha',  //Optional
                         todayLabel: 'Hoy',  //Optional
                         closeLabel: 'Cerrar',  //Optional
                         setLabel: 'Ok',  //Optional
                         setButtonType : 'button button-payin',  //Optional
                         todayButtonType : 'button button-payin',  //Optional
                         closeButtonType : 'button button-payin', //Optional
                         inputDate: new Date(),  //Optional
                         mondayFirst: true,  //Optional
                         showTodayButton: 'true', //Optional
                         templateType: 'popup', //Optional
                         to: new Date(),  //Optional
                         callback: function (val) {  //Mandatory
                               var finalDate;
                             if(val === undefined){
                               val = new Date();
                               finalDate = val.getFullYear()+'-'+(val.getMonth()+1)+'-'+val.getDate();
                               $scope.arguments.birthday = finalDate;
                                 
                             }else{
                               finalDate = val.getFullYear()+'-'+(val.getMonth()+1)+'-'+val.getDate();
                               $scope.arguments.birthday = finalDate;
                                 }
                         },
                         dateFormat: 'yyyy-MM-dd', //Optional
                         closeOnSelect: false, //Optional
                     };

                 }]);