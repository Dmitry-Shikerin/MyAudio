// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using Doozy.Runtime.Common.Extensions;
using Doozy.Runtime.Reactor.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable MemberCanBePrivate.Global

namespace Doozy.Runtime.Reactor.Ticker
{
    /// <summary> Tick Service system that ticks every registered IUseTickService interfaces </summary>
    public class TickService
    {
        public static int minFPS => 1;
        public static int maxFPS => (int)FPS.MaxFPS;

        public float tickInterval { get; private set; }
        public int fps { get; private set; }

        private readonly List<IUseTickService> m_Targets = new List<IUseTickService>();
        private List<IUseTickService> safeTargets { get; }
        private List<Action> _action = new List<Action>();
        public UnityAction OnTick;
        
        public TickService(int fps)
        {
            SetFPS(fps);
            safeTargets = new List<IUseTickService>();
        }

        public void SetFPS(int value)
        {
            fps = Mathf.Max(minFPS, value);
            tickInterval = ReactorSettings.GetTickInterval(value);
        }

        public void SetFPS(FPS value)
        {
            SetFPS((int)value);
        }

        public int registeredTargetsCount =>
            m_Targets.Count;

        public bool hasRegisteredTargets =>
            registeredTargetsCount > 0;

        private void UpdateActions()
        {
            for (int i = _action.Count; i > 0; i--)
            {
                _action[i].Invoke();
            }
        }
        
        public void Remove(Action action) =>
            _action.Remove(action);

        public void Register(Action action) =>
            _action.Add(action);

        public void Register(IUseTickService target)
        {
            if (target == null)
                return;
            
            if (m_Targets.Contains(target))
                return;
            
            m_Targets.RemoveNulls();
            m_Targets.Add(target);
        }

        public void Unregister(IUseTickService target)
        {
            if (target == null)
                return;
            
            if (!m_Targets.Contains(target))
                return;
            
            m_Targets.RemoveNulls();
            m_Targets.Remove(target);
        }

        public void Tick()
        {
            m_Targets.RemoveNulls();
            safeTargets.Clear();
            safeTargets.AddRange(m_Targets);
            
            for (int i = 0; i < safeTargets.Count; i++)
                safeTargets[i].Tick();
            
            OnTick?.Invoke();
            UpdateActions();
        }
    }
}
