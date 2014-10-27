﻿namespace Fooidity.CodeSwitches
{
    using System;
    using Events;


    /// <summary>
    /// A CodeSwitch that can be changed by calling the appropriate method
    /// </summary>
    /// <typeparam name="TFeature">The code feature</typeparam>
    public class ToggleCodeSwitch<TFeature> :
        IToggleCodeSwitch<TFeature>,
        IObservable<CodeSwitchEvaluated>
        where TFeature : struct, CodeFeature
    {
        readonly Lazy<bool> _enabled;
        readonly CodeSwitchEvaluatedObservable<TFeature> _evaluated;
        readonly IToggleSwitchState<TFeature> _toggleSwitchState;

        public ToggleCodeSwitch(IToggleSwitchState<TFeature> toggleSwitchState)
        {
            _toggleSwitchState = toggleSwitchState;
            _evaluated = new CodeSwitchEvaluatedObservable<TFeature>();
            _enabled = new Lazy<bool>(Evaluate);
        }

        public IDisposable Subscribe(IObserver<CodeSwitchEvaluated> observer)
        {
            return _evaluated.Connect(observer);
        }

        public bool Enabled
        {
            get { return _enabled.Value; }
        }

        public void Enable()
        {
            _toggleSwitchState.Enabled = true;
        }

        public void Disable()
        {
            _toggleSwitchState.Enabled = false;
        }

        bool Evaluate()
        {
            bool enabled = _toggleSwitchState.Enabled;

            _evaluated.Evaluated(enabled);

            return enabled;
        }
    }
}