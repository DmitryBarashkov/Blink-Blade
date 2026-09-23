using System;
using System.Collections.Generic;
using BlinkBlade.Enemies;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class GlobalInstaller : MonoInstaller
    {
        [Header("Databases")]
        [SerializeField] private List<PlayerEquipment> _weapons;
        [SerializeField] private List<PlayerEquipment> _skins;
        [Header("Services")]
        [SerializeField] private AudioService _audioServicePrefab;
        [Header("Languages flags")]
        [SerializeField] private List<LanguageSpritePair> _languageFlags;

        public override void InstallBindings()
        {
            BindLoaders();
            BindData();
            BindServices();
            BindLanguages();
        }

        private void BindLoaders()
        {
            Container.Bind<SavesYG>().AsSingle();
            Container.BindInterfacesTo<Bootstrap>().AsSingle().NonLazy();
        }

        private void BindData()
        {
            Container.Bind<List<PlayerEquipment>>().WithId("Weapons").FromInstance(_weapons);
            Container.Bind<List<PlayerEquipment>>().WithId("Skins").FromInstance(_skins);
            Container.Bind<EnemyFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<LevelLoadService>().AsSingle();
            Container.Bind<LevelState>().AsSingle().NonLazy();
            Container.Bind<InputService>().AsSingle();
            Container.Bind<LevelBridge>().AsSingle();

            Container.BindInterfacesAndSelfTo<AudioService>()
                .FromComponentInNewPrefab(_audioServicePrefab)
                .UnderTransformGroup("GlobalServices")
                .AsSingle()
                .NonLazy();
        }

        private void BindLanguages()
        {
            var flagsDictionary = new Dictionary<string, Sprite>();

            foreach (var pair in _languageFlags)
            {
                if (pair.Sprite == null)
                    continue;

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

            Container.Bind<Dictionary<string, Sprite>>().WithId("Languages").FromInstance(flagsDictionary);
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
}