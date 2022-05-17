using System;
using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Infrastructure.States;
using Zenject;

namespace neeksdk.Scripts.Infrastructure.DIInstallers
{
    public class DiMonoInstaller : MonoInstaller<DiMonoInstaller>, IInitializable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DiMonoInstaller>().FromInstance(this).AsSingle();
            
            BindServices();
            BindStateMachine();
        }
        
        public void Initialize() => StartGame();

        private void BindServices()
        {
            
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
            
            StateMachine stateMachine = Container.Resolve<StateMachine>();
            stateMachine.SetupStateMachine(GetStateMachineStates());
            stateMachine.Enter<LoadingState>();
        }
    }
}
