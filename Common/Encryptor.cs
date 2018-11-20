using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;


using System.Text;
using System.IO;
/// <summary>
///Encryptor 的摘要说明
/// </summary>
public class Encryptor
{
	public Encryptor()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    private static string key = "shhjjczx";
    private static string iv = "shhjjczx";

    public static string DesEncrypt(string src)
    {
        return DESEncrypt(src);
    }

    public static string DesDecrypt(string src)
    {
        return DESDecrypt(src);
    }

    private static string DESEncrypt(string pToEncrypt)
    {

        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);
        des.Key = ASCIIEncoding.ASCII.GetBytes(key);
        des.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        return ret.ToString();
    }

    private static string DESDecrypt(string pToDecrypt)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
        for (int x = 0; x < pToDecrypt.Length / 2; x++)
        {
            string str = pToDecrypt.Substring(x * 2, 2);
            int i = Convert.ToInt32(str, 16);
            inputByteArray[x] = (byte)i;
        }
        des.Key = ASCIIEncoding.ASCII.GetBytes(key);
        des.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        return System.Text.Encoding.UTF8.GetString(ms.ToArray());
    }
}