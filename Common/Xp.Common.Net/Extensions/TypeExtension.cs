using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
	public static partial class TypeExtension
	{
		#region GetProperties
		public static PropertyInfo[] GetProperties(this Type THIS, Type type)
		{
			return (
				from t in THIS.GetProperties()
				where t.PropertyType.IsAssignableFrom(type)
				select t
			).ToArray();
		}
		#endregion GetProperties

		//#region CreateEntity
		//public static object CreateEntity(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values, IEnumerable<int> indices)
		//{
		//	// Crear instancia
		//	var instance = THIS.CreateEntity();
		//	foreach (var i in indices)
		//	{
		//		if (i < 0)
		//			continue;
		//		instance.AddPropertyValue(paths.ElementAt(i), values.ElementAt(i));
		//	}

		//	return instance;
		//}
		//public static object CreateEntity(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values)
		//{
		//	// Crear instancia
		//	var instance = THIS.CreateEntity();
		//	for (var i = 0; i < paths.Count(); i++)
		//		instance.AddPropertyValue(paths.ElementAt(i), values.ElementAt(i));

		//	return instance;
		//}
		//public static object CreateEntity(this Type THIS, IEnumerable<string> paths, Object value)
		//{
		//	// Crear instancia
		//	var instance = THIS.CreateEntity();
		//	for (var i = 0; i < paths.Count(); i++)
		//	{
		//		var attribPath = paths.ElementAt(i);
		//		var attribValue = value.GetPropertyValue(paths.ElementAt(i));
		//		instance.AddPropertyValue(attribPath, attribValue);
		//	}

		//	return instance;
		//}
		//public static object CreateEntity(this Type THIS)
		//{
		//	// Crear instancia
		//	return Activator.CreateInstance(THIS);
		//}
		//public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values, IEnumerable<int> indices)
		//	where E : class
		//{
		//	return THIS.CreateEntity(paths, values, indices) as E;
		//}
		//public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values)
		//	where E : class
		//{
		//	return THIS.CreateEntity(paths, values) as E;
		//}
		//public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, Object value)
		//	where E : class
		//{
		//	return THIS.CreateEntity(paths, value) as E;
		//}
		//public static E CreateEntity<E>(this Type THIS)
		//	where E : class
		//{
		//	return THIS.CreateEntity() as E;
		//}
		//#endregion CreateEntity

		//#region CreateObject
		//public static object CreateObject(this Type THIS, params object[] args)
		//{
		//	return Activator.CreateInstance(THIS, args);
		//}
		//public static O CreateObject<O>(this Type THIS, params object[] args)
		//	where O : class
		//{
		//	return THIS.CreateObject(args) as O;
		//}
		//#endregion CreateObject

		//#region GetPropertyInfo
		//public static PropertyInfo GetPropertyInfo_Path(this Type THIS, string path, bool withGuionize = false)
		//{
		//	if (string.IsNullOrEmpty(path))
		//		return null;

		//	return THIS.GetPropertyInfo_Path(path.Split('.'), withGuionize);
		//}
		//public static PropertyInfo GetPropertyInfo_Path(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		//{
		//	if (roles.Count() == 0)
		//		return null;

		//	var role = roles.First();
		//	var lastRoles = roles.Skip(1).ToList();

		//	var propertyInfo = THIS.GetPropertyInfo(role);
		//	if (propertyInfo != null)
		//	{
		//		if (lastRoles.Count() > 0)
		//			return propertyInfo.PropertyType.GetPropertyInfo_Path(lastRoles, withGuionize);

		//		return propertyInfo;
		//	}

		//	if ((withGuionize) && (propertyInfo == null) && (lastRoles.Count() > 0))
		//		return THIS.GetPropertyInfo(string.Join("_", roles));

		//	return null;
		//}
		//public static PropertyInfo GetPropertyInfo(this Type THIS, string propertyName)
		//{
		//	return THIS.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase)
		//		.Where(x => string.Compare(x.Name, propertyName, true) == 0)
		//		.FirstOrDefault();
		//}
		//#endregion GetPropertyInfo

		//#region GetPropertyType
		//public static Type GetPropertyType(this Type THIS, string propertyName)
		//{
		//	if (string.IsNullOrEmpty(propertyName))
		//		return THIS;

		//	var propertyInfo = THIS.GetPropertyInfo(propertyName);
		//	if (propertyInfo == null)
		//		return null;

		//	return propertyInfo.PropertyType;
		//}
		//public static Type GetPropertyType_Path(this Type THIS, string path, bool withGuionize = false)
		//{
		//	if (string.IsNullOrEmpty(path))
		//		return THIS;

		//	return THIS.GetPropertyType_Path(path.Split('.'), withGuionize);
		//}
		//public static Type GetPropertyType_Path(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		//{
		//	if (roles.Count() == 0)
		//		return THIS;

		//	var propertyInfo = THIS.GetPropertyInfo_Path(roles, withGuionize);
		//	if (propertyInfo == null)
		//		return null;

		//	return propertyInfo.PropertyType;
		//}
		//#endregion GetPropertyType

		#region GetMethodInfo
		public static MethodInfo GetMethodInfo(this Type that, string methodName, object[] parameters)
		{
			var arrayTypes = new Type[parameters.Count()];
			for (int i = 0; i < arrayTypes.Count(); i++)
				arrayTypes[i] = parameters[i]?.GetType();

			var methodInfos = (
				from m in that.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase)
				where (string.Compare(m.Name, methodName, StringComparison.CurrentCultureIgnoreCase) == 0)
				select m
			);

			foreach (var m in methodInfos)
			{
				int i = 0;
				var isMethod = true;
				foreach (var p in m.GetParameters())
				{
					if (arrayTypes[i] != null && !p.ParameterType.IsAssignableFrom(arrayTypes[i]))
					{
						isMethod = false;
						break;
					}
					i++;
				}
				if (isMethod)
					return m;
			}

			return null;
		}
		public static MethodInfo GetMethodInfo(this Type THIS, string methodName, Type returnType)
		{
			return (
				from m in THIS.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase)
				where
					(string.Compare(m.Name, methodName, StringComparison.CurrentCultureIgnoreCase) == 0) &&
					(m.ReturnType == returnType)
				select m
			).FirstOrDefault();
		}
		#endregion GetMethodInfo

		//#region InheriteFrom
		//public static Type InheriteFrom(this Type THIS, Type supertype)
		//{
		//	if (THIS == supertype)
		//		return THIS;
		//	if ((THIS.IsGenericType) && (THIS.GetGenericTypeDefinition() == supertype))
		//		return THIS;

		//	foreach (var i in THIS.GetInterfaces())
		//	{
		//		var temp = i.InheriteFrom(supertype);
		//		if (temp != null)
		//			return temp;
		//	}

		//	return THIS.BaseType.InheriteFrom(supertype); ;
		//}
		//#endregion InheriteFrom

		#region GetEnumValues
		public static IEnumerable<int> GetEnumValues(this Type value)
		{
			return GetEnumValues<int>(value);
		}
		public static IEnumerable<T> GetEnumValues<T>(this Type value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (!typeof(Enum).IsAssignableFrom(value))
				throw new ArgumentException(string.Format("En tipo {0} debe ser un enumerado", value.Name), "value");

			var result = (
				from field in value.GetFields()
				where
					field.IsStatic
				select ((T)field.GetValue(null))
			).ToList();

			return result;
		}
		#endregion GetEnumValues

		#region ExecuteStaticMethod
		public static object ExecuteStaticMethod(this Type THIS, string methodName, params object[] parameters)
		{
			var methodInfo = THIS.GetMethodInfo(methodName, parameters);
			if (methodInfo == null)
				return null;

			return methodInfo.Invoke(null, parameters);
		}
		#endregion ExecuteStaticMethod
	}
}
