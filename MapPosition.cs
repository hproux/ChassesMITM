namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MapPosition
    {
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("posX", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? PosX { get; set; }

        [JsonProperty("posY", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? PosY { get; set; }

        [JsonProperty("outdoor", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Outdoor { get; set; }

        [JsonProperty("capabilities", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Capabilities { get; set; }

        [JsonProperty("nameId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? NameId { get; set; }

        [JsonProperty("sounds", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Sound[] Sounds { get; set; }

        [JsonProperty("subAreaId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? SubAreaId { get; set; }

        [JsonProperty("worldMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? WorldMap { get; set; }

        [JsonProperty("hasPriorityOnWorldmap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPriorityOnWorldmap { get; set; }
    }

    public partial class Sound
    {
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("volume", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Volume { get; set; }

        [JsonProperty("criterionId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? CriterionId { get; set; }

        [JsonProperty("silenceMin", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? SilenceMin { get; set; }

        [JsonProperty("silenceMax", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? SilenceMax { get; set; }

        [JsonProperty("channel", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Channel { get; set; }

        [JsonProperty("type_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? TypeId { get; set; }
    }
}
