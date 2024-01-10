using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MemberData")]
public class MemberData : ScriptableObject
{
    [field: SerializeField] public float Velocity { get; private set; }
    [field: SerializeField] public float Agility { get; private set; }
    [field: SerializeField] public float Stamina { get; private set; }

    private void OnEnable()
    {
        InitRandomStats();
    }

    private void InitRandomStats()
    {
        Velocity = UnityEngine.Random.Range(2f, 5f);
        Agility = UnityEngine.Random.Range(2f, 5f);
        Stamina = UnityEngine.Random.Range(2f, 5f);
    }
}
