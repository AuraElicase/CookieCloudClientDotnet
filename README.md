# CookieCloudClientDotnet
用于连接CookieCloud下载Cookie和Localstorage内容的dotnet库

## API列表
CookieCloudClient.Get(string url, string uuid, string password, OutputRestrict restrict, string? userAgent)

|参数|说明|示例|
|:-:|:-:|:-:|
|url|CookieCloud服务器地址|https://cc.example.com|
|uuid|CookieCloud的uuid|1234567890|
|password|CookieCloud的密码|1234567890|
|restrict|要输出的CookieCloud存储<br>All: Cookie+LocalStorage<br>CookieOnly：Cookie<br>LocalStorageOnly：LocalStorage|OutputRestrict.All<br>OutputRestrict.CookieOnly<br>OutputRestrict.LocalStorageOnly|


## See Also

[CookieCloud](https://github.com/easychen/CookieCloud/blob/master/README_cn.md)