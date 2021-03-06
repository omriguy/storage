﻿using System.Collections.Generic;

namespace OhioBox.Storage.MySql
{
	internal static class DictionaryExtentions
	{
		internal static TValue GetItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key, TValue @default = default(TValue))
		{
			return target.TryGetValue(key, out var value) ? value : @default;
		}
	}
}