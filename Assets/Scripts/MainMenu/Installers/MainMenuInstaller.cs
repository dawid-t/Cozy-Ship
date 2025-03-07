using Zenject;
using Critsoft.CozyShip.MainMenu.Models;
using Critsoft.CozyShip.MainMenu.Views;
using Critsoft.CozyShip.MainMenu.Controllers;

namespace Critsoft.CozyShip.MainMenu.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Models, Views, Controllers
            Container.Bind<MainMenuModel>().AsSingle().NonLazy();
            Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MainMenuController>().FromComponentInHierarchy().AsSingle();

            // Other
            Container.Bind<GameResultsStorage>().AsSingle();
            Container.Bind<AmbientAudioManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
