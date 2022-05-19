using System;
using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board;
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
        [SerializeField] private BoardController _boardController;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DiMonoInstaller>().FromInstance(this).AsSingle();
            
            BindServices();
            BindStateMachine();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        }
        
        public void Initialize() =>
            StartGame();

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ObjectPool>().FromInstance(_objectPool).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardController>().FromInstance(_boardController).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            BindStates();
        }

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<LoadingState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSelectionState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameAnimationState>().AsSingle();
        }

        private Dictionary<Type, IExitableState> GetStateMachineStates()
        {
            return new Dictionary<Type, IExitableState>()
            {
                [typeof(LoadingState)] = Container.Resolve<LoadingState>(),
                [typeof(GameSelectionState)] = Container.Resolve<GameSelectionState>(),
                [typeof(GameAnimationState)] = Container.Resolve<GameAnimationState>()
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
