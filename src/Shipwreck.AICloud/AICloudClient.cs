using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Shipwreck.AICloud
{
    public class AICloudClient : IDisposable
    {
        #region URL

        public static string HttpBaseUrl => "http://webapi.aitalk.jp/webapi/v2/";

        public static string HttpsBaseUrl => "https://webapi.aitalk.jp/webapi/v2/";

        public static string DefaultBaseUrl { get; set; } = "https://webapi.aitalk.jp/webapi/v2/";

        private string _BaseUrl = DefaultBaseUrl;

        public string BaseUrl
        {
            get => _BaseUrl;
            set
            {
                if (value != _BaseUrl)
                {
                    _BaseUrl = value;
                    _TtsgetUrl = null;
                }
            }
        }

        private Uri _TtsgetUrl;

        private Uri TtsgetUrl
            => _TtsgetUrl ?? (_TtsgetUrl = new Uri(_BaseUrl + "ttsget.php"));

        private HttpClient _HttpClient;

        public HttpClient HttpClient
        {
            get
            {
                if (_HttpClient == null)
                {
                    _HttpClient = new HttpClient();
                }
                return _HttpClient;
            }
            set => _HttpClient = value;
        }

        #endregion URL

        #region Identity

        public static string DefaultUserName { get; set; }
        public static string DefaultPassword { get; set; }

        public string UserName { get; set; } = DefaultUserName;
        public string Password { get; set; } = DefaultPassword;

        #endregion Identity

        public async Task<SynthesisSpeechResult> SynthesisSpeechAsync(SynthesisSpeechParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var ps = new List<KeyValuePair<string, string>>(16);

            ps.Add(new KeyValuePair<string, string>("username", UserName ?? string.Empty));
            ps.Add(new KeyValuePair<string, string>("password", Password ?? string.Empty));
            ps.Add(new KeyValuePair<string, string>("text", parameter.Text ?? string.Empty));
            ps.Add(new KeyValuePair<string, string>("speaker_name", parameter.SpeakerName ?? string.Empty));
            AppendInputType(ps, parameter);
            AppendFloat(ps, "volume", parameter.Volume, min: 0.01f, max: 2);
            AppendFloat(ps, "speed", parameter.Speed, min: 0.5f, max: 4);
            AppendFloat(ps, "pitch", parameter.Pitch, min: 0.5f, max: 2);
            AppendFloat(ps, "range", parameter.Range, min: 0.5f, max: 2);
            if (parameter.UseDictionary)
            {
                ps.Add(new KeyValuePair<string, string>("use_wdic", "1"));
            }
            AppendOutputType(ps, parameter);
            AppendExtension(ps, parameter);
            AppendWavFormat(ps, parameter);
            AppendStyle(ps, parameter);

            var req = new HttpRequestMessage(HttpMethod.Post, TtsgetUrl);
            req.Content = new FormUrlEncodedContent(ps);

            try
            {
                var res = await HttpClient.SendAsync(req).ConfigureAwait(false);

                if (res.IsSuccessStatusCode)
                {
                    return new SynthesisSpeechResult(res);
                }
                else if (res.StatusCode == HttpStatusCode.InternalServerError
                        && res.Content?.Headers?.ContentType?.MediaType == "application/xml")
                {
                    var xml = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var d = new XmlDocument();
                    d.LoadXml(xml);

                    var ces = d.DocumentElement?.ChildNodes.Cast<XmlElement>();

                    var code = ces?.FirstOrDefault(e => "code".Equals(e.LocalName, StringComparison.InvariantCulture))?.InnerText;
                    if (int.TryParse(code, out var ic))
                    {
                        var message = ces?.FirstOrDefault(e => "message".Equals(e.LocalName, StringComparison.InvariantCulture))?.InnerText;
                        var detail = ces?.FirstOrDefault(e => "detail".Equals(e.LocalName, StringComparison.InvariantCulture))?.InnerText;

                        throw new AICloudException((ErrorCode)ic, message, detail);
                    }
                }
                res.EnsureSuccessStatusCode();
                throw new AICloudException();
            }
            catch (Exception ex) when (!(ex is AICloudException))
            {
                throw new AICloudException(null, ex);
            }
        }

        private static void AppendInputType(List<KeyValuePair<string, string>> ps, SynthesisSpeechParameter parameter)
        {
            var it = parameter.InputType;
            if (it != InputType.Default)
            {
                ps.Add(
                    new KeyValuePair<string, string>(
                        "input_type",
                        it == InputType.Ssml ? "ssml"
                        : it == InputType.Text ? "text"
                        : it.ToString("G").ToLowerInvariant()));
            }
        }

        private static void AppendFloat(List<KeyValuePair<string, string>> ps, string name, float p, float min = 0, float max = 2)
        {
            if (p != 1)
            {
                ps.Add(new KeyValuePair<string, string>("pitch", Math.Max(min, Math.Min(p, max)).ToString("0.00")));
            }
        }

        private static void AppendOutputType(List<KeyValuePair<string, string>> ps, SynthesisSpeechParameter parameter)
        {
            var ot = parameter.OutputType;
            if (ot != OutputType.Default)
            {
                ps.Add(
                    new KeyValuePair<string, string>(
                        "output_type",
                        ot == OutputType.Sound ? "sound"
                        : ot == OutputType.Kana ? "kana"
                        : ot == OutputType.JeitaTT6004 ? "jeita"
                        : ot.ToString("G").ToLowerInvariant()));
            }
        }

        private static void AppendExtension(List<KeyValuePair<string, string>> ps, SynthesisSpeechParameter parameter)
        {
            var ext = parameter.Extension;
            if (ext != Extension.Default)
            {
                ps.Add(
                    new KeyValuePair<string, string>(
                        "ext",
                        ext == Extension.Ogg ? "ogg"
                        : ext == Extension.Aac ? "aac"
                        : ext == Extension.Mp3 ? "mp3"
                        : ext == Extension.Wav ? "wav"
                        : ext == Extension.Wav8 ? "wav8"
                        : ext == Extension.Wav8k_8b ? "wav8k-8b"
                        : ext == Extension.Wav11 ? "wav11"
                        : ext == Extension.Wav11k_8b ? "wav11k-8b"
                        : ext == Extension.Wav16 ? "wav16"
                        : ext == Extension.Wav22 ? "wav22"
                        : ext == Extension.Wav44 ? "wav44"
                        : ext == Extension.Alaw ? "alaw"
                        : ext == Extension.Ulaw ? "ulaw"
                        : ext.ToString("G").ToLowerInvariant()));
            }
        }

        private static void AppendWavFormat(List<KeyValuePair<string, string>> ps, SynthesisSpeechParameter parameter)
        {
            var r = (int)parameter.WavSamplingRate;
            var b = (int)parameter.WavBitDepth;
            var c = (int)parameter.WavChannels;

            if (r > 0 || b > 0 || c > 0)
            {
                var sb = new StringBuilder(48);
                sb.Append('{');
                if (r > 0)
                {
                    sb.Append("\"rate\":").Append('"').Append(r).Append('"');
                }
                if (b > 0)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    sb.Append("\"bit\":").Append('"').Append(b).Append('"');
                }
                if (c > 0)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    sb.Append("\"channels\":").Append('"').Append(c).Append('"');
                }
                sb.Append('}');
                ps.Add(new KeyValuePair<string, string>("wav_format", sb.ToString()));
            }
        }

        private static void AppendStyle(List<KeyValuePair<string, string>> ps, SynthesisSpeechParameter parameter)
        {
            var j = parameter.Joy;
            var s = parameter.Sadness;
            var a = parameter.Anger;

            if (j != 0 || s != 0 || a != 0)
            {
                var sb = new StringBuilder(32);
                sb.Append('{');
                if (j > 0)
                {
                    sb.Append("\"j\":").Append('"').Append(j.ToString("0.0")).Append('"');
                }
                if (s > 0)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    sb.Append("\"s\":").Append('"').Append(s.ToString("0.0")).Append('"');
                }
                if (a > 0)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    sb.Append("\"a\":").Append('"').Append(a.ToString("0.0")).Append('"');
                }
                sb.Append('}');
                ps.Add(new KeyValuePair<string, string>("style", sb.ToString()));
            }
        }

        public void Dispose()
        {
            _HttpClient?.Dispose();
            _HttpClient = null;
        }
    }
}