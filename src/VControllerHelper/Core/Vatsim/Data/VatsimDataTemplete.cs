#pragma warning disable CS0649

using System.Collections.Generic;

namespace VControllerHelper.Core.Vatsim.Data
{
    internal class ContorllerAndATISDataTemplete
    {
        #region Public Fields

        public int cid, facility, rating, visual_range;
        public string name, callsign, frequency, server, last_updated, logon_time, atis_code;
        public string[] text_atis;

        #endregion Public Fields
    }

    internal class FacilityAndRatingDataTemplete
    {
        #region Public Fields

        public string @short, @long;
        public int id;

        #endregion Public Fields
    }

    internal class FlightPlanDataTemplete
    {
        #region Public Fields

        public string flight_rules, aircraft, aircraft_faa, aircraft_short, departure, arrival, alternate, cruise_tas, altitude, deptime, enroute_time, fuel_time, remarks, route, assigned_transponder;
        public int revision_id;

        #endregion Public Fields
    }

    internal class GeneralDataTemplete
    {
        #region Public Fields

        public string update, update_timestamp;
        public int version, reload, connected_clients, unique_users;

        #endregion Public Fields
    }

    internal class PilotRatingDataTemplete
    {
        #region Public Fields

        public int id;
        public string short_name, long_name;

        #endregion Public Fields
    }

    internal class PilotsDataTemplete
    {
        #region Public Fields

        public int cid, pilot_rating, altitude, groundspeed, heading, qnh_mb;
        public FlightPlanDataTemplete flight_plan;
        public double latitude, longitude, qnh_i_hg;
        public string name, callsign, server, transponder, logon_time, last_updated;

        #endregion Public Fields
    }

    internal class ServerDataTemplete
    {
        #region Public Fields

        public bool client_connections_allowed, is_sweatbox;
        public int clients_connection_allowed;
        public string ident, hostname_or_ip, location, name;

        #endregion Public Fields
    }

    internal class VatsimDataTemplete
    {
        #region Public Fields

        public List<ContorllerAndATISDataTemplete> atis;
        public List<ContorllerAndATISDataTemplete> controllers;
        public List<FacilityAndRatingDataTemplete> facilities;
        public GeneralDataTemplete general;
        public List<PilotRatingDataTemplete> pilot_ratings;
        public List<PilotsDataTemplete> pilots;
        public List<PilotsDataTemplete> prefiles;
        public List<FacilityAndRatingDataTemplete> ratings;
        public List<ServerDataTemplete> servers;

        #endregion Public Fields
    }
}