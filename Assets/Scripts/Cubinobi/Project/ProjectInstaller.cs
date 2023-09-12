using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Cubinobi.Project
{
    
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EventManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        }
    }
}