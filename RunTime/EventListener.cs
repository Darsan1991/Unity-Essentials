using System;
using System.Collections;
using System.Collections.Generic;
using DGames.Essentials.Attributes;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace DGames.Essentials
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private UnityEventRunner _runner;

        protected override void Awake()
        {
            base.Awake();
            //TODO: Uncomment
            // BindTo(CreateEventReceiver(_name), _ => { StartCoroutine(RunOnEvent()); });
        }

        private IEnumerator RunOnEvent()
        {
            yield return _runner.Run();
        }
    }

    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private UnityEventRunner<T> _runner;

        protected override void Awake()
        {
            base.Awake();
            //TODO: Uncomment
            // BindTo(CreateEventReceiver<T>(_name), (_, v) => { StartCoroutine(RunOnEvent(v)); });
        }

        private IEnumerator RunOnEvent(T val)
        {
            yield return _runner.Run(val);
        }
    }

    public abstract class BaseUnityEventRunner
    {

        public interface IAction
        {
            IEnumerator Run();
        }

        public interface IAction<in T>
        {
            IEnumerator Run(T args);
        }

        [Serializable]
        public struct WaitAction<T> : IAction<T>
        {
            [SerializeField] private float _time;

            public IEnumerator Run(T args)
            {
                yield return new WaitForSeconds(_time);
            }
        }

        [Serializable]
        public struct EventAction<T> : IAction<T>
        {
            [SerializeField] private UnityEvent<T> _action;

            public IEnumerator Run(T args)
            {
                _action?.Invoke(args);
                yield break;
            }
        }

        [Serializable]
        public struct WaitAction : IAction
        {
            [SerializeField] private float _time;

            public IEnumerator Run()
            {
                yield return new WaitForSeconds(_time);
            }
        }

        [Serializable]
        public struct EventAction : IAction
        {
            [SerializeField] private UnityEvent _action;

            public IEnumerator Run()
            {
                _action?.Invoke();
                yield break;
            }
        }

        public enum ActionType
        {
            Event,
            Wait
        }
    }

    [Serializable]
    public class UnityEventRunner<T> : BaseUnityEventRunner
    {

        [SerializeField] private List<ActionGroup> _actions = new();

        public IEnumerator Run(T obj)
        {

            foreach (var action in _actions)
            {
                yield return action.Run(obj);
            }
        }

        [Serializable]
        public struct ActionGroup : IAction<T>
        {
            [SerializeField] private ActionType _actionType;

            [Condition(nameof(_actionType), ActionType.Wait)] [SerializeField]
            private WaitAction<T> _waitAction;

            [Condition(nameof(_actionType), ActionType.Event)] [SerializeField]
            private EventAction<T> _eventAction;


            public IEnumerator Run(T args)
            {
                yield return GetAction().Run(args);
            }

            public IAction<T> GetAction()
            {
                return _actionType switch
                {
                    ActionType.Event => _eventAction,
                    ActionType.Wait => _waitAction,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }


    [Serializable]
    public class UnityEventRunner : BaseUnityEventRunner
    {

        [SerializeField] private List<ActionGroup> _actions = new();

        public IEnumerator Run()
        {
            foreach (var action in _actions)
            {
                yield return action.Run();
            }
        }

        [Serializable]
        public struct ActionGroup : IAction
        {
            [SerializeField] private ActionType _actionType;

            [Condition(nameof(_actionType), ActionType.Wait)] [SerializeField]
            private WaitAction _waitAction;

            [Condition(nameof(_actionType), ActionType.Event)] [SerializeField]
            private EventAction _eventAction;


            public IEnumerator Run()
            {
                yield return GetAction().Run();
            }

            public IAction GetAction()
            {
                return _actionType switch
                {
                    ActionType.Event => _eventAction,
                    ActionType.Wait => _waitAction,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

}

namespace DGames.Essentials.UI{
}