using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace System
{
	public static class TypeExtension
	{
		#region GetAscendants
		public static IEnumerable<Type> GetAscendants(this Type that)
		{
			var result = new List<Type>();

			if (that.GetTypeInfo().BaseType != null)
			{
				result.Add(that.GetTypeInfo().BaseType);
				result.AddRange(that.GetTypeInfo().BaseType.GetAscendants());
			}

			return result;
		}
		#endregion GetAscendants

		#region CreateEntity
		public static object CreateEntity(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values, IEnumerable<int> indices)
		{
			// Crear instancia
			var instance = THIS.CreateEntity();
			foreach (var i in indices)
			{
				if (i < 0)
					continue;
				instance.AddPropertyValue(paths.ElementAt(i), values.ElementAt(i));
			}

			return instance;
		}
		public static object CreateEntity(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values)
		{
			// Crear instancia
			var instance = THIS.CreateEntity();
			for (var i = 0; i < paths.Count(); i++)
				instance.AddPropertyValue(paths.ElementAt(i), values.ElementAt(i));

			return instance;
		}
		public static object CreateEntity(this Type THIS, IEnumerable<string> paths, Object value)
		{
			// Crear instancia
			var instance = THIS.CreateEntity();
			for (var i = 0; i < paths.Count(); i++)
			{
				var attribPath = paths.ElementAt(i);
				var attribValue = value.GetPropertyValue(paths.ElementAt(i));
				instance.AddPropertyValue(attribPath, attribValue);
			}

			return instance;
		}
		public static object CreateEntity(this Type THIS)
		{
			// Crear instancia
			return Activator.CreateInstance(THIS);
		}
		public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values, IEnumerable<int> indices)
			where E : class
		{
			return THIS.CreateEntity(paths, values, indices) as E;
		}
		public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, IEnumerable<Object> values)
			where E : class
		{
			return THIS.CreateEntity(paths, values) as E;
		}
		public static E CreateEntity<E>(this Type THIS, IEnumerable<string> paths, Object value)
			where E : class
		{
			return THIS.CreateEntity(paths, value) as E;
		}
		public static E CreateEntity<E>(this Type THIS)
			where E : class
		{
			return THIS.CreateEntity() as E;
		}
		#endregion CreateEntity

		#region CreateObject
		public static object CreateObject(this Type THIS, params object[] args)
		{
			return Activator.CreateInstance(THIS, args);
		}
		public static O CreateObject<O>(this Type THIS, params object[] args)
			where O : class
		{
			return THIS.CreateObject(args) as O;
		}
		#endregion CreateObject

		#region GetPropertyValue
		public static object GetPropertyValue(this Type THIS, string propertyName)
		{
			return THIS.GetPropertyValue<object>(propertyName);
		}
		public static T GetPropertyValue<T>(this Type THIS, string propertyName)
		{
			if (THIS == null)
				return default(T);
			if (string.IsNullOrEmpty(propertyName))
				return default(T);

			var roles = propertyName.Split('.').ToList();
			var rol = roles.FirstOrDefault();
			roles.RemoveAt(0);
			var path = string.Join(".", roles.ToArray());

			object value = null;
			var property = THIS.GetPropertyInfo(rol);
			if ((property == null) && (!string.IsNullOrEmpty(path)))
			{
				rol += "_" + path.Replace(".", "_");
				path = "";
				property = THIS.GetPropertyInfo(rol);
			}
			if (property == null)
				return default(T);

			value = property.GetValue(null);
			if ((value != null) && (!string.IsNullOrEmpty(path)))
				value = value.GetPropertyValue<T>(path);

			return (T)value;
		}
		public static object GetPropertyValue_Path(this Type THIS, string path, bool withGuionize = false)
		{
			return THIS.GetPropertyValue_Path<object>(path, withGuionize);
		}
		public static T GetPropertyValue_Path<T>(this Type THIS, string path, bool withGuionize = false)
		{
			if (string.IsNullOrEmpty(path))
				return default(T);

			return THIS.GetPropertyValue_Path<T>(path.Split('.'), withGuionize);
		}
		public static object GetPropertyValue_Path(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		{
			return THIS.GetPropertyValue_Path<object>(roles, withGuionize);
		}
		public static T GetPropertyValue_Path<T>(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		{
			if (roles.Count() == 0)
				return default(T);

			var role = roles.First();
			var lastRoles = roles.Skip(1).ToList();

			var propertyInfo = THIS.GetPropertyInfo(role);
			if (propertyInfo != null)
			{
				var value = THIS.GetPropertyValue<T>(role);
				if (lastRoles.Count() > 0)
					return value.GetPropertyValue_Path<T>(lastRoles, withGuionize);

				return value;
			}

			if ((withGuionize) && (propertyInfo == null) && (lastRoles.Count() > 0))
				return THIS.GetPropertyValue<T>(string.Join("_", roles));

			return default(T);
		}
		#endregion GetPropertyValue

		#region GetPropertyInfo
		public static PropertyInfo GetPropertyInfo_Path(this Type THIS, string path, bool withGuionize = false)
		{
			if (string.IsNullOrEmpty(path))
				return null;

			return THIS.GetPropertyInfo_Path(path.Split('.'), withGuionize);
		}
		public static PropertyInfo GetPropertyInfo_Path(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		{
			if (roles.Count() == 0)
				return null;

			var role = roles.First();
			var lastRoles = roles.Skip(1).ToList();

			var propertyInfo = THIS.GetPropertyInfo(role);
			if (propertyInfo != null)
			{
				if (lastRoles.Count() > 0)
					return propertyInfo.PropertyType.GetPropertyInfo_Path(lastRoles, withGuionize);

				return propertyInfo;
			}

			if ((withGuionize) && (propertyInfo == null) && (lastRoles.Count() > 0))
				return THIS.GetPropertyInfo(string.Join("_", roles));

			return null;
		}
		public static PropertyInfo GetPropertyInfo(this Type THIS, string propertyName)
		{
			return THIS.GetRuntimeProperties()
				.Where(x => string.Compare(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) == 0)
				.FirstOrDefault();
		}
		#endregion GetPropertyInfo

		#region GetPropertyType
		public static Type GetPropertyType(this Type THIS, string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				return THIS;

			var propertyInfo = THIS.GetPropertyInfo(propertyName);
			if (propertyInfo == null)
				return null;

			return propertyInfo.PropertyType;
		}
		public static Type GetPropertyType_Path(this Type THIS, string path, bool withGuionize = false)
		{
			if (string.IsNullOrEmpty(path))
				return THIS;

			return THIS.GetPropertyType_Path(path.Split('.'), withGuionize);
		}
		public static Type GetPropertyType_Path(this Type THIS, IEnumerable<string> roles, bool withGuionize = false)
		{
			if (roles.Count() == 0)
				return THIS;

			var propertyInfo = THIS.GetPropertyInfo_Path(roles, withGuionize);
			if (propertyInfo == null)
				return null;

			return propertyInfo.PropertyType;
		}
		#endregion GetPropertyType

		#region InheriteFrom
		public static Type InheriteFrom(this Type THIS, Type supertype)
		{
			if (THIS == supertype)
				return THIS;
			if ((THIS.GetTypeInfo().IsGenericType) && (THIS.GetGenericTypeDefinition() == supertype))
				return THIS;

			foreach (var i in THIS.GetTypeInfo().ImplementedInterfaces)
			{
				var temp = i.InheriteFrom(supertype);
				if (temp != null)
					return temp;
			}

			return THIS.GetTypeInfo().BaseType.InheriteFrom(supertype); ;
		}
		#endregion InheriteFrom
	}
}
