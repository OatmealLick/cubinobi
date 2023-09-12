using UnityEngine;
using Zenject;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        public Settings Settings;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Settings>().FromInstance(Settings);
        }
    }
}