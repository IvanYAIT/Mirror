using Network;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuData menuData;
        [SerializeField] private Connection connection;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuData>().FromInstance(menuData).AsSingle().NonLazy();
            Container.Bind<Connection>().FromInstance(connection).AsSingle().NonLazy();
        }
    }
}