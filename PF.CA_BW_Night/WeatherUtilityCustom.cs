using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PF.CA_BW_Night
{
    class WeatherUtilityCustom
    { 
     public static string TQCodeToText(string code)
    {
        string str = String.Empty;
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"data\tq.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("information");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xns in xnl)
            {



                XmlElement xe = (XmlElement)xns;
                XmlNodeList xnls = xe.ChildNodes;

                if (xnls[1].LastChild.InnerText == code)
                {
                    str = xnls[0].FirstChild.InnerText;
                    break;
                }
            }

        }
        catch (Exception error)
        {
        }
        return str;
    }

    public static string TQNameToImage(string tianqiname)
    {
        string str = String.Empty;
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"data\tq.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("information");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xns in xnl)
            {



                XmlElement xe = (XmlElement)xns;
                XmlNodeList xnls = xe.ChildNodes;

                if (xnls[0].LastChild.InnerText == tianqiname)
                {
                    str = xnls[2].FirstChild.InnerText;
                    break;
                }
            }

        }
        catch (Exception error)
        {
        }
        return str;
    }
    public static string TQCodeToImage(string tianqicode)
    {
        string str = String.Empty;
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"data\tq.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("information");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xns in xnl)
            {



                XmlElement xe = (XmlElement)xns;
                XmlNodeList xnls = xe.ChildNodes;

                if (xnls[1].LastChild.InnerText == tianqicode)
                {
                    str = xnls[2].FirstChild.InnerText;
                    break;
                }
            }

        }
        catch (Exception error)
        {
        }
        return str;
    }

    public static string FSCodeToText(string code)
    {
        string str = "";
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"data\fs.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("information");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xns in xnl)
            {
                XmlElement xe = (XmlElement)xns;
                XmlNodeList xnls = xe.ChildNodes;

                if (xnls[1].LastChild.InnerText == code)
                {
                    str = xnls[0].FirstChild.InnerText;
                    break;
                }
            }

        }
        catch (Exception error)
        {
        }
        return str;
    }
    public static string FXCodeToText(string code)
    {
        string str = "";
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"data\fx.xml");
            XmlNode xn = xmlDoc.SelectSingleNode("information");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xns in xnl)
            {
                XmlElement xe = (XmlElement)xns;
                XmlNodeList xnls = xe.ChildNodes;

                if (xnls[1].LastChild.InnerText == code)
                {
                    str = xnls[0].FirstChild.InnerText;
                    break;
                }
            }

        }
        catch (Exception error)
        {
        }
        return str;
    }
}
}