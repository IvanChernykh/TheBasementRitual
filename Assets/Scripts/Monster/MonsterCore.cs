using UnityEngine;
using UnityEngine.AI;

public class MonsterCore : MonoBehaviour {
    [SerializeField] private MonsterAnimation monsterAnimation;
    [SerializeField] private MonsterSounds monsterSounds;
    [SerializeField] private NavMeshAgent monsterAgent;

    public MonsterAnimation Animation { get => monsterAnimation; }
    public MonsterSounds Sounds { get => monsterSounds; }
    public NavMeshAgent Agent { get => monsterAgent; }
}
