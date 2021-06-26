using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
{
	public static class ObjectExtension
	{
		//#region GetPropertyValue
		//public static object GetPropertyValue(this object THIS, string propertyName)
		//{
		//	return THIS.GetPropertyValue<object>(propertyName);
		//}
		//public static T GetPropertyValue<T>(this object THIS, string propertyName)
		//{
		//	if (THIS == null)
		//		return default(T);
		//	if (string.IsNullOrEmpty(propertyName))
		//		return default(T);

		//	var roles = propertyName.Split('.').ToList();
		//	var rol = roles.FirstOrDefault();
		//	roles.RemoveAt(0);
		//	var path = string.Join(".", roles.ToArray());

		//	object value = null;
		//	var property = THIS.GetType().GetPropertyInfo(rol);
		//	if ((property == null) && (!string.IsNullOrEmpty(path)))
		//	{
		//		rol += "_" + path.Replace(".", "_");
		//		path = "";
		//		property = THIS.GetType().GetPropertyInfo(rol);
		//	}
		//	if (property == null)
		//		return default(T);

		//	value = property.GetValue(THIS, null);
		//	if ((value != null) && (!string.IsNullOrEmpty(path)))
		//		value = value.GetPropertyValue<T>(path);

		//	return (T)value;
		//}
		//public static object GetPropertyValue_Path(this object THIS, string path, bool withGuionize = false)
		//{
		//	return THIS.GetPropertyValue_Path<object>(path, withGuionize);
		//}
		//public static T GetPropertyValue_Path<T>(this object THIS, string path, bool withGuionize = false)
		//{
		//	if (string.IsNullOrEmpty(path))
		//		return default(T);

		//	return THIS.GetPropertyValue_Path<T>(path.Split('.'), withGuionize);
		//}
		//public static object GetPropertyValue_Path(this object THIS, IEnumerable<string> roles, bool withGuionize = false)
		//{
		//	return THIS.GetPropertyValue_Path<object>(roles, withGuionize);
		//}
		//public static T GetPropertyValue_Path<T>(this object THIS, IEnumerable<string> roles, bool withGuionize = false)
		//{
		//	if (roles.Count() == 0)
		//		return default(T);

		//	var role = roles.First();
		//	var lastRoles = roles.Skip(1).ToList();

		//	var propertyInfo = THIS.GetType().GetPropertyInfo(role);
		//	if (propertyInfo != null)
		//	{
		//		var value = THIS.GetPropertyValue<T>(role);
		//		if (lastRoles.Count() > 0)
		//			return value.GetPropertyValue_Path<T>(lastRoles, withGuionize);

		//		return value;
		//	}

		//	if ((withGuionize) && (propertyInfo == null) && (lastRoles.Count() > 0))
		//		return THIS.GetPropertyValue<T>(string.Join("_", roles));

		//	return default(T);
		//}
		//#endregion GetPropertyValue

		//#region SetPropertyValue
		//public static void SetPropertyValue(this object THIS, string propertyName, object value)
		//{
		//	if (string.IsNullOrEmpty(propertyName))
		//		return;

		//	var propertyInfo = THIS.GetType().GetPropertyInfo(propertyName);
		//	if (propertyInfo == null)
		//		return;

		//	propertyInfo.SetValue(THIS, value, null);
		//}
		//public static void SetPropertyValue_Path(this object THIS, string pathToDo, object value, bool createInstances = false,
		//																				 string serviceOperationName = "", Dictionary<string, Type> tipos = null)
		//{
		//	THIS.SetPropertyValue_Path(pathToDo.Split('.'), new List<string>(), value, createInstances, serviceOperationName,
		//														 tipos);
		//}
		//private static void SetPropertyValue_Path(this object THIS, IEnumerable<string> rolesToDo,
		//																					IEnumerable<string> rolesDone, object value, bool createInstances = false,
		//																					string serviceOperationName = "", Dictionary<string, Type> tipos = null)
		//{
		//	if (rolesToDo.Count() == 0)
		//		return;

		//	var role = rolesToDo.First();
		//	var lastRoles = rolesToDo.Skip(1).ToList();
		//	var firstRoles = rolesDone.ToList();
		//	firstRoles.Add(role);

		//	if (THIS.GetType().GetPropertyInfo(role) == null)
		//		return;

		//	if (lastRoles.Count() > 0)
		//	{
		//		var elementType = THIS.GetType().GetPropertyType(role);
		//		var collectionType = elementType.InheriteFrom(typeof(IEnumerable<>));
		//		if (collectionType != null)
		//			elementType = collectionType.GetGenericArguments()[0];

		//		if (tipos.ContainsKey(serviceOperationName + "_" + string.Join("_", firstRoles)))
		//			elementType = tipos[serviceOperationName + "_" + string.Join("_", firstRoles)];

		//		if (collectionType != null)
		//		{
		//			object element = null;

		//			var collection = THIS.GetPropertyValue<IList>(role);
		//			if (collection.Count() == 0)
		//			{
		//				if (createInstances)
		//					collection.Add(element = elementType.CreateEntity());
		//			}
		//			else
		//				element = collection[0];

		//			if (element != null)
		//				element.SetPropertyValue_Path(lastRoles, firstRoles, value, createInstances, serviceOperationName, tipos);
		//		}
		//		else
		//			THIS.SetPropertyValue(role, value);
		//	}
		//	else
		//		THIS.SetPropertyValue(role, value);
		//}
		//#endregion SetPropertyValue

		//#region ConvertTo
		//public static object ConvertTo(this object THIS, Type type)
		//{
		//	if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
		//		return THIS == null ? null : THIS.ConvertTo(type.GetGenericArguments()[0]);
		//	if ((THIS is DateTimeOffset) && (type == typeof(DateTime)))
		//		return ((DateTimeOffset)THIS).DateTime;

		//	return Convert.ChangeType(THIS, type, CultureInfo.CurrentCulture);
		//}
		//public static T ConvertTo<T>(this object THIS)
		//{
		//	return (T)THIS.ConvertTo(typeof(T));
		//}
		//#endregion ConvertTo

		//#region SetPropertyValueWithConversion
		//public static void SetPropertyValueWithConversion(this object THIS, string propertyName, object value)
		//{
		//	var propertyType = THIS.GetType().GetPropertyType(propertyName);

		//	if (value == null)
		//		THIS.SetPropertyValue(propertyName, null);
		//	else
		//		THIS.SetPropertyValue(propertyName, value.ConvertTo(propertyType));
		//}
		//#endregion SetPropertyValueWithConversion

		//#region AddPropertyValues
		//public static void AddPropertyValues(this object THIS, IList<string> paths, IList<object> values)
		//{
		//	for (var i = 0; i < values.Count(); i++)
		//		THIS.AddPropertyValue(paths[i], values[i]);
		//}
		//#endregion AddPropertyValues

		//#region AddPropertyValue
		//public static void AddPropertyValue(this object THIS, string path, object value)
		//{
		//	var roles = path.Split('.');

		//	// El path se ha terminado
		//	if (roles.Length == 0)
		//		return;
		//	var rol = roles[0];
		//	if (string.IsNullOrEmpty(rol))
		//		return;

		//	if (value == DBNull.Value)
		//		value = null;

		//	// Eliminar el rol del path
		//	if (roles.Length > 1)
		//		path = path.Substring(path.IndexOf('.') + 1);
		//	else
		//		path = "";

		//	var propertyType = THIS.GetType().GetPropertyType(rol);
		//	if ((propertyType == null) && (!string.IsNullOrEmpty(path)))
		//	{
		//		rol += "_" + path.Replace(".", "_");
		//		propertyType = THIS.GetType().GetPropertyType(rol);
		//		path = "";
		//	}
		//	if (propertyType == null)
		//		return;

		//	if (string.IsNullOrEmpty(path))
		//		// Dato-valuado
		//		THIS.SetPropertyValueWithConversion(rol, value);
		//	else
		//	// Objeto-valuado
		//	{
		//		#region Asignar valores

		//		var collection = THIS.GetPropertyValue<IList>(rol);
		//		//object instanceRelated = null;

		//		if (collection != null)
		//		{
		//			if (collection.Count > 0)
		//			{
		//				// La instancia ya existe, sólo hay que propagar
		//				AddPropertyValue(collection[0], path, value);
		//			}
		//			else
		//			{
		//				// La instancia relacionada no existe, hay que crear la nueva instancia y propagar
		//				var instanceRelated = Activator.CreateInstance(propertyType);
		//				THIS.ExecuteMethod("Add" + rol, new object[] { instanceRelated });

		//				AddPropertyValue(instanceRelated, path, value);
		//			}
		//		}
		//		else
		//		{
		//			// La instancia relacionada no existe, hay que crear la nueva instancia y propagar
		//			var instanceRelated = Activator.CreateInstance(propertyType);
		//			THIS.SetPropertyValue(rol, instanceRelated);

		//			AddPropertyValue(instanceRelated, path, value);
		//		}

		//		#endregion Asignar valores
		//	}
		//}
		//#endregion AddPropertyValue

		#region DynamicCast
		public static object DynamicCast(this object THIS, Type targetType)
		{
			if (THIS == null)
				return null;

			var sourceType = THIS.GetType();

			// Mirar si la conversión es automatica
			if (targetType.IsAssignableFrom(sourceType))
				return THIS;

			// Mirar si existe algun cast
			var methodInfo = sourceType.GetMethodInfo("op_Implicit", targetType);
			if (methodInfo == null)
				methodInfo = sourceType.GetMethodInfo("op_Explicit", targetType);
			if (methodInfo != null)
				return methodInfo.Invoke(null, new object[] { THIS });

			return THIS.ConvertTo(targetType);
		}
		public static T DynamicCast<T>(this object THIS)
		{
			return (T)THIS.DynamicCast(typeof(T));
		}
		#endregion DynamicCast

		#region ExecuteMethod
		public static object ExecuteMethod(this object THIS, string methodName, object[] parameters)
		{
			var methodInfo = THIS.GetType().GetMethodInfo(methodName, parameters);
			if (methodInfo == null)
				return null;

			return methodInfo.Invoke(THIS, parameters);
		}
		#endregion ExecuteMethod
	}
}

