using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http;
using Newtonsoft.Json;
using apiTokenInfo;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Text;
using Newtonsoft.Json.Linq;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
// services.AddResponseCaching();

builder.Services.AddControllers();
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

static ApiTokenInfo LoginToKeyCloak(user InputUser, string apiUrl)
{
    //bypass ssl
    HttpClientHandler clientHandler = new HttpClientHandler();
    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

    // Pass the handler to httpclient(from you are calling api)
    HttpClient client = new HttpClient(clientHandler);

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
    //Them data vao body
    var dict = new Dictionary<string, string>();
    dict.Add("username", "" + InputUser.username);
    dict.Add("password", "" + InputUser.password);
    dict.Add("grant_type", "password");
    dict.Add("client_id", "arito");
    var content = new FormUrlEncodedContent(dict);
    //post
    var response = Task.Run(() => client.PostAsync(apiUrl, content));
    response.Wait();
    var result = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    //parse result to object
    var token_info = ApiTokenInfo.FromJson(result);
    return token_info;
}
static string ExtractUsernameFromAccessToken(string accessToken)
{

    if (accessToken.Split(".").Length != 3) {
        return "";
    }
    else
    {
        string inputStr = Encoding.UTF8.GetString(Convert.FromBase64String(accessToken.Split(".")[1]));
        dynamic payload = JObject.Parse(inputStr);
        string email = "";
        if (payload.email != null)
        {
            email = payload.email;
        }
        else {
            email = "empty";
        }
        return email;
    }
}
static string LayThongTinUser(string email)
{
    db _db = new db();
    string sqlData = _db.LayUserWithEmail($"select * from sysuserinfo where e_mail = '{email}'");
    dynamic userInfo = JObject.Parse(sqlData);
    return JsonConvert.SerializeObject(userInfo.Table1[0], Formatting.Indented);
}
app.MapGet("/health", () =>
{
    return "Login's health is good";
});

app.MapPost("/login", async (user InputUser) =>
{
    string url = "https://key.arito.vn:4433/realms/master/protocol/openid-connect/token";

    ApiTokenInfo token_info = LoginToKeyCloak(InputUser, url);

    //get username from token
    string email = ExtractUsernameFromAccessToken(token_info.AccessToken);
    Console.WriteLine("Email in access token payload : "+email);

    //query data from sql with above email
    string userInfo = LayThongTinUser(email);
    Console.WriteLine("sql data : " + userInfo);

    //write userInfo to redis
    RedisClient redis_cli = new RedisClient();
    redis_cli.writeDataToRedis(email, userInfo);
    Console.WriteLine("wrote data to redis");

    //return token to client
    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(token_info);
    return jsonString;
});
app.MapGet("/connect-to-db", () =>
{
    //db _db = new db();
    //_db.ThemKhachHang("insert into dmkh(ma_kh, ten_kh, dia_chi, ma_so_thue, status, datetime0, datetime2, user_id0, user_id2) values('KH22', 'duyv22', 'dia chi 22', '2200', 1, convert(datetime, '18-06-12 10:34:09 PM', 5), convert(datetime, '18-06-12 10:34:09 PM', 5), 1, 1)");
    //_db.XoaKhachHang("delete from dmkh where ma_kh = 'KH5'");
    return "Dang thuc thi sql";
});

app.MapGet("/test-redis", () =>
{
    RedisClient redisClient = new RedisClient();
    redisClient.Test();
    return "1";
});

app.Run();