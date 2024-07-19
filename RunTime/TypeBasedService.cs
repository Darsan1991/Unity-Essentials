using System;
using System.Collections.Generic;
using System.Linq;
using DGames.Essentials.Extensions;
using UnityEngine;

namespace DGames.Essentials
{
    public class TypeBasedService : Provider<TypeAndTag,object>,ITypeBasedProvider
    {
        private readonly Dictionary<TypeAndTag, object> _typeVsObjects = new();
        private readonly Dictionary<Type, RegisterSettings> _typeVsSettings = new();

        
        public TypeBasedService(string tag) : base(tag)
        {
        }


        protected override void ProcessRegister(TypeAndTag typeAndTag, object instance)
        {
            var tag = typeAndTag.Tag;
            var type = typeAndTag.Type;
            
            if (instance == null)
                throw new InvalidOperationException();

            ConfirmTagConditionSatisfy(type,tag);

            
            if (!_typeVsSettings.ContainsKey(type))
            {
                _typeVsSettings.Add(type, new RegisterSettings());
            }

            var typeVsSetting = _typeVsSettings[type];

            typeVsSetting.Settings.Add(new RegisterSettings.RegisterSetting
            {
                IsInstance = true,
                Tag = tag
            });
            _typeVsObjects.Add(new TypeAndTag { Type = type, Tag = tag }, instance);
            Debug.Log($"Register Type:{type}");
        }


        private void ConfirmTagConditionSatisfy(Type type,string tag)
        {

            if (!_typeVsSettings.ContainsKey(type))
            {
                return;
            }

            if (_typeVsSettings[type].Settings.Any(s => string.IsNullOrEmpty(s.Tag))
                || string.IsNullOrEmpty(tag)
                || _typeVsSettings[type].Settings.Any(s => s.Tag == tag))
            {
                throw new Exception();
            }
        }

        public void Register( TypeAndTag typeAndTag,RegisterOptions options)
        {
            var tag = typeAndTag.Tag;
            var type = typeAndTag.Type;
            ConfirmTagConditionSatisfy(type,tag);

            if (!_typeVsSettings.ContainsKey(type))
            {
                _typeVsSettings.Add(type, new RegisterSettings());
            }

            var typeVsSetting = _typeVsSettings[type];
            if (typeVsSetting.Settings.Any(s => s.Tag == tag))
            {
                throw new InvalidOperationException();
            }

            typeVsSetting.Settings.Add(new RegisterSettings.RegisterSetting
            {
                IsInstance = false,
                Options = options,
                Tag = tag
            });

            RegisteredEvent(typeAndTag);
            // Debug.Log($"Register Type:{type} , tag:{tag}");
        }

        protected override void ProcessUnRegisterInThis(TypeAndTag typeAndTag)
        {
            var type = typeAndTag.Type;
            var tag = typeAndTag.Tag;
            
            var typeVsSetting = _typeVsSettings[type];
            typeVsSetting.Settings.RemoveAll(s => string.IsNullOrEmpty(tag) || s.Tag == tag);
            if (!typeVsSetting.Settings.Any())
            {
                _typeVsSettings.Remove(type);
            }

            if (string.IsNullOrEmpty(tag))
            {
                _typeVsObjects.Keys.Where(k => k.Type == type).ForEach(k => _typeVsObjects.Remove(k));
            }
            else
            {
                _typeVsObjects.Remove(typeAndTag);
            }
        }

        private object InstantiateInstanceOfType(Type type,string tag)
        {
            var settings = _typeVsSettings[type];

            var list = settings.Settings.Where(s => !s.IsInstance && (string.IsNullOrEmpty(tag) || s.Tag == tag))
                .ToList();

            if (!list.Any())
                throw new Exception();

            var options = list.First().Options;
            var obj = options.Get();

            if (!options.IsNewInstanceEachTime)
                _typeVsObjects.Add(new TypeAndTag { Tag = tag, Type = type }, obj);
            return obj;
        }
        
        
        

        
        public override object Get(TypeAndTag typeAndTag, out bool success,bool allowSubs=true)
        {
            success = Has(typeAndTag,allowSubs);

            if (!success)
                return default;
            
            var type = typeAndTag.Type;
            var tag = typeAndTag.Tag;
            if (Has(typeAndTag,false))
            {
                var pairs = _typeVsObjects.Where(t => t.Key.Type == type && (string.IsNullOrEmpty(tag) || t.Key.Tag == tag))
                    .ToList();
                if (pairs.Any())
                {
                    return pairs.First().Value;
                }

                return InstantiateInstanceOfType(type,tag);
            }
            
            return allowSubs ? Subs.Where(s => s.Has(typeAndTag)).Select(s => s.Get(typeAndTag,out _)).First() : default;
        }

        public IEnumerable<object> GetAll(Type type,bool allowSubService = true)
        {
            if (Has(new TypeAndTag{Type = type},allowSubService:false))
            {
                foreach (var setting in _typeVsSettings[type].Settings)
                {
                    yield return Get(new TypeAndTag{Type = type,Tag = setting.Tag},out _,allowSubs: false);
                }
            }
            
            if(!allowSubService)
                yield break;

            foreach (var item in Subs.Cast<ITypeBasedProvider>().Where(s=>s.Has(new TypeAndTag{Type = type})).Select(s=>s.GetAll(type)).SelectMany(s=>s))
            {
                yield return item;
            }
        }

        public override bool Has(TypeAndTag typeAndTag,bool allowSubService = true)
        {
            var tag = typeAndTag.Tag;
            var type = typeAndTag.Type;
            return (string.IsNullOrEmpty(tag)
                       ? _typeVsSettings.ContainsKey(type)
                       : _typeVsSettings.ContainsKey(type) && _typeVsSettings[type].Settings.Any(s => s.Tag == tag))
                   || (allowSubService && Subs.Any(s => s.Has(typeAndTag)));
        }
    }
}