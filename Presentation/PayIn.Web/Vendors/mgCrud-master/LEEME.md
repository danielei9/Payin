# Magic Crud Angular (mgCrud)

El objetivo principal de este m�dulo es demostrar como hacer todas las llamadas a servicios RESTfull que necesitemos de una forma declarativa con 7Kb y con menos de 600 l�neas de JavaScript gracias a la potencia de [Angularjs](https://angularjs.org/). Es decir en un 98% de tu app no tener que escribir ni controladores ni servicios y por tanto no c�digo.
Nos gustan los m�dulos [ngResource](https://docs.angularjs.org/api/ngResource/service/$resource) y [restangular](https://github.com/mgonto/restangular), pero nuestro objetivo es simplemente convencerte que con Angular Magic tu puedes evitar escribir JavaScript en un porcentaje muy alto de tu aplicaci�n.

## Directive mg-ajax

**mg-ajax** es la �nica directiva y tiene solo 4 atributos opcionales.

### Atributo path

Este atributo permite enlazar parte de la ruta al modelo de datos o parametros.
/invoices/{{model.id}}
/invoices/{{params.id}}
El valor por defecto de este atributo es [location.path()](https://docs.angularjs.org/api/ng/service/$location#path) como el action de un Forms de Html.

### Atributo options

Este m�dulo define c�mo y qu� se env�a a la capa servidora y c�mo se sincronizan los datos de respuesta con los ya existentes.
Lo m�s importante es definir qu� verbo http se va a utilizar entre: GET, POST, PUT, PATCH  o DELETE. Para ello se ha escrito un wrapper sobre [$http](https://docs.angularjs.org/api/ng/service/$http) que inserta llamadas a functions JavaScript en el ciclo de vida de la llamada a la servidora. Estas funciones se pueden insertar **before** o despu�s de que la promise [$http](https://docs.angularjs.org/api/ng/service/$http) retorne **success** o **error**. Adem�s te permite declarar distintos command que podr�s bindear a directivas de angular como [ngClick](https://docs.angularjs.org/api/ngTouch/directive/ngClick).
Para la configuraci�n de peticiones se han predefinido las siguientes opciones: **mgQuery**, **mgPost**, **mgPut**, **mgPach** y **mgDelete**. Si algunos de las opciones predefinidas en el m�dulo no se ajustan a tus necesidades puedes crear tus propios m�dulos o bien declarar un json con un esquema espec�fico en el atributo **options**.
Para modificar el comportamiento de un verbo en tu m�dulo o crear uno nuevo puedes implementarlo de la siguiente forma:

```
(function (angular, undefined) {
    var module;
    if (!angular) return;

    module = angular.module('myModule', ['mgCrud']);

    myIndex.$inject = ['mgIndex'];
    function myIndex(mgIndex) {
        return angular.extend(mgIndex, {init:"{index.model.description='hello'}"});
    }

    module.factory('myIndex', myIndex);

})(window.angular);
```

Una vez creado myIndex tu puedes declarar en el html el comportamiento de index de la siguiente forma:

```
<mg-ajax mg-path=�/invoices� mg-options=�myIndex�>
�
</mg-ajax> 
```

o sin declarar una nueva factor�a:

```
<mg-ajax mg-path=�/invoices� mg-options=�{.......}�>
�
</mg-ajax> 
```

### Atributo override

Este atributo se utiliza para reemplazar el comportamiento predifinido de options de forma declarativa. El tipo de dato permitido es un object js declarado como string.

```
<mg-ajax mg-path=�/invoices� mg-options=�mgIndex� mg-override=�{init:�index.filter={page:0,recordsPerPage:15}�}� >
�
</mg-ajax> 
```

En este ejemplo se utiliza el comportamiento predefinido de mgIndex pero se reemplaza en el init el valor de recordsPerPage por 15. En concreto este html realiza la petici�n:

```
GET /invoices?page=0&recordsPerPage=15
```

### Atributo partialmodel

En determinados escenarios al editar una entidad podemos mostrar una descripci�n de todas clases relacionadas y solo querer actualizar solo sus identificadores (una proyecci�n de la informaci�n almacenada en el cliente). Con esto ahorramos un gran volumen de tr�fico en cada roundtrip. Esto se puede utilizar en todos los verbos sin ninguna restricci�n, pero solo tiene sentido para los verbos http POST, PUT y PATCH.
En este ejemplo de edit solamente se enviar� al servidor el name, puesto que el id ya va en el URI.

```
<mg-ajax data-path="/invoices/get/1" data-options="mgEdit" data-scope="false">
	<mg-ajax data-path="/invoices/put/{{edit.model.id}}" data-options="mgPut" data-scope="false" data-partialmodel='{name:edit.model.name}'>
		<form name="updatefrm" ng-submit="put.accept()">
			<input type="text" ng-model="edit.model.id" />
			<input type="text" ng-model="edit.model.name" />
			<br />
			computed field: {{edit.model.computed}}
			<br />
			<button type="submit">Guardar</button>
			<button type="button" ng-click="put.close()">Cancelar</button>
		</form>
	</mg-ajax>
</mg-ajax>
```

## Modelo de objeto JavaScript para los atributos options y override

### as

El campo as tiene un comportamiento similar a ngController as y crea dentro de tu scope un object con ese nombre. Adem�s se hace un bind contra este objeto con lo que cambia el �mbito de todas las functions y nos permite utilizar "this" en cualquier function de nuestras factor�as. Es requerido y �nico dentro de un mismo scope.

###  config

Aqu� se pueden definir las cabeceras http que se quieren enviar al servidor. De forma temporal estas son almacenadas en un m�dulo y luego son enviadas al servidor cuando se realice la llamada. Con esto se puede tener diferentes proveedores de servicios RESTFul.

El objeto config tiene la siguiente signatura

```
config{url,additionalConfig{...}}
```

Con additionalConfig se pueden resolver los siguientes valores de $http

* headers
* xsrfHeaderName
* xsrfCookieName 
* transformRequest 
* transformResponse 
* cache 
* timeout 
* withCredentials 
* responseType 

Opcionalmente tambi�n se puede configurar un valor por defecto dentro de un m�dulo consumidor.

```
var module = angular.module('myModule', ['mgCrud']);
module.config(function (mgHttpProvider) {
        mgHttpProvider.setDefaultConfig({ url: 'http://localhost:48196' });
});
```

### init

Tiene un comportamiento id�ntico a ngInit. Nos permite actualizar nuestro �mbito (a unos valores predeterminados) dentro del objeto as.

### method

Los m�todos soportados son query, get, post, put, patch y delete. Este campo es requerido.

### service

Es una factor�a wrapper sobre $http a la que se le dota de m�todos cortos para query (que se resuelve con el verbo http GET) y patch. El valor es requerido y por defecto se utiliza �mgHttpFactory� aunque se puede crear un servicio propio y reemplazar este con override o crear tu propia factor�a. Se resuelve con el m�todo get de $injector. 

### before

Conjunto de funciones que se van a ejecutar antes de la llamada al servicio rest. Este campo es optional. Es una factoria donde se van a ejecutar todas las funciones de esta y en el orden en el que est�n declaradas. 
Por ejemplo antes de cada llamada http nos puede interesar establecer dentro de mi objeto �mbito (as) una propiedad show con valor a true. Esto por ejemplo  va a permitir mostrar un spinner que se ocultar� con otra function desde success y error.

```
beforeHttpFactory.$inject = ['phSpinnerFactory'];
function beforeHttpFactory(spinner) {
	return {
		show: spinner.show
	};
}
```

### success

Tiene el mismo comportamiento que before, con la �nica salvedad que se ejecuta en el success de la promise $http. Este campo es optional, pero lo m�s l�gico es usarlo para actualizar informaci�n una vez invocado al servidor con �xito.

###  error

Tiene el mismo comportamiento que success y before pero se ejecuta en caso de error de la promise $http. Tambi�n es optional.

### cacheService

Angular magic nos permite cachear informaci�n en la cache propia de angular ($cacheFactory) con el id de mgCache en localStorage y sesionStorage.

### cacheFactory

Array de datos que queremos guardar en cache y que se corresponden con nuestro m�delo.

### cacheKey

Esta es la clave de la cache que equivale a location.path() + el valor de as. Si por cualquier circustancia esto causa alg�n tipo de colisi�n ser� el desarrollador el responsable de establecer una key espec�fica.

```
if (factory.cache) {
	factory.cache = parse(factory.cache)();
factory.cacheKey = factory.cacheKey || location.path() + (factory.as || '');
```

### cmd

Factor�a de m�todos que se van a exponer como p�blicos en nuestro �mbito (as). Por ejemplo para mgIndex por defecto se exponen accept, previousPage y nextPage. Estos m�todos son los que bindearemos a nuestra vista por ejemplo en la directiva ngClick.

### auto

Funci�n que queremos que se ejecute una vez se lea la directiva, esto es v�lido para la carga inicial de datos en un index. En este caso en auto pondremos �accept�.
Esta funci�n se resuelve despu�s de resolver el atributo path mediante $attrs.observe, puesto que el atributo path es bindeable y las llamadas ajax se ejecutan de forma as�ncrona. Con lo cual si nuestro path por ejemplo contiene �invoices/{{param.id}}' no podemos ejecutar la llamada ajax para esta directiva hasta que no se haya resuelto una directiva de nivel superior en caso de anidaci�n de directivas mgAjax

```
function checkPath(fn) {
	if (factory.regexPath) {
		attrs.$observe('path', function (value) {
			var result = factory.regexPath.regexp.exec(value);
			if (result) {
				factory.path = value;
				fn();
			};
		});
	} else {
		fn();
	}
}
```

### ajaxCmd

Tiene le mismo comportamiento que auto pero para los verbos http POST, PUT, PATCH y DELETE.

## Factor�as predefinidas

### Factor�a mgIndex

```
module.factory('mgIndex', function () {
	return {
		as: 'index',
		init: 'index.filter={page:0,records:20}',
		method: 'query',
		service: 'mgHttpFactory',
		cacheService: 'mgCacheFactory',
		cache: '["filter"]',
		before: 'mgBeforeHttpFactory',
		success: 'mgSuccessFactoryIndex',
		error: 'mgErrorHttpFactory',
		cmd: 'mgCommandIndex',
		auto: 'accept'
	};
});
```

### Factor�a mgEdit

```
 module.factory('mgEdit', function () {
	return {
		as: 'edit',
		method: 'get',
		service: 'mgHttpFactory',
		cacheService: 'mgCacheFactory',
		cache: '["model"]',
		before: 'mgBeforeHttpFactory',
		success: 'mgSuccessFactoryIndex',
		error: 'mgErrorHttpFactory',
		cmd: 'mgAcceptFactory',
		auto: 'accept'
	};
});
```

### Factor�a mgPut

```
module.factory('mgPut', function () {
	return {
		as: 'put',
		init: 'put.model=edit.model',
		method: 'put',
		service: 'mgHttpFactory',
		before: 'mgBeforeHttpFactory',
		success: 'mgSuccessFactoryCreate',
		error: 'mgErrorHttpFactory',
		cmd: 'mgCommandCreate',
		ajaxCmd: 'accept'
	};
});
```

### Factor�a mgPatch

```
module.factory('mgPatch', function () {
	return {
		as: 'patch',
		init: patch.model=edit.model',
		method: 'patch',
		service: 'mgHttpFactory',
		before: 'mgBeforeHttpFactory',
		success: 'mgSuccessFactoryCreate',
		error: 'mgErrorHttpFactory',
		cmd: 'mgCommandCreate',
		ajaxCmd: 'accept'
	};
});
```

### Factor�a mgCreate

```
module.factory('mgCreate', function () {
	return {
		as: 'create',
		method: 'post',
		service: 'mgHttpFactory',
		cacheService: 'mgCacheFactory',
		cache: '["model"]',
		before: 'mgBeforeHttpFactory',
		success: 'mgSuccessFactoryCreate',
		error: 'mgErrorHttpFactory',
		cmd: 'mgCommandCreate',
		ajaxCmd: 'accept'
	};
});
```

### Factor�a mgDelete

```
module.factory('mgDelete', function () {
	return {
		as: 'delete',
		method: 'delete',
		service: 'mgHttpFactory',
		before: 'mgBeforeHttpFactory',
		success: 'mgSucessFactoryDelete',
		error: 'mgErrorHttpFactory',
		cmd: 'mgCommandCreate',
		ajaxCmd: 'accept'
	};
});
```
	
Tanto service, before, success, error y cmd son factor�as que agrupan una o m�s functions y se resuelven en tiempo de ejecuci�n con el m�todo get de $injector.
Todos los cmd reciben como par�metro un objeto factory donde guardamos el path y la resoluci�n de todas las subfactorias.
Todos las funcciones agrupadas en success y error reciben como par�metro la respuesta http agrupada en el siguiente objeto.

```
{ data: data, status: status, headers: headers, config: config }
```

Las funciones agrupadas en before no reciben ning�n par�metro.

## Modelo de datos

La directiva mgAjax, agrega internamente siempre filter y model, aunque filter solo se utiliza con options mgIndex.

Antes de enviar datos a nuestro servidor a trav�s de mgHttpFactory el modelo se resuelve con la siguiente factor�a.

```
acceptFactory.$inject = [];
function acceptFactory() {
	function accept(factory) {
		var model = (factory.partialModel && this.mgEval(factory.partialModel)) || this.filter || this.model || {};
		factory.service(factory.path, model);
	}
	return {
		accept: accept
	};
}
module.factory('mgAcceptFactory', acceptFactory);
```

Una vez que los datos son recibidos desde el servidor estos siempre se alojan en el objeto model dentro del �mbito 'as' y se resuelven con la siguiente factor�a.

```
createModelFactory.$inject = [];
function createModelFactory() {
	function assignModel(response) {
		angular.extend(this.model, response.data || {});
	}
	return {
		assignModel: assignModel
	};
 }
module.factory('mgCreateModelFactory', createModelFactory);
```

Con lo cual nuestro �mbito 'as' dentro de un scope para index quedar�a de la siguiente forma.

```
{
	filter: {page:0,recorsPerPage:25} //filtro aplicado
	model:[...]
	status:200 // despu�s de llamar al servicio http
	errorText : // solo presente si se ha producido un error una vez llamado al servicio http
	show:false // spinner oculto
	accept:function() // llama al servicio http pasando como query filter
	previousPage: function()  // resta 1 a page y llama a accept
	mgEval:function() // bindeada a $scope y de esa forma en cualquier sitio poder resolver una expresion global.
	nextPage: function() // suma 1 a page y llama a accept.   
	params:{} // solo disponible en el caso de que la ruta se resuelva con parametros.
}
```

Un ejemplo de esta representaci�n en html ser�a el siguiente.

```
<mg-ajax data-path="/invoices" data-options="mgIndex">
	<div class=�spinner� ng-show=�index.show�/>
	<div class=�error�� ng-show=�index.errorText> 
		{{index.errorText}}
	</div>
	<input type="text" ng-model="parent.filter.name" ng-change="parent.accept()" />
	<div>
		<button ng-click="index.accept()">Accept</button>
		<button ng-click="index.nextPage()">Next</button>
		<button ng-click="index.previousPage()" ng-disabled="index.filter.page==0">Previous</button>
	</div>
	<div>          
		<ul>
			<li ng-repeat="item in index.model">
				{{item.id}}-{{item.name}}
			</li>               
		</ul>
	</div>
</mg-ajax>
```

Como se puede observar se ha resuelto un sencillo index sin servicios ni controller. Lo que nos permite un lenguaje totalmente declarativo gracias a la magia de mgAjax.

## Estructura del directorio src

* Directives
  * mgAjaxDirective: Responsable de toda la m�gia del m�dulo
* Factories
  * mgCacheFactory: Agrega el m�todo responsable de gestionar la cache de Angularjs
  * mgSessionStorageFactory: Responsable de gestionar la cache con SessionStorage
  * mgLocalStorageFactory: Responsable de gestionar la cache con LocalStorage
  * mgCreateFactory: Comportamiento predefinido de para options=mgCreate
  * mgDeleteFactory: Comportamiento predefinido de para options=mgDelete
  * mgGlobalFactory: Funciones globales que nos permiten reutilizaci�n de c�digo
  * mgIndexFactory: Comportamiento predefinido de para options=mgIndex
  * mgPutFactory: Comportamiento predefinido de para options=mgPut
  * mgPatchFactory: Comportamiento predefinido de para options=mgPatch
  * mgResolveFactory: Resuelve las dependencias del controllador de la directiva
* Providers
  * mgHttpProvider: Wrapper sobre $http para permitir patch y query
* Services
  * mgResolvePathService: Resoluci�n de los path bindeados
* Global
  * module: archivo global que define una function para comprobar si un obj es vac�o

## Dependencias

El modulo mgCrud tiene dependencia de:
* Angularjs
* ngRoute

## Forma de utilizarlo

```
<script scr=�angular.js�>
<script src=�angular-route.js��>
<script src=�mgcrud.js�>
```

## Ventajas de utilizar mgCrud.

* Evitar c�digo repetitivo.
* Evitar un JavaScript muy grande en el caso de una gran app.
* Centrarnos en la vista y olvidarnos de escribir un c�digo que poco nos aporta.
* Tener dentro de nuestro scope un sub�mbito que siempre se resuelve con this.
* Encapsulaci�n de nuestro �mbito this que es igual a 'as'.
* �mbito global para los binding en el caso de anidaci�n de scopes.
* Evaluaci�n de expresiones para el �mbito this (as) con alcance global (mgEval).
* Llamada a diferentes proveedores de servicios Rest.
* Posibilidad de hacer llamadas a diferentes proveedores RESTFul desde el mismo m�dulo.


## Hoja de ruta

* Soporte para foreingKey y mantener el estado de la vista una vez que volvamos de crear una foreingKey.
* Directiva para cachear los datos de la vista actual al navegar a otras vistas.
* Directiva para hacer clear de la cache.
* Helpers para Razor de MVC
* Helpers para Jade