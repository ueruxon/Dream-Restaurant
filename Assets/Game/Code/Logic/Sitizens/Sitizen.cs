using UnityEngine;
using UnityEngine.AI;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Collectables;
using Game.Code.Logic.FoodAreas;
using Game.Code.Logic.Loots;
using Game.Code.Logic.Pools;
using Game.Code.StaticData;

public enum SitizenState
{
    None,
    Walking,
    GoingToSpot,
    Waiting,
    Interactive
}

namespace Game.Code.Logic.Sitizens
{
    public class Sitizen : PooledObject, IClient, ICoroutineRunner
    {
        [Header("References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _vipIndicator;
        [SerializeField] private FoodIndicator _foodIndicator;

        [Header("FX")] [Space(2)] 
        [SerializeField] private GameObject _happyFXTemplate;
        [SerializeField] private GameObject _sadFxTemplate;

        private IFXManagerService _fxManager;
        private SitizenStaticData _sitizenData;
        private FoodStaticData _foodData;

        private LootSpawner _lootSpawner;

        private SitizenAnimator _animator;
        private SitizenMovement _movement;
        private SitizenEmployment _employment;

        private Spot _spot;
        private Food _food;
        public SitizenState _state;

        private bool _serviced;
        public bool Serviced => _serviced;

        public void Construct(SitizenAnimator animator) =>
            _animator = animator;

        public void Init(SitizenStaticData data, IFXManagerService fxManagerService, SitizenWaypointNetwork waypointNetwork, 
            FoodStaticData foodData, LootSpawner lootSpawner, Vector3 spawnPoint)
        {
            _fxManager = fxManagerService;
            
            _sitizenData = data;
            _foodData = foodData;
            _lootSpawner = lootSpawner;

            _movement = new SitizenMovement(_animator, _agent, waypointNetwork);
            _movement.ReachedToLastPoint += OnReachedToLastPoint;
            _movement.ReachedToTarget += OnReachedToTarget;

            _employment = new SitizenEmployment(_animator, _agent, this, _sitizenData.WaitingTimeForOrder);
            _employment.EmploymentEnded += OnEmploymentEnded;
            _employment.StartWaitingOrdered += OnStartWaitingOrder;

            SetDefaultProps(spawnPoint);
            SetDefaultBehavior();
        }

        public FoodType GetFoodType() => 
            _foodData.Type;

        private void Update()
        {
            switch (_state)
            {
                case SitizenState.None:
                    break;
                case SitizenState.Walking:
                    NormalMovement();
                    break;
                case SitizenState.GoingToSpot:
                    MovementToSpot();
                    break;
                    ;
                case SitizenState.Waiting:
                    _employment.WaitingOrder(Time.deltaTime);
                    break;
                case SitizenState.Interactive:
                    _employment.Interaction(Time.deltaTime);
                    break;
            }
        }

        private void SetDefaultProps(Vector3 position)
        {
            transform.position = position;
            _serviced = false;
            _spot = null;
            _food = null;

            _foodIndicator.Init(_foodData.Icon);
            _foodIndicator.Hide();

            if (_sitizenData.Type == SitizenType.Vip)
                _vipIndicator.gameObject.SetActive(true);
            else
                _vipIndicator.gameObject.SetActive(false);
        }

        private void SetState(SitizenState newState) =>
            _state = newState;

        #region Movement

        private void SetDefaultBehavior() =>
            SetState(SitizenState.Walking);

        private void NormalMovement() =>
            _movement.Move();

        private void MovementToSpot() =>
            _movement.MoveTo(transform, _spot.transform);
        
        private void OnReachedToTarget()
        {
            SetState(SitizenState.None);
            _employment.Start(_spot, transform);
        }
        
        private void OnReachedToLastPoint() {
            _movement.ReachedToLastPoint -= OnReachedToLastPoint;
            _employment.EmploymentEnded -= OnEmploymentEnded;
            _employment.StartWaitingOrdered -= OnStartWaitingOrder;
            
            ReturnToPool(_sitizenData.Type);
        }

        #endregion

        #region Interactive

        public void SetSpot(Spot spot)
        {
            _spot = spot;
            SetState(SitizenState.GoingToSpot);
        }

        public void SetFood(Food food)
        {
            SetState(SitizenState.Interactive);

            _food = food;
            _serviced = true;
            _employment.StartInteraction();
            _foodIndicator.Hide();
        }

        private void OnStartWaitingOrder()
        {
            SetState(SitizenState.Waiting);
            
            // клиент ожидает
            _spot.Pending();
            _vipIndicator.gameObject.SetActive(false);
            _foodIndicator.Show();
        }

        private void OnEmploymentEnded(bool complete)
        {
            SetState(SitizenState.Walking);

            if (complete)
            {
                Vector3 spawnPoint = new Vector3(transform.position.x,
                    transform.position.y + _agent.height * .5f, transform.position.z);
                
                _lootSpawner.SpawnLoot(_sitizenData.LootType, _sitizenData.GetRandomLootCount(), spawnPoint);
                _food.DestroyCollectable();
            }

            PlayFx(complete ? _happyFXTemplate : _sadFxTemplate);

            _foodIndicator.Hide();
            _spot.Clear();
            _employment.Reset();
        }

        private void PlayFx(GameObject fxTemplate)
        {
            Vector3 playPoint = new Vector3(transform.position.x, _agent.height + 2f, transform.position.z);
            
            _fxManager.PlayFX(fxTemplate, playPoint);
        }

        #endregion
    }
}