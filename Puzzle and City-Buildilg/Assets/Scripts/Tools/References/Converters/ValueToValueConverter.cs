using UnityEngine;
using UnityEngine.Events;

namespace Tools.References.Converters
{
    public abstract class ValueToValueConverter<TV1, TV2> : MonoBehaviour
    {
        public UnityEvent<TV2> valueConverted;

        protected void OnValueConverted(TV2 convertedValue)
        {
            valueConverted?.Invoke(convertedValue);
        }
        
        public abstract void Convert(TV1 value);
    }
}