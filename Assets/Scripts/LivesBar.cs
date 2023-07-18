using System;
using System.Linq;
using UnityEngine;

public partial class HUD
{
    [Serializable]
    private class LivesBar
    {
        [SerializeField] private GameObject _liveBarParent;

        private LiveBarObject[] _allLivesChildren;
        private LiveBarObject[] _activeLivesChildren;
        
        public void InitHealthChildren(Player player)
        {
            _allLivesChildren = Resources.FindObjectsOfTypeAll<LiveBarObject>();

            for (int i = 0; i < player.MaxLives; i++)
            {
                ObjectPool.Instance.SpawnFromPoolToParent("HealthObject", _liveBarParent.transform);
            }

            _activeLivesChildren = _allLivesChildren.Where(healthObj => healthObj.gameObject.activeInHierarchy).ToArray();
        }

        public void LoseLiveChild()
        {
            foreach (LiveBarObject healthObj in _activeLivesChildren)
            {
                if (!healthObj.isActiveAndEnabled) continue;

                healthObj.gameObject.SetActive(false);
                break;
            }
        }
    }
}
