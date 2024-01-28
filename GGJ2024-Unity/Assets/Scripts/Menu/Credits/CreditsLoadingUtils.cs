#region

using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

//Needed for Lists

#endregion

//Needed for XML functionality



namespace Scripts.MainMenuStuff.Credits{

    /// <summary>
    /// Utilities for the credits loading stuff
    /// </summary>
    internal static class CreditsLoadingUtils
    {

        /// <summary>
        /// It resizes the rectTransform of a TMP_Text object to the preferred height of the text.
        /// </summary>
        /// <param name="tmpro">The text object you want to resize.</param>
        /// <returns>
        /// The preferred height of the text.
        /// </returns>
        internal static float ResizeTMPRectTransformPreferredY(TMP_Text tmpro)
        {
            Vector2 rectDelta = tmpro.rectTransform.sizeDelta;
            //Debug.Log($"Initial y: {rectDelta.y} preferred {tmpro.preferredHeight} rendered {tmpro.renderedHeight}");
            tmpro.rectTransform.sizeDelta = new Vector2(rectDelta.x, tmpro.preferredHeight);
            return tmpro.preferredHeight;
        }

    }

    /// <summary>
    /// Class for the root element in the CREDITS_DATA.xml file
    /// </summary>
    internal class CreditsRootData
    {

        public List<AssetItemData> Credits { get; set; }
        public List<LicenseItemData> Licenses { get; set; }
    }


    /// <summary>
    /// Class for credits items loaded from the CREDITS_DATA.xml file
    /// </summary>
    internal struct AssetItemData
    {
        public readonly string assetname;
        public readonly string author;
        public readonly string from;

        public readonly string license;

        public readonly string extra;


        /// <summary>
        /// A constructor for the AssetItemData struct.
        /// It takes an XmlNode as a parameter and uses it to initialize the struct.
        /// </summary>
        /// <param name="node">XmlNode with the AssetItemData data</param>
        public AssetItemData(XmlNode node)
        {
            assetname = node["assetname"].InnerText;
            author = node["author"].InnerText;
            from = node["from"].InnerText;
            license = node["license"].InnerText;
            XmlElement extraXML = node["extra"];
            if (extraXML is null)
            {
                extra = "";
            }
            else
            {
                string extraText = extraXML.InnerText.Trim();
                if (extraText.Length == 0)
                {
                    extra = "";
                }
                else
                {
                    extra = $"\n{extraText}";
                }
            }
        }
    }

    /// <summary>
    /// Interface for something with a 'MyText' string property
    /// </summary>
    internal interface IHaveASingleTextString
    {
        string MyText { get; }
    }

    /// <summary>
    /// Class for license text items loaded from the CREDITS_DATA.xml file
    /// </summary>
    internal struct LicenseItemData : IHaveASingleTextString
    {
        public readonly string licensetext;

        public string MyText => licensetext;
        /// <summary>
        /// A constructor for the LicenseItemData struct.
        /// It takes an XmlNode as a parameter and uses it to initialize the struct.
        /// </summary>
        /// <param name="node">XmlNode with the LicenseItemData data</param>
        public LicenseItemData(XmlNode node)
        {
            licensetext = node.InnerText.Trim();
            //licensetext = node["licensetext"].InnerText;
        }

    }

    /// <summary>
    /// Struct for the `other` XML element from CREDITS_DATA.xml
    /// </summary>
    internal struct OtherItemData : IHaveASingleTextString
    {
        public readonly string otherText;

        public string MyText => otherText;

        /// <summary>
        /// A constructor for the OtherItemData struct.
        /// It takes an XmlNode as a parameter and uses it to initialize the struct.
        /// </summary>
        /// <param name="node">XmlNode with the OtherItemData data</param>
        public OtherItemData(XmlNode node)
        {
            otherText = node.InnerText.Trim();
        }
    }
}
