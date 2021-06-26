angular
.module('xp.database', ['ngCordova.plugins.sqlite'])
.factory('xpDatabase', ['$cordovaSQLite', '$rootScope', 
    function($cordovaSQLite, $rootScope) {
        var db;
        return {
            initDb:function() {
                if (!$rootScope.initDb)
                    angular.extend($rootScope, this);
                if (!db)
                {
                    db = $cordovaSQLite.openDB({ name: "payin.db" });
                 //   alert(db);
                    $cordovaSQLite.execute(db, "CREATE TABLE IF NOT EXISTS options (Id integer PRIMARY KEY AUTOINCREMENT, Name nvarchar, Value nvarchar, ValueType nvarchar)");
                }
            },
           insertDb: function(table, name, value, valueType) {
                var query = "INSERT INTO "+table+ " (name, value,valueType) VALUES (?,?,?)";
                //alert(db + " " + query + " " + name + " " + value + " " + valueType);
                $cordovaSQLite.execute(db, query, [name, value, valueType])
                .then(function(res) {
                  //  alert("INSERT ID -> " + res.insertId);
                }, function (err) {
                    console.error(err);
                });
            },
            selectDb: function(name) {
                var query = "SELECT name, value, valueType FROM options WHERE name = ?";
                $cordovaSQLite.execute(db, query, [lastname]).then(function(res) {
                    if(res.rows.length > 0) {
                    //    alert(res.rows.item(0).name)
//                        console.log("SELECTED -> " + res.rows.item(0).name + " " + res.rows.item(0).value" " + res.rows.item(0).valueType);
                    } else {
                        console.log("No results found");
                    }
                }, function (err) {
                    console.error(err);
                });
            }  
        };
    }
])
.directive('xpDatabase', [function() {
    //Etiqueta
    return {
        restrict: 'A',
        controller: ['$rootScope', 'xpDatabase',
             function($rootScope, xpDatabase) {
                 angular.extend($rootScope, xpDatabase);  
                 $rootScope.initDb();
             }
        ]
    };
}]);