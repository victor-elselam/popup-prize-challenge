using DG.Tweening;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public static class ExtensionMethods
    {
        public static Tweener DOTextDouble(this Text text, double initialValue, double finalValue, float duration, Func<double, string> convertor)
        {
            return DOTween.To(() => initialValue,
                it => text.text = convertor(it).ToString(CultureInfo.InvariantCulture),
                finalValue,
                duration);
        }

        public static float GetAnimationTime(float currentValue)
        {
            var minTime = 1f;
            var maxTime = 6;

            //normalize prize between 0 and 30.000
            var normalizedPrizeValue = (float)currentValue / 30000;
            //find a time between min and max that fits prize (lerp clamps for min and max)
            var speed = Mathf.Lerp(minTime, maxTime, normalizedPrizeValue);
            return speed;
        }
    }
}