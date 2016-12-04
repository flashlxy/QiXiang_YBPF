using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetworkConnection;

namespace PF.Utility
{
    public static class FileHelper
    {
        /// <summary>
        /// 获取指定目录下的所有文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static List<FileInfo> GetFileInfos(string path)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<FileInfo> list = forder.GetFiles().OrderByDescending(a => a.LastWriteTime).ToList();
            return list;
        }

        /// <summary>
        /// 获取指定目录下的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public static List<FileInfo> GetFileInfos(string path, string para)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<FileInfo> list = forder.GetFiles(para).OrderByDescending(a => a.LastWriteTime).ToList();
            return list;
        }

        /// <summary>
        /// 获取指定目录下的单个文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string path, string para)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<FileInfo> list = forder.GetFiles(para).OrderByDescending(a => a.LastWriteTime).ToList();

            if (list.Count() > 0)
            {
                return list.FirstOrDefault();

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 如果指定路径的文件夹不存在，则创建文件夹
        /// </summary>
        /// <param name="path">路径</param>

        public static void CreateForderIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 获取指定目录下的所有文件目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static List<DirectoryInfo> GetDirectorys(string path)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<DirectoryInfo> list = forder.GetDirectories().OrderByDescending(a => a.LastWriteTime).ToList();
            return list;
        }

        /// <summary>
        /// 获取指定目录下的最新文件目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static DirectoryInfo GetLastNameDirectory(string path)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<DirectoryInfo> list = forder.GetDirectories().OrderByDescending(a => a.FullName).ToList();
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 获取指定目录下的最新文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static FileInfo GetLastNameFileInfo(string path)
        {
            DirectoryInfo forder = new DirectoryInfo(path);
            List<FileInfo> list = forder.GetFiles().OrderByDescending(a => a.FullName).ToList();
            return list.FirstOrDefault();
        }

        //public static bool DocToHtml(string filename, bool isFixed)
        //{
        //    try
        //    {
        //        AsposeWordHelper wordHelper = new AsposeWordHelper();
        //        string docpath = filename;

        //        bool result = wordHelper.DocToHtml(docpath, isFixed);

        //        return result;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }




        //}

        #region 共享

        /// <summary>
        /// 访问共享目录
        /// </summary>
        /// <param name="path">共享路线  例如：\\172.18.226.52\预报科各种预报和服务</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>目录下所有文件列表</returns>
        public static List<FileInfo> GetShareFileInfos(string path, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\");
                NetworkConnection.Operation.Disconnect(localpath);
                return list;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        /// <summary>
        /// 访问共享目录
        /// </summary>
        /// <param name="path">共享路线  例如：\\172.18.226.52\预报科各种预报和服务</param>
        /// <param name="para">文件过滤参数</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>目录下所有文件列表</returns>
        //public static List<FileInfo> GetShareFileInfos(string path, string para, string username, string password)
        //{
        //    string localpath = "Q:";
        //    int status = NetworkConnection.Connect(path, localpath, username, password);
        //    if (status == (int)ERROR_ID.ERROR_SUCCESS)
        //    {
        //        List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\", para);
        //        NetworkConnection.Disconnect(localpath);
        //        return list;
        //    }
        //    else
        //    {
        //        NetworkConnection.Disconnect(localpath);
        //        return null;
        //    }
        //}
        public static List<FileInfo> GetShareFileInfos(string path, string para, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\", para);
                NetworkConnection.Operation.Disconnect(localpath);
                return list;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }





        public static List<DirectoryInfo> GetShareDirectorys(string path, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                //List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\", para);
                //NetworkConnection.Disconnect(localpath);
                //return list;



                DirectoryInfo forder = new DirectoryInfo(@"Q:\");
                List<DirectoryInfo> list = forder.GetDirectories().OrderByDescending(a => a.LastWriteTime).ToList();
                return list;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }


        public static List<DirectoryInfo> GetShareDirectorys(string path, string para, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                //List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\", para);
                //NetworkConnection.Disconnect(localpath);
                //return list;



                DirectoryInfo forder = new DirectoryInfo(@"Q:\");
                List<DirectoryInfo> list = forder.GetDirectories(para).OrderByDescending(a => a.LastWriteTime).ToList();
                return list;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        public static FileInfo GetLastShareFileInfo(string path, string para, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\", para);
                NetworkConnection.Operation.Disconnect(localpath);


                if (list.Count() > 0)
                {
                    return list.OrderByDescending(a => a.LastWriteTime).FirstOrDefault();
                }
                else
                {
                    return null;
                }


            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        public static FileInfo GetLastShareFileInfo(string path, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                List<FileInfo> list = FileHelper.GetFileInfos(@"Q:\");
                NetworkConnection.Operation.Disconnect(localpath);


                if (list.Count() > 0)
                {
                    return list.OrderByDescending(a => a.LastWriteTime).FirstOrDefault();
                }
                else
                {
                    return null;
                }


            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }








        public static bool CopyShareFile(string frompath, string topath, string filename, string username,
            string password)
        {
            string localpath = "Q:";

            string parentpath = frompath.Substring(0, frompath.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = frompath.Substring(frompath.LastIndexOf(@"\") + 1,
                frompath.Length - frompath.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(frompath, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                string fromfile = @"Q:\" + filename;
                string tofile = topath + @"\" + filename;

                if (File.Exists(fromfile))
                {
                    File.Copy(fromfile, tofile, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return false;
            }
        }



        public static DirectoryInfo GetShareLastDirectory(string path, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {

                DirectoryInfo forder = new DirectoryInfo(@"Q:\");
                List<DirectoryInfo> list = forder.GetDirectories().OrderByDescending(a => a.FullName).ToList();
                return list.FirstOrDefault();

            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        public static DirectoryInfo GetShareLastDirectory(string path, string para, string username, string password)
        {
            string localpath = "Q:";

            string parentpath = path.Substring(0, path.LastIndexOf(@"\"));
            //string parentpath = @"\\172.18.226.52\预报科各种预报和服务";

            string childpath = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1);


            int status = NetworkConnection.Operation.Connect(path, localpath, @username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {

                DirectoryInfo forder = new DirectoryInfo(@"Q:\");
                List<DirectoryInfo> list = forder.GetDirectories(para).OrderByDescending(a => a.FullName).ToList();
                return list.FirstOrDefault();

            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }


        #endregion


        public static string GetShareTextContent(string filename, string username, string password, Encoding encoding)
        {
            string localpath = "Q:";
            string path = filename.Substring(0, filename.LastIndexOf(@"\"));
            string name = Path.GetFileName(filename);

            int status = NetworkConnection.Operation.Connect(path, localpath, username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {



                string content = File.ReadAllText(@"Q:\" + name, encoding);



                NetworkConnection.Operation.Disconnect(localpath);
                return content;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        public static string[] GetShareTextLines(string filename, string username, string password)
        {
            string localpath = "Q:";
            string path = filename.Substring(0, filename.LastIndexOf(@"\"));
            string name = Path.GetFileName(filename);

            int status = NetworkConnection.Operation.Connect(path, localpath, username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {



                string[] content = File.ReadAllLines(@"Q:\" + name);



                NetworkConnection.Operation.Disconnect(localpath);
                return content;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return null;
            }
        }

        //public static string GetShareDocContent(string filename, string username, string password)
        //{
        //    string localpath = "Q:";
        //    string path = filename.Substring(0, filename.LastIndexOf(@"\"));
        //    string name = Path.GetFileName(filename);

        //    int status = NetworkConnection.Operation.Connect(path, localpath, username, password);
        //    if (status == (int)ERROR_ID.ERROR_SUCCESS)
        //    {
        //        AsposeWordHelper awh = new AsposeWordHelper();
        //        string temp = HttpContext.Current.Server.MapPath("~/Data/Temp/" + name); //复制缓存

        //        if (File.Exists(temp))
        //        {
        //            try
        //            {
        //                File.Delete(temp);

        //            }
        //            catch (Exception)
        //            {


        //            }
        //        }


        //        File.Copy(@"Q:\" + name, temp);
        //        //string content = awh.ReadDoc(@"Q:\" + name);
        //        string content = awh.ReadDoc(temp);
        //        File.Delete(temp);
        //        NetworkConnection.Operation.Disconnect(localpath);
        //        return content;
        //    }
        //    else
        //    {
        //        NetworkConnection.Operation.Disconnect(localpath);
        //        return null;
        //    }
        //}



        //public static bool ShareDocToHtml(string filename, string username, string password, bool isFixed)
        //{

        //    string path = filename.Substring(0, filename.LastIndexOf(@"\"));
        //    string name = Path.GetFileName(filename);


        //    string localpath = "Q:";
        //    int status = NetworkConnection.Operation.Connect(path, localpath, username, password);
        //    if (status == (int)ERROR_ID.ERROR_SUCCESS)
        //    {




        //        AsposeWordHelper wordHelper = new AsposeWordHelper();
        //        string docpath = @"Q:\" + name;

        //        bool result = wordHelper.DocToHtml(docpath, isFixed);
        //        NetworkConnection.Operation.Disconnect(localpath);
        //        return result;

        //    }
        //    else
        //    {
        //        NetworkConnection.Operation.Disconnect(localpath);
        //        return false;
        //    }


        //}

        //2016年11月29日21:17:14
        public static bool WriteShareText(string filename, string content, string username, string password)
        {
            string localpath = "Q:";
            string path = filename.Substring(0, filename.LastIndexOf(@"\"));
            string name = Path.GetFileName(filename);

            int status = NetworkConnection.Operation.Connect(path, localpath, username, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                if (File.Exists(@"Q:\" + name))
                {
                    File.Delete(@"Q:\" + name);
                }

                File.WriteAllText(@"Q:\" + name, content);



                //string content = File.ReadAllText(@"Q:\" + name);



                NetworkConnection.Operation.Disconnect(localpath);
                return true;
            }
            else
            {
                NetworkConnection.Operation.Disconnect(localpath);
                return false;
            }
        }

    }
}