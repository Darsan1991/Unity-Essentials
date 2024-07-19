using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DGames.Essentials.Extensions;
using DGames.ObjectEssentials;
using UnityEngine;

namespace DGames.Essentials
{
    public partial class MonoBehaviour : UnityEngine.MonoBehaviour
    {

    
        
        protected readonly List<IDisposable> disposablesAtDestroy = new();
        protected readonly List<BindDetails> unBindAt = new();

        protected virtual void Awake()
        {
            RegisterThisIfCan();
        }


        protected virtual void OnDisable()
        {
            unBindAt.Where(d=>d is { Bind: not null, Binding: not null, Duration: Duration.Active }).ForEach(p=>p.Bind.UnBind(p.Binding));
            unBindAt.RemoveAll(d => d.Duration == Duration.Active);
        }

      


        protected void DisposeAtDestroy(params IDisposable[] disposables) => disposablesAtDestroy.AddRange(disposables);

        #region Receiver

        public Receiver<T> ReceiverToGetFromGlobalService<T>()
        {
            var receiver = ReceiverUtils.ReceiverToGetFromGlobalService<T>();
            DisposeAtDestroy(receiver);
            return receiver;
        }
        
        public Receiver<TJ> CreateReceiver<T,TJ>(T key,Receiver<IProvider<T,TJ>> providerReceiver,bool autoDispose= true)
        {
            var receiver = new Receiver<T,TJ>(key,providerReceiver);
            if(autoDispose) 
                DisposeAtDestroy(receiver);
            return receiver;
        }
        
        public Receiver<TJ> CreateReceiver<T,TJ>(T key,IRepoProvider<T,TJ> provider,bool autoDispose= true)
        {
            var receiver = new Receiver<T,TJ>(key,provider);
            if(autoDispose) 
                DisposeAtDestroy(receiver);
            return receiver;
        }
        

        #endregion

        #region Binder

        public void BindTo<T>(IEvent<T> e, Action<IEvent, T> action,Duration duration) where T : IEventArgs
        {
            UnBindAt(e.Bind(action,this),this,duration);
        }
        
        public void BindTo(IEvent e, Action<IEvent> action,Duration duration)
        {
            UnBindAt(e.Bind(action,this),this,duration);
        }
        
        public void BindTo<T>(EventReceiver<T> e, Action<IEvent,T> action,Duration duration = Duration.LifeTime)
        {
            var eArgsBinder = e.ArgsBinder;
            eArgsBinder.Bind(action, this);
            UnBindAt(eArgsBinder,this,duration);
        }
        
        public void BindTo(EventReceiver e, Action<IEvent> action,Duration duration = Duration.LifeTime)
        {
            var eArgsBinder = e.ArgsBinder;
            eArgsBinder.Bind(action, this);
            UnBindAt(eArgsBinder,this,duration);
        }

        public void BindTo<T>(ValueReceiver<T> receiver, Action<T> action,Duration duration = Duration.LifeTime)
        {
            var binder = receiver.ContentBinder;
            binder.Bind(action,this);
            UnBindAt(binder,this,duration);
        }
        

        #endregion
        
        protected void UnBindAt(BinderBase bind, object obj,Duration duration = Duration.LifeTime)
        {
            unBindAt.Add(new BindDetails
            {
                Bind = bind,
                Binding = obj,
                Duration = duration
            });
        }

        protected virtual void OnDestroy()
        {
            UnRegisterThisIfCan();
            disposablesAtDestroy.Where(d=>d!=null).ForEach(d=>d.Dispose());
            unBindAt.Where(d=>d.Bind!=null && d.Binding!=null && d.Duration == Duration.LifeTime).ForEach(p=>p.Bind.UnBind(p.Binding));
        }

        #region Services

        private void RegisterThisIfCan()
        {
            RegisterDetails.RegisteringTypes?.ForEach(t => Services.Register(t, this, RegisterDetails.Tag, RegisterDetails.IsGlobal ? null : gameObject));
        }
        private void UnRegisterThisIfCan()
        {
            RegisterDetails.RegisteringTypes?.ForEach(t =>
            {
                if(RegisterDetails.IsGlobal)
                    Services.UnRegister(t, RegisterDetails.Tag);
                else
                    Services.GetLocalService(gameObject.scene).UnRegister(new TypeAndTag{Type = t,Tag = tag});
            });
        }

