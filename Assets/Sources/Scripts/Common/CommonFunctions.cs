using System;
using System.Collections.Generic;
using BlinkBlade.Game;
using UnityEngine;
using YG;

namespace BlinkBlade.Common
{
    public static class CommonFunctions
    {
        public static void FixPositionZ(Transform transform, float fixedZ = 0)
        {
            Vector3 fixedZPosition = transform.position;

            fixedZPosition.z = fixedZ;
            transform.position = fixedZPosition;
        }

        public static T GetRandomElement<T>(IReadOnlyList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentNullException(nameof(list));

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static void ShowAdvForReward(IAudioService audioService, string rewardId, Action callback)
        {
            audioService.PlaySound(SoundType.ButtonClick);
            audioService.Deactivate();

            YG2.RewardedAdvShow(rewardId, () =>
            {
                audioService.Activate();
                callback?.Invoke();
            });
        }
    }
}