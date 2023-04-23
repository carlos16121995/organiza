using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Organiza.Infrastructure.CrossCutting.Extensions.JsonConverters
{
    public class AnonymizeJsonConverter : JsonConverter
    {
        public List<string> StopWordsProperties = new List<string>() { "cvc", "cardnumber", "documentnumber" };
        public AnonymizeJsonConverter(List<string> stopWordsProperties = null)
        {
            if (stopWordsProperties != null)
            {
                StopWordsProperties.AddRange(stopWordsProperties.Select(o => o.ToLower()).ToList());
            }
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(object).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken obj = ApplyRuleStopWord(null, JToken.Load(reader));
            var obj1 = obj.ToObject(objectType);
            return obj1;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = ApplyRuleStopWord(value);
            jToken.WriteTo(writer);
        }

        public JToken ApplyRuleStopWord(object value, JToken? jToken = null)
        {
            JToken token = jToken ?? JToken.FromObject(value);
            if (token.Type == JTokenType.Array)
            {
                for (int i = 0; i < token.Count(); i++)
                {
                    token[i] = ApplyRuleStopWord(value, token[i]);
                }
            }

            if (token.Type == JTokenType.Object)
            {
                foreach (var item in token)
                {
                    if (item.Type == JTokenType.Property)
                    {
                        JProperty property = (JProperty)item;
                        if (property.Value.Type == JTokenType.Array || property.Value.Type == JTokenType.Object)
                        {
                            token[property.Name] = ApplyRuleStopWord(value, token[property.Name]);
                        }
                        else if (property.Value.Type == JTokenType.String && property.Name.ToLower() == "cardnumber")
                        {
                            token[property.Name] = (token[property.Name].Value<string>().BuildMaskedCardNumber());
                        }
                        else if (property.Value.Type == JTokenType.String && StopWordsProperties.Contains(property.Name.ToLower()))
                        {
                            token[property.Name] = "****";
                        }
                        else if (IsNumber(property.Value.Type) && StopWordsProperties.Contains(property.Name.ToLower()))
                        {
                            token[property.Name] = 0;
                        }
                    }
                }
            }
            return token;
        }

        private bool IsNumber(JTokenType type)
        {
            JTokenType[] typesNumber = new JTokenType[] { JTokenType.Integer, JTokenType.Float };
            return typesNumber.Contains(type);
        }
    }

}