        public static void RegisterGlobally<T>(T instance, string tag=null)
        {
            Services.Register(instance, tag);
        }
        
        public void RegisterLocally<T>(T instance, string tagOfInstance=null)
        {
            Services.Register(instance, tagOfInstance,gameObject);
        }

        public static void UnRegister<T>(string tag = null) => Services.UnRegister<T>(tag);
        
        public virtual ServiceRegisterDetails RegisterDetails => new();

        
        public struct ServiceRegisterDetails
        {
            public bool IsGlobal { get; set; }
            public Type[] RegisteringTypes { get; set; }
            public string Tag { get; set; }
        }
        

        #endregion
        public struct BindDetails
        {
            public BinderBase Bind { get; set; }
            public object Binding { get; set; }
            public Duration Duration { get; set; }
        }
        
        public enum Duration
        {
            LifeTime,Active
        }
    }

    // public partial class MonoBehaviour
    // {
    //     #region Value
    //     public IValue<T> CreateValue<T>(string key,T def,bool register = true, bool global=false)
    //     {
    //         var value = new Value<T>(key,def,register,global?null:gameObject.scene.name);
    //         DisposeAtDestroy(value);
    //         return value;
    //     }
    //
    //     public ValueReceiver<T> CreateValueReceiver<T>(string key, T def, bool local = false)
    //     {
    //         var value = new ValueReceiver<T>(key,def,local,gameObject.scene.name);
    //         DisposeAtDestroy(value);
    //         return value;
    //     }
    //     
    //     public ValueEmitter<T> CreateValueEmitter<T>(string key,T def=default,bool local = false,bool autoDispose= true) where T : IEventArgs
    //     {
    //         var e = new ValueEmitter<T>(key,def,local,gameObject.scene.name);
    //         if(autoDispose)
    //             DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     #endregion
    //     
    //     #region Event
    //
    //     public IEvent CreateEvent(string key, bool register = true, bool global = false)
    //     {
    //         var e = new Event(key, register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public IEvent<T> CreateEvent<T>(string key, bool register = true, bool global = false) where T : IEventArgs
    //     {
    //         var e = new Event<T>(key, register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public EventReceiver<T> CreateEventReceiver<T>(string key,bool local = false,bool autoDispose= true)
    //     {
    //         var e = new EventReceiver<T>(key,local,gameObject.scene.name);
    //         if(autoDispose)
    //             DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public EventReceiver CreateEventReceiver(string key,bool local = false,bool autoDispose= true)
    //     {
    //         var e = new EventReceiver(key,local,gameObject.scene.name);
    //         if(autoDispose)
    //             DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public EventEmitter<T> CreateEventEmitter<T>(string key,bool local=false,bool autoDispose= true) where T : IEventArgs
    //     {
    //         var e = new EventEmitter<T>(key,local,gameObject.scene.name);
    //         if(autoDispose) DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     
    //     public EventEmitter CreateEventEmitter(string key,bool local=false,bool autoDispose= true)
    //     {
    //         var e = new EventEmitter(key,local,gameObject.scene.name);
    //         if(autoDispose) DisposeAtDestroy(e);
    //         return e;
    //     }
    //
    //     
    //
    //     #endregion
    //
    //     #region Query
    //
    //     public IQueryItem CreateQuery(string key, Func<object,object> runner,bool register = true, bool global = false)
    //     {
    //         var e = new Query(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public IQueryItem<T> CreateQuery<T>(string key, Func<T> runner,bool register = true, bool global = false)
    //     {
    //         var e = new Query<T>(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public IQueryItem<T,TJ> CreateQuery<T,TJ>(string key, Func<T,TJ> runner,bool register = true, bool global = false)
    //     {
    //         var e = new Query<T,TJ>(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryReceiver CreateQueryReceiver(string key, Func<object,object> runner,bool register = true, bool global = false)
    //     {
    //         var e = new QueryReceiver(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryReceiver<T> CreateQueryReceiver<T>(string key, Func<T> runner,bool register = true, bool global = false)
    //     {
    //         var e = new QueryReceiver<T>(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryReceiver<T,TJ> CreateQueryReceiver<T,TJ>(string key, Func<T,TJ> runner,bool register = true, bool global = false)
    //     {
    //         var e = new QueryReceiver<T,TJ>(key, runner,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryEmitter CreateQueryEmitter(string key,bool register = true, bool global = false)
    //     {
    //         var e = new QueryEmitter(key,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryEmitter<T> CreateQueryEmitter<T>(string key,bool register = true, bool global = false)
    //     {
    //         var e = new QueryEmitter<T>(key,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public QueryEmitter<T,TJ> CreateQueryEmitter<T,TJ>(string key,bool register = true, bool global = false)
    //     {
    //         var e = new QueryEmitter<T,TJ>(key,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //
    //     #endregion
    //
    //     #region Command
    //
    //     public ICommandItem CreateCommand(string key,Action action ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandItem(key, action,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public ICommandItem<T> CreateCommand<T>(string key,Action<T> action ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandItem<T>(key, action,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public CommandReceiver CreateCommandReceiver(string key,Action action ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandReceiver(key, action,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public CommandReceiver<T> CreateCommandReceiver<T>(string key,Action<T> action ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandReceiver<T>(key, action,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public CommandEmitter CreateCommandEmitter(string key ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandEmitter(key,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //     
    //     public CommandEmitter<T> CreateCommandEmitter<T>(string key ,bool register = true, bool global = false)
    //     {
    //         var e = new CommandEmitter<T>(key,register, global ? null : gameObject.scene.name);
    //         DisposeAtDestroy(e);
    //         return e;
    //     }
    //
    //     
    //
    //     #endregion
    //
    // }

