using DmCommons;
using Newtonsoft.Json;

namespace VControllerHelper.Core.Vatsim.Data
{
    internal class DataLoader
    {
        #region Private Fields

        private static DataLoader _Instance;

        #endregion Private Fields

        #region Public Properties

        public static DataLoader Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DataLoader();
                }

                return _Instance;
            }
        }

        public VatsimDataTemplete Data { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Start()
        {
            //Clock.Instance.TimerTickedPerMinute += DataUpdateTick;

            DataUpdateTick();
        }

        #endregion Public Methods

        #region Private Methods

        private void DataUpdateTick()
        {
            try
            {
                string str = Http.GetBody("https://data.vatsim.net/v3/vatsim-data.json");
                Data = JsonConvert.DeserializeObject<VatsimDataTemplete>(str);
                SubscribeCenter.RaiseVatsimOnlineDataUpdated(this, new SubscribedValueChangedArgs<VatsimDataTemplete> { key = "VatsimData", value = Data });
            }
            catch { }
        }

        #endregion Private Methods
    }
}