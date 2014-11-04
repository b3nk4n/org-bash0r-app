using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OrgBash.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrgBash.Tests
{
    [TestClass]
    public class JsonConvertTest
    {
        [TestMethod]
        public void ParseTextNoApostrophInText()
        {
            string dataString = "{\"Inhalte\":{\"last_page\":0,\"data\":[{\"ident\":\"954863\",\"content\":\" i just paid $85 for a hoodie...[newline] but it lasts for a decade right[newline] youll be 30 before you need to buy another hoodie[newline] and by then youll be an adult and just buy a coat\",\"rating\":\"54\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954863\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954863\"},{\"ident\":\"954760\",\"content\":\" Having a girls head on your chest is one of the best feelings ever[newline] Especially when it's still attached.\",\"rating\":\"350\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954760\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954760\"},{\"ident\":\"954654\",\"content\":\" First they came for the verbs, and I said nothing because verbing weirds language. Then they arrival for the nouns, and I speech nothing because I no verbs.\",\"rating\":\"351\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954654\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954654\"}]}}";
            var bashData = JsonConvert.DeserializeObject<BashCollection>(dataString);
            Assert.IsNotNull(bashData);
        }

        [TestMethod]
        public void ParseTextWithApostrophInText() // -> must be trippe quoted!!
        {
            string dataString = "{\"Inhalte\":{\"last_page\":0,\"data\":[{\"ident\":\"954863\",\"content\":\" i just paid \\\"$85\\\" for a hoodie...[newline] but it lasts for a decade right[newline] youll be 30 before you need to buy another hoodie[newline] and by then youll be an adult and just buy a coat\",\"rating\":\"54\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954863\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954863\"},{\"ident\":\"954760\",\"content\":\" Having a girls head on your chest is one of the best feelings ever[newline] Especially when it's still attached.\",\"rating\":\"350\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954760\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954760\"},{\"ident\":\"954654\",\"content\":\" First they came for the verbs, and I said nothing because verbing weirds language. Then they arrival for the nouns, and I speech nothing because I no verbs.\",\"rating\":\"351\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954654\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954654\"}]}}";
            var bashData = JsonConvert.DeserializeObject<BashCollection>(dataString);
            Assert.IsNotNull(bashData);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void ParseTextWithShlashAtTheEnd()
        {
            string dataString = "{\"Inhalte\":{\"last_page\":0,\"data\":[{\"ident\":\"954863\",\"content\":\" i just paid $85 for a hoodie...[newline] but it lasts for a decade right[newline] youll be 30 before you need to buy another hoodie[newline] and by then youll be an adult and just buy a coat\\\",\"rating\":\"54\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954863\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954863\"},{\"ident\":\"954760\",\"content\":\" Having a girls head on your chest is one of the best feelings ever[newline] Especially when it's still attached.\",\"rating\":\"350\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954760\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954760\"},{\"ident\":\"954654\",\"content\":\" First they came for the verbs, and I said nothing because verbing weirds language. Then they arrival for the nouns, and I speech nothing because I no verbs.\",\"rating\":\"351\",\"posLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&rox=954654\",\"negLinkParam\":\"le=90f326cd3ffb47fce432ab5534a8af65&sox=954654\"}]}}";
            var bashData = JsonConvert.DeserializeObject<BashCollection>(dataString);
            Assert.IsNotNull(bashData);
        } 
    }
}
