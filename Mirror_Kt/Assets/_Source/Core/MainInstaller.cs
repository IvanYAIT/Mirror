using UnityEngine;
using Zenject;
using Player;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().AsSingle().NonLazy();
        }
    }
}