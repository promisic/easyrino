/*
 *  RINO obligation manager class
 *  Copyright (C) 2016 - 2019  Dusan Misic <promisic@outlook.com>
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
    internal class RinoObligationManager
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        internal RinoObligationManager()
        {
            FillDataTableWithColumns();
        }

        #region Convert XML to Rino List region

        /// <summary>
        /// Converts XmlDocument object to RinoObligationItem object.
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <returns>List of RinoObligationItem objects.</returns>
        internal List<RinoObligationItem> ConvertXmlToRinoList(XmlDocument rinoXmlDoc)
        {
            var roi = new List<RinoObligationItem>();
            var rinoXmlHeaderNodes = rinoXmlDoc.SelectNodes("//RINO");

            if (rinoXmlHeaderNodes != null)
                foreach (XmlNode xmlNode in rinoXmlHeaderNodes)
                {
                    var jbbkNode = xmlNode.SelectSingleNode("JBBK");
                    var typeNode = xmlNode.SelectSingleNode("Tip");
                    if (jbbkNode != null) Jbbk = jbbkNode.InnerText;
                    if (typeNode != null) ValidObligation = typeNode.InnerText == "Obaveza";
                }

            if (ValidObligation)
            {
                var xmlNodes = rinoXmlDoc.SelectNodes("//RINO/Obaveze/Obaveza");
                if (xmlNodes != null)
                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        var roiObj = new RinoObligationItem();
                        if (xmlNode.Attributes != null)
                        {
                            var xmlNodeValue = xmlNode.Attributes["VrstaPosla"].Value;

                            switch (xmlNodeValue)
                            {
                                case "U":
                                    roiObj.Action = RinoActionType.Unos;
                                    break;
                                case "I":
                                    roiObj.Action = RinoActionType.Izmena;
                                    break;
                                case "O":
                                    roiObj.Action = RinoActionType.Otkazivanje;
                                    break;
                            }
                        }

                        var iznosNode = xmlNode.SelectSingleNode("Iznos");
                        if (iznosNode != null)
                            roiObj.Iznos = decimal.Parse(iznosNode.InnerText,
                                CultureInfo.CreateSpecificCulture("en-US"));
                        var nazivNode = xmlNode.SelectSingleNode("NazivPoverioca");
                        if (nazivNode != null) roiObj.NazivPoverioca = nazivNode.InnerText;
                        var pibNode = xmlNode.SelectSingleNode("PIBPoverioca");
                        if (pibNode != null) roiObj.PibPoverioca = pibNode.InnerText;
                        var mbNode = xmlNode.SelectSingleNode("MBPoverioca");
                        if (mbNode != null) roiObj.MbPoverioca = mbNode.InnerText;
                        var vpNode = xmlNode.SelectSingleNode("VrstaPoverioca");
                        if (vpNode != null)
                            switch (vpNode.InnerText)
                            {
                                case "0":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                                    break;
                                case "1":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
                                    break;
                                case "8":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
                                    break;
                                case "9":
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
                                    break;
                                default:
                                    roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                                    break;
                            }

                        var ndNode = xmlNode.SelectSingleNode("NazivDokumenta");
                        if (ndNode != null) roiObj.NazivDokumenta = ndNode.InnerText;
                        var bdNode = xmlNode.SelectSingleNode("BrojDokumenta");
                        if (bdNode != null) roiObj.BrojDokumenta = bdNode.InnerText;
                        var ddNode = xmlNode.SelectSingleNode("DatumDokumenta");
                        roiObj.DatumDokumenta = DateTime.ParseExact(ddNode.InnerText, "yyyy-MM-dd", null);
                        var dnNode = xmlNode.SelectSingleNode("DatumNastanka");
                        roiObj.DatumNastanka = DateTime.ParseExact(dnNode.InnerText, "yyyy-MM-dd", null);

                        // This node may be missing / is optional.
                        if (xmlNode.SelectSingleNode("DatumRokaZaIzmirenje") != null)
                        {
                            var driNode = xmlNode.SelectSingleNode("DatumRokaZaIzmirenje");
                            roiObj.DatumRokaZaIzmirenje = DateTime.ParseExact(driNode.InnerText, "yyyy-MM-dd", null);
                        }

                        // This node may be missing / is optional.
                        if (xmlNode.SelectSingleNode("RazlogIzmene") != null)
                        {
                            var riNode = xmlNode.SelectSingleNode("RazlogIzmene");
                            roiObj.RazlogIzmene = riNode.InnerText;
                        }

                        roi.Add(roiObj);
                    }
            }

            return roi;
        }

        #endregion

        #region Convert Rino List to XML region

        /// <summary>
        /// Converts list of RinoObligationItem's to RINO XmlDocument object
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's object</param>
        /// <returns>RINO XmlDocument object</returns>
        internal XmlDocument ConvertRinoListToXml(List<RinoObligationItem> roiList)
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
            rinoTipNode.InnerText = "Obaveza";
            rinoRootNode.AppendChild(rinoTipNode);

            XmlNode rinoObavezeNode = rinoXml.CreateElement("Obaveze");
            rinoRootNode.AppendChild(rinoObavezeNode);

            foreach (var roiItem in roiList)
            {
                XmlNode rinoObavezaNode = rinoXml.CreateElement("Obaveza");
                var rinoObavezaAttr = rinoXml.CreateAttribute("VrstaPosla");
                string vrstaPosla = null;

                switch (roiItem.Action)
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

                rinoObavezaAttr.Value = vrstaPosla;
                rinoObavezaNode.Attributes?.Append(rinoObavezaAttr);
                rinoObavezeNode.AppendChild(rinoObavezaNode);

                XmlNode rinoIznosNode = rinoXml.CreateElement("Iznos");
                rinoIznosNode.InnerText = roiItem.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                rinoObavezaNode.AppendChild(rinoIznosNode);

                XmlNode rinoNpNode = rinoXml.CreateElement("NazivPoverioca");
                rinoNpNode.InnerText = roiItem.NazivPoverioca;
                rinoObavezaNode.AppendChild(rinoNpNode);

                XmlNode rinoPibNode = rinoXml.CreateElement("PIBPoverioca");
                rinoPibNode.InnerText = roiItem.PibPoverioca;
                rinoObavezaNode.AppendChild(rinoPibNode);

                XmlNode rinoMbNode = rinoXml.CreateElement("MBPoverioca");
                rinoMbNode.InnerText = roiItem.MbPoverioca;
                rinoObavezaNode.AppendChild(rinoMbNode);

                XmlNode rinoVpNode = rinoXml.CreateElement("VrstaPoverioca");
                string vrstaPoverioca = null;

                switch (roiItem.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        vrstaPoverioca = "0";
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        vrstaPoverioca = "1";
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        vrstaPoverioca = "8";
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        vrstaPoverioca = "9";
                        break;
                }

                rinoVpNode.InnerText = vrstaPoverioca ?? throw new InvalidOperationException();
                rinoObavezaNode.AppendChild(rinoVpNode);

                XmlNode rinoNdNode = rinoXml.CreateElement("NazivDokumenta");
                rinoNdNode.InnerText = roiItem.NazivDokumenta;
                rinoObavezaNode.AppendChild(rinoNdNode);

                XmlNode rinoBdNode = rinoXml.CreateElement("BrojDokumenta");
                rinoBdNode.InnerText = roiItem.BrojDokumenta;
                rinoObavezaNode.AppendChild(rinoBdNode);

                XmlNode rinoDdNode = rinoXml.CreateElement("DatumDokumenta");
                rinoDdNode.InnerText = roiItem.DatumDokumenta.ToString("yyyy-MM-dd");
                rinoObavezaNode.AppendChild(rinoDdNode);

                XmlNode rinoDnNode = rinoXml.CreateElement("DatumNastanka");
                rinoDnNode.InnerText = roiItem.DatumNastanka.ToString("yyyy-MM-dd");
                rinoObavezaNode.AppendChild(rinoDnNode);

                // Checking for optional DatumRokaZaIzmirenje
                if (CheckUninitializedDate(roiItem.DatumRokaZaIzmirenje) != true)
                {
                    XmlNode rinoDrziNode = rinoXml.CreateElement("DatumRokaZaIzmirenje");
                    rinoDrziNode.InnerText = roiItem.DatumRokaZaIzmirenje.ToString("yyyy-MM-dd");
                    rinoObavezaNode.AppendChild(rinoDrziNode);
                }

                // Checking for optional RazlogIzmene
                if (roiItem.RazlogIzmene != null)
                    if (roiItem.RazlogIzmene.Length > 0)
                    {
                        XmlNode rinoRiNode = rinoXml.CreateElement("RazlogIzmene");
                        rinoRiNode.InnerText = roiItem.RazlogIzmene;
                        rinoObavezaNode.AppendChild(rinoRiNode);
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

            return dateCompResult == 0;
        }

        #endregion

        #region Internal DataTable and properties region

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
        public bool ValidObligation { get; private set; }

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
            RinoDataTable.Columns.Add("rinoIznos", typeof(decimal));
            RinoDataTable.Columns.Add("rinoNazivPoverioca", typeof(string));
            RinoDataTable.Columns.Add("rinoPIB", typeof(string));
            RinoDataTable.Columns.Add("rinoMB", typeof(string));
            RinoDataTable.Columns.Add("rinoVrstaPoverioca", typeof(string));
            RinoDataTable.Columns.Add("rinoNazivDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoBrojDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumDokumenta", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumNastanka", typeof(string));
            RinoDataTable.Columns.Add("rinoDatumRokaZaIzmirenje", typeof(string));
            RinoDataTable.Columns.Add("rinoRazlogIzmene", typeof(string));
        }

        /// <summary>
        /// Converts list of RinoObligationItem's objects to DataTable rows.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        public void ConvertRinoListToDataTable(List<RinoObligationItem> roiList)
        {
            ClearDataTableRows();

            foreach (var listItem in roiList)
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

                var rinoVrstaPoverica = "";

                switch (listItem.VrstaPoverioca)
                {
                    case RinoVrstaPoverioca.PravnaLica:
                        rinoVrstaPoverica = "Pravno lice";
                        break;
                    case RinoVrstaPoverioca.JavniSektor:
                        rinoVrstaPoverica = "Javni sektor";
                        break;
                    case RinoVrstaPoverioca.PoljoprivrednaGazdinstva:
                        rinoVrstaPoverica = "Polj. gazdinstva";
                        break;
                    case RinoVrstaPoverioca.Kompenzacija:
                        rinoVrstaPoverica = "Kompenzacija";
                        break;
                }

                var rinoDueDate = !CheckUninitializedDate(listItem.DatumRokaZaIzmirenje)
                    ? listItem.DatumRokaZaIzmirenje.ToString("dd.MM.yyyy")
                    : null;
                var rinoReasonForChange = listItem.RazlogIzmene;
                RinoDataTable.Rows.Add(
                    rinoAction,
                    listItem.Iznos,
                    listItem.NazivPoverioca,
                    listItem.PibPoverioca,
                    listItem.MbPoverioca,
                    rinoVrstaPoverica,
                    listItem.NazivDokumenta,
                    listItem.BrojDokumenta,
                    listItem.DatumDokumenta.ToString("dd.MM.yyyy"),
                    listItem.DatumNastanka.ToString("dd.MM.yyyy"),
                    rinoDueDate,
                    rinoReasonForChange);
            }
        }

        #endregion

        #region Rino List manipulation methods region

        /// <summary>
        /// Get's RinoObligationItem at selected index.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects.</param>
        /// <param name="index">Index position in the list.</param>
        /// <returns>RinoObligationItem object at requested index position.</returns>
        public RinoObligationItem GetRoiItemAt(List<RinoObligationItem> roiList, int index)
        {
            return roiList.ElementAt(index);
        }

        /// <summary>
        /// Remove RinoObligationItem object from list at selected index.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoObligationItem's objects.</returns>
        public List<RinoObligationItem> RemoveItemAt(List<RinoObligationItem> roiList, int index)
        {
            roiList.RemoveAt(index);

            return roiList;
        }

        /// <summary>
        /// Inserts RinoObligationItem object to list.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="roiItem">RinoObligationItem object to insert</param>
        /// <returns>New lkist of RinoObligationItem's objects.</returns>
        public List<RinoObligationItem> InsertNewItem(List<RinoObligationItem> roiList, RinoObligationItem roiItem)
        {
            roiList.Add(roiItem);

            return roiList;
        }

        /// <summary>
        /// Modifies existing list of RinoObligationItem's objects. Removes object at selected index and inserts a new
        /// one in its place.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        /// <param name="roiItem">RinoObligationItem object</param>
        /// <param name="index">Index position in the list</param>
        /// <returns>New list of RinoObligationItem's objects</returns>
        public List<RinoObligationItem> ModifyExistingItem(List<RinoObligationItem> roiList, RinoObligationItem roiItem,
            int index)
        {
            roiList.RemoveAt(index);
            roiList.Insert(index, roiItem);
            return roiList;
        }

        #endregion
    }
}