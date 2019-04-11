using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(TransformSystemGroup))]
public class PurePlayerMovementSystem : JobComponentSystem
{
    private EntityQuery m_LocalPlayer;

    protected override void OnCreate()
    {
        m_LocalPlayer = GetEntityQuery(
            ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<PlayerInputData>(),
            ComponentType.ReadOnly<Rigidbody>(),
            ComponentType.Exclude<DeadData>());
    }

    [BurstCompile]
    public struct PlayerMoveJob : IJobForEach<Translation, Rotation, PlayerInputData>
    {
        [DeallocateOnJobCompletion]
        public NativeArray<Translation> target;

        public float speed;
        public float deltaTime;

        public void Execute(ref Translation playerPosition,
            ref Rotation playerRotation,
            [ReadOnly]  ref PlayerInputData input)
        {
            // check if player exist
            if (target.Length == 0) { return; }

            var move = input.Move;
            var zero = new float2(0, 0);
            var test = move == zero;

            if (test.x == true && test.y == true) { return; }
            var movement = new float3(move.x, 0, move.y);
            movement = math.normalize(movement);

            // Move the Player
            //float3 currentPosition = target[0].Value;
            float3 desiredPosition = target[0].Value + movement;
            float3 smoothedPosition = math.lerp(playerPosition.Value, desiredPosition, speed * deltaTime);
            playerPosition.Value = smoothedPosition;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new PlayerMoveJob
        {
            target = m_LocalPlayer.ToComponentDataArray<Translation>(Allocator.TempJob),
            deltaTime = Time.deltaTime,
            speed = 6
        };

        return job.Schedule(this, inputDeps);
    }
}