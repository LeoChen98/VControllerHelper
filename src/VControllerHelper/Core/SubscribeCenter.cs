using System.Collections.Generic;

namespace VControllerHelper.Core
{
    /// <summary>
    /// 事件订阅中心
    /// </summary>
    internal class SubscribeCenter
    {
        #region Public Delegates

        /// <summary>
        /// 订阅事件委托
        /// </summary>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">可选，事件参数</param>
        public delegate void SubscribedEventHandler(object sender, object e = null);

        /// <summary>
        /// 订阅值变化事件委托
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">事件参数</param>
        public delegate void SubscribedValueChangedHandler<T>(object sender, SubscribedValueChangedArgs<T> e);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// ATIS自动更新状态变化通知事件
        /// </summary>
        public static event SubscribedValueChangedHandler<bool> ATISAutoUpdateStatusChanged;

        /// <summary>
        /// ATIS更新通知事件
        /// </summary>
        public static event SubscribedValueChangedHandler<KeyValuePair<string, string>> ATISUpdated;

        /// <summary>
        /// Metar更新通知事件
        /// </summary>
        public static event SubscribedEventHandler MetarsUpdated;

        /// <summary>
        /// Pipe服务状态变化通知事件
        /// </summary>
        public static event SubscribedValueChangedHandler<bool> PipeServerStatusChanged;

        /// <summary>
        /// Vatsim在线数据刷新通知事件
        /// </summary>
        public static event SubscribedValueChangedHandler<Vatsim.Data.VatsimDataTemplete> VatsimOnlineDataUpdated;

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// ATIS自动更新状态变化通知事件激发函数
        /// </summary>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">事件参数</param>
        public static void RaiseATISAutoUpdateStatusChanged(object sender, SubscribedValueChangedArgs<bool> e)
        {
            ATISAutoUpdateStatusChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// ATIS更新通知事件激发函数
        /// </summary>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">事件参数</param>
        public static void RaiseATISUpdated(object sender, SubscribedValueChangedArgs<KeyValuePair<string, string>> e)
        {
            ATISUpdated?.Invoke(sender, e);
        }

        /// <summary>
        /// Metar更新通知事件激发函数
        /// </summary>
        /// <param name="sender">事件调用者</param>
        public static void RaiseMetarsUpdated(object sender)
        {
            MetarsUpdated?.Invoke(sender);
        }

        /// <summary>
        /// Pipe服务状态变化通知事件激发函数
        /// </summary>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">事件参数</param>
        public static void RaisePipeServerStatusChanged(object sender, SubscribedValueChangedArgs<bool> e)
        {
            PipeServerStatusChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Vatsim在线数据刷新通知事件激发函数
        /// </summary>
        /// <param name="sender">事件调用者</param>
        /// <param name="e">事件参数</param>
        public static void RaiseVatsimOnlineDataUpdated(object sender, SubscribedValueChangedArgs<Vatsim.Data.VatsimDataTemplete> e)
        {
            VatsimOnlineDataUpdated?.Invoke(sender, e);
        }

        #endregion Public Methods
    }
}