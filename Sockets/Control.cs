﻿using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using ZMQServer.Messages;
using static ZMQServer.Server;

namespace ZMQServer.Sockets
{
    public static class Control
    {

        public static RouterSocket controlSocket;
        public static string controlAddress;
        public static Thread controlSocketLoop = null;
        public static event MessageHandler ControlMessageReceived;

        public static void Init(connection currentConnection)
        {
            controlSocket = new RouterSocket();
            controlAddress = $"{currentConnection.transport}://{currentConnection.ip}:{currentConnection.control_port}";
            controlSocket.Bind(controlAddress);
            ControlMessageReceived += ControlMessageProcessing;
        }

        public static void StartLoop()
        {
            controlSocketLoop = new Thread(ControlLoop);
            controlSocketLoop.Start();
            Logger.Log("control socket started", Logger.controlFilename);
        }

        private static void ControlLoop()
        {
            List<byte[]> identeties = new List<byte[]>();
            List<byte[]> messageBytes = new List<byte[]>();
            while (true)
            {
                var text = "";
                while (text != "<IDS|MSG>")
                {
                    var rec = controlSocket.ReceiveFrameBytes();
                    text = Encoding.UTF8.GetString(rec);
                    if (text == "<IDS|MSG>")
                        break;
                    identeties.Add(rec);
                }

                //signature
                messageBytes.Add(controlSocket.ReceiveFrameBytes());
                //header
                messageBytes.Add(controlSocket.ReceiveFrameBytes());
                //parent header
                messageBytes.Add(controlSocket.ReceiveFrameBytes());
                //metadata
                messageBytes.Add(controlSocket.ReceiveFrameBytes());
                //content
                messageBytes.Add(controlSocket.ReceiveFrameBytes());

                ControlMessageReceived?.Invoke(identeties, messageBytes);

                identeties.Clear();
                messageBytes.Clear();
            }
        }

        private static void ControlMessageProcessing(List<byte[]> identeties, List<byte[]> messageBytes)
        {
            var message = Encode(messageBytes);
            var parentHeader = JsonConvert.DeserializeObject<Header>(message[1]);

            if (parentHeader.msg_type == "interrupt_request")
            {
                var metadata = Dict();
                var content = Dict("status", "ok");
                var ourHeader = Dict("msg_id", Guid.NewGuid(),
                                     "session", global_session,
                                     "username", "username",
                                     "date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
                                     "msg_type", "interrupt_reply",
                                     "version", "5.3");

                foreach (var item in identeties)
                {
                    controlSocket.SendMoreFrame(item);
                }
                controlSocket.SendMoreFrame("<IDS|MSG>");
                controlSocket.SendMoreFrame(CreateSign(currentConnection.key,
                                                     new List<string>() {
                                                                JsonConvert.SerializeObject(ourHeader),       
                                                                JsonConvert.SerializeObject(parentHeader.ToDict()),
                                                                JsonConvert.SerializeObject(metadata),
                                                                JsonConvert.SerializeObject(content),
                                                     }));
                controlSocket.SendMoreFrame(JsonConvert.SerializeObject(ourHeader));
                controlSocket.SendMoreFrame(JsonConvert.SerializeObject(parentHeader.ToDict()));
                controlSocket.SendMoreFrame(JsonConvert.SerializeObject(metadata));
                controlSocket.SendFrame(JsonConvert.SerializeObject(content));
            }
            //if (proc!=null)
            //    proc.Kill();
            Compiler.InputToCompiler("[BREAK]");
            Shell.firstLine = true;
            Iopub.SendStatus("idle", parentHeader, identeties);
            Iopub.ClearOutput();
        }
    }
}
