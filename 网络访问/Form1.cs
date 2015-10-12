using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
namespace 网络访问
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**
         * Get方式请求
         **/
        private void button1_Click(object sender, EventArgs e)
        {
            //发送请求
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/");

            //获得响应
            string res = string.Empty;
            try
            {
                //获取响应流
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
                response.Close();
                //控件显示
                label1.Text = res;
                //写入文件
                FileStream fs = new FileStream("test.html", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
                sw.Write(res);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }
        }

        /**
        * 文件下载
        **/
        private void button2_Click(object sender, EventArgs e)
        {
            //发送请求
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://su.bdimg.com/static/superpage/img/logo_white.png");

            //获得响应
            try
            {
                //获取响应流
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Stream stream = response.GetResponseStream();
                
                //写入文件
                Stream fs = new FileStream("test.jpg", FileMode.Create);
                byte[] bytes = new byte[1024];
                int osize = stream.Read(bytes, 0, (int)bytes.Length);
                while (osize > 0)
                {
                    fs.Write(bytes, 0, osize);
                    osize = stream.Read(bytes, 0, (int)bytes.Length);
                }
                stream.Close();
                response.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }
        }

        /**
        * Post方式请求
        **/
        private void button3_Click(object sender, EventArgs e)
        {
            Encoding myEncoding = Encoding.GetEncoding("gb2312");
            CookieContainer cookieCon = new CookieContainer();
            string Cookiesstr = string.Empty;
            string indata = HttpUtility.UrlEncode("j_username", myEncoding) + "=" + HttpUtility.UrlEncode("2120131096", myEncoding) + "&" + HttpUtility.UrlEncode("j_password", myEncoding) + "=" + HttpUtility.UrlEncode("izzz0928", myEncoding); 
            
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bs = encoding.GetBytes(indata);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://grdms.bit.edu.cn/yjs/login.do");
            myReq.CookieContainer = cookieCon;
            myReq.Method = "POST";
            myReq.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:11.0) Gecko/20100101 Firefox/11.0";
            myReq.ContentLength = bs.Length;
            myReq.ContentType = "application/x-www-form-urlencoded";        //POST类型需要设置
            myReq.AllowAutoRedirect = false;        //禁止重定向，方便获取Cookie
            //创建输入流
            Stream dataStream = null;
            try
            {
                //把数据写入HttpWebRequest的Request流 
                dataStream = myReq.GetRequestStream();
                dataStream.Write(bs, 0, bs.Length);
                //dataStream.Close();
            }
            catch (Exception)
            {
            }

            //获得响应及Cookie
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Cookiesstr = cookieCon.GetCookieHeader(myReq.RequestUri);
                Encoding respEncoding =  Encoding.GetEncoding(response.CharacterSet);
                StreamReader reader = new StreamReader(response.GetResponseStream(),respEncoding);
                res = reader.ReadToEnd();
                reader.Close();
                response.Close();
                label1.Text = res;
                FileStream fs = new FileStream("test.html", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, respEncoding);
                sw.Write(res);
                sw.Close();
                fs.Close();
            }
            catch (Exception e1)
            {
                label1.Text = e1.Message;
            }

            //获取主页
            myReq = (HttpWebRequest)WebRequest.Create("http://grdms.bit.edu.cn/yjs/yanyuan/py/pychengji.do?method=enterChaxun");
            myReq.CookieContainer = cookieCon;
            myReq.Headers.Add("Cookie:" + Cookiesstr);
            //获得响应
            try
            {
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Cookiesstr = cookieCon.GetCookieHeader(myReq.RequestUri);
                Encoding respEncoding = Encoding.GetEncoding(response.CharacterSet);
                StreamReader reader = new StreamReader(response.GetResponseStream(), respEncoding);
                res = reader.ReadToEnd();
                reader.Close();
                response.Close();
                label1.Text = res;
                FileStream fs = new FileStream("result.html", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, respEncoding);
                sw.Write(res);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://grdms.bit.edu.cn/yjs/login.do");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            label1.Text = "" + response.StatusCode;

        }

        /**
         * 人人网登录
         **/
        public String getContent()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string gethost = string.Empty;
            string content="";
            CookieContainer cc = new CookieContainer(); // 若要从远程调用中获取COOKIE一定要为request设定一个CookieContainer用来装载返回的cookies
            string Cookiesstr = string.Empty;
           
            try
            {
                //第一次POST请求 
                string postdata = "username=zzzbit&password=izzz0928&actionType=login";//模拟请求数据
                string LoginUrl = "http://www.rozo360.com/user/login.html";
                request = (HttpWebRequest)WebRequest.Create(LoginUrl);//实例化web访问类 
                request.Method = "POST";//数据提交方式为POST 
                //模拟头 
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] postdatabytes = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = postdatabytes.Length;
                //下面是禁止自动跳转
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:11.0) Gecko/20100101 Firefox/11.0";   //非常重要，验证身份
                //request.Credentials = CredentialCache.DefaultCredentials;   //非常重要，验证身份
                //提交请求 
                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(postdatabytes, 0, postdatabytes.Length);
                stream.Close();
                //接收响应 
                response = (HttpWebResponse)request.GetResponse();
                //保存返回cookie 
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                CookieCollection cook = response.Cookies;
                string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
                Cookiesstr = strcrook;
                //取第一次GET跳转地址 
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                content = sr.ReadToEnd();
                response.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return content;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string result = getContent();
            FileStream fs = new FileStream("test.html", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("gb2312"));
            sw.Write(result);
            sw.Close();
            fs.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //要匹配的字符串
            string text = "1A 2B 3C 4D 5E 6F 7G 8H 9I 10J 11Q 12J 13K 14L 15M 16N ffee80 #800080";
            //正则表达式
            string pattern = @"((\d+)(?<letter>[a-z]))\s+";
            MatchCollection matchs;
            matchs = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchs)
            {
                label1.Text += m.Groups["letter"];
            }
        }
    }
}
