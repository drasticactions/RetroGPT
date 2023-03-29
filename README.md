# RetroGPT - The IE 3 Compatible ChatGPT Client.

![1444070256569233](https://user-images.githubusercontent.com/898335/167266846-1ad2648f-91c1-4a04-a18d-6dd4d6c7d21c.gif)

![logo](images/retrogpt-logo.png)

RetroGPT is a wrapper around the OpenAIs APIs, allowing us to access them on older web browsers and computers. You can throw away your "MSDOS for Dummies" books; now you can ask ChatGPT how to edit your Autoexec.bat and finally get your SoundBlaster 16 working on your 486, all while nestled in your comfort zone of Windows 3.11 for Workgroups.

![logo](images/image1.png)

RetroGPT is optimized for early to mid-90s web browsers. It should work on most versions of Netscape, Internet Explorer, Opera, and others. The ChatGPT sessions are structured with basic HTML table layouts. The DALL·E 2 images, the raw image is downloaded to a cache on your computer, and a downconverted GIF is sent in its place.

## Setup

In order to use RetroGPT, you must edit the included `APITokens.json` file and include your OpenAI Tokens. To get those tokens:

- [Login to the OpenAI site](https://platform.openai.com/account/api-keys), and create a new secret key.
- [Copy your Organization ID](https://platform.openai.com/account/org-settings)

```json
{
  "OpenAIServiceOptions": {
    "ApiKey": "{SECRET KEY}",
    "Organization": "{ORGANIZATION ID}"
  }
}
```

# Compile

RetroGPT is an ASP.NET Core Program. To compile:

- Clone this repository
- Install the [.NET 7 or later](https://dotnet.microsoft.com/en-us/download/dotnet) SDK
- From the `src` directory, run `dotnet build` and `dotnet run`.
