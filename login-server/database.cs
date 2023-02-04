using System.Data;
using Newtonsoft.Json;
public class db {
    private string connectString = "Password=Matkhaum@nh124;Persist Security Info=True;User ID=sa;Initial Catalog=demo_ms;Data Source=10.10.100.14";

    // lay khach hang
    public string LayKhachHang(string querystr)
    {
        DataSet ds = Database.GetData(connectString, querystr);
        string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
        return json;
    }
    //Them khach hang
    public void ThemKhachHang(string querystr)
    {
        Database.ExecuteData(connectString, querystr);//insert into dmkh(ma_kh, ten_kh, dia_chi, ma_so_thue, status, datetime0, datetime2, user_id0, user_id2) values('KH20', 'duyv20', 'dia chi 20', '2000', 1, convert(datetime, '18-06-12 10:34:09 PM', 5), convert(datetime, '18-06-12 10:34:09 PM', 5), 1, 1)
    }
    //xoa khach khang
    public void XoaKhachHang(string querystr)
    {
        Database.ExecuteData(connectString, querystr);//delete from dmkh where
    }
    //Select * from sysuserinfo where email = 
    public string LayUserWithEmail(string querystr)
    {
        DataSet ds = Database.GetData(connectString, querystr);
        string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
        return json;
    }
}