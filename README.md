# WVCore.Server
Tiny Server. Example of [WVCore](https://github.com/nilaoda/WVCore) Api.

# Complie

* Native AOT
```
dotnet publish -r win-x64 -c Release
```

* Normal
```
del Directory.Build.props
dotnet publish -r win-x64 -c Release
```

# Api

## `/wvapi` 

METHOD: POST

REQ:
```json
{
    "PSSH":"PSSH",
    "Headers":{
        "User-Agent":"IOS"
    },
    "LicenseUrl":"https://auth"
}
```

RESP:
```json
{
    "pssh":"PSSH",
    "keys":[
        "kid:key",
        "kid:key"
    ]
}
```

## `/getchallenge`

METHOD: POST

REQ:
```json
{
    "PSSH":"PSSH",
    "CertBase64":"CertBase64"
}
```

RESP:
```json
{
    "challengeBase64":"challengeBase64"
}
```

## `/getkeys`

METHOD: POST

REQ:
```json
{
    "PSSH":"PSSH",
    "ChallengeBase64":"ChallengeBase64",
    "LicenseBase64":"LicenseBase64"
}
```

RESP:
```json
{
    "pssh":"PSSH",
    "keys":[
        "kid:key",
        "kid:key"
    ]
}
```

# JS Example

```js
let body = {
    "PSSH": "AAAAp3Bzc2gAAAAA7e+LqXnWSs6jyCfc1R0h7QAAAIcSEFF0U4YtQlb9i61PWEIgBNcSEPCTfpp3yFXwptQ4ZMXZ82USEE1LDKJawVjwucGYPFF+4rUSEJAqBRprNlaurBkm/A9dkjISECZHD0KW1F0Eqbq7RC4WmAAaDXdpZGV2aW5lX3Rlc3QiFnNoYWthX2NlYzViZmY1ZGM0MGRkYzlI49yVmwY=",
    "Headers": {
        "User-Agent": "Test"
    },
    "LicenseUrl": "https://cwip-shaka-proxy.appspot.com/no_auth"
}
let json = await fetch("http://127.0.0.1:18888/wvapi", {
    body: JSON.stringify(body),
    headers: {
        "Content-Type": "application/json"
    },
    method: "POST"
}).then(resp => resp.json());
console.log(json.keys);
```

# More

https://github.com/nilaoda/Blog/discussions/58


