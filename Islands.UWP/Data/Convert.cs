using Islands.UWP.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Islands.UWP.Data
{
    public static class Convert
    {
        public static T JsonDeserialize<T>(string json)
        {
            T model = default(T);
            StringReader sr = new StringReader(json);
            JsonSerializer serializer = new JsonSerializer();
            model = (T)serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            return model;
        }

        public static bool JsonTryDeserializeObject(string json, out JObject JObj)
        {
            JObj = null;
            try
            {
                JObj = (JObject)JsonConvert.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool JsonTryDeserialize<T>(string json, out T Object)
        {
            Object = default(T);
            try
            {
                Object = (T)JsonConvert.DeserializeObject(json);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static ReplyModel RefStringToReplyModel(string res, IslandsCode islandCode)
        {
            ReplyModel rm = null;
            JObject jObj;
            if (!JsonTryDeserializeObject(res, out jObj)) throw new Exception(res.UnicodeDencode());
            switch (islandCode)
            {
                case IslandsCode.A:
                case IslandsCode.Beitai:
                    rm = JsonDeserialize<ReplyModel>(res); break;
                case IslandsCode.Koukuko:
                    KReplyQueryResponse kResModel = JsonDeserialize<KReplyQueryResponse>(res);
                    if (kResModel != null)
                    {
                        if (!kResModel.success) throw new Exception(kResModel.message);
                        rm = kResModel.data;
                    }
                    break;
            }
            if (rm != null) rm.islandCode = islandCode;
            return rm;
        }

        public static void ResStringToThreadAndReplyList(string res, IslandsCode islandCode, out ThreadModel top, out List<ReplyModel> replys)
        {
            replys = null;
            top = null;
            JObject jObj;
            if (!JsonTryDeserializeObject(res, out jObj)) throw new Exception(res.UnicodeDencode());
            switch (islandCode)
            {
                case IslandsCode.A:
                case IslandsCode.Beitai:
                    top = JsonDeserialize<ThreadModel>(res);
                    var abResModel = JsonDeserialize<ABReplyQueryResponse>(res);
                    if (abResModel != null) replys = abResModel.replys;
                    break;
                case IslandsCode.Koukuko:
                    var kResModel = JsonDeserialize<KReplyQueryResponse>(res);
                    if (kResModel != null)
                    {
                        if (!kResModel.success) throw new Exception(kResModel.message);
                        top = kResModel.threads;
                        replys = kResModel.replys;
                    }
                    break;
            }
        }

        public static void ResStringToThreadList(string res, IslandsCode islandCode, out List<ThreadResponseModel> threads)
        {
            threads = null;
            switch (islandCode)
            {
                case IslandsCode.A:
                case IslandsCode.Beitai:
                    JArray ja = null;
                    if (!JsonTryDeserialize(res, out ja)) throw new Exception(res.UnicodeDencode());
                    if (ja != null)
                    {
                        threads = ja.ToObject<List<ThreadResponseModel>>();
                    }
                    break;
                case IslandsCode.Koukuko:
                    var kRes = JsonDeserialize<KThreadQueryResponse>(res);
                    if (kRes != null && kRes.data != null)
                    {
                        threads = kRes.data.threads;
                    }
                    break;
            }
        }
    }
}
