﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tsi.Blazor.Component.Core.TsiComponents.Extensions
{
    public class ValueDebouncer : IDisposable
    {
        #region Members

        /// <summary>
        /// Internal timer used to debounce the value.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Holds the last updated value.
        /// </summary>
        private string value;

        /// <summary>
        /// Event raised after the interval has passed and with new updated value.
        /// </summary>
        public event EventHandler<string> Debounce;

        #endregion

        #region Constructors

        /// <summary>
        /// Default debouncer constructor.
        /// </summary>
        /// <param name="interval">Interval by which the value will be debounced.</param>
        public ValueDebouncer(int interval)
        {
            timer = new(interval);
            timer.Elapsed += OnElapsed;
            timer.AutoReset = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// Invokes the <see cref="Debounce"/> event.
        /// </summary>
        /// <param name="source">Reference to the object tha raised the event.</param>
        /// <param name="eventArgs">Timer event arguments.</param>
        private void OnElapsed(object source, ElapsedEventArgs eventArgs)
        {
            Debounce?.Invoke(this, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the internal value.
        /// </summary>
        /// <param name="value">New value.</param>
        public void Update(string value)
        {
            timer.Stop();

            this.value = value;

            timer.Start();
        }

        /// <summary>
        /// Stops the debouncer and raises the <see cref="Debounce"/> event if <see cref="Running"/> is enabled.
        /// </summary>
        public void Flush()
        {
            if (Running)
            {
                timer.Stop();

                Debounce?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Releases all subscribed events.
        /// </summary>
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= OnElapsed;
                timer.Dispose();
                timer = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if debouncer timer is running.
        /// </summary>
        public bool Running => timer.Enabled;

        #endregion
    }
}
