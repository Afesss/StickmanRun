using UnityEngine;
using PathCreation;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TouchPad>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PathCreator>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Strafe>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Movement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Spawner>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Finish>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Score>().AsSingle();
    }
}