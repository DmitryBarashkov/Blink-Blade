using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [Header("Настройки языковых флагов")]
    [SerializeField] private List<LanguageSpritePair> _languageFlags;
    [Header("База врагов")]
    [SerializeField] private EnemyDatabase _enemyDatabase;
    [SerializeField] private WeaponDatabase _weaponDatabase;

    public override void InstallBindings()
    {
        Container.Bind<EnemyDatabase>().FromInstance(_enemyDatabase).AsSingle();
        Container.Bind<WeaponDatabase>().FromInstance(_weaponDatabase).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<LevelLoadService>().AsSingle();
        Container.BindInterfacesTo<Bootstrap>().AsSingle().NonLazy();
        Container.Bind<InputService>().AsSingle();
        Container.Bind<SavesYG>().AsSingle();        

        BindLanguages();
    }

    private void BindLanguages()
    {
        var flagsDictionary = new Dictionary<string, Sprite>();

        foreach (var pair in _languageFlags)
        {
            if (pair.Sprite == null) continue;

            string key = pair.LanguageCode.ToLower().Trim();

            if (!flagsDictionary.ContainsKey(key))
            {
                flagsDictionary.Add(key, pair.Sprite);
            }
            else
            {
                Debug.LogWarning($"[GlobalInstaller] Дубликат ключа языка: {key}");
            }
        }

        Container.Bind<Dictionary<string, Sprite>>().WithId("Languages").FromInstance(flagsDictionary).AsSingle();
    }

    [Serializable]
    public struct LanguageSpritePair
    {
        [Tooltip("Код языка, например: ru, en, de")]
        public string LanguageCode;

        [Tooltip("Спрайт флага для этого языка")]
        public Sprite Sprite;
    }
}
