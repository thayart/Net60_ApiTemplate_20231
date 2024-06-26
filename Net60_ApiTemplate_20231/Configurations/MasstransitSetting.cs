﻿namespace Net60_ApiTemplate_20231.Configurations
{
    public class MasstransitSetting
    {
        public bool EnableRabbitMQ { get; set; }
        public RabbitMQSetting RabbitMQSetting { get; set; }
        public bool EnableKafka { get; set; }
        public KafkaSetting KafkaSetting { get; set; }
    }
}