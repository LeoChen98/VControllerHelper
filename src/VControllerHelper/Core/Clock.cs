using System.Threading;

namespace VControllerHelper.Core
{
    internal class Clock
    {
        #region Private Fields

        private static Clock _Instance;
        private Timer _Timer;
        private int TickCount = 0;

        #endregion Private Fields

        #region Private Constructors

        private Clock()
        {
            _Timer = new Timer(Timer_Tick, null, 0, 1000);
        }

        #endregion Private Constructors

        #region Public Delegates

        /// <summary>
        /// 计时器执行事件委托
        /// </summary>
        public delegate void TimerTickedHandler();

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// 每10分钟触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPer10Minutes;

        /// <summary>
        /// 每5分钟触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPer5Minutes;

        /// <summary>
        /// 每半小时触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPerHalfHour;

        /// <summary>
        /// 每小时触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPerHour;

        /// <summary>
        /// 每分钟触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPerMinute;

        /// <summary>
        /// 每秒触发一次的计时器执行事件
        /// </summary>
        public event TimerTickedHandler TimerTickedPerSecond;

        #endregion Public Events

        #region Public Properties

        public static Clock Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Clock();
                }

                return _Instance;
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void Timer_Tick(object state)
        {
            if (TickCount == 3599)
            {
                TickCount = 0;
                TimerTickedPerHour?.Invoke();
            }

            TimerTickedPerSecond?.Invoke();

            if (TickCount % 60 == 0)
                TimerTickedPerMinute?.Invoke();

            if (TickCount % 300 == 0)
                TimerTickedPer5Minutes?.Invoke();

            if (TickCount % 600 == 0)
                TimerTickedPer10Minutes?.Invoke();

            if (TickCount % 1800 == 0)
                TimerTickedPerHalfHour?.Invoke();
        }

        #endregion Private Methods
    }
}