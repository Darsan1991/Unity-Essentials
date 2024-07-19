using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DGames.Essentials
{
    public static class EssentialCoroutine
    {
        public static IEnumerator WaitUntil(Func<bool> until, Action action = null)
        {
            yield return new WaitUntil(until);
            action?.Invoke();
        }
        
        public static IEnumerator Delay(float seconds, Action action = null)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }
        
        public static IEnumerator MoveTowardsEnumerator(float start = 0f, float end = 1f, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time=1f)
        {
            var speed = Mathf.Abs(end-start)/time;
            if (Math.Abs(start - end) < float.Epsilon)
            {
                onFinished?.Invoke();
                yield break;
            }

            var currentNormalized = start;
            while (true)
            {
                currentNormalized = Mathf.MoveTowards(currentNormalized, end,  speed* Time.deltaTime);

                if (start < end && currentNormalized >= end || start > end && currentNormalized <= end)
                {
                    currentNormalized = end;
                    onCallOnFrame?.Invoke(currentNormalized);
                    break;
                }
                
                onCallOnFrame?.Invoke(currentNormalized);
                yield return null;
            }
            onFinished?.Invoke();
        }
        
        public static IEnumerator MoveTowardsEnumerator(AnimationCurve curve, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time = 1,float offset=0f)
        {
            var start = curve.keys.Min(k=>k.time);
            var end = curve.keys.Max(k=>k.time);
            
            yield return MoveTowardsEnumerator(start + (end-start)*offset, end, n => onCallOnFrame?.Invoke(curve.Evaluate(n)), time: Mathf.Max(0.01f,time*(1-offset)),
                onFinished: onFinished);
        }
    }
    




}