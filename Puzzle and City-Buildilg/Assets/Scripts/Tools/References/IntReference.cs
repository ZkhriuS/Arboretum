﻿using System;
using UnityEngine;

namespace Tools.References
{
    [CreateAssetMenu(fileName = "IntReference", menuName = "Custom/Reference/Int Reference")]
    [Serializable]
    public class IntReference : ValueReference<int> { }
}