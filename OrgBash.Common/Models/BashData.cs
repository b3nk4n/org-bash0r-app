using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using PhoneKit.Framework.Core.MVVM;

namespace OrgBash.Common.Models
{
    [DataContract]
    public class BashData : ViewModelBase
    {
        private const string NEWLINE = "[newline]";

        private List<BashQuoteItem> _cachedQuoteItems;

        private const double LINE_LENGTH = 52.0;

        public BashData()
        {
        }

        [DataMember(Name = "ident")]
        public int Id { get; set; }

        [DataMember(Name = "posLinkParam")]
        public string VotePosLinkParam { get; set; }

        [DataMember(Name = "negLinkParam")]
        public string VoteNegLinkParam { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        private int _rating;

        [DataMember(Name = "rating")]
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }

        public List<BashQuoteItem> QuoteItems
        {
            get
            {
                if (_cachedQuoteItems != null)
                    return _cachedQuoteItems;

                var result = new List<BashQuoteItem>();
                var persons = new Dictionary<string, int>();

                string[] splittedConversation = Content.Split(new string[]{ NEWLINE }, StringSplitOptions.RemoveEmptyEntries);
                bool isDoublePointNickMode = false;
                foreach (var conversationPart in splittedConversation)
                {
                    string nick = "";
                    int personIndex = -1;
                    string text = "";
                    int nameOpen = conversationPart.IndexOf('<');
                    int nameClose = conversationPart.IndexOf('>');
                    if (nameOpen == -1 && nameClose == -1)
                    {
                        nameOpen = conversationPart.IndexOf('(');
                        nameClose = conversationPart.IndexOf(')');
                    }
                    if (nameOpen == -1 && nameClose == -1)
                    {
                        nameOpen = conversationPart.IndexOf('[');
                        nameClose = conversationPart.IndexOf(']');
                    }
                    if (nameOpen == -1 && nameClose == -1)
                    {
                        nameOpen = conversationPart.IndexOf('{');
                        nameClose = conversationPart.IndexOf('}');
                    }
                    if (nameOpen == -1 && nameClose == -1)
                    {
                        nameOpen = 0;
                        nameClose = conversationPart.IndexOf("| ");
                    }
                    int nameDoublePointFinish = conversationPart.IndexOf(':'); // 2. type of quotes (bash.org only)
                    int heightScore = 0;

                    if (IsServerText(conversationPart))
                    {
                        nick = "server";
                        personIndex = -1;
                        text = TrimServerText(conversationPart);
                        heightScore = 2;
                    }
                    else if (nameOpen != -1 && nameClose != -1 && !isDoublePointNickMode)
                    {
                        nick = conversationPart.Substring(nameOpen + 1, nameClose - nameOpen - 1);
                        text = conversationPart.Substring(nameClose + 1, conversationPart.Length - nameClose - 1).Trim();

                        if (text.StartsWith(": "))
                        {
                            text = text.Substring(2, text.Length - 2);
                        }

                        // person index
                        if (persons.ContainsKey(nick))
                        {
                            personIndex = persons[nick];
                        }
                        else
                        {
                            personIndex = persons.Count;
                            persons.Add(nick, personIndex);
                        }
                        // heightScore
                        heightScore = 1 + (int)Math.Ceiling(text.Length / LINE_LENGTH);
                    }
                    else if (nameDoublePointFinish != -1 && nameDoublePointFinish < 17)
                    {
                        isDoublePointNickMode = true;
                        nick = conversationPart.Substring(0, nameDoublePointFinish);
                        text = conversationPart.Substring(nameDoublePointFinish + 1, conversationPart.Length - nameDoublePointFinish - 1).Trim();

                        if (text.StartsWith(": "))
                        {
                            text = text.Substring(2, text.Length - 2);
                        }

                        // person index
                        if (persons.ContainsKey(nick))
                        {
                            personIndex = persons[nick];
                        }
                        else
                        {
                            personIndex = persons.Count;
                            persons.Add(nick, personIndex);
                        }
                        // heightScore
                        heightScore = 1 + (int)Math.Ceiling(text.Length / LINE_LENGTH);
                    }
                    else
                    {
                        if (result.Count > 0)
                        {
                            var itemBefore = result[result.Count - 1];
                            itemBefore.Text += '\n' + conversationPart;
                            itemBefore.HeightScore += (int)Math.Ceiling(conversationPart.Length / LINE_LENGTH);
                        }
                        continue;
                    }

                    result.Add(new BashQuoteItem
                    {
                        Nick = nick,
                        PersonIndex = personIndex,
                        Text = text,
                        IndexPosition = result.Count,
                        HeightScore = heightScore
                    });
                }

                if (result.Count == 0)
                {
                    var singleResult = new BashQuoteItem();
                    singleResult.HeightScore = 3;
                    singleResult.IndexPosition = 0;
                    singleResult.Nick = string.Empty;
                    singleResult.PersonIndex = -1;
                    singleResult.Text = string.Empty;

                    foreach (var item in splittedConversation)
                    {
                        singleResult.Text += item + '\n';
                    }

                    var singleResultList = new List<BashQuoteItem>();
                    singleResultList.Add(singleResult);
                    return singleResultList;
                }

                _cachedQuoteItems = result;
                return result;
            }
        }

