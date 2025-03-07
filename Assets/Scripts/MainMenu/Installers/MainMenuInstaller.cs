using Zenject;
using Critsoft.CozyShip.MainMenu.Models;
using Critsoft.CozyShip.MainMenu.Views;
using Critsoft.CozyShip.MainMenu.Controllers;
using TMPro;
using UnityEngine;

namespace Critsoft.CozyShip.MainMenu.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private TMP_FontAsset _standardFont;
        [SerializeField] private TMP_FontAsset _alternativeFont;

        public override void InstallBindings()
        {
            // Models, Views, Controllers
            Container.Bind<MainMenuModel>().AsSingle().NonLazy();
            Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MainMenuController>().FromComponentInHierarchy().AsSingle();

            // Other
            Container.Bind<GameResultsStorage>().AsSingle();
            Container.Bind<AmbientAudioManager>().FromComponentInHierarchy().AsSingle();

            TMP_FontAsset[] availableFonts = new TMP_FontAsset[] { _standardFont, _alternativeFont };
            Container.Bind<TMP_FontAsset[]>().WithId(GameConfig.AvailableFontsId).FromInstance(availableFonts).AsSingle();
        }
    }
}
