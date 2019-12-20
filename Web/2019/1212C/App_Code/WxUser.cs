using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// WxUser 的摘要说明
/// </summary>
public class WxUser
{
    public WxUser()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    #region 类

    private string _openid;

    private int? _subscribe;

    private string _nickname;

    private int? _sex;

    private string _language;

    private string _city;

    private string _province;

    private string _country;

    private string _headimgurl;

    private DateTime? _subscribe_time;

    private string _unionid;

    private string _remark;

    private int? _groupid;

    private string _title;

    public string OpenId
    {
        get
        {
            return this._openid;
        }
        set
        {
            this._openid = value;
        }
    }

    public int? Subscribe
    {
        get
        {
            return this._subscribe;
        }
        set
        {
            this._subscribe = value;
        }
    }

    public string NickName
    {
        get
        {
            return this._nickname;
        }
        set
        {
            this._nickname = value;
        }
    }

    public int? Sex
    {
        get
        {
            return this._sex;
        }
        set
        {
            this._sex = value;
        }
    }

    public string Language
    {
        get
        {
            return this._language;
        }
        set
        {
            this._language = value;
        }
    }

    public string City
    {
        get
        {
            return this._city;
        }
        set
        {
            this._city = value;
        }
    }

    public string Province
    {
        get
        {
            return this._province;
        }
        set
        {
            this._province = value;
        }
    }

    public string Country
    {
        get
        {
            return this._country;
        }
        set
        {
            this._country = value;
        }
    }

    public string Headimgurl
    {
        get
        {
            return this._headimgurl;
        }
        set
        {
            this._headimgurl = value;
        }
    }

    public DateTime? Subscribe_Time
    {
        get
        {
            return this._subscribe_time;
        }
        set
        {
            this._subscribe_time = value;
        }
    }

    public string Unionid
    {
        get
        {
            return this._unionid;
        }
        set
        {
            this._unionid = value;
        }
    }

    public string Remark
    {
        get
        {
            return this._remark;
        }
        set
        {
            this._remark = value;
        }
    }

    public int? GroupId
    {
        get
        {
            return this._groupid;
        }
        set
        {
            this._groupid = value;
        }
    }

    public string Title
    {
        get
        {
            return this._title;
        }
        set
        {
            this._title = value;
        }
    }

    #endregion

