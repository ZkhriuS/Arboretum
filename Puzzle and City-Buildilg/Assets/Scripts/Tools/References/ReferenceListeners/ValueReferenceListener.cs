using UnityEngine;
using UnityEngine.Events;

namespace Tools.References.ReferenceListeners
{
    public class ValueReferenceListener<T> : MonoBehaviour
    {
        public ValueReference<T> valueReference;
        
        public UnityEvent<T> valueChanged;

        private void Start()
        {
            ValueChanged();
        }

        private void OnEnable()
        {
            if (valueReference != null)
            {
                valueReference.OnValueChanged += ValueChanged;
            }
        }
    
        private void OnDisable()
        {
            if (valueReference != null)
            {
                valueReference.OnValueChanged -= ValueChanged;
            }
        }
    
        private void ValueChanged()
        {
            valueChanged?.Invoke(valueReference.Value);
        }
    }
}