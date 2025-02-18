using System;
using System.Collections.Generic;

namespace LitematicaPreviewer.LitematicaLib.Common
{
    public delegate (bool, string) ValidatorFunction(string key, object item);
    public delegate void ReactionFunction(string key, object item);

    public class DiscriminatingDictionary(ValidatorFunction validator, ReactionFunction onAdd = null, ReactionFunction onRemove = null) : Dictionary<string, object>
    {
        private readonly ValidatorFunction _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ReactionFunction _onAdd = onAdd;
        private readonly ReactionFunction _onRemove = onRemove;

        private void Validate(string key, object item)
        {
            var (canStore, msg) = _validator(key, item);
            if (!canStore)
            {
                throw new InvalidOperationException(msg);
            }
        }

        public new void Add(string key, object item)
        {
            Validate(key, item);
            base.Add(key, item);
            _onAdd?.Invoke(key, item);
        }

        public new bool Remove(string key)
        {
            if (TryGetValue(key, out var item))
            {
                var removed = base.Remove(key);
                if (removed)
                {
                    _onRemove?.Invoke(key, item);
                }
                return removed;
            }
            return false;
        }

        public new object this[string key]
        {
            get => base[key];
            set
            {
                Validate(key, value);
                if (ContainsKey(key))
                {
                    var oldItem = base[key];
                    base[key] = value;
                    _onRemove?.Invoke(key, oldItem);
                }
                else
                {
                    base[key] = value;
                }
                _onAdd?.Invoke(key, value);
            }
        }

        public new void Clear()
        {
            var items = new Dictionary<string, object>(this);
            base.Clear();
            foreach (var kvp in items)
            {
                _onRemove?.Invoke(kvp.Key, kvp.Value);
            }
        }

        public new void Update(IDictionary<string, object> other)
        {
            foreach (var kvp in other)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        public new object Pop(string key)
        {
            if (TryGetValue(key, out var item))
            {
                base.Remove(key);
                _onRemove?.Invoke(key, item);
                return item;
            }
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        public new KeyValuePair<string, object> PopItem()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The dictionary is empty.");
            }
            var enumerator = GetEnumerator();
            enumerator.MoveNext();
            var kvp = enumerator.Current;
            Remove(kvp.Key);
            _onRemove?.Invoke(kvp.Key, kvp.Value);
            return kvp;
        }
    }
}
