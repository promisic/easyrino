/*
 *  RINO reconcilement manager class
 *  Copyright (C) 2016 - 2019 Dusan Misic <promisic@outlook.com>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.Core
{
    internal class RinoReconcilementManager
    {
        #region Convert XML to Rino List region

        /// <summary>
        /// Converts XmlDocument object to RinoReconcilementItem object.
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <returns>List of RinoReconcilementItem objects.</returns>
        internal List<RinoReconcilementItem> ConvertXmlToRinoList(XmlDocument rinoXmlDoc)
        {
            var rri = new List<RinoReconcilementItem>();
            var rinoXmlHeaderNodes = rinoXmlDoc.SelectNodes("//RINO");

            if (rinoXmlHeaderNodes != null)
                foreach (XmlNode xmlNode in rinoXmlHeaderNodes)
                {
                    var jbbkNode = xmlNode.SelectSingleNode("JBBK");
                    var typeNode = xmlNode.SelectSingleNode("Tip");
                    if (jbbkNode != null) Jbbk = jbbkNode.InnerText;
                    if (typeNode != null) ValidReconcilement = typeNode.InnerText == "Izmirenje";
                }

            if (ValidReconcilement)
            {
                var xmlNodes = rinoXmlDoc.SelectNodes("//RINO/Izmirenja/Izmirenje");

                if (xmlNodes != null)
                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        var rriObj = new RinoReconcilementItem();

                        if (xmlNode.Attributes != null)
                        {
                            var xmlNodeValue = xmlNode.Attributes["VrstaPosla"].Value;

                            switch (xmlNodeValue)
                            {
                                case "U":
                                    rriObj.Action = RinoActionType.Unos;
                                    break;
                                case "I":
                                    rriObj.Action = RinoActionType.Izmena;
                                    break;
                                case "O":
                                    rriObj.Action = RinoActionType.Otkazivanje;
                                    break;
                            }
                        }

                        var idNode = xmlNode.SelectSingleNode("Id");
                        if (idNode != null) rriObj.RinoId = Convert.ToInt64(idNode.InnerText);
                        var bdNode = xmlNode.SelectSingleNode("BrojDokumenta");
                        if (bdNode != null) rriObj.BrojDokumenta = bdNode.InnerText;
                        var pibNode = xmlNode.SelectSingleNode("PIBPoverioca");
                        if (pibNode != null) rriObj.PibPoverioca = pibNode.InnerText;
                        var bankNode = xmlNode.SelectSingleNode("Banka");

                        if (bankNode != null)
                        {
                            rriObj.Banka = bankNode.InnerText;
                            var rpzrNode = xmlNode.SelectSingleNode("ReklPodZaRek");
                            if (rpzrNode != null) rriObj.ReklPodZaRek = rpzrNode.InnerText;
                            rriObj.Banka = bankNode.InnerText;
                        }

                        var diNode = xmlNode.SelectSingleNode("DatumIzmirenja");
                        rriObj.DatumIzmirenja = diNode != null && diNode.InnerText.Length > 0
                            ? DateTime.ParseExact(diNode.InnerText, "yyyy-MM-dd", null)
                            : new DateTime();
                        var iznosNode = xmlNode.SelectSingleNode("Iznos");
                        if (iznosNode != null)
                            rriObj.Iznos = decimal.Parse(iznosNode.InnerText, CultureInfo.GetCultureInfo("en-US"));

                        // This node may be missing / is optional
                        if (xmlNode.SelectSingleNode("RazlogIzmene") != null)
                        {
                            var riNode = xmlNode.SelectSingleNode("RazlogIzmene");
                            if (riNode != null) rriObj.RazlogIzmene = riNode.InnerText;
                        }

                        rri.Add(rriObj);
                    }
            }

            return rri;
        }

        #endregion

        #region Convert Rino List to XML region

        /// <summary>
        /// Converts list of RinoReconcilementItem's to RINO XmlDocument object
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's object</param>
        /// <returns>RINO XmlDocument object</returns>
        internal XmlDocument ConvertRinoListToXml(List<RinoReconcilementItem> rriList)
        {
            var rinoXml = new XmlDocument();
            var xmlOrderDecl = rinoXml.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            XmlNode rinoRootNode = rinoXml.CreateElement("RINO");
            rinoXml.AppendChild(rinoRootNode);
            rinoXml.InsertBefore(xmlOrderDecl, rinoRootNode);
            XmlNode rinoJbbkNode = rinoXml.CreateElement("JBBK");
            rinoJbbkNode.InnerText = Jbbk;
            rinoRootNode.AppendChild(rinoJbbkNode);
            XmlNode rinoTipNode = rinoXml.CreateElement("Tip");
            rinoTipNode.InnerText = "Izmirenje";
            rinoRootNode.AppendChild(rinoTipNode);
            XmlNode rinoIzmirenjaNode = rinoXml.CreateElement("Izmirenja");
            rinoRootNode.AppendChild(rinoIzmirenjaNode);

            foreach (var rriItem in rriList)
            {
                XmlNode rinoIzmirenjeNode = rinoXml.CreateElement("Izmirenje");
                var rinoIzmirenjeAttr = rinoXml.CreateAttribute("VrstaPosla");
                string vrstaPosla = null;

                switch (rriItem.Action)
                {
                    case RinoActionType.Unos:
                        vrstaPosla = "U";
                        break;
                    case RinoActionType.Izmena:
                        vrstaPosla = "I";
                        break;
                    case RinoActionType.Otkazivanje:
                        vrstaPosla = "O";
                        break;
                }

                rinoIzmirenjeAttr.Value = vrstaPosla;
                rinoIzmirenjeNode.Attributes?.Append(rinoIzmirenjeAttr);
                rinoIzmirenjaNode.AppendChild(rinoIzmirenjeNode);

                XmlNode rinoIdNode = rinoXml.CreateElement("Id");
                rinoIdNode.InnerText = rriItem.RinoId.ToString();
                rinoIzmirenjeNode.AppendChild(rinoIdNode);

                XmlNode rinoBdNode = rinoXml.CreateElement("BrojDokumenta");
                rinoBdNode.InnerText = rriItem.BrojDokumenta;
                rinoIzmirenjeNode.AppendChild(rinoBdNode);

                XmlNode rinoPibNode = rinoXml.CreateElement("PIBPoverioca");
                rinoPibNode.InnerText = rriItem.PibPoverioca;
                rinoIzmirenjeNode.AppendChild(rinoPibNode);

                XmlNode rinoBankaNode = rinoXml.CreateElement("Banka");
                rinoBankaNode.InnerText = rriItem.Banka;
                rinoIzmirenjeNode.AppendChild(rinoBankaNode);

                XmlNode rinoRpzrNode = rinoXml.CreateElement("ReklPodZaRek");
                rinoRpzrNode.InnerText = rriItem.ReklPodZaRek;
                rinoIzmirenjeNode.AppendChild(rinoRpzrNode);

                XmlNode rinoDiNode = rinoXml.CreateElement("DatumIzmirenja");
                rinoDiNode.InnerText = rriItem.DatumIzmirenja.ToString("yyyy-MM-dd");
                rinoIzmirenjeNode.AppendChild(rinoDiNode);

                XmlNode rinoIznosNode = rinoXml.CreateElement("Iznos");
                rinoIznosNode.InnerText = rriItem.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                rinoIzmirenjeNode.AppendChild(rinoIznosNode);

                // Checking for optional RazlogIzmene
                if (!string.IsNullOrEmpty(rriItem.RazlogIzmene))
                {
                    XmlNode rinoRiNode = rinoXml.CreateElement("RazlogIzmene");
                    rinoRiNode.InnerText = rriItem.RazlogIzmene;
                    rinoIzmirenjeNode.AppendChild(rinoRiNode);
                }
            }

            return rinoXml;
        }

        #endregion

        #region Utility method region

        /// <summary>
        /// Checks for uninitialized date.
        /// </summary>
        /// <param name="dateToCheck">DateTime object to check.</param>
        /// <returns>True if date is "default uninitialized one", fasle if otherwise.</returns>
        public bool CheckUninitializedDate(DateTime dateToCheck)
        {
            var uninitDateTime = new DateTime(0001, 1, 1, 0, 0, 0); // Target date is: 1.1.0001. 00.00.00
            var dateCompResult = DateTime.Compare(dateToCheck, uninitDateTime);
            if (dateCompResult == 0) return true;

            return false;
        }

        #endregion

        #region Internal DataTable and properties region

        /// <summary>
        /// Constructor
        /// </summary>
        internal RinoReconcilementManager()
        {
            FillDataTableWithColumns();
        }

        /// <summary>
        /// Property to get RINO data table.
        /// </summary>
        public DataTable RinoDataTable { get; private set; }

        /// <summary>
        /// Property to get or set JBBK ID.
        /// </summary>
        public string Jbbk { get; set; }

        /// <summary>
        /// Property to get or set valid obligation field.
        /// </summary>
        public bool ValidReconcilement { get; private set; }

        #endregion

        #region DataTable manipulation methods region

        /// <summary>
        /// Clears all rows inside internal DataTable object.
        /// </summary>
        private void ClearDataTableRows()
        {
            RinoDataTable.Rows.Clear();
        }

        /// <summary>
        /// Fill's internal DataTable object with columns.
        /// </summary>
        private void FillDataTableWithColumns()
        {
            RinoDataTable.Columns.Add("rinoAction", typeof(string));
            RinoDataTable.Columns.Add("rinoId", typeof(long));
            RinoDataTable.Columns.Add("rinoBrojDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoPIB", typeof(string));
            RinoDataTable.Columns.Add("rinoBanka", typeof(string));
            RinoDataTable.Columns.Add("rinoReklPodZaRek", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumIzmirenja", typeof(string));
            RinoDataTable.Columns.Add("rinoIznos", typeof(decimal));
            RinoDataTable.Columns.Add("rinoRazlogIzmene", typeof(string));
        }

        /// <summary>
        /// Converts list of RinoReconcilementItem's objects to DataTable rows.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        public void ConvertRinoListToDataTable(List<RinoReconcilementItem> rriList)
        {
            ClearDataTableRows();

            foreach (var listItem in rriList)
            {
                var rinoAction = "";

                switch (listItem.Action)
                {
                    case RinoActionType.Unos:
                        rinoAction = "Unos";
                        break;
                    case RinoActionType.Izmena:
                        rinoAction = "Izmena";
                        break;
                    case RinoActionType.Otkazivanje:
                        rinoAction = "Otkazivanje";
                        break;
                }

                var rinoPaidDate = !CheckUninitializedDate(listItem.DatumIzmirenja)
                    ? listItem.DatumIzmirenja.ToString("dd.MM.yyyy")
                    : null;
                var rinoReasonForChange = listItem.RazlogIzmene;
                RinoDataTable.Rows.Add(
                    rinoAction,
                    listItem.RinoId,
                    listItem.BrojDokumenta,
                    listItem.PibPoverioca,
                    listItem.Banka,
                    listItem.ReklPodZaRek,
                    rinoPaidDate,
                    listItem.Iznos,
                    rinoReasonForChange);
            }
        }

        #endregion

        #region Rino List manipulation methods region

        /// <summary>
        /// Get's RinoReconcilementItem at selected index.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects.</param>
        /// <param name="index">Index position in the list.</param>
        /// <returns>RinoReconcilementItem object at requested index position.</returns>
        public RinoReconcilementItem GetRriItemAt(List<RinoReconcilementItem> rriList, int index)
        {
            return rriList.ElementAt(index);
        }

        /// <summary>
        /// Remove RinoReconcilementItem object from list at selected index.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoReconcilementItem's objects.</returns>
        public List<RinoReconcilementItem> RemoveItemAt(List<RinoReconcilementItem> rriList, int index)
        {
            rriList.RemoveAt(index);

            return rriList;
        }

        /// <summary>
        /// Inserts RinoReconcilementItem object to list.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="rriItem">RinoReconcilementItem object to insert</param>
        /// <returns>New lkist of RinoReconcilementItem's objects.</returns>
        public List<RinoReconcilementItem> InsertNewItem(List<RinoReconcilementItem> rriList,
            RinoReconcilementItem rriItem)
        {
            rriList.Add(rriItem);

            return rriList;
        }

        /// <summary>
        /// Modifies existing list of RinoReconcilementItem's objects. Removes object at selected index and inserts a new
        /// one in its place.
        /// </summary>
        /// <param name="rriList">List of RinoReconcilementItem's objects</param>
        /// <param name="rriItem">RinoReconcilementItem object</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoReconcilementItem's objects</returns>
        public List<RinoReconcilementItem> ModifyExistingItem(List<RinoReconcilementItem> rriList,
            RinoReconcilementItem rriItem,
            int index)
        {
            rriList.RemoveAt(index);

            rriList.Insert(index, rriItem);

            return rriList;
        }

        #endregion
    }
}