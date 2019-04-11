using Unity.Entities;
using UnityEngine;

public class RunPureFixedUpdateSystems : MonoBehaviour
{
    private PurePlayerMovementSystem playerMovementSystem;

    //private PurePlayerInputSystem playerInputSystem;
    private PlayerInputSystem playerInputSystem;
    private PureCameraFollowSystem cameraFollowSystem;

    private void Start()
    {
        playerInputSystem = World.Active.GetOrCreateSystem<PlayerInputSystem>();
        playerMovementSystem = World.Active.GetOrCreateSystem<PurePlayerMovementSystem>();
        cameraFollowSystem = World.Active.GetOrCreateSystem<PureCameraFollowSystem>();
    }

    private void FixedUpdate()
    {
        playerInputSystem.Update();
        playerMovementSystem.Update();
        cameraFollowSystem.Update();
    }
}