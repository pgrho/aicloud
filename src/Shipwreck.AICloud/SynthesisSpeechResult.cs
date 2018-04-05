using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shipwreck.AICloud
{
    public sealed class SynthesisSpeechResult : IDisposable
    {
        private readonly HttpResponseMessage _Response;

        internal SynthesisSpeechResult(HttpResponseMessage response)
        {
            _Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public Task<Stream> ReadAsStreamAsync()
            => _Response.Content.ReadAsStreamAsync();

        public Task<string> ReadAsStringAsync()
            => _Response.Content.ReadAsStringAsync();

        public Task<byte[]> ReadAsByteArrayAsync()
            => _Response.Content.ReadAsByteArrayAsync();

        public async Task SaveAsync(string fileName)
        {
            using (var s = await ReadAsStreamAsync().ConfigureAwait(false))
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                await s.CopyToAsync(fs).ConfigureAwait(false);
            }
        }

        public void Dispose()
            => _Response.Dispose();
    }
}