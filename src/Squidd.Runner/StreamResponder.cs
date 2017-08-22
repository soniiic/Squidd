using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Squidd.Runner
{
    public class StreamResponder : IDisposable
    {
        private readonly BinaryWriter writer;

        public StreamResponder(Stream stream)
        {
            writer = new BinaryWriter(stream, Encoding.UTF8, true);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Error(string message)
        {
            Write("EROR", message);
        }

        public void Log(string message)
        {
            Write("LOG", message);
        }

        public void Internal(string subType, string message)
        {
            Write("INT", message, new
            {
                SubType = subType
            });
        }

        private void Write(string type, string message, dynamic headerValues = null)
        {
            var encodedMessage = Encoding.UTF8.GetBytes(message);
            dynamic defaultHeader = new
            {
                Type = type,
                ContentLength = encodedMessage.Length
            };

            var header = Merge(defaultHeader, headerValues);

            var jsonHeader = JsonConvert.SerializeObject(header);

            writer.Write(jsonHeader);
            writer.Write(encodedMessage);
        }

        public dynamic Merge(object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in item1.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item1, null);
            }
            foreach (System.Reflection.PropertyInfo fi in item2.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item2, null);
            }
            return result;
        }

    }
}