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

namespace ZMQServer.Sockets
{
    public static class Iopub
    {

        public static PublisherSocket iopubSocket;
        public static string iopubAddress;
        public static Thread iopubSocketLoop = null;

        public static void Init(connection currentConnection)
        {
            iopubSocket = new PublisherSocket();
            iopubAddress = $"{currentConnection.transport}://{currentConnection.ip}:{currentConnection.iopub_port}";
            iopubSocket.Bind(iopubAddress);
        }

        public static void SendDisplayData(string data, Header parentHeader, List<byte[]> identeties, bool isUpdate = false, string displayId = null)
        {
            var ourHeader = Server.Dict("msg_id", Guid.NewGuid(),
                                         "session", Server.global_session,
                                         "username", "username",
                                         "date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
                                         "msg_type", isUpdate ? "update_display_data" : "display_data",
                                         "version", "5.3");
            var metadata = Server.Dict();
            var content = Server.Dict("data", Server.Dict("text/html", data),
                                        "metadata", Server.Dict(),
                                        "transient", Server.Dict("display_id", displayId ?? ""));

            foreach (var item in identeties)
            {
                iopubSocket.SendMoreFrame(item);
            }

            Logger.Log(JsonConvert.SerializeObject(ourHeader), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(parentHeader.ToDict()), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(metadata), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(content), "iopub.txt");

            iopubSocket.SendMoreFrame("<IDS|MSG>");
            iopubSocket.SendMoreFrame(Server.CreateSign(Server.currentConnection.key,
                                                 new List<string>() {
                                                         JsonConvert.SerializeObject(ourHeader),
                                                         JsonConvert.SerializeObject(parentHeader.ToDict()),
                                                         JsonConvert.SerializeObject(metadata),
                                                         JsonConvert.SerializeObject(content)
                                                 }));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(ourHeader));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(parentHeader.ToDict()));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(metadata));
            iopubSocket.SendFrame(JsonConvert.SerializeObject(content));
        }

        public static void SendExecutionData(string data, Header parentHeader, List<byte[]> identeties)
        {
            var ourHeader = Server.Dict("msg_id", Guid.NewGuid(),
                                         "session", Server.global_session,
                                         "username", "username",
                                         "date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
                                         "msg_type", "execute_result",
                                         "version", "5.3");
            var metadata = Server.Dict();
            var content = Server.Dict("execution_count", Server.executionCounter,
                                "data", Server.Dict("text/html", data),
                                "metadata", Server.Dict());

            foreach (var item in identeties)
            {
                iopubSocket.SendMoreFrame(item);
            }

            Logger.Log(JsonConvert.SerializeObject(ourHeader), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(parentHeader.ToDict()), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(metadata), "iopub.txt");
            Logger.Log(JsonConvert.SerializeObject(content), "iopub.txt");

            iopubSocket.SendMoreFrame("<IDS|MSG>");
            iopubSocket.SendMoreFrame(Server.CreateSign(Server.currentConnection.key,
                                                 new List<string>() {
                                                         JsonConvert.SerializeObject(ourHeader),
                                                         JsonConvert.SerializeObject(parentHeader.ToDict()),
                                                         JsonConvert.SerializeObject(metadata),
                                                         JsonConvert.SerializeObject(content)
                                                 }));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(ourHeader));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(parentHeader.ToDict()));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(metadata));
            iopubSocket.SendFrame(JsonConvert.SerializeObject(content));
        }

        public static void SendStatus(string status, Header parentHeader, List<byte[]> identeties)
        {
            var ourHeader = Server.Dict("msg_id", Guid.NewGuid(),
                                         "session", Server.global_session,
                                         "username", "username",
                                         "date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
                                         "msg_type", "status",
                                         "version", "5.3");
            var metadata = Server.Dict();
            var content = Server.Dict("execution_state", status);

            foreach (var item in identeties)
            {
                iopubSocket.SendMoreFrame(item);
            }

            iopubSocket.SendMoreFrame("<IDS|MSG>");
            iopubSocket.SendMoreFrame(Server.CreateSign(Server.currentConnection.key,
                                                 new List<string>() {
                                                         JsonConvert.SerializeObject(ourHeader),
                                                         JsonConvert.SerializeObject(parentHeader.ToDict()),
                                                         JsonConvert.SerializeObject(metadata),
                                                         JsonConvert.SerializeObject(content)
                                                 }));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(ourHeader));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(parentHeader.ToDict()));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(metadata));
            iopubSocket.SendFrame(JsonConvert.SerializeObject(content));

        }

        public static void ClearOutput()
        {
            var identeties = Shell.currentIdenteties;
            var parentHeader = Shell.currentHeader;

            if (identeties == null)
                return;

            var ourHeader = Server.Dict("msg_id", Guid.NewGuid(),
                                         "session", Server.global_session,
                                         "username", "username",
                                         "date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
                                         "msg_type", "clear_output",
                                         "version", "5.3");
            var metadata = Server.Dict();
            var content = Server.Dict("wait", "false");

            foreach (var item in identeties)
            {
                iopubSocket.SendMoreFrame(item);
            }

            iopubSocket.SendMoreFrame("<IDS|MSG>");
            iopubSocket.SendMoreFrame(Server.CreateSign(Server.currentConnection.key,
                                                 new List<string>() {
                                                         JsonConvert.SerializeObject(ourHeader),
                                                         JsonConvert.SerializeObject(parentHeader.ToDict()),
                                                         JsonConvert.SerializeObject(metadata),
                                                         JsonConvert.SerializeObject(content)
                                                 }));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(ourHeader));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(parentHeader.ToDict()));
            iopubSocket.SendMoreFrame(JsonConvert.SerializeObject(metadata));
            iopubSocket.SendFrame(JsonConvert.SerializeObject(content));

            var script = "<script>" +
                "var classElements = document.getElementsByClassName('jp-Cell-outputWrapper');" +
                "for (var i = 0; i < classElements.length; i++)" +
                "{" +
                "var preElements = classElements[i].getElementsByTagName('pre');" +
                "for (var j = 0; j < preElements.length; j++)" +
                "{" +
                "   preElements[j].remove();" +
                "}" +
                "}" +
                "</script>";

            //SendDisplayData("", parentHeader, identeties, true, Shell.currentId);
            SendDisplayData(script, parentHeader, identeties, true, Shell.currentId);
            //SendExecutionData("", parentHeader, identeties);
            //Server.executionCounter--;
        }

    }
}