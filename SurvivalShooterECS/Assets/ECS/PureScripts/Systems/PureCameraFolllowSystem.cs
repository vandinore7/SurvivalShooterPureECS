﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PurePlayerMovementSystem))]
public class PureCameraFollowSystem : JobComponentSystem
{
    private EntityQuery m_LocalPlayer;

    protected override void OnCreate()
    {
        // Cached access to a set of ComponentData based on a specific query
        m_LocalPlayer = GetEntityQuery(
            ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<PlayerData>());
    }

    [BurstCompile]
    public struct CameraFollowJob : IJobForEach<Translation, Rotation, CameraData>
    {
        [DeallocateOnJobCompletion]
        public NativeArray<Translation> target;

        public float deltaTime;

        public void Execute(ref Translation camPosition, ref Rotation camRotation, [ReadOnly]  ref CameraData c2)
        {
            // check if player exist
            if (target.Length == 0)
            {
                return;
            }

            // Follow the Player
            float3 desiredPosition = target[0].Value + 50;
            float3 smoothedPosition = math.lerp(camPosition.Value, desiredPosition, 5 * deltaTime);
            camPosition.Value = smoothedPosition;

            Debug.Log(camPosition.Value);

            // Rotate Camera to the Player
            float3 lookVector = target[0].Value - camPosition.Value;
            Quaternion rotation = Quaternion.LookRotation(lookVector);
            camRotation.Value = rotation;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CameraFollowJob
        {
            target = m_LocalPlayer.ToComponentDataArray<Translation>(Allocator.TempJob),
            deltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }
}