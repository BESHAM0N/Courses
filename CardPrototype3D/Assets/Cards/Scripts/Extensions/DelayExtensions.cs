using System;
using System.Collections;
using UnityEngine;

namespace Cards.Extensions
{
    public static class DelayExtensions
    {
        public static IEnumerator DoActionAfterSecondsRoutine(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }
    }
}