        public int GuessHeightScore()
        {
            int heightScore = 0;

            foreach (var item in QuoteItems)
            {
                heightScore += item.HeightScore;
            }

            return heightScore;
        }

        private string TrimServerText(string text)
        {
            if (text.StartsWith("*** ") || text.StartsWith("<-- ") || text.StartsWith("--> "))
            {
                return text.Substring(4, text.Length - 4);
            }
            else if (text.StartsWith("** "))
            {
                return text.Substring(3, text.Length - 3);
            }
            else if (text.StartsWith("* "))
            {
                return text.Substring(2, text.Length - 2);
            }
            else if (text.StartsWith("*"))
            {
                return text.Substring(1, text.Length - 1);
            }

            return text;
        }

        private bool IsServerText(string text)
        {
            return ((text.StartsWith("*") || text.StartsWith("<--") || text.StartsWith("-->") || text.StartsWith("-!-") || text.StartsWith("-")) && 
                    (text.Contains("For more info type: ") || // #178890 bah.org
                    text.Contains(" was banned from the server") || 
                    text.Contains(" is back from") || 
                    text.Contains(" was kicked ") ||
                    text.Contains(" has been kicked") || 
                    text.Contains("Quits: ") || 
                    text.Contains("Joins: ") || 
                    text.Contains(" has joined") || 
                    text.Contains(" has quit (") ||
                    text.Contains(") Quit (") ||
                    text.Contains(" dances :D") || // #4281 bash.org
                    text.Contains(" changed nick to") ||
                    text.Contains("Now talking in ") || 
                    text.Contains(" wirft seine Tastatur ausm Fenster") || 
                    text.Contains(" is now known as") ||
                    text.Contains("Parts: ") ||
                    text.Contains(" sets mode: ") ||
                    text.Contains(" left channel (") ||
                    text.Contains(" has left ") ||
                    text.Contains(" left #") ||
                    text.Contains(" is away - ") || // #5775 bash.org
                    text.Contains("psychs himself up to play ") ||
                    text.Contains(" are now chatting in ") ||
                    text.Contains(" (Read error: ") ||
                    text.Contains(" Topic in ") ||
                    text.Contains(" changes topic to ") ||
                    text.Contains(" bangs his head repeatedly ") ||
                    text.Contains(" takes a bow") ||
                    text.Contains(" sucks huge cock") ||
                    text.Contains(" designed this dc to dc power ") ||
                    text.Contains(" sent the satellite into orbit ") ||
                    text.Contains(" has changed topic for ") ||
                    text.Contains(" is omfg") ||
                    text.Contains(" datacide looks desperate") ||
                    text.Contains("rsynnott hopes") ||
                    text.Contains("martin adds it") ||
                    text.Contains(" is away -")) ||
                text.Contains(" has quit IRC") ||
                text.Equals("10 minutes later.") ||
                text.Equals("3 hours later.") ||
                text.Equals("15 minutes later.") ||
                text.Equals("--------------") ||
                text.StartsWith("[Privmsg] ") ||
                text.StartsWith("quit: (") ||
                text.StartsWith("join: ("));
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;
            var data = obj as BashData;

            return data.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public string QuoteString {
            get
            {
                var sb = new StringBuilder();
                bool isNotFirst = false;
                foreach(var quote in QuoteItems)
                {
                    if (isNotFirst)
                        sb.Append('\n');
                    else
                        isNotFirst = true;

                    if (quote.PersonIndex == -1)
                        sb.Append(string.Format("*** {0}", quote.Text));
                    else
                    sb.Append(string.Format("<{0}> {1}", quote.Nick, quote.Text));
                }
                return sb.ToString();
            }
        }

        public Uri Uri
        {
            get
            {
                return new Uri(string.Format(@"http://bash.org/?{0}", Id), UriKind.Absolute);
            }
        }
    }
}
