<script src="~/Vendors/angulr/vendor/angular/angular.js"></script>
<script type="text/javascript">
  angular.module('Store', [])
       .controller('CtrlOne', ['$http', '$scope', function ($http, $scope) {
			$http.get('/Api/Shop')
			.then(function (data) {
               $scope.events = data.data;
			})
       }])
</script>