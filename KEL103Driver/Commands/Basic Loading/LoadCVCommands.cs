﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KEL103Driver
{
    public static partial class KEL103Command
    {
        public static async Task SetConstantVoltageTarget(IPAddress device_address, double target_voltage)
        {
            using (UdpClient client = new UdpClient(KEL103Persistance.Configuration.CommandPort))
            {
                KEL103Tools.ConfigureClient(device_address, client);

                await SetConstantVoltageTarget(client, target_voltage);
            }
        }

        public static async Task SetConstantVoltageTarget(UdpClient client, double target_voltage)
        {
            var tx_bytes = Encoding.ASCII.GetBytes(":VOLT " + KEL103Tools.FormatString(target_voltage) + "V\n");

            await client.SendAsync(tx_bytes, tx_bytes.Length);
        }

        public static async Task<double> GetConstantVoltageTarget(IPAddress device_address)
        {
            using (UdpClient client = new UdpClient(KEL103Persistance.Configuration.CommandPort))
            {
                KEL103Tools.ConfigureClient(device_address, client);

                return await GetConstantVoltageTarget(client);
            }
        }

        public static async Task<double> GetConstantVoltageTarget(UdpClient client)
        {
            var tx_bytes = Encoding.ASCII.GetBytes(":VOLT?\n");

            await client.SendAsync(tx_bytes, tx_bytes.Length);

            var rx = (await client.ReceiveAsync()).Buffer;

            return Convert.ToDouble(Encoding.ASCII.GetString(rx).Split('V')[0]);
        }
    }
}
