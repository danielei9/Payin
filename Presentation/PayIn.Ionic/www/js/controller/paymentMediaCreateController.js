angular
.module('app')
.controller('paymentMediaCreateController', ['$scope','$state', '$ionicActionSheet', '$timeout', '$ionicPopup', 'xpDelete','$ionicSlideBoxDelegate', '$ionicLoading',
  function($scope,$state, $ionicActionSheet, $timeout, $ionicPopup, xpDelete,$ionicSlideBoxDelegate, $ionicLoading ) {
  $scope.show = function() {
    $ionicLoading.show({
      template: 'Loading...'
    });
  };
  $scope.hide = function(){
    $ionicLoading.hide();
  };
    $scope.next = function(nameCard) {
      var name = nameCard;
      var current = $ionicSlideBoxDelegate.currentIndex();
      var slidesCount = $ionicSlideBoxDelegate.slidesCount();
      if(current == slidesCount-1)
        $scope.create(name);
      else 
        $ionicSlideBoxDelegate.$getByHandle('facil').next();
    };
    $scope.previous = function() {
        $ionicSlideBoxDelegate.$getByHandle('facil').previous();
    };
    
    $scope.create = function(name) {
      $scope.arguments.name = name;
      
        $ionicPopup.alert({
          title: 'Se requiere atención',
          template:
            '<div>Para comprobar la autenticidad de la tarjeta de crédito se le va a hacer un cargo y una devolución de 1€ en su tarjeta de crédito, es importante que disponga de saldo suficiente. Recuerde, ¡Esto no le costará nada a usted!</div>',
          okType: 'button-payin'
          })
        .then(function(res) {
          if(res) {
          
          $scope.arguments.bankEntity = $scope.arguments.name;
            $scope.accept()
            .then(function(data) {
              if (data && data.request) {
                $scope.show();
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
                setTimeout(function(){ $scope.hide(); }, 4000);
                document.getElementById('slide').style.display='none';
                form.submit();
                document.getElementById('bankFrame').style.display='block';
              }
            });
          }       
        });
    };
  }]
);
