using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenWaypointNetwork
    {
        public event Action LastPointReached;
        
        private List<Transform> _waypointList;

        private int _currentIndex = 0;

        public SitizenWaypointNetwork(List<Transform> sitizenWaypointList) => 
            _waypointList = sitizenWaypointList;

        public Transform GetNextWaypoint()
        {
            if (_waypointList.Count == 0)
            {
                return null;
            }
            
            Transform nextWaypoint = null;
            int nextIndex = _currentIndex + 1;

            if (nextIndex <= _waypointList.Count)
            {
                nextWaypoint = _waypointList[_currentIndex];
                _currentIndex = nextIndex;
                
                if (_currentIndex == _waypointList.Count)
                    LastPointReached?.Invoke();
            }

            return nextWaypoint;
        }
    }
}
