//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;
//using UnitySampleAssets.CrossPlatformInput;

//public class PurePlayerInputSystem : JobComponentSystem
//{
//    private EntityQuery m_LocalPlayer;

// protected override void OnCreate() { m_LocalPlayer = GetEntityQuery( ComponentType.ReadOnly<Translation>(),

// ComponentType.ReadOnly<PlayerInputData>(), ComponentType.Exclude<DeadData>()); }

// [BurstCompile] public struct PlayerMoveJob : IJobForEach<PlayerInputData> {
// [DeallocateOnJobCompletion] public NativeArray<Translation> target;

// public void Execute(ref PlayerInputData input) { // check if player exist if (target.Length == 0)
// { return; }

// var newInput = new PlayerInputData { Move = new
// float2(CrossPlatformInputManager.GetAxisRaw("Horizontal"),
// CrossPlatformInputManager.GetAxisRaw("Vertical")) };

// input = newInput; } }

//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {
//        var job = new PlayerMoveJob
//        {
//            target = m_LocalPlayer.ToComponentDataArray<Translation>(Allocator.TempJob),
//        };
//        return job.Schedule(this, inputDeps);
//    }
//}