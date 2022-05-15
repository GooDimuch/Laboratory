using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

public static class EnumExtension {
	/// <summary>
	/// HasFlag()
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="values">Enum value (flag)</param>
	/// <returns>Enum value cast to int</returns>
	public static bool Contains<T>(this T container, params T[] values) where T : Enum => 
			values.All(value => container.HasFlag(value));

	/// <summary>
	/// Set flag value(s) 1
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="value">Enum flag value(s)</param>
	/// <returns>Enum value cast to int</returns>
	public static T On<T>(this ref T container, T value) where T : struct, Enum {
		if (Enum.GetUnderlyingType(typeof(T)) == typeof(int)) {
			container = (T) (object) ((int) (object) container | (int) (object) value);
		} else { container = (T) (object) ((long) (object) container | (long) (object) value); }
		return container;
	}

	/// <summary>
	/// Set flag value(s) 1 and set other values 0
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="value">Enum flag value(s)</param>
	/// <returns>Enum value cast to int</returns>
	public static T OnOnly<T>(this ref T container, T value) where T : struct, Enum {
		container.OffAll();
		return container.On(value);
	}

	/// <summary>
	/// Set all flag values 1
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <returns>Enum value cast to int</returns>
	public static T OnAll<T>(this ref T container) where T : struct, Enum {
		if (Enum.GetUnderlyingType(typeof(T)) == typeof(int)) { container = (T) (object) ~(int) 0; } else {
			container = (T) (object) ~(long) 0;
		}
		return container;
	}

	/// <summary>
	/// Set flag value(s) 0
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="value">Enum flag value(s)</param>
	/// <returns>Enum value cast to int</returns>
	public static T Off<T>(this ref T container, T value) where T : struct, Enum {
		if (Enum.GetUnderlyingType(typeof(T)) == typeof(int)) {
			container = (T) (object) ((int) (object) container & ~(int) (object) value);
		} else { container = (T) (object) ((long) (object) container & ~(long) (object) value); }
		return container;
	}

	/// <summary>
	/// Set all flag values 0
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <returns>Enum value cast to int</returns>
	public static T OffAll<T>(this ref T container) where T : struct, Enum {
		if (Enum.GetUnderlyingType(typeof(T)) == typeof(int)) { container = (T) (object) (int) 0; } else {
			container = (T) (object) (long) 0;
		}
		return container;
	}

	/// <summary>
	/// Get flag value(s). Alternative HasFlag()
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="value">Enum flag value(s)</param>
	/// <returns>Enum value cast to int</returns>
	public static bool Get<T>(this T container, T value) where T : struct, Enum => container.Contains(value);

	/// <summary>
	/// Set indicated value to flag value(s)
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <param name="value">Enum flag value(s)</param>
	/// <returns>Enum value cast to int</returns>
	public static T Set<T>(this ref T container, T value, bool state) where T : unmanaged, Enum {
		container = state ? container.On(value) : container.Off(value);
		return container;
	}

	/// <summary>
	/// Cast to int
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <returns>Enum value cast to int</returns>
	public static int TI<T>(this T container) where T : Enum => (int) (object) container;

	/// <summary>
	/// Cast to long
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <returns>Enum value cast to long</returns>
	public static long TL<T>(this T container) where T : Enum => (long) (object) container;

	/// <summary>
	/// ToString()
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	/// <param name="container">Enum ref</param>
	/// <returns>Enum value to string</returns>
	public static string TS<T>(this T container) where T : Enum => container.ToString();

    /// <summary>
    /// Get description of enum value
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="e">Enum ref</param>
    /// <returns>Return description attribute value.</returns>
    public static string GetDescription<T>(this T e) where T : Enum, IConvertible {
		var type = e.GetType();
		var values = Enum.GetValues(type);

		foreach (int val in values) {
			if (val != e.ToInt32(CultureInfo.InvariantCulture)) { continue; }
			var memInfo = type.GetMember(((T) (object) val).ToString());
			var first = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
			if (first == null) { return string.Empty; }
			if (first.GetType() == typeof(DescriptionAttribute)) { return ((DescriptionAttribute) first).Description; }
		}
		return string.Empty;
    }

    public static List<string> AllDescriptions<T>(this T container) where T : Enum => container.ListValues().Select(rs => rs.GetDescription()).ToList();

    public static T GetByIndex<T>(this T e, int idx) where T : Enum, IConvertible => e.ListValues()[idx];

    /// <summary>
    /// List of all enum values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="e"></param>
    /// <returns></returns>
    public static List<T> ListValues<T>(this T e) where T : Enum, IConvertible
    {
        List<T> result = new List<T>();
        foreach (var eval in Enum.GetValues(typeof(T)))
            result.Add((T)eval);
        return result;
    }

    /// <summary>
    /// Finds enum value with specified description attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="e"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static T FindWithDescription<T>(this T e, string description) where T : Enum, IConvertible => ListValues<T>(e).FirstOrDefault(v => v.GetDescription() == description);

    public static bool TryFindWithDescription<T>(this T e, string description, out T found) where T : Enum, IConvertible
    {
        var list = ListValues<T>(e);
        foreach (var v in ListValues<T>(e))
            if (v.GetDescription() == description)
            {
                found = v;
                return true;
            }
        found = default(T);
        return false;
    }
}