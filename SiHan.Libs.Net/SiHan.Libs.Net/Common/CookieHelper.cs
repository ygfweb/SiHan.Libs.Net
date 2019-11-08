/**
 * 代码来源：https://www.cnblogs.com/scgw/archive/2012/03/26/2418161.html 
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SiHan.Libs.Net.Common
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public static class CookieHelper
    {

        public static void WriteCookiesToDisk(string file, CookieContainer cookie)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (cookie == null)
            {
                throw new ArgumentNullException(nameof(cookie));
            }
            using (Stream stream = File.Create(file))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, cookie);
            }
        }

        public static CookieContainer ReadCookiesFromDisk(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }
            using (Stream stream = File.Open(file, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (CookieContainer)formatter.Deserialize(stream);
            }
        }
    }
}
