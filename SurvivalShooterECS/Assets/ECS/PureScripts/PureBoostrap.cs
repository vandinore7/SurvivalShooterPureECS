using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class PureBootstrap
{
    public static PureSettings Settings;

    public static void NewGame()
    {
        var player = Object.Instantiate(Settings.PlayerPrefab);
        var playerEntity = player.GetComponent<GameObjectEntity>().Entity;
        var entityManager = World.Active.EntityManager;

        var mainCamera = Camera.main;
        if (mainCamera == null) { return; }

        var cameraEntity = mainCamera.GetComponent<GameObjectEntity>().Entity;

        entityManager.AddComponentData(cameraEntity, new CameraData());
        entityManager.AddComponentData(cameraEntity, new Translation());
        entityManager.AddComponentData(cameraEntity, new Rotation());

        entityManager.AddComponentData(playerEntity, new PlayerData());
        entityManager.AddComponentData(playerEntity, new Translation());
        entityManager.AddComponentData(playerEntity, new Rotation());
        entityManager.AddComponentData(playerEntity, new LocalToWorld());
        entityManager.AddComponentData(playerEntity, new HealthData { Value = Settings.StartingPlayerHealth });
        entityManager.AddComponentData(playerEntity, new PlayerInputData { Move = new float2(0, 0) });

        Settings.GameUi = GameObject.Find("GameUi").GetComponent<GameUi>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        var settingsGo = GameObject.Find("Settings");
        Settings = settingsGo?.GetComponent<PureSettings>();
        Assert.IsNotNull(Settings);
    }
}