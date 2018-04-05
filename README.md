# Shipwreck.AICloud

Simple client for AITalk WebAPI v2

## Installation

[NuGet](https://www.nuget.org/packages/Shipwreck.AICloud)

## Usage

```
using (var client = AICloudClient())
{
    client.UserName = userName;
    client.Password = password;

    var result = await client.SynthesisSpeechAsync(new SpeechSynthesisParameter()
    {
        SpeakerName = KnownSpeakers.Nozomi,
        Extension = Extension.Mp3,
        Text = "こんにちわ、世界"
    });

    await result.SaveAsync("output.mp3");
}
```