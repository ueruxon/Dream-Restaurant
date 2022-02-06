using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Logic.Sitizens;
using UnityEngine;

namespace Game.Code.StaticData
{
    [CreateAssetMenu(fileName = "New Sitizen", menuName = "Static Data/Sitizen Data", order = 1)]
    public class SitizenStaticData : ScriptableObject
    {
        public SitizenType Type;
        public LootType LootType;
        public Sitizen Template;
        [Range(20, 35)]
        public int WaitingTimeForOrder = 25;
        
        [Range(1, 3)][SerializeField] private int _minLootCount = 2;
        [Range(4, 7)][SerializeField] private int _maxnLootCount = 4;
        [SerializeField] private List<GameObject> _visualTemplates;

        public GameObject GetRandomVisual()
        {
            int randomIndex = Random.Range(0, _visualTemplates.Count);
            return _visualTemplates[randomIndex];
        }

        public int GetRandomLootCount() => 
            Random.Range(_minLootCount, _maxnLootCount);
    }
}