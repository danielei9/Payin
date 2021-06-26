angular
.module('app')
.controller('paymentMediaCreateController', ['$scope','$state', '$ionicActionSheet', '$timeout', '$ionicPopup', 'xpDelete', 
  function($scope,$state, $ionicActionSheet, $timeout, $ionicPopup, xpDelete) {
    $scope.create = function() {
      $ionicPopup.alert({
          title: 'Se requiere atención',
          template:
            '<div>Para comprobar la autenticidad de la tarjeta de crédito se le va a hacer un cargo y una devolución de 1€ en su tarjeta de crédito, es importante que disponga de saldo suficiente. Recuerde, ¡Esto no le costará nada a usted!</div>',
          okType: 'button-payin'
        });
      $scope.accept()
      .then(function(data) {
        if (data && data.request) {
          var query = data.request;
          //Create Form
          var form = document.getElementById("formPost");
          var split = query.split("&");		
          split.forEach(function(element) {
            var input = element.split("=");
            var name = input[0];
            var value = input[1];
            var inp = document.createElement("input");
            inp.setAttribute("type","text");
            inp.setAttribute("name",name);
            inp.setAttribute("value",value);
            form.appendChild(inp);
          }, this);	
          
          document.getElementById('form').style.display='none';
          form.submit();
          document.getElementById('bankFrame').style.display='visible';
        }
      })
    };
  }]
);