    public WxUser(string OpenId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select OpenId,Subscribe,NickName,Sex,Language,City,Province,Country,Headimgurl,Subscribe_Time,Unionid,Remark,GroupId,Title ");
        stringBuilder.Append(" FROM [WxUser] ");
        stringBuilder.Append(" where OpenId=@OpenId ");
        SqlParameter[] array = new SqlParameter[]
        {
                new SqlParameter("@OpenId", SqlDbType.NVarChar, -1)
        };
        array[0].Value = OpenId;
        DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            if (dataSet.Tables[0].Rows[0]["OpenId"] != null)
            {
                this.OpenId = dataSet.Tables[0].Rows[0]["OpenId"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Subscribe"] != null && dataSet.Tables[0].Rows[0]["Subscribe"].ToString() != "")
            {
                this.Subscribe = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Subscribe"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["NickName"] != null)
            {
                this.NickName = dataSet.Tables[0].Rows[0]["NickName"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Sex"] != null && dataSet.Tables[0].Rows[0]["Sex"].ToString() != "")
            {
                this.Sex = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Sex"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Language"] != null)
            {
                this.Language = dataSet.Tables[0].Rows[0]["Language"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["City"] != null)
            {
                this.City = dataSet.Tables[0].Rows[0]["City"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Province"] != null)
            {
                this.Province = dataSet.Tables[0].Rows[0]["Province"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Country"] != null)
            {
                this.Country = dataSet.Tables[0].Rows[0]["Country"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Headimgurl"] != null)
            {
                this.Headimgurl = dataSet.Tables[0].Rows[0]["Headimgurl"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Subscribe_Time"] != null && dataSet.Tables[0].Rows[0]["Subscribe_Time"].ToString() != "")
            {
                this.Subscribe_Time = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["Subscribe_Time"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Unionid"] != null)
            {
                this.Unionid = dataSet.Tables[0].Rows[0]["Unionid"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Remark"] != null)
            {
                this.Remark = dataSet.Tables[0].Rows[0]["Remark"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["GroupId"] != null && dataSet.Tables[0].Rows[0]["GroupId"].ToString() != "")
            {
                this.GroupId = new int?(int.Parse(dataSet.Tables[0].Rows[0]["GroupId"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Title"] != null)
            {
                this.Title = dataSet.Tables[0].Rows[0]["Title"].ToString();
            }
        }
    }

    public bool Exists(string OpenId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select count(1) from [WxUser]");
        stringBuilder.Append(" where OpenId=@OpenId ");
        SqlParameter[] array = new SqlParameter[]
        {
                new SqlParameter("@OpenId", SqlDbType.NVarChar, -1)
        };
        array[0].Value = OpenId;
        return DbHelperSQL.Exists(stringBuilder.ToString(), array);
    }

    public void Add()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("insert into [WxUser] (");
        stringBuilder.Append("OpenId,Subscribe,NickName,Sex,Language,City,Province,Country,Headimgurl,Subscribe_Time,Unionid,Remark,GroupId,Title)");
        stringBuilder.Append(" values (");
        stringBuilder.Append("@OpenId,@Subscribe,@NickName,@Sex,@Language,@City,@Province,@Country,@Headimgurl,@Subscribe_Time,@Unionid,@Remark,@GroupId,@Title)");
        SqlParameter[] array = new SqlParameter[]
        {
                new SqlParameter("@OpenId", SqlDbType.NVarChar, 50),
                new SqlParameter("@Subscribe", SqlDbType.Int, 4),
                new SqlParameter("@NickName", SqlDbType.NVarChar, 50),
                new SqlParameter("@Sex", SqlDbType.Int, 4),
                new SqlParameter("@Language", SqlDbType.NVarChar, 50),
                new SqlParameter("@City", SqlDbType.NVarChar, 50),
                new SqlParameter("@Province", SqlDbType.NVarChar, 50),
                new SqlParameter("@Country", SqlDbType.NVarChar, 50),
                new SqlParameter("@Headimgurl", SqlDbType.NVarChar, 550),
                new SqlParameter("@Subscribe_Time", SqlDbType.DateTime),
                new SqlParameter("@Unionid", SqlDbType.NVarChar, 50),
                new SqlParameter("@Remark", SqlDbType.NVarChar, 550),
                new SqlParameter("@GroupId", SqlDbType.Int, 4),
                new SqlParameter("@Title", SqlDbType.NVarChar, 50)
        };
        array[0].Value = this.OpenId;
        array[1].Value = Subscribe == null ? -1 : Subscribe;
        array[2].Value = this.NickName;
        array[3].Value = this.Sex;
        array[4].Value = this.Language;
        array[5].Value = this.City;
        array[6].Value = this.Province;
        array[7].Value = this.Country;
        array[8].Value = this.Headimgurl;
        array[9].Value = this.Subscribe_Time;
        array[10].Value = this.Unionid == null ? DBNull.Value.ToString() : Unionid;
        array[11].Value = this.Remark;
        array[12].Value = this.GroupId == null ? -1 : GroupId;
        array[13].Value = this.Title;
        DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
    }

    public bool Update()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("update [WxUser] set ");
        stringBuilder.Append("Subscribe=@Subscribe,");
        stringBuilder.Append("NickName=@NickName,");
        stringBuilder.Append("Sex=@Sex,");
        stringBuilder.Append("Language=@Language,");
        stringBuilder.Append("City=@City,");
        stringBuilder.Append("Province=@Province,");
        stringBuilder.Append("Country=@Country,");
        stringBuilder.Append("Headimgurl=@Headimgurl,");
        stringBuilder.Append("Subscribe_Time=@Subscribe_Time,");
        stringBuilder.Append("Unionid=@Unionid,");
        stringBuilder.Append("Remark=@Remark,");
        stringBuilder.Append("GroupId=@GroupId,");
        stringBuilder.Append("Title=@Title");
        stringBuilder.Append(" where OpenId=@OpenId ");
        SqlParameter[] array = new SqlParameter[]
        {
                new SqlParameter("@Subscribe", SqlDbType.Int, 4),
                new SqlParameter("@NickName", SqlDbType.NVarChar, 50),
                new SqlParameter("@Sex", SqlDbType.Int, 4),
                new SqlParameter("@Language", SqlDbType.NVarChar, 50),
                new SqlParameter("@City", SqlDbType.NVarChar, 50),
                new SqlParameter("@Province", SqlDbType.NVarChar, 50),
                new SqlParameter("@Country", SqlDbType.NVarChar, 50),
                new SqlParameter("@Headimgurl", SqlDbType.NVarChar, 550),
                new SqlParameter("@Subscribe_Time", SqlDbType.DateTime),
                new SqlParameter("@Unionid", SqlDbType.NVarChar, 50),
                new SqlParameter("@Remark", SqlDbType.NVarChar, 550),
                new SqlParameter("@GroupId", SqlDbType.Int, 4),
                new SqlParameter("@Title", SqlDbType.NVarChar, 50),
                new SqlParameter("@OpenId", SqlDbType.NVarChar, 50)
        };
        array[0].Value = this.Subscribe == null ? -1 : Subscribe;
        array[1].Value = this.NickName;
        array[2].Value = this.Sex;
        array[3].Value = this.Language;
        array[4].Value = this.City;
        array[5].Value = this.Province;
        array[6].Value = this.Country;
        array[7].Value = this.Headimgurl;
        array[8].Value = this.Subscribe_Time;
        array[9].Value = this.Unionid == null ? DBNull.Value.ToString() : Unionid;
        array[10].Value = this.Remark;
        array[11].Value = this.GroupId == null ? -1 : GroupId;
        array[12].Value = this.Title;
        array[13].Value = this.OpenId;
        int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
        return num > 0;
    }

    public bool Delete(string OpenId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("delete from [WxUser] ");
        stringBuilder.Append(" where OpenId=@OpenId ");
        SqlParameter[] array = new SqlParameter[]
        {
            new SqlParameter("@OpenId", SqlDbType.NVarChar, -1)
        };
        array[0].Value = OpenId;
        int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
        return num > 0;
    }

    public void GetModel(string OpenId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select OpenId,Subscribe,NickName,Sex,Language,City,Province,Country,Headimgurl,Subscribe_Time,Unionid,Remark,GroupId,Title ");
        stringBuilder.Append(" FROM [WxUser] ");
        stringBuilder.Append(" where OpenId=@OpenId ");
        SqlParameter[] array = new SqlParameter[]
        {
                new SqlParameter("@OpenId", SqlDbType.NVarChar, -1)
        };
        array[0].Value = OpenId;
        DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
            if (dataSet.Tables[0].Rows[0]["OpenId"] != null)
            {
                this.OpenId = dataSet.Tables[0].Rows[0]["OpenId"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Subscribe"] != null && dataSet.Tables[0].Rows[0]["Subscribe"].ToString() != "")
            {
                this.Subscribe = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Subscribe"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["NickName"] != null)
            {
                this.NickName = dataSet.Tables[0].Rows[0]["NickName"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Sex"] != null && dataSet.Tables[0].Rows[0]["Sex"].ToString() != "")
            {
                this.Sex = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Sex"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Language"] != null)
            {
                this.Language = dataSet.Tables[0].Rows[0]["Language"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["City"] != null)
            {
                this.City = dataSet.Tables[0].Rows[0]["City"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Province"] != null)
            {
                this.Province = dataSet.Tables[0].Rows[0]["Province"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Country"] != null)
            {
                this.Country = dataSet.Tables[0].Rows[0]["Country"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Headimgurl"] != null)
            {
                this.Headimgurl = dataSet.Tables[0].Rows[0]["Headimgurl"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Subscribe_Time"] != null && dataSet.Tables[0].Rows[0]["Subscribe_Time"].ToString() != "")
            {
                this.Subscribe_Time = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["Subscribe_Time"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Unionid"] != null)
            {
                this.Unionid = dataSet.Tables[0].Rows[0]["Unionid"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["Remark"] != null)
            {
                this.Remark = dataSet.Tables[0].Rows[0]["Remark"].ToString();
            }
            if (dataSet.Tables[0].Rows[0]["GroupId"] != null && dataSet.Tables[0].Rows[0]["GroupId"].ToString() != "")
            {
                this.GroupId = new int?(int.Parse(dataSet.Tables[0].Rows[0]["GroupId"].ToString()));
            }
            if (dataSet.Tables[0].Rows[0]["Title"] != null)
            {
                this.Title = dataSet.Tables[0].Rows[0]["Title"].ToString();
            }
        }
    }

    public DataSet GetList(string strWhere)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select * ");
        stringBuilder.Append(" FROM [WxUser] ");
        if (strWhere.Trim() != "")
        {
            stringBuilder.Append(" where " + strWhere);
        }
        return DbHelperSQL.Query(stringBuilder.ToString());
    }
}