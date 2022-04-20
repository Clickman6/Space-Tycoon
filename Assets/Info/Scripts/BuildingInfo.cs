using System.Numerics;
using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building/Create Building")]
public class BuildingInfo : ScriptableObject {
    public string Title;
    [TextArea(1, 10)] public string Description;
    public uint PriceForBuy;
    public uint PriceForUpdate;
    public float UpgradeMultiplierCoefficient;
    public int StepForUpgrade;
    public UpdatableType Type;

    public enum UpdatableType {
        IncomeForPerson,
        People,
        LifeQuality,
        Rocket,
    }
}
