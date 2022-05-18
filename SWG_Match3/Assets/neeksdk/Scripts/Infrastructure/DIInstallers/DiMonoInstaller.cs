using System;
using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Infrastructure.Factory;
using neeksdk.Scripts.Infrastructure.Pool;
using neeksdk.Scripts.Infrastructure.States;
using neeksdk.Scripts.StaticData;
using UnityEngine;
using Zenject;

namespace neeksdk.Scripts.Infrastructure.DIInstallers
{
    public class DiMonoInstaller : MonoInstaller<DiMonoInstaller>, IInitializable
    {
        [SerializeField] private ObjectPool _objectPool;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DiMonoInstaller>().FromInstance(this).AsSingle();
            
            BindServices();
            BindStateMachine();
        }
        
        public void Initialize() =>
            StartGame();

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ObjectPool>().FromInstance(_objectPool).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            BindStates();
        }

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<LoadingState>().AsSingle();
        }

        private Dictionary<Type, IExitableState> GetStateMachineStates()
        {
            return new Dictionary<Type, IExitableState>()
            {
                [typeof(LoadingState)] = Container.Resolve<LoadingState>()
            };
        }

        private void StartGame()
        {
            DOTween.Init(logBehaviour: LogBehaviour.ErrorsOnly);

            StaticDataService staticDataService = Container.Resolve<StaticDataService>();
            staticDataService.LoadTiles();
            staticDataService.LoadBackgrounds();
            
            StateMachine stateMachine = Container.Resolve<StateMachine>();
            stateMachine.SetupStateMachine(GetStateMachineStates());
            stateMachine.Enter<LoadingState>();
        }
    }
}
