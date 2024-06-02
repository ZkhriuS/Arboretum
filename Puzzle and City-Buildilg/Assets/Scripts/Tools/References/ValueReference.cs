using System;
using UnityEngine;

namespace Tools.References
{
    [Serializable]
    public abstract class ValueReference<T> : ScriptableObject
    {
        [SerializeField]
        private T value;

        public Action OnValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke();
            }
        }
        //
        // public static implicit operator T(ValueReference<T> reference)
        // {
        //     return reference.Value;
        // }
        //
        // public static implicit operator ValueReference<T>(T newValue)
        // {
        //     var reference = CreateInstance<ValueReference<T>>();
        //     reference.Value = newValue;
        //     return reference;
        // }
    }
}