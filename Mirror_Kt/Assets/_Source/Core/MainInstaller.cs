using Zenject;
using UnityEngine;
using PlayerCharacter;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private GameObject gameUi;

        public override void InstallBindings()
        {
            Container.Bind<GameObject>().FromInstance(gameUi).AsSingle().NonLazy();

            Container.Bind<PlayerController>().AsSingle().NonLazy();
        }
    }
}