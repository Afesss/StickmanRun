using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Installers/PlayerSettings")]
public class GameSettings : ScriptableObjectInstaller<GameSettings>
{
    public SpawnerSettings SpawnerSettings;
    public PlayerSettings PlayerSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(SpawnerSettings);
        Container.BindInstance(PlayerSettings);
    }
}