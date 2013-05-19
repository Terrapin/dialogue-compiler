using System;
using System.Text.RegularExpressions;
using Compiler.At;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Compiler {
	public class LineOptions : IDictionary<string, object> {
		private IDictionary<string, object> inner = new Dictionary<string, object>();

		public void Add(string key, object value) {
			inner.Add(key, value);
		}

		public bool ContainsKey(string key) {
			return inner.ContainsKey(key);
		}

		public bool Remove(string key) {
			return inner.Remove(key);
		}

		public bool TryGetValue(string key, out object value) {
			return inner.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item) {
			inner.Add(item);
		}

		public void Clear() {
			inner.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item) {
			return inner.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
			inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item) {
			return inner.Remove(item);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
			return inner.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return inner.GetEnumerator();
		}

		public object this[string key] {
			get {
				return inner[key];
			}
			set {
				inner[key] = value;
			}
		}

		public ICollection<string> Keys {
			get {
				return inner.Keys;
			}
		}

		public ICollection<object> Values {
			get {
				return inner.Values;
			}
		}

		public int Count {
			get {
				return inner.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return inner.IsReadOnly;
			}
		}

		public void SetCheck(string key, object value) {
			object o;
			IDictionary<string, JToken> check;
			if (inner.TryGetValue("check", out o) && o is IDictionary<string, JToken>) {
				check = (IDictionary<string, JToken>)o;
			} else {
				check = new Dictionary<string, JToken>();
				inner["check"] = check;
			}

			check[key] = JToken.FromObject(value);
		}

		public void SetSet(string key, object value) {
			object o;
			IDictionary<string, JToken> set;
			if (inner.TryGetValue("set", out o) && o is IDictionary<string, JToken>) {
				set = (IDictionary<string, JToken>)o;
			} else {
				set = new Dictionary<string, JToken>();
				inner["set"] = set;
			}

			set[key] = JToken.FromObject(value);
		}

		[JsonIgnore]
		public bool IsEmpty { get { return inner.Count == 0; } }

		public override string ToString() {
			if (IsEmpty) {
				return "";
			}

			return JsonConvert.SerializeObject(this);
		}
	}
}

