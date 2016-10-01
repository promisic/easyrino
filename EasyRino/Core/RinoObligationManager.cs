/*
 *  RINO obligation manager class
 *  Copyright (C) 2016  Dusan Misic <promisic@outlook.com>
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
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using GriffinSoft.EasyRino.RinoCore;

namespace GriffinSoft.EasyRino.Core
{
    internal class RinoObligationManager
    {

        /// <summary>
        /// Internal DataTable field.
        /// </summary>
        private DataTable rinoDt = new DataTable();

        /// <summary>
        /// Property to get RINO data table.
        /// </summary>
        public DataTable RinoDataTable
        {
            get
            {
                return this.rinoDt;
            }
        }

        /// <summary>
        /// Property to get or set JBBK ID.
        /// </summary>
        public string Jbbk { get; set; }

        /// <summary>
        /// Property to get or set valid obligation field.
        /// </summary>
        public bool ValidObligation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        internal RinoObligationManager()
        {
            // Filling datatable with columns
            this.FillDataTableWithColumns();
        }

        /// <summary>
        /// Converts XmlDocument object to RinoObligationItem object.
        /// </summary>
        /// <param name="rinoXmlDoc">RINO XmlDocument object</param>
        /// <returns>List of RinoObligationItem objects.</returns>
        internal List<RinoObligationItem> ConvertXmlToRinoList(XmlDocument rinoXmlDoc)
        {
            // Creating RinoObligationItem list object
            List<RinoObligationItem> roi = new List<RinoObligationItem>();

            // Getting RINO header nodes
            XmlNodeList rinoXmlHeaderNodes = rinoXmlDoc.SelectNodes("//RINO");

            foreach (XmlNode xmlNode in rinoXmlHeaderNodes)
            {
                XmlNode jbbkNode = xmlNode.SelectSingleNode("JBBK");
                XmlNode typeNode = xmlNode.SelectSingleNode("Tip");

                // Setting JBBK
                this.Jbbk = jbbkNode.InnerText;

                if (typeNode.InnerText == "Obaveza")
                {
                    this.ValidObligation = true;
                }
                else
                {
                    this.ValidObligation = false;
                }
            }

            // Execute parsing stage ONLY if it is valid obligation XML file
            if (this.ValidObligation)
            {
                // Doing XPath search query
                XmlNodeList xmlNodes = rinoXmlDoc.SelectNodes("//RINO/Obaveze/Obaveza");

                // Creating CultureInfo object for date parsing
                CultureInfo usCulture = new CultureInfo("en-US");

                // Iterating each XmlNode
                foreach (XmlNode xmlNode in xmlNodes)
                {
                    // Creating ROI object to add to existing list
                    RinoObligationItem roiObj = new RinoObligationItem();

                    // Checking type of current XmlNode
                    if (xmlNode.Attributes["VrstaPosla"].Value == "U")
                    {
                        // Inserting new item
                        roiObj.Action = RinoActionType.Unos;
                    }
                    else if (xmlNode.Attributes["VrstaPosla"].Value == "I")
                    {
                        // Changing existing item
                        roiObj.Action = RinoActionType.Izmena;
                    }
                    else if (xmlNode.Attributes["VrstaPosla"].Value == "O")
                    {
                        // Deleting existing item
                        roiObj.Action = RinoActionType.Otkazivanje;
                    }

                    // Getting ammount
                    XmlNode iznosNode = xmlNode.SelectSingleNode("Iznos");

                    // Setting amount
                    roiObj.Iznos = Convert.ToDecimal(iznosNode.InnerText, CultureInfo.GetCultureInfo("en-US"));

                    // Getting name
                    XmlNode nazivNode = xmlNode.SelectSingleNode("NazivPoverioca");

                    // Setting name
                    roiObj.NazivPoverioca = nazivNode.InnerText;

                    // Getting PIB ID
                    XmlNode pibNode = xmlNode.SelectSingleNode("PIBPoverioca");

                    // Setting PIB ID
                    roiObj.PIBPoverioca = pibNode.InnerText;

                    // Getting MB ID
                    XmlNode mbNode = xmlNode.SelectSingleNode("MBPoverioca");

                    // Setting MB ID
                    roiObj.MBPoverioca = mbNode.InnerText;

                    // Getting VrstaPoverioca
                    XmlNode vpNode = xmlNode.SelectSingleNode("VrstaPoverioca");

                    // Parsing and setting VrstaPoverioca
                    if (vpNode.InnerText == "0")
                    {
                        roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                    }
                    else if (vpNode.InnerText == "1")
                    {
                        roiObj.VrstaPoverioca = RinoVrstaPoverioca.JavniSektor;
                    }
                    else if (vpNode.InnerText == "8")
                    {
                        roiObj.VrstaPoverioca = RinoVrstaPoverioca.PoljoprivrednaGazdinstva;
                    }
                    else if (vpNode.InnerText == "9")
                    {
                        roiObj.VrstaPoverioca = RinoVrstaPoverioca.Kompenzacija;
                    }
                    else
                    {
                        roiObj.VrstaPoverioca = RinoVrstaPoverioca.PravnaLica;
                    }

                    // Getting document name
                    XmlNode ndNode = xmlNode.SelectSingleNode("NazivDokumenta");

                    // Setting document name
                    roiObj.NazivDokumenta = ndNode.InnerText;

                    // Getting document number
                    XmlNode bdNode = xmlNode.SelectSingleNode("BrojDokumenta");

                    // Setting document number
                    roiObj.BrojDokumenta = bdNode.InnerText;

                    // Getting document creation date
                    XmlNode ddNode = xmlNode.SelectSingleNode("DatumDokumenta");

                    // Setting document creation date
                    roiObj.DatumDokumenta = DateTime.ParseExact(ddNode.InnerText, "yyyy-MM-dd", null);

                    // Getting document obligation start date
                    XmlNode dnNode = xmlNode.SelectSingleNode("DatumNastanka");

                    // Setting document obligation start date
                    roiObj.DatumNastanka = DateTime.ParseExact(dnNode.InnerText, "yyyy-MM-dd", null);

                    // This node may be missing / is optional.
                    if (xmlNode.SelectSingleNode("DatumRokaZaIzmirenje") != null)
                    {
                        // Getting document payment due date
                        XmlNode driNode = xmlNode.SelectSingleNode("DatumRokaZaIzmirenje");

                        // Setting document payment due date
                        roiObj.DatumRokaZaIzmirenje = DateTime.ParseExact(driNode.InnerText, "yyyy-MM-dd", null);
                    }

                    // This node may be missing / is optional.
                    if (xmlNode.SelectSingleNode("RazlogIzmene") != null)
                    {
                        // Getting reason for change
                        XmlNode riNode = xmlNode.SelectSingleNode("RazlogIzmene");

                        // Setting reason for change
                        roiObj.RazlogIzmene = riNode.InnerText;
                    }

                    // Adding roiObj object to roi
                    roi.Add(roiObj);
                }
            }

            return roi;
        }

        /// <summary>
        /// Converts list of RinoObligationItem's to RINO XmlDocument object
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's object</param>
        /// <returns>RINO XmlDocument object</returns>
        internal XmlDocument ConvertRinoListToXml(List<RinoObligationItem> roiList)
        {
            // Creating RINO XML object
            XmlDocument rinoXml = new XmlDocument();

            // Creating XML declaration
            XmlDeclaration xmlOrderDecl = rinoXml.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            // Creating RINO root node
            XmlNode rinoRootNode = rinoXml.CreateElement("RINO");
            // Append node
            rinoXml.AppendChild(rinoRootNode);

            // Inserting XML declaration
            rinoXml.InsertBefore(xmlOrderDecl, rinoRootNode);

            // Creating JBBK node
            XmlNode rinoJbbkNode = rinoXml.CreateElement("JBBK");
            // Setting JBBK value
            rinoJbbkNode.InnerText = this.Jbbk;

            // Append node
            rinoRootNode.AppendChild(rinoJbbkNode);

            // Creating Tip node
            XmlNode rinoTipNode = rinoXml.CreateElement("Tip");
            // Setting Tip value
            rinoTipNode.InnerText = "Obaveza";
            // Append node
            rinoRootNode.AppendChild(rinoTipNode);

            // Creating Obaveze node
            XmlNode rinoObavezeNode = rinoXml.CreateElement("Obaveze");
            // Append node
            rinoRootNode.AppendChild(rinoObavezeNode);

            // Iterating through roiList object
            foreach (RinoObligationItem roiItem in roiList)
            {
                // Creating Obaveza node
                XmlNode rinoObavezaNode = rinoXml.CreateElement("Obaveza");
                // Creating attribute
                XmlAttribute rinoObavezaAttr = rinoXml.CreateAttribute("VrstaPosla");
                // Setting attribute value
                string vrstaPosla = null;

                if (roiItem.Action == RinoActionType.Unos)
                {
                    vrstaPosla = "U";
                }
                else if (roiItem.Action == RinoActionType.Izmena)
                {
                    vrstaPosla = "I";
                }
                else if (roiItem.Action == RinoActionType.Otkazivanje)
                {
                    vrstaPosla = "O";
                }
                rinoObavezaAttr.Value = vrstaPosla;
                // Appending attribute
                rinoObavezaNode.Attributes.Append(rinoObavezaAttr);
                // Append node
                rinoObavezeNode.AppendChild(rinoObavezaNode);

                // Creating Iznos node
                XmlNode rinoIznosNode = rinoXml.CreateElement("Iznos");
                // Setting Iznos value
                rinoIznosNode.InnerText = roiItem.Iznos.ToString("#.##", CultureInfo.GetCultureInfo("en-US"));
                // Append node
                rinoObavezaNode.AppendChild(rinoIznosNode);

                // Creating NazivPoverioca
                XmlNode rinoNpNode = rinoXml.CreateElement("NazivPoverioca");
                // Setting NazivPoverioca value
                rinoNpNode.InnerText = roiItem.NazivPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoNpNode);

                // Creating PIBPoverioca node
                XmlNode rinoPibNode = rinoXml.CreateElement("PIBPoverioca");
                // Setting PIBPoverica value
                rinoPibNode.InnerText = roiItem.PIBPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoPibNode);

                // Creating MBPoverioca node
                XmlNode rinoMbNode = rinoXml.CreateElement("MBPoverioca");
                // Setting MBPoverioca value
                rinoMbNode.InnerText = roiItem.MBPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoMbNode);

                // Creating VrstaPoverioca node
                XmlNode rinoVpNode = rinoXml.CreateElement("VrstaPoverioca");
                // Setting VrstaPosla value
                string vrstaPoverioca = null;

                if (roiItem.VrstaPoverioca == RinoVrstaPoverioca.PravnaLica)
                {
                    vrstaPoverioca = "0";
                }
                else if (roiItem.VrstaPoverioca == RinoVrstaPoverioca.JavniSektor)
                {
                    vrstaPoverioca = "1";
                }
                else if (roiItem.VrstaPoverioca == RinoVrstaPoverioca.PoljoprivrednaGazdinstva)
                {
                    vrstaPoverioca = "8";
                }
                else if (roiItem.VrstaPoverioca == RinoVrstaPoverioca.Kompenzacija)
                {
                    vrstaPoverioca = "9";
                }

                rinoVpNode.InnerText = vrstaPoverioca;
                // Append node
                rinoObavezaNode.AppendChild(rinoVpNode);

                // Creating NazivDokumenta node
                XmlNode rinoNdNode = rinoXml.CreateElement("NazivDokumenta");
                // Setting NazivDokumenta value
                rinoNdNode.InnerText = roiItem.NazivDokumenta;
                // Append node
                rinoObavezaNode.AppendChild(rinoNdNode);

                // Creating BrojDokumenta node
                XmlNode rinoBdNode = rinoXml.CreateElement("BrojDokumenta");
                // Setting BrojDokumenta value
                rinoBdNode.InnerText = roiItem.BrojDokumenta;
                // Append node
                rinoObavezaNode.AppendChild(rinoBdNode);

                // Creating DatumDokumenta node
                XmlNode rinoDdNode = rinoXml.CreateElement("DatumDokumenta");
                // Setting DatumDokumenta value
                rinoDdNode.InnerText = roiItem.DatumDokumenta.ToString("yyyy-MM-dd");
                // Append node
                rinoObavezaNode.AppendChild(rinoDdNode);

                // Creating DatumNastanka node
                XmlNode rinoDnNode = rinoXml.CreateElement("DatumNastanka");
                // Setting DatumNastanka value
                rinoDnNode.InnerText = roiItem.DatumNastanka.ToString("yyyy-MM-dd");
                // Append node
                rinoObavezaNode.AppendChild(rinoDnNode);

                // Checking for optional DatumRokaZaIzmirenje
                if (this.CheckUninitializedDate(roiItem.DatumRokaZaIzmirenje) != true)
                {
                    // Creating DatumRokaZaIzmirenje node
                    XmlNode rinoDrziNode = rinoXml.CreateElement("DatumRokaZaIzmirenje");
                    // Setting DatumRokaZaIzmirenje value
                    rinoDrziNode.InnerText = roiItem.DatumRokaZaIzmirenje.ToString("yyyy-MM-dd");
                    // Append node
                    rinoObavezaNode.AppendChild(rinoDrziNode);
                }

                // Checking for optional RazlogIzmene
                if (roiItem.RazlogIzmene != null)
                {
                    if (roiItem.RazlogIzmene.Length > 0)
                    {
                        // Creating RazlogIzmene node
                        XmlNode rinoRiNode = rinoXml.CreateElement("RazlogIzmene");
                        // Setting RazlogIzmene value
                        rinoRiNode.InnerText = roiItem.RazlogIzmene;
                        // Append node
                        rinoObavezaNode.AppendChild(rinoRiNode);
                    }
                }
            }

            return rinoXml;
        }

        /// <summary>
        /// Destroy everything inside internal DataTable object.
        /// </summary>
        public void NukeDataTable()
        {
            this.rinoDt.Clear();
        }

        /// <summary>
        /// Clears all rows inside internal DataTable object.
        /// </summary>
        public void ClearDataTableRows()
        {
            this.rinoDt.Rows.Clear();
        }

        /// <summary>
        /// Fill's interal DataTable object with columns.
        /// </summary>
        public void FillDataTableWithColumns()
        {
            // Adding columns to DataTable object
            this.rinoDt.Columns.Add("rinoAction", typeof(string));
            this.rinoDt.Columns.Add("rinoIznos", typeof(decimal));
            this.rinoDt.Columns.Add("rinoNazivPoverioca", typeof(string));
            this.rinoDt.Columns.Add("rinoPIB", typeof(string));
            this.rinoDt.Columns.Add("rinoMB", typeof(string));
            this.rinoDt.Columns.Add("rinoVrstaPoverioca", typeof(string));
            this.rinoDt.Columns.Add("rinoNazivDokumenta", typeof(string));
            this.rinoDt.Columns.Add("rinoBrojDokumenta", typeof(string));
            this.rinoDt.Columns.Add("rinoDatumDokumenta", typeof(string));
            this.rinoDt.Columns.Add("rinoDatumNastanka", typeof(string));
            this.rinoDt.Columns.Add("rinoDatumRokaZaIzmirenje", typeof(string));
            this.rinoDt.Columns.Add("rinoRazlogIzmene", typeof(string));
        }

        /// <summary>
        /// Converts list of RinoObligationItem's objects to DataTable rows.
        /// </summary>
        /// <param name="roiList">List of RinoObligationItem's objects</param>
        public void ConvertRinoListToDataTable(List<RinoObligationItem> roiList)
        {
            // Clearing RINO DataTable object
            this.ClearDataTableRows();

            foreach (RinoObligationItem listItem in roiList)
            {
                // RINO action
                string rinoAction = "";

                if (listItem.Action == RinoActionType.Unos)
                {
                    rinoAction = "Unos";
                }
                else if (listItem.Action == RinoActionType.Izmena)
                {
                    rinoAction = "Izmena";
                }
                else if (listItem.Action == RinoActionType.Otkazivanje)
                {
                    rinoAction = "Otkazivanje";
                }

                // RINO vrsta poverioca
                string rinoVrstaPoverica = "";

                if (listItem.VrstaPoverioca == RinoVrstaPoverioca.PravnaLica)
                {
                    rinoVrstaPoverica = "0";
                }
                else if (listItem.VrstaPoverioca == RinoVrstaPoverioca.JavniSektor)
                {
                    rinoVrstaPoverica = "1";
                }
                else if (listItem.VrstaPoverioca == RinoVrstaPoverioca.PoljoprivrednaGazdinstva)
                {
                    rinoVrstaPoverica = "8";
                }
                else if (listItem.VrstaPoverioca == RinoVrstaPoverioca.Kompenzacija)
                {
                    rinoVrstaPoverica = "9";
                }

                // RINO due date variable
                string rinoDueDate = null;

                // RINO due date validity check
                if (this.CheckUninitializedDate(listItem.DatumRokaZaIzmirenje) != true)
                {
                    rinoDueDate = listItem.DatumRokaZaIzmirenje.ToString("dd.MM.yyyy");
                }
                else
                {
                    rinoDueDate = null;
                }

                // RINO reason for change variable
                string rinoReasonForChange = null;

                if (listItem.RazlogIzmene != null)
                {
                    rinoReasonForChange = listItem.RazlogIzmene;
                }
                else
                {
                    rinoReasonForChange = null;
                }

                // Adding new row to DataTable object
                this.rinoDt.Rows.Add(
                    rinoAction,
                    listItem.Iznos,
                    listItem.NazivPoverioca,
                    listItem.PIBPoverioca,
                    listItem.MBPoverioca,
                    rinoVrstaPoverica,
                    listItem.NazivDokumenta,
                    listItem.BrojDokumenta,
                    listItem.DatumDokumenta.ToString("dd.MM.yyyy"),
                    listItem.DatumNastanka.ToString("dd.MM.yyyy"),
                    rinoDueDate,
                    rinoReasonForChange);
            }
        }

        /// <summary>
        /// Checks for uninitialized date.
        /// </summary>
        /// <param name="dateToCheck">DateTime object to check.</param>
        /// <returns>True if date is "default uninitialized one", fasle if otherwise.</returns>
        public bool CheckUninitializedDate(DateTime dateToCheck)
        {
            // Creating DateTime object similar to uninitialized datetime
            DateTime uninDateTime = new DateTime(0001, 1, 1, 0, 0, 0); // Target date is: 1.1.0001. 00.00.00

            // Comparing dates
            int dateCompResult = DateTime.Compare(dateToCheck, uninDateTime);

            if (dateCompResult == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

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
            // Removing item from list at specific index
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
    }
}