    public partial class MonoBehaviour
    {
        public static IEnumerator WaitUntil(Func<bool> until, Action action = null)
        {
            yield return EssentialCoroutine.WaitUntil(until, action);
        }
        
        public static IEnumerator WithCallback(IEnumerator enumerator, Action action = null)
        {
            yield return enumerator;
            action?.Invoke();
        }
        
        public static IEnumerator Delay(float seconds, Action action = null)
        {
            yield return EssentialCoroutine.Delay(seconds, action);
        }
        
        public static IEnumerator MoveTowardsEnumerator(float start = 0f, float end = 1f, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time=1f)
        {
            yield return EssentialCoroutine.MoveTowardsEnumerator(start, end, onCallOnFrame, onFinished, time);
        }
        
        public IEnumerator MoveTowardsEnumerator(AnimationCurve curve, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time = 1)
        {
            yield return EssentialCoroutine.MoveTowardsEnumerator(curve, onCallOnFrame, onFinished, time);
        }
    }
   
     public partial class MonoBehaviour : ISerializationCallbackReceiver
        {
           
           [HideInInspector]
           [SerializeField] protected List<FieldVsObject> fieldVsObjects = new();
    
    
            public void OnBeforeSerialize()
            {
                fieldVsObjects.Clear();
                var fields = SerializeInterfaceField.GetAllSerializedInterfaceFields(GetType()).ToList();
         
    
                foreach (var field in fields.Where(field =>
                             field.GetValue(this)?.GetType().IsSubclassOf(typeof(UnityEngine.Object)) ?? false))
                {
                    fieldVsObjects.Add(new FieldVsObject
                    {
                        name = field.Name,
                        obj = (UnityEngine.Object)field.GetValue(this)
                    });
                }
            }
    
            public void OnAfterDeserialize()
            {
                if (fieldVsObjects.Count == 0)
                {
                    return;
                }
    
                var fields = SerializeInterfaceField.GetAllSerializedInterfaceFields(GetType()).ToList();
    
                foreach (var fieldVsObject in fieldVsObjects)
                {
                    var field = fields.FirstOrDefault(f => f.Name == fieldVsObject.name);
                    if (field != null)
                    {
                        field.SetValue(this, fieldVsObject.obj);
                    }
                }
            }
        
    
            [Serializable]
            public struct FieldVsObject
            {
                public string name;
                public UnityEngine.Object obj;
            }
        }
}