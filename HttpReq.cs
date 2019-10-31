using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.IO;
using QuickTypeDM;
using Newtonsoft.Json;
using System.Web.UI;
using System.Web.Script.Serialization;

namespace AmaknaCore.Sniffer
{
    class HttpReq
    {

        public DofusMap RecepJSON(string PosX, string PosY, string direction)
        {
            switch (direction)
            {
                case "2":
                    direction = "bottom";
                    break;
                case "4":
                    direction = "left";
                    break;
                case "6":
                    direction = "top";
                    break;
                case "0":
                    direction = "right";
                    break;
                default:
                    MessageBox.Show("Erreur direction inconnue");
                    break;
            }

            string url = "https://dofus-map.com/huntTool/getData.php?x=" + PosX + "&y=" + PosY + "&direction=" + direction + "&world=" + AmaknaCore.Sniffer.View .MainForm.ChoixMap+ "&language=fr";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            DofusMap IndicesDirection;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = streamReader.ReadToEnd();
                IndicesDirection = (DofusMap)js.Deserialize(json, typeof(DofusMap));
            }
            return IndicesDirection;
        }
    }
}
